namespace Catalog.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync();
    }
}