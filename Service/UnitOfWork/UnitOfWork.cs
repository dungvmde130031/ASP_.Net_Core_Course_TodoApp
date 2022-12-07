using System;
using Service.Database;
using Service.Repositories;

namespace Service.UnitOfWork
{
	public interface IUnitOfWork
	{
        ITaskRepository TaskRepository { get; }

        IAccountRepository AccountRepository { get; }

        int Complete();

		void Dispose();
	}

    public class UnitOfWork : IUnitOfWork
    {
		private readonly ApplicationDbContext _dbContext;
		public UnitOfWork(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;

			TaskRepository = new TaskRepository(dbContext);
            AccountRepository = new AccountRepository(dbContext);
        }

		public ITaskRepository TaskRepository { get; set; }

		public IAccountRepository AccountRepository { get; set; }

		public int Complete()
		{
			return _dbContext.SaveChanges();
		}

		public void Dispose()
		{
			_dbContext.Dispose();
		}
    }
}

