using System;
using Service.Database;

namespace Service.Service
{
	public interface IAccountService
	{

	}

	public class AccountService : IAccountService
    {
        private ApplicationDbContext _dbContext;

        public AccountService(ApplicationDbContext dbContext)
		{
            _dbContext = dbContext;
        }
	}
}

