using System;
using System.Security.Principal;
using Service.Database;

namespace Service.Repositories
{

	public interface IAccountRepository : IBaseRepository<Account>
	{
    }

    public class AccountRepository : BaseRepository<Account>, IAccountRepository
	{
        public AccountRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            
        }

        public Account Add(Database.Task entity)
        {
            throw new NotImplementedException();
        }

        public List<Account> AddRange(List<Account> taskEntities)
        {
            throw new NotImplementedException();
        }

        public List<Database.Task> AddRange(List<Database.Task> taskEntities)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Account Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Account> GetList()
        {
            throw new NotImplementedException();
        }

        public Account Update(Account taskEntity)
        {
            throw new NotImplementedException();
        }

        public Database.Task Update(Database.Task taskEntity)
        {
            throw new NotImplementedException();
        }

        Database.Task IBaseRepository<Database.Task>.Add(Database.Task entity)
        {
            throw new NotImplementedException();
        }

        List<Database.Task> IBaseRepository<Database.Task>.GetList()
        {
            throw new NotImplementedException();
        }
    }
}

