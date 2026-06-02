using System.ComponentModel.DataAnnotations;

namespace GesCPSI_Project.ViewModels
{
    public class ClientModelViewModel : IValidatableObject
    {
        public int? IdClt { get; set; }

        [Required(ErrorMessage = "Type client obligatoire")]
        public string? TypeClient { get; set; }

        public string? NomRaisonsociale { get; set; }
        public string? NomRepresentant { get; set; }

        public string? Nom { get; set; }
        public string? Prenoms { get; set; }
        public string? Sexe { get; set; }

        public string? NumCompte { get; set; }
        public string? NumRCCM { get; set; }

        [Required(ErrorMessage = "Date de naissance obligatoire")]
        public DateTime? DateNaiss { get; set; }

        [Required(ErrorMessage = "Lieu de naissance obligatoire")]
        public string? LieuNaiss { get; set; }

        public string? PaysNaiss { get; set; }

        [Required(ErrorMessage = "Nationalité obligatoire")]
        public string? Nationalite { get; set; }

        [Required(ErrorMessage = "Adresse obligatoire")]
        public string? Adressecomplet { get; set; }   // corrigé

        [Required(ErrorMessage = "Ville obligatoire")]
        public string? VilleCommune { get; set; }     // corrigé

        [Required(ErrorMessage = "Pays de résidence obligatoire")]
        public string? PaysResidn { get; set; }

        public string? BPClt { get; set; }
        public string? TelBur { get; set; }

        [Required(ErrorMessage = "Téléphone portable obligatoire")]
        [Phone(ErrorMessage = "Format de téléphone invalide")]
        public string? TelCel { get; set; }

        public string? TelDom { get; set; }

        [Required(ErrorMessage = "Profession obligatoire")]
        public string? ProfessionClt { get; set; }

        [Required(ErrorMessage = "Situation matrimoniale obligatoire")]
        public string? SituationMatrim { get; set; }

        public string? NomConjoint { get; set; }
        public DateTime? DateMariage { get; set; }
        public string? LieuMariage { get; set; }
        public string? RegimeMatrim { get; set; }

        [Required(ErrorMessage = "Type de pièce obligatoire")]
        public string? TypePieceID { get; set; }

        [Required(ErrorMessage = "Numéro de pièce obligatoire")]
        public string? NumPieceID { get; set; }

        [Required(ErrorMessage = "Date de délivrance obligatoire")]
        public DateTime? DateDelivPiece { get; set; }

        public string? LieuDelivPiece { get; set; }
        public string? PersonDelivPiece { get; set; }
        public bool CopiePieceJointe { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (TypeClient == "Personne Physique")
            {
                if (string.IsNullOrWhiteSpace(Nom))
                    yield return new ValidationResult("Nom obligatoire", new[] { nameof(Nom) });

                if (string.IsNullOrWhiteSpace(Prenoms))
                    yield return new ValidationResult("Prénoms obligatoires", new[] { nameof(Prenoms) });

                if (string.IsNullOrWhiteSpace(NumCompte))
                    yield return new ValidationResult("Numéro de compte obligatoire", new[] { nameof(NumCompte) });
            }

            if (TypeClient == "Personne Morale")
            {
                if (string.IsNullOrWhiteSpace(NomRaisonsociale))
                    yield return new ValidationResult("Raison sociale obligatoire", new[] { nameof(NomRaisonsociale) });

                if (string.IsNullOrWhiteSpace(NumRCCM))
                    yield return new ValidationResult("Numéro RCCM obligatoire", new[] { nameof(NumRCCM) });

                if (string.IsNullOrWhiteSpace(NumCompte))
                    yield return new ValidationResult("Numéro de compte obligatoire", new[] { nameof(NumCompte) });

                if (string.IsNullOrWhiteSpace(NomRepresentant))
                    yield return new ValidationResult("Représentant obligatoire", new[] { nameof(NomRepresentant) });
            }
        }

    }


}
