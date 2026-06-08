namespace GesCPSI_Project.Models
{
    /// <summary>
    /// Noms canoniques des rôles métier — à utiliser dans [Authorize(Roles=...)] et dans les seeds.
    /// Centraliser ça évite les fautes de frappe ("Admin" vs "admin").
    /// </summary>
    /// 
    public class RoleNames
    {
        public const string Admin = "Admin";
        public const string Agent = "Agent";
        public const string Responsable = "Responsable";

        public static readonly string[] All = { Admin, Agent, Responsable };
    }
}
