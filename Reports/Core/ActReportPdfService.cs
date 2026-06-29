using FastReport;
using FastReport.Export.PdfSimple;
using GesCPSI_Project.Data;
using GesCPSI_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace GesCPSI_Project.Reports
{
    public class ActReportPdfService
    {
        private readonly ActReportDataBuilder _builder;
        private readonly IWebHostEnvironment _env;
        private readonly IDbContextFactory<GesDbContext> _dbFactory;

        public ActReportPdfService(
            ActReportDataBuilder builder,
            IWebHostEnvironment env,
            IDbContextFactory<GesDbContext> dbFactory)
        {
            _builder = builder;
            _env = env;
            _dbFactory = dbFactory;
        }

        public async Task<string> GeneratePdfAsync(int acteId)
        {
            // 🔧 ÉTAPE 1 : Récupérer le TemplateType de l'acte
            var templateType = await GetTemplateTypeForActeAsync(acteId);

            // 🔧 ÉTAPE 2 : Choisir le bon fichier .frx selon le TemplateType
            var templateFileName = GetTemplateFileName(templateType);

            // ÉTAPE 3 : Construire le chemin complet du template
            var reportPath = Path.Combine(
                _env.ContentRootPath,
                "Reports",
                "Templates",
                templateFileName);

            if (!File.Exists(reportPath))
                throw new FileNotFoundException(
                    $"Template introuvable pour {templateType} : {templateFileName}",
                    reportPath);

            // ÉTAPE 4 : Construire le payload (données à injecter dans le PDF)
            var payload = await BuildPayloadAsync(acteId);

            // ÉTAPE 5 : Préparer le dossier de sortie
            var outputFolder = Path.Combine(_env.WebRootPath, "uploads", "actes", "pdf");
            Directory.CreateDirectory(outputFolder);

            var outputFile = Path.Combine(
                outputFolder,
                $"acte_{acteId}_{DateTime.Now:yyyyMMddHHmmss}.pdf");

            // ÉTAPE 6 : Génération avec FastReport
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

        // ════════════════════════════════════════════════════════
        // 🆕 Récupère le TemplateType de l'acte via sa relation avec AjoutActModel
        // ════════════════════════════════════════════════════════
        private async Task<TemplateType> GetTemplateTypeForActeAsync(int acteId)
        {
            using var db = await _dbFactory.CreateDbContextAsync();

            var acte = await db.TypesActModels
                .Include(a => a.AjoutActModel)
                .FirstOrDefaultAsync(a => a.IdActe == acteId);

            if (acte == null)
                throw new Exception($"Acte #{acteId} introuvable.");

            if (acte.AjoutActModel == null)
                throw new Exception($"L'acte #{acteId} n'a pas de type associé (AjoutActModel manquant).");

            return acte.AjoutActModel.TemplateType;
        }

        // ════════════════════════════════════════════════════════
        // 🆕 Mappe chaque TemplateType vers son fichier .frx correspondant
        // ════════════════════════════════════════════════════════
        private static string GetTemplateFileName(TemplateType templateType)
        {
            return templateType switch
            {
                TemplateType.CautionnementSpecifiquePhysique => "cautionnement_specifique_physique.frx",
                TemplateType.CautionnementSpecifiqueMorale => "cautionnement_specifique_morale.frx",
                TemplateType.ToutEngagementPhysique => "tout_engagement_physique.frx",
                TemplateType.ToutEngagementMorale => "tout_engagement_morale.frx",
                TemplateType.DelegationLoyersMorale => "delegation_loyers_morale.frx",

                _ => throw new NotSupportedException(
                    $"Aucun template défini pour TemplateType={templateType}.")
            };
        }

        private async Task<List<ActCautionnementReportModel>> BuildPayloadAsync(int acteId)
        {
            var model = await _builder.BuildAsync(acteId);
            return new List<ActCautionnementReportModel> { model };
        }
    }
}