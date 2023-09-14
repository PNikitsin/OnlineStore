using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Exceptions;
using Catalog.Application.Services.Interfaces;
using Catalog.Domain.Entities.Mongo;
using Catalog.Domain.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;
        private readonly ICacheRepository _cacheRepository;

        public ReportService(
            IReportRepository reportRepository,
            IHttpContextAccessor contextAccessor,
            IMapper mapper,
            ICacheRepository cacheRepository)
        {
            _reportRepository = reportRepository;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
            _cacheRepository = cacheRepository;
        }

        public async Task<IEnumerable<Report>> GetReportsAsync()
        {
            var reports = await _cacheRepository.GetDataAsync<IEnumerable<Report>>("report");

            if (reports != null)
            {
                return reports;
            }

            reports = await _reportRepository.GetAllAsync();

            BackgroundJob.Enqueue(() => _cacheRepository.SetDataAsync("report", reports));

            return reports;
        }

        public async Task<Report> GetReportAsync(string id)
        {
            Report report = new();
            var reports = await _cacheRepository.GetDataAsync<IEnumerable<Report>>("report");

            if (reports != null)
            {
                report = reports.FirstOrDefault(report => report.Id == id)!;
            }

            var resultReport = report is null ? await _reportRepository.GetByIdAsync(id) : report;

            return resultReport ?? throw new NotFoundException("Report not found");
        }

        public async Task<Report> CreateReport(CreateReportDto createReportDto)
        {
            var report = _mapper.Map<CreateReportDto, Report>(createReportDto);

            report.Username = _contextAccessor.HttpContext.User.Identity.Name;
            report.CreatedAt = DateTime.UtcNow;

            await _reportRepository.CreateAsync(report);

            BackgroundJob.Enqueue(() => _cacheRepository.RemoveAsync("report"));

            return report;
        }

        public async Task DeleteReportAsync(string id)
        {
            var report = _reportRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Report not found");

            await _reportRepository.DeleteAsync(id);

            BackgroundJob.Enqueue(() => _cacheRepository.RemoveAsync("report"));
        }
    }
}