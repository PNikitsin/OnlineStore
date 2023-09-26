using AutoFixture;
using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Services.Implementations;
using Catalog.Domain.Entities.Mongo;
using Catalog.Domain.Interfaces;
using FluentAssertions;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Moq;

namespace OnlineStore.Tests.Catalog.UnitTests.Services
{
    public class ReportServiceTests
    {
        private readonly Mock<IReportRepository> _reportRepositoryMock = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ICacheRepository> _cacheRepositoryMock = new();
        private readonly Mock<IBackgroundJobClient> _backgroundJobClientMock = new();
        private readonly Fixture _fixture = new();
        private readonly ReportService _reportService;

        public ReportServiceTests()
        {
            _reportService = new ReportService(
                _reportRepositoryMock.Object,
                _httpContextAccessorMock.Object,
                _mapperMock.Object,
                _cacheRepositoryMock.Object,
                _backgroundJobClientMock.Object);

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetReportsWithСache_ShouldReturnReports()
        {
            // Arrange
            var reportCache = _fixture.CreateMany<Report>();

            _cacheRepositoryMock.Setup(_cacheRepositoryMock =>
                _cacheRepositoryMock.GetDataAsync<IEnumerable<Report>>("report"))
                    .ReturnsAsync(reportCache);

            // Act
            var result = await _reportService.GetReportsAsync();

            // Assert
            result.Should().BeEquivalentTo(reportCache);
        }

        [Fact]
        public async Task GetReportsWithoutСache_ShouldReturnReports()
        {
            // Arrange
            var reportsData = _fixture.CreateMany<Report>();
            IEnumerable<Report>? reportsCache = null;

            _cacheRepositoryMock.Setup(_cacheRepositoryMock =>
                _cacheRepositoryMock.GetDataAsync<IEnumerable<Report>?>("report"))
                    .ReturnsAsync(reportsCache);

            _reportRepositoryMock.Setup(_reportRepositoryMock =>
                _reportRepositoryMock.GetAllAsync())
                    .ReturnsAsync(reportsData);

            // Act
            var result = await _reportService.GetReportsAsync();

            // Assert
            result.Should().BeEquivalentTo(reportsData);
        }

        [Fact]
        public async Task GetReportById_ShouldReturnReport()
        {
            // Arrange
            var firstReport = _fixture.Create<Report>();
            var secondReport = _fixture.Create<Report>();

            var reportsCach = new List<Report>
            {
                firstReport,
                secondReport
            };

            _cacheRepositoryMock.Setup(_cacheRepositoryMock =>
                _cacheRepositoryMock.GetDataAsync<IEnumerable<Report>>("report"))
                    .ReturnsAsync(reportsCach);

            // Act
            var result = await _reportService.GetReportAsync(firstReport.Id);

            // Assert
            result.Should().BeEquivalentTo(firstReport);
        }

        [Fact]
        public async Task CreateReport_ShouldReturnCreatedReport()
        {
            var createReportDto = _fixture.Create<CreateReportDto>();

            Report createdReport = new();

            _mapperMock.Setup(_mapperMock =>
                _mapperMock.Map<CreateReportDto, Report>(createReportDto))
                    .Returns(createdReport);

            _httpContextAccessorMock.Setup(_httpContextAccessorMock =>
                _httpContextAccessorMock.HttpContext.User.Identity.Name)
                    .Returns("testName");

            // Act
            var result = await _reportService.CreateReport(createReportDto);

            // Assert
            result.Should().BeEquivalentTo(createdReport);
        }

        [Fact]
        public async Task DeleteReport_ShouldDeleteReport()
        {
            // Arrange
            var id = _fixture.Create<string>();
            var report = _fixture.Create<Report>();

            _reportRepositoryMock.Setup(_reportRepositoryMock =>
                _reportRepositoryMock.GetByIdAsync(id))
                    .ReturnsAsync(report);

            // Act
            var result = async () => await _reportService.DeleteReportAsync(id);

            // Assert
            await result.Should().NotThrowAsync<Exception>();
        }
    }
}