using Repository.Providers.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        Guid InstanceId { get; }

        void Update(TEntity entity);
        void Delete(params object[] keyValues);
        void Delete(TEntity entity);
        TEntity Find(params object[] keyValues);
        Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
        Task<TEntity> FindAsync(params object[] keyValues);
        IDataContext GetContext();
        TEntity Insert(TEntity entity);
        RepositoryQuery<TEntity> Query();
    }
}