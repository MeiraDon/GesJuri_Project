using GesCPSI_Project.Reports;
using Microsoft.AspNetCore.Mvc;


namespace GesCPSI_Project.Controllers
{
    [ApiController]
    [Route("api/report-test")]
    public class ReportTestController : ControllerBase
    {

        private readonly ActReportJsonService _jsonService;
        private readonly ActReportPdfService _pdfService;
        public ReportTestController(ActReportJsonService jsonService,
            ActReportPdfService pdfService)
        {
            _jsonService = jsonService;
            _pdfService = pdfService;
        }

        [HttpGet("test-simple")]
        public IActionResult TestSimple()
        {
            return Ok(new
            {
                success = true,
                message = "controller ok"
            });
        }

        [HttpGet("{acteId}/json-text")]
        public async Task<IActionResult> GetJsonText(int acteId)
        {
            try
            {
                var json = await _jsonService.GetJsonContentAsync(acteId);
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    details = ex.InnerException?.Message
                });
            }
        }

        [HttpGet("{acteId}/json-file")]
        public async Task<IActionResult> GenerateJsonFile(int acteId)
        {
            try
            {
                var path = await _jsonService.GenerateJsonFileAsync(acteId);
                return Ok(new
                {
                    success = true,
                    filePath = path
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    details = ex.InnerException?.Message
                });
            }
        }

        [HttpGet("{acteId}/pdf-file")]
        public async Task<IActionResult> GeneratePdfFile(int acteId)
        {
            try
            {
                var path = await _pdfService.GeneratePdfAsync(acteId);
                return Ok(new
                {
                    success = true,
                    filePath = path
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    details = ex.InnerException?.Message
                });
            }
        }

    }
}
