using System;

namespace Repository
{
    public interface IUnitOfWork
    {
        void Dispose();

        void Dispose(bool disposing);

        Guid InstanceId { get; }

        void Save();

        System.Threading.Tasks.Task<int> SaveAsync();

        System.Threading.Tasks.Task<int> SaveAsync(System.Threading.CancellationToken cancellationToken);
    }
}
