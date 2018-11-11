using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MySQLDataProviderPlugin.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity>
       where TEntity : class, new()
    {
        protected readonly DbContext _context;

        public long Count => _context.Set<TEntity>().Count();

        public Repository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected virtual IQueryable<TEntity> Include()
        {
            return _context.Set<TEntity>();
        }

        public virtual int Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.Set<TEntity>().Add(entity);
            return _context.SaveChanges();
        }

        public virtual async Task<int> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _context.Set<TEntity>().AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        public virtual int AddRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().AddRange(entities);
            return _context.SaveChanges();
        }

        public virtual async Task<int> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
            return await _context.SaveChangesAsync();
        }

        public virtual void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // If entity is not being tracked by the context
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Set<TEntity>().Attach(entity);
                _context.Entry<TEntity>(entity).State = EntityState.Modified;
            }
            else // If entity is being tracked by the context
            {
                _context.Set<TEntity>().Update(entity);
            }

            _context.SaveChanges();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // If entity is not being tracked by the context
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Set<TEntity>().Attach(entity);
                _context.Entry<TEntity>(entity).State = EntityState.Modified;
            }
            else // If entity is being tracked by the context
            {
                _context.Set<TEntity>().Update(entity);
            }

            await _context.SaveChangesAsync();
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Include().Where(predicate).ToList();
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, int page, int pageSize)
        {
            return Include().Where(predicate).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public IEnumerable<TProjection> Get<TProjection>(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).ProjectTo<TProjection>().ToList();
        }

        public IEnumerable<TProjection> Get<TProjection>(Expression<Func<TEntity, bool>> predicate, int page, int pageSize)
        {
            return _context.Set<TEntity>().Where(predicate).Skip((page - 1) * pageSize).Take(pageSize).ProjectTo<TProjection>().ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Include().Where(predicate).ToListAsync();
        }

        public virtual async Task<IEnumerable<TProjection>> GetAsync<TProjection>(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().Where(predicate).ProjectTo<TProjection>().ToListAsync();
        }

        public virtual async Task<IEnumerable<TProjection>> GetAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, int page, int pageSize)
        {
            return await _context.Set<TEntity>().Where(predicate).ProjectTo<TProjection>().Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public virtual TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Include().SingleOrDefault(predicate);
        }

        public virtual TProjection SingleOrDefault<TProjection>(Expression<Func<TEntity, bool>> predicate, object parameters = null)
        {
            IQueryable<TProjection> projection;
            var query = _context.Set<TEntity>().Where(predicate);

            if (parameters != null)
            {
                projection = query.ProjectTo<TProjection>(parameters);
            }
            else
            {
                projection = query.ProjectTo<TProjection>();
            }

            return projection.SingleOrDefault();
        }

        public virtual async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Include().SingleOrDefaultAsync(predicate);
        }

        public virtual async Task<TProjection> SingleOrDefaultAsync<TProjection>(Expression<Func<TEntity, bool>> predicate, object parameters = null)
        {
            IQueryable<TProjection> projection;
            var query = _context.Set<TEntity>().Where(predicate);

            if (parameters != null)
            {
                projection = query.ProjectTo<TProjection>(parameters);
            }
            else
            {
                projection = query.ProjectTo<TProjection>();
            }

            return await projection.SingleOrDefaultAsync();
        }

        public virtual IEnumerable<TEntity> All()
        {
            return Include().ToList();
        }

        public virtual IEnumerable<TProjection> All<TProjection>()
        {
            return _context.Set<TEntity>().ProjectTo<TProjection>().ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> AllAsync()
        {
            return await Include().ToListAsync();
        }

        public virtual async Task<IEnumerable<TProjection>> AllAsync<TProjection>()
        {
            return await _context.Set<TEntity>().ProjectTo<TProjection>().ToListAsync();
        }

        public virtual int Remove(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.Set<TEntity>().Remove(entity);
            return _context.SaveChanges();
        }

        public virtual async Task<int> RemoveAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.Set<TEntity>().Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public virtual int RemoveRange(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
            return _context.SaveChanges();
        }

        public virtual async Task<int> RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
            return await _context.SaveChangesAsync();
        }

        public void DetachAllEntities()
        {
            foreach (var entity in _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted))
            {
                _context.Entry(entity.Entity).State = EntityState.Detached;
            }
        }
    }
}
