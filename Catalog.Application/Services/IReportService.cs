using Catalog.Application.DTOs;
using Catalog.Domain.Entities.Mongo;

namespace Catalog.Application.Services
{
    public interface IReportService
    {
        Task<Report> CreateReport(CreateReportDto createReportDto);
        Task<IEnumerable<Report>> GetReportsAsync();
        Task<Report> GetReportAsync(string id);
        Task DeleteReportAsync(string id);
    }
}