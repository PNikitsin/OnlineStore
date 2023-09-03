using Catalog.Domain.Entities.Mongo;

namespace Catalog.Domain.Interfaces
{
    public interface IReportRepository
    {
        Task<IEnumerable<Report>> GetAllAsync();
        Task<Report> GetByIdAsync(string id);
        Task CreateAsync(Report report);
        Task UpdateAsync(string id, Report report);
        Task DeleteAsync(string id);
    }
}