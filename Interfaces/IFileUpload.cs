using Microsoft.AspNetCore.Components.Forms;

namespace GesCPSI_Project.Interfaces
{
    public class UploadResult
    {
        public bool Success { get; set; }
        public string? FilePath { get; set; }
        public string? ErrorMessage { get; set; }

        public static UploadResult Ok(string path) => new() { Success = true, FilePath = path };
        public static UploadResult Fail(string error) => new() { Success = false, ErrorMessage = error };
    }

    public interface IFileUpload
    {
        /// <summary>
        /// Upload un PDF signé pour un acte donné.
        /// Stocke dans /wwwroot/uploads/actes/signes/.
        /// Retourne le chemin relatif web (ex: /uploads/actes/signes/acte_42_signed.pdf).
        /// </summary>
        Task<UploadResult> UploadSignedPdfAsync(int acteId, IBrowserFile file);
    }
}