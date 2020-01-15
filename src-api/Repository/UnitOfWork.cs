#region

using Repository.Providers.EntityFramework;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Repository
{
    public class UnitOfWork : Repository.IUnitOfWork
    {
        #region Private Fields

        //private readonly IAuditDataService _auditDataService;
        private readonly IDataContext _context;
        private readonly Guid _instanceId;
        private bool _disposed;

        #endregion Private Fields

        #region Constuctor/Dispose

        public UnitOfWork(IDataContext context
            //, IAuditDataService auditDataService
            )
        {
            _context = context;
            _instanceId = Guid.NewGuid();
            //_auditDataService = auditDataService;
        }

        public virtual Guid InstanceId
        {
            get { return _instanceId; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        #endregion Constuctor/Dispose

        public virtual void Save()
        {
            SaveAuditData();
            _context.SaveChanges();
        }

        public virtual Task<int> SaveAsync()
        {
            SaveAuditData();
            return _context.SaveChangesAsync();
        }

        public virtual Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        private void SaveAuditData()
        {
            //try
            //{
            //    Guid sessionId;
            //    if (Guid.TryParse(System.Web.HttpContext.Current.User.Identity.Name, out sessionId))
            //    {
            //        _auditDataService.SaveData(this._context.GetEntries(), sessionId);
            //    }
            //    else
            //    {
            //        _auditDataService.SaveData(this._context.GetEntries(), null);
            //    }
            //}
            //catch { }
        }
    }
}
