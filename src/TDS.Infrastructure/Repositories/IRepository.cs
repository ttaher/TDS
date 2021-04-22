using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Infrastructure.Models;

namespace TDS.Infrastructure.Repositories
{
    public interface IRepository<TEntity, TKey>
    where TEntity : EntitytBase<TKey>

    {
        Task<TEntity> InsertAsync(TEntity entity);
        Task InsertAsync(List<TEntity> entities);

        IQueryable<TEntity> GetAll();

        Task<TEntity> GetAsync(Guid Id);

        Task UpdateAsync(TEntity entity);

        Task<int> SaveChangesAsync();

    }
}
