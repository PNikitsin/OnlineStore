namespace Ordering.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IOrderRepository Orders { get; }
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync();
    }
}