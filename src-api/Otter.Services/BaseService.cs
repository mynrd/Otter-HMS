using Otter.Core;
using Otter.Core.Account;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Otter.Services
{
    public class BaseService
    {
        protected ICurrentUser _currentUser;

        public BaseService(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        protected TEntity PrepareEntity<TEntity>(TEntity entity) where TEntity : EntityBase
        {
            entity.LastUpdatedBy = _currentUser.UserId.ToInt();
            entity.LastUpdatedDate = DateTime.UtcNow;
            return entity;
        }
    }
}