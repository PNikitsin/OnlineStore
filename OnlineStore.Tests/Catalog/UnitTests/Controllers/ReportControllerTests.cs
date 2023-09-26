using AutoFixture;
using Catalog.Application.DTOs;
using Catalog.Application.Services.Interfaces;
using Catalog.Domain.Entities.Mongo;
using Catalog.Web.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace OnlineStore.Tests.Catalog.UnitTests.Controllers
{
    public class ReportControllerTests
    {
        private readonly Mock<IReportService> _reportServiceMock = new();
        private readonly Fixture _fixture = new();
        private readonly ReportController _controller;

        public ReportControllerTests()
        {
            _controller = new(_reportServiceMock.Object);

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetReports_ShouldReturnOkObjectResultWithReports()
        {
            // Arrange
            var reports = _fixture.CreateMany<Report>();

            _reportServiceMock.Setup(_reportServiceMock =>
                _reportServiceMock.GetReportsAsync())
                    .ReturnsAsync(reports);

            // Act
            var result = await _controller.GetReportsAsync();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(reports);
        }

        [Fact]
        public async Task GetReportById_ShouldReturnOkObjectResultWithReport()
        {
            // Arrange 
            var id = _fixture.Create<string>();
            var product = _fixture.Create<Report>();

            _reportServiceMock.Setup(_reportServiceMock =>
                _reportServiceMock.GetReportAsync(id))
                    .ReturnsAsync(product);

            // Act
            var result = await _controller.GetReportAsync(id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(product);
        }

        [Fact]
        public async Task CreateReport_ShouldReturnOkObjectResultWithCreatedReport()
        {
            // Arrange
            var report = _fixture.Create<Report>();
            var createReportDto = _fixture.Create<CreateReportDto>();

            _reportServiceMock.Setup(_reportServiceMock =>
                _reportServiceMock.CreateReport(createReportDto))
                    .ReturnsAsync(report);

            // Act
            var result = await _controller.CreateReportAsync(createReportDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(report);
        }

        [Fact]
        public async Task DeleteReport_ShouldReturnNoContentResult()
        {
            // Arrange 
            var id = _fixture.Create<string>();

            // Act
            var result = await _controller.DeleteReportAsync(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}