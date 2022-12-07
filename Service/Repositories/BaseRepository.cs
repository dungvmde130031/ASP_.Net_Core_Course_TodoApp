using System;
using Microsoft.EntityFrameworkCore;
using Service.Database;

namespace Service.Repositories
{
	public interface IBaseRepository<T> where T : class
	{
        T Get(Guid id);

        List<T> GetList();

        T Add(T entity);

        T Update(T taskEntity);

        List<T> AddRange(List<T> taskEntities);

    }


    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private ApplicationDbContext _dbContext;

        public BaseRepository(ApplicationDbContext dbContext)
		{
            _dbContext = dbContext;
        }

        public T Get(Guid id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public List<T> GetList()
        {
            return _dbContext.Set<T>().ToList();
        }

        public T Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);

            return entity;
        }

        public List<T> AddRange(List<T> taskEntities)
        {
            _dbContext.Set<T>().AddRange(taskEntities);

            return taskEntities;
        }

        //public bool Delete(int id)
        //{
        //    var current = Get(id);

        //    var removedEntity = _dbContext.Set<T>().Remove(current);

        //    return removedEntity.State == EntityState.Deleted;
        //}

        public T Update(T taskEntity)
        {
            return _dbContext.Set<T>().Update(taskEntity).Entity;
        }
    }
}

