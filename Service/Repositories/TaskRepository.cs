using System;
using Service.Database;

namespace Service.Repositories
{
    
	public interface ITaskRepository : IBaseRepository<Database.Task>
    {
        bool CompleteTask(Guid id);

        bool DeleteTask(Guid id);

        bool ReopenTask(Guid id);

        List<Database.Task> SearchTasks(string keyword);
    }
    

    public class TaskRepository : BaseRepository<Database.Task>, ITaskRepository
	{
		public TaskRepository(ApplicationDbContext dbContext) : base (dbContext)
		{
            
		}

        public bool CompleteTask(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool DeleteTask(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool ReopenTask(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Database.Task> SearchTasks(string keyword)
        {
            throw new NotImplementedException();
        }
    }
}

