using System;
using Service.Database;
using Service.UnitOfWork;

namespace Service.Service
{
	public interface ITaskService
	{
        Database.Task GetTask(Guid id);

        Database.Task AddTask(Database.Task taskEntity);

        Database.Task UpdateTask(Database.Task task);

        bool CompleteTask(Guid id);

        bool DeleteTask(Guid id);

        bool ReopenTask(Guid id);

        List<Database.Task> GetAllTasks();
	}

	public class TaskService : ITaskService
	{
        private readonly IUnitOfWork _unitOfWork;

        public TaskService(IUnitOfWork unitOfWork)
		{
            _unitOfWork = unitOfWork;
        }

        public Database.Task GetTask(Guid id)
        {
            return _unitOfWork.TaskRepository.Get(id);
        }

        public List<Database.Task> GetAllTasks()
        {
            return _unitOfWork.TaskRepository.GetList();
        }

        // Add Task Service
        public Database.Task AddTask(Database.Task taskEntity)
        {
            var entity = _unitOfWork.TaskRepository.Add(taskEntity);
            _unitOfWork.Complete();

            return entity;
        }

        // Update Task Service
        public Database.Task UpdateTask(Database.Task task)
        {
            var entity = _unitOfWork.TaskRepository.Update(task);
            _unitOfWork.Complete();

            return entity;
        }

        // Delete Task Service
        public bool DeleteTask(Guid id)
        {
            return _unitOfWork.TaskRepository.DeleteTask(id);
        }

        public bool CompleteTask(Guid id)
        {
            return _unitOfWork.TaskRepository.CompleteTask(id);
        }

        public bool ReopenTask(Guid id)
        {
            return _unitOfWork.TaskRepository.ReopenTask(id);
        }

        public Database.Task SaveTask(Database.Task taskEntity)
        {
            if (taskEntity.Id > 0)
            {
                _unitOfWork.TaskRepository.Update(taskEntity);
            }
            else
            {
                _unitOfWork.TaskRepository.Add(taskEntity);
            }

            _unitOfWork.Complete();

            return taskEntity;
        }

        
    }
}

