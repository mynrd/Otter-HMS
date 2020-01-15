using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Repository.Providers.EntityFramework
{
    public class DataContext : DbContext, IDataContext
    {
        private readonly Guid _instanceId;

        public DataContext()
        {
            _instanceId = Guid.NewGuid();
        }

        public DataContext(DbContextOptions options)
            : base(options)
        {
            _instanceId = Guid.NewGuid();
        }

        public Guid InstanceId
        {
            get { return _instanceId; }
        }

        public new DbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public override int SaveChanges()
        {
            SyncObjectsStatePreCommit();
            var changes = base.SaveChanges();
            SyncObjectsStatePostCommit();
            return changes;
        }

        public Task<int> SaveChangesAsync()
        {
            SyncObjectsStatePreCommit();
            Task<int> changesAsync = base.SaveChangesAsync(default);
            changesAsync.ContinueWith(a =>
            {
                SyncObjectsStatePostCommit();
            });
            return changesAsync;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            SyncObjectsStatePreCommit();
            Task<int> changesAsync = base.SaveChangesAsync(cancellationToken);
            SyncObjectsStatePostCommit();
            return changesAsync;
        }

        public void SyncObjectState(object entity)
        {
            Entry(entity).State = StateHelper.ConvertState(((IObjectState)entity).ObjectState);
        }

        private void SyncObjectsStatePreCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
                dbEntityEntry.State = StateHelper.ConvertState(((IObjectState)dbEntityEntry.Entity).ObjectState);
        }

        private void SyncObjectsStatePostCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
                ((IObjectState)dbEntityEntry.Entity).ObjectState = StateHelper.ConvertState(dbEntityEntry.State);
        }

        //public virtual int ExecuteQuery(string query)
        //{
        //    var cmd = new SqlCommand();
        //    var con = new SqlConnection(this.Database.Connection.ConnectionString);
        //    if (con.State != System.Data.ConnectionState.Open) con.Open();
        //    cmd.Connection = con;
        //    cmd.CommandText = query;
        //    return cmd.ExecuteNonQuery();
        //}

        //public virtual int ExecuteQuery(SqlCommand cmd)
        //{
        //    var con = this.Database.Connection as SqlConnection;
        //    if (con.State != System.Data.ConnectionState.Open) con.Open();
        //    cmd.Connection = con;
        //    return cmd.ExecuteNonQuery();
        //}

        public IEnumerable<EntityEntry> GetEntries()
        {
            return ChangeTracker.Entries().Where(x => new[] { EntityState.Added, EntityState.Modified, EntityState.Deleted, }.Contains(x.State));
        }
    }
}