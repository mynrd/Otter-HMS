using Microsoft.EntityFrameworkCore;
using Repository.Providers.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        private readonly IDataContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly Guid _instanceId;

        public Repository(IDataContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _instanceId = Guid.NewGuid();
        }

        public Guid InstanceId
        {
            get { return _instanceId; }
        }

        #region Create/Delete/Insert

        public virtual TEntity Insert(TEntity entity)
        {
            ((IObjectState)entity).ObjectState = ObjectState.Added;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
            return entity;
        }

        public virtual void Delete(TEntity entity)
        {
            ((IObjectState)entity).ObjectState = ObjectState.Deleted;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
        }

        public virtual void Delete(params object[] keyValues)
        {
            var ent = _dbSet.Find(keyValues);
            this.Delete(ent);
        }

        public virtual void Update(TEntity entity)
        {
            ((IObjectState)entity).ObjectState = ObjectState.Modified;
            _dbSet.Attach(entity);
            _context.SyncObjectState(entity);
        }

        #endregion Create/Delete/Insert

        #region Find

        public virtual TEntity Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public virtual async Task<TEntity> FindAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues);
        }

        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await _dbSet.FindAsync(cancellationToken, keyValues);
        }

        #endregion Find

        public IDataContext GetContext()
        {
            return _context;
        }

        public virtual RepositoryQuery<TEntity> Query()
        {
            var repoHelper = new RepositoryQuery<TEntity>(this);
            return repoHelper;
        }

        internal IQueryable<TEntity> Get(
            List<Expression<Func<TEntity, bool>>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includeProperties != null)
                includeProperties.ForEach(i => query = query.Include(i));

            if (filter != null)
                filter.ForEach(f =>
                {
                    query = query.Where(f);
                });

            if (orderBy != null)
                query = orderBy(query);

            if (page != null && pageSize != null)
                query = query
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);

            return query;
        }

        internal async Task<IQueryable<TEntity>> GetAsync(
            List<Expression<Func<TEntity, bool>>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null)
        {
            var result = await Task.Run(() =>
            {
                return Get(filter, orderBy, includeProperties, page, pageSize);
            });

            return result;
        }
    }
}