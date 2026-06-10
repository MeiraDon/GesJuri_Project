using GesCPSI_Project.Interfaces;
using Microsoft.AspNetCore.Components.Forms;

namespace GesCPSI_Project.Services
{
    public class FileUploadService : IFileUpload
    {
        private readonly IWebHostEnvironment _env;
        private const int MaxFileSizeMB = 10;
        private const string AllowedExtension = ".pdf";

        public FileUploadService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<UploadResult> UploadSignedPdfAsync(int acteId, IBrowserFile file)
        {
            // Validation : fichier fourni
            if (file is null || file.Size == 0)
                return UploadResult.Fail("Aucun fichier sélectionné.");

            // Validation : taille
            var maxBytes = MaxFileSizeMB * 1024L * 1024L;
            if (file.Size > maxBytes)
                return UploadResult.Fail($"Le fichier dépasse la taille maximum de {MaxFileSizeMB} Mo.");

            // Validation : extension
            var ext = Path.GetExtension(file.Name).ToLowerInvariant();
            if (ext != AllowedExtension)
                return UploadResult.Fail($"Seuls les fichiers PDF sont acceptés (votre fichier : {ext}).");

            try
            {
                // Préparer le dossier de destination
                var uploadFolder = Path.Combine(_env.WebRootPath, "uploads", "actes", "signes");
                Directory.CreateDirectory(uploadFolder);

                // Nommer le fichier : acte_{id}_signed_{timestamp}.pdf
                var fileName = $"acte_{acteId}_signed_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                var fullPath = Path.Combine(uploadFolder, fileName);

                // Copier le fichier sur disque
                await using var fs = new FileStream(fullPath, FileMode.Create);
                await using var sourceStream = file.OpenReadStream(maxAllowedSize: maxBytes);
                await sourceStream.CopyToAsync(fs);

                // Retourner le chemin relatif web
                var webPath = $"/uploads/actes/signes/{fileName}";
                return UploadResult.Ok(webPath);
            }
            catch (Exception ex)
            {
                return UploadResult.Fail($"Erreur lors de l'upload : {ex.Message}");
            }
        }
    }
}