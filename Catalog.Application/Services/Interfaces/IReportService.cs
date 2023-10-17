using Catalog.Application.DTOs;

namespace Catalog.Application.Services.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<OutputReportDto>> GetReportsAsync();
        Task<OutputReportDto> GetReportAsync(string id);
        Task<OutputReportDto> CreateReport(InputReportDto createReportDto);
        Task DeleteReportAsync(string id);
    }
}