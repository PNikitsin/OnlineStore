using Catalog.Application.DTOs;
using Catalog.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetReports()
        {
            var reports = await _reportService.GetReportsAsync();

            return Ok(reports);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetReport(string id)
        {
            var report = await _reportService.GetReportAsync(id);

            return Ok(report);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] CreateReportDto createReportDto)
        {
            var report = await _reportService.CreateReport(createReportDto);

            var actionName = nameof(GetReport);
            var routeValue = new { id = report.Id };

            return CreatedAtAction(actionName, routeValue, report);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteReport(string id)
        {
            await _reportService.DeleteReportAsync(id);

            return NoContent();
        }
    }
}