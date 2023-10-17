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
        private readonly IBackgroundJobClient _backgroundJobClient;

        public ReportService(
            IReportRepository reportRepository,
            IHttpContextAccessor contextAccessor,
            IMapper mapper,
            ICacheRepository cacheRepository,
            IBackgroundJobClient backgroundJobClient)
        {
            _reportRepository = reportRepository;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
            _cacheRepository = cacheRepository;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<IEnumerable<OutputReportDto>> GetReportsAsync()
        {
            var reportsCache = await _cacheRepository.GetDataAsync<IEnumerable<Report>>("report");

            if (reportsCache != null)
            {
                return _mapper.Map<IEnumerable<OutputReportDto>>(reportsCache);
            }

            var reports = await _reportRepository.GetAllAsync();

            _backgroundJobClient.Enqueue(() => _cacheRepository.SetDataAsync("report", reports));

            return _mapper.Map<IEnumerable<OutputReportDto>>(reports);
        }

        public async Task<OutputReportDto> GetReportAsync(string id)
        {
            Report report = new();

            var reportsCache = await _cacheRepository.GetDataAsync<IEnumerable<Report>>("report");

            if (reportsCache != null)
            {
                report = reportsCache.FirstOrDefault(report => report.Id == id)!;
            }

            var reportResult = report is null ? await _reportRepository.GetByIdAsync(id) : report;

            if (reportResult == null)
            {
                throw new NotFoundException("Report not found");
            }

            return _mapper.Map<OutputReportDto>(reportResult);
        }

        public async Task<OutputReportDto> CreateReport(InputReportDto inputReportDto)
        {
            var report = _mapper.Map<Report>(inputReportDto);

            report.Username = _contextAccessor.HttpContext!.User.Identity!.Name!;
            report.CreatedAt = DateTime.UtcNow;

            await _reportRepository.CreateAsync(report);

            _backgroundJobClient.Enqueue(() => _cacheRepository.RemoveAsync("report"));

            return _mapper.Map<OutputReportDto>(report);
        }

        public async Task DeleteReportAsync(string id)
        {
            var report = _reportRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Report not found");

            await _reportRepository.DeleteAsync(id);

            _backgroundJobClient.Enqueue(() => _cacheRepository.RemoveAsync("report"));
        }
    }
}