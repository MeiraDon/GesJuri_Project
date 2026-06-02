using System.ComponentModel.DataAnnotations;

namespace GesCPSI_Project.Models
{
    public class BanqueModel
    {
        [Key]
        public int IdBnq { get; set; }
        public string NomRaisonSocialBnq { get; set; } = string.Empty;
        public string SigleBnq { get; set; } = string.Empty;
        public string FormeJuridique { get; set; } = string.Empty;
        public string CapitalSocial { get; set; } = string.Empty;
        public string SiegeSocial { get; set; } = string.Empty;
        public string RegistreCommerce { get; set; } = string.Empty;
        public string Swift { get; set; } = string.Empty;
        public string TelBnq { get; set; } = string.Empty;
        public string EmailBnq { get; set; } = string.Empty;
        public string RepresentantBnq { get; set; } = string.Empty;
        public string FonctionRepresentant { get; set; } = string.Empty;
        public string NomCollaborateur { get; set; } = string.Empty;
        public string VisaCollaborateur { get; set; } = string.Empty;


        // Navigation : une banque a plusieurs actes
        public ICollection<TypesActModel> TypesActModels { get; set; } = new List<TypesActModel>();
    }
}
