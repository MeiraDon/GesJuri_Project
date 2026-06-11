namespace GesCPSI_Project.Interfaces
{
    public class ProfileDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string? Nom { get; set; }
        public string? Prenoms { get; set; }
        public string? NomComplet { get; set; }
        public string? Telephone { get; set; }
        public string Role { get; set; } = "";
        public string RoleLabel { get; set; } = "";
        public DateTime DateCreation { get; set; }
        public DateTime? DerniereConnexion { get; set; }
        public bool IsActive { get; set; }
    }

    public class ProfileUpdateInput
    {
        public string? Nom { get; set; }
        public string? Prenoms { get; set; }
        public string? Telephone { get; set; }
    }

    public class ProfileStats
    {
        public int ActesCrees { get; set; }
        public int ActesValides { get; set; }
        public int ActesRejetes { get; set; }
        public int ActesEnAttente { get; set; }
        public int ActesArchives { get; set; }

        // Pour les Responsables/Admin
        public int ActesValidesParMoi { get; set; }
        public int ActesRejetesParMoi { get; set; }

        public string? LastActionLabel { get; set; }
        public DateTime? LastActionDate { get; set; }
        public int? LastActionActeId { get; set; }
    }

    public class ProfileUpdateResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }

        public static ProfileUpdateResult Ok() => new() { Success = true };
        public static ProfileUpdateResult Fail(string error) => new() { Success = false, ErrorMessage = error };
    }

    public interface IProfile
    {
        Task<ProfileDto?> GetProfileAsync(int userId);
        Task<ProfileUpdateResult> UpdateProfileAsync(int userId, ProfileUpdateInput input);
        Task<ProfileStats> GetStatsAsync(int userId, string userRole);
    }
}