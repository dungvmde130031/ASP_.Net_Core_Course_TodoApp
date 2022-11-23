﻿using TodoApp.Business.Model;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using TodoApp.Business.Models;
using TaskStatus = TodoApp.Common.TaskStatus;

namespace TodoApp.Business
{
    public interface ITaskBusiness
    {
        TaskModel AddTask(TaskModel taskModel);

        TaskModel GetTask(Guid id);

        List<TaskModel> GetTasks();

        TaskModel UpdateTask(TaskModel taskModel);

        bool DeleteTask(Guid id);

        bool CompleteTask(Guid id);

        bool ReopenTask(Guid id);

        List<TaskModel> TaskFilter(TaskSearchModel model);
    }

    public class TaskBusiness : ITaskBusiness
    {
        private IHttpContextAccessor _httpContextAccessor;

        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public TaskBusiness(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public TaskModel AddTask(TaskModel taskModel)
        {
            taskModel.Id = Guid.NewGuid();
            taskModel.CreatedTime = DateTime.Now;
            taskModel.CreatedBy = "ADMIN";
            taskModel.UpdatedTime = DateTime.Now;
            taskModel.UpdatedBy = "ADMIN";
            taskModel.Status = Common.TaskStatus.InProgress;

            _session.SetTaskModel(taskModel);
            return taskModel;
        }

        public bool DeleteTask(Guid id)
        {
            var currentTask = _session.GetTaskModel(id);

            if(currentTask != null)
            {
                currentTask.Status = Common.TaskStatus.Deleted;
                _session.SetTaskModel(currentTask);

                return true;
            }
            return false;
        }

        public TaskModel GetTask(Guid id)
        {
            var currentTask = _session.GetTaskModel(id);

            return currentTask;
        }

        public List<TaskModel> GetTasks()
        {
            var tasks = new List<TaskModel>();

            if(_session.Keys.Any())
            {
                foreach(var key in _session.Keys)
                {
                    var task = _session.GetTaskModel(Guid.Parse(key));

                    if(task != null)
                    {
                        tasks.Add(task);
                    }
                }
            }
            return tasks.OrderByDescending(t => t.UpdatedTime).ToList();
        }

        public TaskModel UpdateTask(TaskModel taskModel)
        {
            var currentTask = _session.GetTaskModel(taskModel.Id);

            if (currentTask != null)
            {
                currentTask.Status = taskModel.Status;
                currentTask.Title = taskModel.Title;
                currentTask.Content = taskModel.Content;
                currentTask.UpdatedTime = DateTime.Now;
                currentTask.UpdatedBy = "ADMIN";

                _session.SetTaskModel(currentTask);

                return currentTask;
            }
            return currentTask;
        }

        public bool CompleteTask(Guid id)
        {
            var currentTask = _session.GetTaskModel(id);

            if (currentTask != null)
            {
                currentTask.Status = Common.TaskStatus.Completed;
                _session.SetTaskModel(currentTask);

                return true;
            }
            return false;
        }

        public bool ReopenTask(Guid id)
        {
            var deteledTask = _session.GetTaskModel(id);

            if (deteledTask != null)
            {
                deteledTask.Status = Common.TaskStatus.InProgress;
                _session.SetTaskModel(deteledTask);

                return true;
            }
            return false;
        }

        public List<TaskModel> TaskFilter(TaskSearchModel model)
        {
            var tasks = GetTasks();

            // Filter by Title/Content
            tasks = tasks.Where(x => (model.StatusFilter == -1) || (x.Status == (TaskStatus)model.StatusFilter)).ToList();
            
            tasks = tasks.Where(x => string.IsNullOrEmpty(model.SearchString)
                || (x.Title.Contains(model.SearchString) || x.Content.Contains(model.SearchString))).ToList();

            // Filter by CreateTime
            if (model.SearchDateFrom.Date.Year == 1 && model.SearchDateTo.Date.Year == 1)
            {
                DateTime dtFrom = Convert.ToDateTime("01/01/0001");
                DateTime dtTo = Convert.ToDateTime("01/01/9999");

                string s1 = dtFrom.ToString("dd-MM-yyyy");
                string s2 = dtTo.ToString("dd-MM-yyyy");

                model.SearchDateFrom = Convert.ToDateTime(s1);
                model.SearchDateTo = Convert.ToDateTime(s2);

                tasks = tasks.Where(x => x.CreatedTime >= model.SearchDateFrom && x.CreatedTime <= model.SearchDateTo).ToList();
            }
            else if (model.SearchDateFrom.Date.Year == 1)
            {
                DateTime dtFrom = Convert.ToDateTime("01/01/0001");

                string s1 = dtFrom.ToString("dd-MM-yyyy");

                model.SearchDateFrom = Convert.ToDateTime(s1);

                tasks = tasks.Where(x => x.CreatedTime >= model.SearchDateFrom && x.CreatedTime <= model.SearchDateTo).ToList();
            }
            else if (model.SearchDateTo.Date.Year == 1)
            {
                DateTime dtTo = Convert.ToDateTime("01/01/9999");

                string s2 = dtTo.ToString("dd-MM-yyyy");

                model.SearchDateTo = Convert.ToDateTime(s2);

                tasks = tasks.Where(x => x.CreatedTime >= model.SearchDateFrom && x.CreatedTime <= model.SearchDateTo).ToList();
            }
            else
            {
                tasks = tasks.Where(x => x.CreatedTime >= model.SearchDateFrom && x.CreatedTime <= model.SearchDateTo).ToList();
            }

            return tasks.OrderByDescending(t => t.UpdatedTime).ToList();
        }

        
    }

    public static class SessionExtension
    {
        public static void SetTaskModel(this ISession session, TaskModel taskModel)
        {
            session.SetString(taskModel.Id.ToString(), JsonConvert.SerializeObject(taskModel));
        }

        public static TaskModel GetTaskModel(this ISession session, Guid taskId)
        {
            if(session.Keys.Any(x => x == taskId.ToString()))
            {
                var taskModelJson = session.GetString(taskId.ToString());

                if(taskModelJson != null)
                {
                    return JsonConvert.DeserializeObject<TaskModel>(taskModelJson);
                }
            }
            return null;
        }
    }
}


