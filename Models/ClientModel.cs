using System.ComponentModel.DataAnnotations;

namespace GesCPSI_Project.Models
{
    public class ClientModel
    {
        [Key]
        public int IdClt { get; set; }
        public string TypeClient { get; set; } = string.Empty; /*type: personne physique ou Moral*/
        public string NomRaisonsociale { get; set; } = string.Empty;/* cas de personne Moral*/
        public string NomRepresentant { get; set; } = string.Empty;/* cas de personne Moral ou cas de personne Physique*/
        public string Nom { get; set; } = string.Empty;
        public string Prenoms { get; set; } = string.Empty;
        [Required(ErrorMessage = "Sexe obligatoire")]
        public string Sexe { get; set; } = string.Empty;
        public string NumCompte { get; set; } = string.Empty; /* Num de compte; cas de personne Physique et ou Moral*/
        public string NumRCCM { get; set; } = string.Empty;/* Num de compte de la chambre commerce; cas de personne Moral*/

        [Required(ErrorMessage = "Date de naissance obligatoire")]
        public DateTime DateNaiss { get; set; }

        [Required(ErrorMessage = "Lieu de naissance obligatoire")]
        public string LieuNaiss { get; set; } = string.Empty;

        [Required(ErrorMessage = "Pays de naissance obligatoire")]
        public string PaysNaiss { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nationnalité obligatoire")]
        public string Nationalite { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adresse obligatoire")]
        public string Adressecomplet { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ville obligatoire")]
        public string VilleCommune { get; set; } = string.Empty;

        [Required(ErrorMessage = "Pays résidence obligatoire")]
        public string PaysResidn { get; set; } = string.Empty;

        public string BPClt { get; set; } = string.Empty;
        public string TelBur { get; set; } = string.Empty;

        [Required(ErrorMessage = "TelCel obligatoire")]
        public string TelCel { get; set; } = string.Empty;

        public string TelDom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Profession obligatoire")]
        public string ProfessionClt { get; set; } = string.Empty;

        [Required(ErrorMessage = "SituationMatrim obligatoire")]
        public string SituationMatrim { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nom du Conjoint obligatoire")]
        public string NomConjoint { get; set; } = string.Empty;

        public DateTime DateMariage { get; set; }
        public string LieuMariage { get; set; } = string.Empty;
        public string RegimeMatrim { get; set; } = string.Empty;

        [Required(ErrorMessage = "TypePieceID obligatoire")]
        public string TypePieceID { get; set; } = string.Empty;

        [Required(ErrorMessage = "NumPieceID obligatoire")]
        public string NumPieceID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date de delivrance obligatoire")]
        public DateTime DateDelivPiece { get; set; }

        [Required(ErrorMessage = "Lieu de delivrance obligatoire")]
        public string LieuDelivPiece { get; set; } = string.Empty;
        public string PersonDelivPiece { get; set; } = string.Empty;
        public bool CopiePieceJointe { get; set; }

        public string? FormeJuridique { get; set; }
        public string? CapitalSocial { get; set; }
        public string? SiegeSocial { get; set; }
        public string? RegistreCommerce { get; set; }
        public string? NumIdentifFiscal { get; set; }
        public string? FonctionRepresentant { get; set; }
        public string? RefPVDelib { get; set; }
        public DateTime? DatePVDelib { get; set; }


        // Navigation many-to-many avec TypesActe
        public ICollection<ClientActModel> ClientActModels { get; set; } = new List<ClientActModel>();
    }
}
