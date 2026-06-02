using FastReport;
using FastReport.Export.PdfSimple;

namespace GesCPSI_Project.Reports
{
    public class ActReportPdfService
    {
        private readonly ActReportDataBuilder _builder;
        private readonly IWebHostEnvironment _env;

        public ActReportPdfService(
            ActReportDataBuilder builder,
            IWebHostEnvironment env)
        {
            _builder = builder;
            _env = env;
        }

        public async Task<string> GeneratePdfAsync(int acteId)
        {
            var payload = await BuildPayloadAsync(acteId);

            var reportPath = Path.Combine(
                _env.ContentRootPath,
                "Reports",
                "Templates",
                "ACTE DE CPSI ENGAG SPECIFIQ PARTICULIER-VF .frx");

            if (!File.Exists(reportPath))
                throw new FileNotFoundException("Fichier rapport introuvable.", reportPath);

            var outputFolder = Path.Combine(_env.WebRootPath, "uploads", "actes", "pdf");
            Directory.CreateDirectory(outputFolder);

            var outputFile = Path.Combine(
                outputFolder,
                $"acte_{acteId}_{DateTime.Now:yyyyMMddHHmmss}.pdf");

            using var report = new Report();
            report.Load(reportPath);

            report.RegisterData(payload, "Data");

            var dataSource = report.GetDataSource("Data");
            if (dataSource == null)
                throw new Exception("La source 'Data' est introuvable dans le template FastReport.");

            dataSource.Enabled = true;

            report.Prepare();

            using var export = new PDFSimpleExport();
            report.Export(export, outputFile);

            return outputFile;
        }

        private async Task<List<ActCautionnementReportModel>> BuildPayloadAsync(int acteId)
        {
            var model = await _builder.BuildAsync(acteId);
            return new List<ActCautionnementReportModel> { model };
        }
    }
}