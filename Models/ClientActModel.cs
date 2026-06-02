using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GesCPSI_Project.Models
{
    public class ClientActModel
    {
        //Table Association ; Jointure : Client + RolesClient + TypesActe

        [Key]
        public int IdCltActe { get; set; }

        public string DescriptionRole { get; set; } = string.Empty;

        // Client
        public int IdClt { get; set; }
        [ForeignKey(nameof(IdClt))]
        public ClientModel ClientModel { get; set; } = null!;

        // Acte
        public int IdActe { get; set; }
        [ForeignKey(nameof(IdActe))]
        public TypesActModel TypesActModel { get; set; } = null!;

        //// Rôle
        public int IdRole { get; set; }
        [ForeignKey(nameof(IdRole))]
        public RolesClientActModel RolesClientActModel { get; set; } = null!;
    }
}
