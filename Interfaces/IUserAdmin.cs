using GesCPSI_Project.Models;

namespace GesCPSI_Project.Interfaces
{
    public class UserAdminDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string? NomComplet { get; set; }
        public string? Fonction { get; set; }
        public string? Role { get; set; }
        public bool IsActive { get; set; }
        public bool MustChangePassword { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime? DerniereConnexion { get; set; }
    }

    public class UserCreateInput
    {
        public string Email { get; set; } = "";
        public string NomComplet { get; set; } = "";
        public string? Fonction { get; set; }
        public string Role { get; set; } = RoleNames.Agent;
        public string TemporaryPassword { get; set; } = "";
    }

    public class UserUpdateInput
    {
        public int Id { get; set; }
        public string NomComplet { get; set; } = "";
        public string? Fonction { get; set; }
        public string Role { get; set; } = RoleNames.Agent;
        public bool IsActive { get; set; }
    }

    public class UserAdminResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public string? GeneratedPassword { get; set; }
        public UserAdminDto? Data { get; set; }

        public static UserAdminResult Ok(UserAdminDto? data = null, string? generatedPassword = null)
            => new() { Success = true, Data = data, GeneratedPassword = generatedPassword };

        public static UserAdminResult Fail(string error)
            => new() { Success = false, ErrorMessage = error };
    }

    public interface IUserAdmin
    {
        Task<List<UserAdminDto>> GetAllAsync();
        Task<UserAdminDto?> GetByIdAsync(int id);
        Task<UserAdminResult> CreateAsync(UserCreateInput input);
        Task<UserAdminResult> UpdateAsync(UserUpdateInput input);
        Task<UserAdminResult> ToggleActiveAsync(int id, int currentUserId);
        Task<UserAdminResult> ResetPasswordAsync(int id);
        Task<UserAdminResult> DeleteAsync(int id, int currentUserId);
    }
}