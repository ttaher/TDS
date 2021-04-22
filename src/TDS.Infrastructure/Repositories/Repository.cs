using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Infrastructure.Data.Context;
using TDS.Infrastructure.Models;

namespace TDS.Infrastructure.Repositories
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
    where TEntity : EntitytBase<TKey>
    {
        private TDSDbContext _dbContext;
        public Repository(TDSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> GetAsync(Guid Id)
        {
            try
            {
                return await _dbContext.Set<TEntity>().FindAsync(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            try
            {
                entity.CreatedAt = DateTime.UtcNow;
                var result = await _dbContext.Set<TEntity>().AddAsync(entity);
                return result.Entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task InsertAsync(List<TEntity> entities)
        {
            try
            {
                entities.ForEach(e => e.CreatedAt = DateTime.UtcNow);
                await _dbContext.Set<TEntity>().AddRangeAsync(entities);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return _dbContext.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task UpdateAsync(TEntity entity)
        {
            try
            {
                entity.UpdateAt = DateTime.UtcNow;
                await Task.FromResult(_dbContext.Update(entity));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
