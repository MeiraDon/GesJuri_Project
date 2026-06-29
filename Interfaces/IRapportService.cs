using GesCPSI_Project.Models;

namespace GesCPSI_Project.Interfaces
{
    public interface IRapportService
    {
        Task<RapportActiviteDto> GenerateActivityReportAsync(
            DateTime dateDebut,
            DateTime dateFin,
            string userRole,
            int? restrictToAgentId = null,
            string generatedByEmail = "");

        Task<byte[]> ExportToPdfAsync(RapportActiviteDto rapport);
        Task<byte[]> ExportToExcelAsync(RapportActiviteDto rapport);
        Task<byte[]> ExportToWordAsync(RapportActiviteDto rapport);
    }
}