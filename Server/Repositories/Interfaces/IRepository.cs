using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        long Count { get; }

        int Add(TEntity entity);

        Task<int> AddAsync(TEntity entity);

        int AddRange(IEnumerable<TEntity> entities);

        Task<int> AddRangeAsync(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        Task UpdateAsync(TEntity entity);

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, int page, int pageSize);

        IEnumerable<TProjection> Get<TProjection>(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TProjection> Get<TProjection>(Expression<Func<TEntity, bool>> predicate, int page, int pageSize);

        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TProjection>> GetAsync<TProjection>(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TProjection>> GetAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, int page, int pageSize);

        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        TProjection SingleOrDefault<TProjection>(Expression<Func<TEntity, bool>> predicate, object parameters = null);

        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TProjection> SingleOrDefaultAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, object parameters = null);

        IEnumerable<TEntity> All();

        IEnumerable<TProjection> All<TProjection>();

        Task<IEnumerable<TEntity>> AllAsync();

        Task<IEnumerable<TProjection>> AllAsync<TProjection>();

        int Remove(TEntity entity);

        Task<int> RemoveAsync(TEntity entity);

        int RemoveRange(IEnumerable<TEntity> entities);

        Task<int> RemoveRangeAsync(IEnumerable<TEntity> entities);
    }
}
