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

        [HttpPost]
        public async Task<IActionResult> CreateReportAsync([FromBody] CreateReportDto createReportDto)
        {
            var report = await _reportService.CreateReport(createReportDto);

            return Ok(report);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetReportsAsync()
        {
            var reports = await _reportService.GetReportsAsync();

            return Ok(reports);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetReportAsync(string id)
        {
            var report = await _reportService.GetReportAsync(id);

            return Ok(report);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteReportAsync(string id)
        {
            await _reportService.DeleteReportAsync(id);

            return NoContent();
        }
    }
}