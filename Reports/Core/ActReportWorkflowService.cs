using GesCPSI_Project.Interfaces;
using GesCPSI_Project.Models;

namespace GesCPSI_Project.Reports
{
    public class ActReportWorkflowService
    {
        private readonly ActReportJsonService _jsonService;
        private readonly ActReportPdfService _pdfService;
        private readonly ITypesAct _typesActService;

        public ActReportWorkflowService(
            ActReportJsonService jsonService,
            ActReportPdfService pdfService,
            ITypesAct typesActService)
        {
            _jsonService = jsonService;
            _pdfService = pdfService;
            _typesActService = typesActService;
        }

        public async Task<(string jsonPath, string pdfPath)> GenerateAndSaveAsync(int acteId)
        {
            var acte = await _typesActService.GetByIdAsync(acteId);
            if (acte is null)
                throw new Exception("Acte introuvable.");

            var jsonPath = await _jsonService.GenerateJsonFileAsync(acteId);
            var pdfPath = await _pdfService.GeneratePdfAsync(acteId);

            acte.JsonSnapshotPath = ToRelativeWebPath(jsonPath);
            acte.PdfGenerePath = ToRelativeWebPath(pdfPath);
            acte.DateGenerationPdf = DateTime.UtcNow;

            var result = await _typesActService.UpdateAsync(acte);
            if (!result.IsSuccess)
                throw new Exception(result.ErrorMessage ?? "Impossible de mettre à jour l'acte.");

            return (acte.JsonSnapshotPath!, acte.PdfGenerePath!);
        }

        private static string ToRelativeWebPath(string fullPath)
        {
            var normalized = fullPath.Replace("\\", "/");
            var marker = "/wwwroot/";

            var index = normalized.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                return "/" + normalized[(index + marker.Length)..];
            }

            return normalized;
        }
    }
}