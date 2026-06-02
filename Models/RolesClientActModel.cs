using System.ComponentModel.DataAnnotations;

namespace GesCPSI_Project.Models
{
    public class RolesClientActModel
    {
        [Key]
        public int IdRole { get; set; }
        public string Libelle { get; set; } = string.Empty; // cation;debiteur;temoins1&2
        public string MentionManuelle { get; set; } = string.Empty;


        // Navigation vers ClientActe
        public ICollection<ClientActModel> ClientActModels { get; set; } = new List<ClientActModel>();
    }
}