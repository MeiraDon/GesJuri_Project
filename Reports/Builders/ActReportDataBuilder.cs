using GesCPSI_Project.Interfaces;
using GesCPSI_Project.Models;
using System.Net;

namespace GesCPSI_Project.Reports
{
    public class ActReportDataBuilder
    {
        private readonly ITypesAct _typesActService;
        private readonly IClientAct _clientActService;
        private readonly IClient _clientService;
        private readonly IPret _pretService;
        private readonly IAutorisation _autorisationService;
        private readonly ICharge _chargeService;
        private readonly IEntiteJur _entiteJurService;
        private readonly ICautionnement _cautionnementService;

        public ActReportDataBuilder(
            ITypesAct typesActService,
            IClientAct clientActService,
            IClient clientService,
            IPret pretService,
            IAutorisation autorisationService,
            ICharge chargeService,
            IEntiteJur entiteJurService,
            ICautionnement cautionnementService)
        {
            _typesActService = typesActService;
            _clientActService = clientActService;
            _clientService = clientService;
            _pretService = pretService;
            _autorisationService = autorisationService;
            _chargeService = chargeService;
            _entiteJurService = entiteJurService;
            _cautionnementService = cautionnementService;
        }

        public async Task<ActCautionnementReportModel> BuildAsync(int acteId)
        {
            var acte = await _typesActService.GetByIdAsync(acteId)
                ?? throw new Exception($"Acte introuvable : {acteId}");

            var allLinks = await _clientActService.GetAllAsync() ?? new List<ClientActModel>();
            var links = allLinks.Where(x => x.IdActe == acteId).ToList();

            var allClients = await _clientService.GetAllAsync() ?? new List<ClientModel>();

            var pret = (await _pretService.GetAllAsync())?.FirstOrDefault(x => x.IdActe == acteId);
            var autorisation = (await _autorisationService.GetAllAsync())?.FirstOrDefault(x => x.IdActe == acteId);
            var charge = (await _chargeService.GetAllAsync())?.FirstOrDefault(x => x.IdActe == acteId);
            var entite = (await _entiteJurService.GetAllAsync())?.FirstOrDefault(x => x.IdActe == acteId);
            var cautionnement = (await _cautionnementService.GetAllAsync())?.FirstOrDefault(x => x.IdActe == acteId);

            var debiteur = GetClientByRoles(links, allClients, "Debiteur", "Débiteur");
            var caution = GetClientByRoles(links, allClients, "Caution", "Garant", "Garant/Caution");
            var temoin1 = GetClientByRoles(links, allClients, "Temoin1", "Temoin 1", "Témoin 1");
            var temoin2 = GetClientByRoles(links, allClients, "Temoin2", "Temoin 2", "Témoin 2");

            return new ActCautionnementReportModel
            {
                ActReportIdActe = acte.IdActe,
                ActReportNomTypesActe = acte.NomTypesActe ?? "",
                ActReportDateCreation = acte.DateCreation,
                ActReportStatut = acte.Statut ?? "",

                BanqueReportNomRaisonSocialBnq = acte.BanqueModel?.NomRaisonSocialBnq ?? "",
                BanqueReportSigleBnq = acte.BanqueModel?.SigleBnq ?? "",
                BanqueReportFormeJuridique = acte.BanqueModel?.FormeJuridique ?? "",
                BanqueReportCapitalSocial = acte.BanqueModel?.CapitalSocial ?? "",
                BanqueReportSiegeSocial = acte.BanqueModel?.SiegeSocial ?? "",
                BanqueReportRegistreCommerce = acte.BanqueModel?.RegistreCommerce ?? "",
                BanqueReportSwift = acte.BanqueModel?.Swift ?? "",
                BanqueReportTelBnq = acte.BanqueModel?.TelBnq ?? "",
                BanqueReportEmailBnq = acte.BanqueModel?.EmailBnq ?? "",
                BanqueReportRepresentantBnq = acte.BanqueModel?.RepresentantBnq ?? "",
                BanqueReportFonctionRepresentant = acte.BanqueModel?.FonctionRepresentant ?? "",
                BanqueReportNomCollaborateur = acte.BanqueModel?.NomCollaborateur ?? "",
                BanqueReportVisaCollaborateur = acte.BanqueModel?.VisaCollaborateur ?? "",

                DebiteurReportNomComplet = BuildNomComplet(debiteur),
                DebiteurReportNomRaisonsociale = debiteur?.NomRaisonsociale ?? "",
                DebiteurReportNomRepresentant = debiteur?.NomRepresentant ?? "",
                DebiteurReportNom = debiteur?.Nom ?? "",
                DebiteurReportPrenoms = debiteur?.Prenoms ?? "",
                DebiteurReportNumCompte = debiteur?.NumCompte ?? "",
                DebiteurReportNumRCCM = debiteur?.NumRCCM ?? "",
                DebiteurReportDateNaiss = SafeDate(debiteur?.DateNaiss),
                DebiteurReportLieuNaiss = debiteur?.LieuNaiss ?? "",
                DebiteurReportPaysNaiss = debiteur?.PaysNaiss ?? "",
                DebiteurReportNationalite = debiteur?.Nationalite ?? "",
                DebiteurReportAdressecomplet = debiteur?.Adressecomplet ?? "",
                DebiteurReportVilleCommune = debiteur?.VilleCommune ?? "",
                DebiteurReportPaysResidn = debiteur?.PaysResidn ?? "",
                DebiteurReportBPClt = debiteur?.BPClt ?? "",
                DebiteurReportTelBur = debiteur?.TelBur ?? "",
                DebiteurReportTelCel = debiteur?.TelCel ?? "",
                DebiteurReportTelDom = debiteur?.TelDom ?? "",
                DebiteurReportProfessionClt = debiteur?.ProfessionClt ?? "",
                DebiteurReportSituationMatrim = debiteur?.SituationMatrim ?? "",
                DebiteurReportNomConjoint = debiteur?.NomConjoint ?? "",
                DebiteurReportDateMariage = SafeDate(debiteur?.DateMariage),
                DebiteurReportLieuMariage = debiteur?.LieuMariage ?? "",
                DebiteurReportRegimeMatrim = debiteur?.RegimeMatrim ?? "",
                DebiteurReportTypePieceID = debiteur?.TypePieceID ?? "",
                DebiteurReportNumPieceID = debiteur?.NumPieceID ?? "",
                DebiteurReportDateDelivPiece = SafeDate(debiteur?.DateDelivPiece),
                DebiteurReportLieuDelivPiece = debiteur?.LieuDelivPiece ?? "",
                DebiteurReportPersonDelivPiece = debiteur?.PersonDelivPiece ?? "",

                CautionReportNomComplet = BuildNomComplet(caution),
                CautionReportNomRaisonsociale = caution?.NomRaisonsociale ?? "",
                CautionReportNomRepresentant = caution?.NomRepresentant ?? "",
                CautionReportNom = caution?.Nom ?? "",
                CautionReportPrenoms = caution?.Prenoms ?? "",
                CautionReportNumCompte = caution?.NumCompte ?? "",
                CautionReportNumRCCM = caution?.NumRCCM ?? "",
                CautionReportDateNaiss = SafeDate(caution?.DateNaiss),
                CautionReportLieuNaiss = caution?.LieuNaiss ?? "",
                CautionReportPaysNaiss = caution?.PaysNaiss ?? "",
                CautionReportNationalite = caution?.Nationalite ?? "",
                CautionReportAdressecomplet = caution?.Adressecomplet ?? "",
                CautionReportVilleCommune = caution?.VilleCommune ?? "",
                CautionReportPaysResidn = caution?.PaysResidn ?? "",
                CautionReportBPClt = caution?.BPClt ?? "",
                CautionReportTelBur = caution?.TelBur ?? "",
                CautionReportTelCel = caution?.TelCel ?? "",
                CautionReportTelDom = caution?.TelDom ?? "",
                CautionReportProfessionClt = caution?.ProfessionClt ?? "",
                CautionReportSituationMatrim = caution?.SituationMatrim ?? "",
                CautionReportNomConjoint = caution?.NomConjoint ?? "",
                CautionReportDateMariage = SafeDate(caution?.DateMariage),
                CautionReportLieuMariage = caution?.LieuMariage ?? "",
                CautionReportRegimeMatrim = caution?.RegimeMatrim ?? "",
                CautionReportTypePieceID = caution?.TypePieceID ?? "",
                CautionReportNumPieceID = caution?.NumPieceID ?? "",
                CautionReportDateDelivPiece = SafeDate(caution?.DateDelivPiece),
                CautionReportLieuDelivPiece = caution?.LieuDelivPiece ?? "",
                CautionReportPersonDelivPiece = caution?.PersonDelivPiece ?? "",

                Temoin1ReportNomComplet = BuildNomComplet(temoin1),
                Temoin1ReportNomRaisonsociale = temoin1?.NomRaisonsociale ?? "",
                Temoin1ReportNomRepresentant = temoin1?.NomRepresentant ?? "",
                Temoin1ReportNom = temoin1?.Nom ?? "",
                Temoin1ReportPrenoms = temoin1?.Prenoms ?? "",
                Temoin1ReportNumCompte = temoin1?.NumCompte ?? "",
                Temoin1ReportNumRCCM = temoin1?.NumRCCM ?? "",
                Temoin1ReportDateNaiss = SafeDate(temoin1?.DateNaiss),
                Temoin1ReportLieuNaiss = temoin1?.LieuNaiss ?? "",
                Temoin1ReportPaysNaiss = temoin1?.PaysNaiss ?? "",
                Temoin1ReportNationalite = temoin1?.Nationalite ?? "",
                Temoin1ReportAdressecomplet = temoin1?.Adressecomplet ?? "",
                Temoin1ReportVilleCommune = temoin1?.VilleCommune ?? "",
                Temoin1ReportPaysResidn = temoin1?.PaysResidn ?? "",
                Temoin1ReportBPClt = temoin1?.BPClt ?? "",
                Temoin1ReportTelBur = temoin1?.TelBur ?? "",
                Temoin1ReportTelCel = temoin1?.TelCel ?? "",
                Temoin1ReportTelDom = temoin1?.TelDom ?? "",
                Temoin1ReportProfessionClt = temoin1?.ProfessionClt ?? "",
                Temoin1ReportSituationMatrim = temoin1?.SituationMatrim ?? "",
                Temoin1ReportNomConjoint = temoin1?.NomConjoint ?? "",
                Temoin1ReportDateMariage = SafeDate(temoin1?.DateMariage),
                Temoin1ReportLieuMariage = temoin1?.LieuMariage ?? "",
                Temoin1ReportRegimeMatrim = temoin1?.RegimeMatrim ?? "",
                Temoin1ReportTypePieceID = temoin1?.TypePieceID ?? "",
                Temoin1ReportNumPieceID = temoin1?.NumPieceID ?? "",
                Temoin1ReportDateDelivPiece = SafeDate(temoin1?.DateDelivPiece),
                Temoin1ReportLieuDelivPiece = temoin1?.LieuDelivPiece ?? "",
                Temoin1ReportPersonDelivPiece = temoin1?.PersonDelivPiece ?? "",

                Temoin2ReportNomComplet = BuildNomComplet(temoin2),
                Temoin2ReportNomRaisonsociale = temoin2?.NomRaisonsociale ?? "",
                Temoin2ReportNomRepresentant = temoin2?.NomRepresentant ?? "",
                Temoin2ReportNom = temoin2?.Nom ?? "",
                Temoin2ReportPrenoms = temoin2?.Prenoms ?? "",
                Temoin2ReportNumCompte = temoin2?.NumCompte ?? "",
                Temoin2ReportNumRCCM = temoin2?.NumRCCM ?? "",
                Temoin2ReportDateNaiss = SafeDate(temoin2?.DateNaiss),
                Temoin2ReportLieuNaiss = temoin2?.LieuNaiss ?? "",
                Temoin2ReportPaysNaiss = temoin2?.PaysNaiss ?? "",
                Temoin2ReportNationalite = temoin2?.Nationalite ?? "",
                Temoin2ReportAdressecomplet = temoin2?.Adressecomplet ?? "",
                Temoin2ReportVilleCommune = temoin2?.VilleCommune ?? "",
                Temoin2ReportPaysResidn = temoin2?.PaysResidn ?? "",
                Temoin2ReportBPClt = temoin2?.BPClt ?? "",
                Temoin2ReportTelBur = temoin2?.TelBur ?? "",
                Temoin2ReportTelCel = temoin2?.TelCel ?? "",
                Temoin2ReportTelDom = temoin2?.TelDom ?? "",
                Temoin2ReportProfessionClt = temoin2?.ProfessionClt ?? "",
                Temoin2ReportSituationMatrim = temoin2?.SituationMatrim ?? "",
                Temoin2ReportNomConjoint = temoin2?.NomConjoint ?? "",
                Temoin2ReportDateMariage = SafeDate(temoin2?.DateMariage),
                Temoin2ReportLieuMariage = temoin2?.LieuMariage ?? "",
                Temoin2ReportRegimeMatrim = temoin2?.RegimeMatrim ?? "",
                Temoin2ReportTypePieceID = temoin2?.TypePieceID ?? "",
                Temoin2ReportNumPieceID = temoin2?.NumPieceID ?? "",
                Temoin2ReportDateDelivPiece = SafeDate(temoin2?.DateDelivPiece),
                Temoin2ReportLieuDelivPiece = temoin2?.LieuDelivPiece ?? "",
                Temoin2ReportPersonDelivPiece = temoin2?.PersonDelivPiece ?? "",

                CautionnementReportNomTypesActe = cautionnement?.NomTypesActe ?? "",
                CautionnementReportNumeroCautionmt = cautionnement?.NumeroCautionmt ?? "",
                CautionnementReportMontantCautionne = cautionnement?.MontantCautionne ?? 0m,
                CautionnementReportMontantLettreCautionmt = cautionnement?.MontantLettreCautionmt ?? "",
                CautionnementReportDateSignatureCautionmt = SafeDate(cautionnement?.DateSignatureCautionmt),
                CautionnementReportDateEffetCautionmt = SafeDate(cautionnement?.DateEffetCautionmt),
                CautionnementReportDateExpirationCautionmt = SafeDate(cautionnement?.DateExpirationCautionmt),
                CautionnementReportLieuSignatureCautionmt = cautionnement?.LieuSignatureCautionmt ?? "",
                CautionnementReportEtatCautionmt = cautionnement?.EtatCautionmt ?? "",
                CautionnementReportConditionsRevocationCautionmt = cautionnement?.ConditionsRevocationCautionmt ?? "",
                CautionnementReportObligationsBanque = cautionnement?.ObligationsBanque ?? "",
                CautionnementReportObligationsCaution = cautionnement?.ObligationsCaution ?? "",
                CautionnementReportClauseSubrogation = cautionnement?.ClauseSubrogation ?? "",
                CautionnementReportClauseNonNovation = cautionnement?.ClauseNonNovation ?? "",
                CautionnementReportElectionDomicile = cautionnement?.ElectionDomicile ?? "",
                CautionnementReportReglementDifferend = cautionnement?.ReglementDifferend ?? "",

                PretReportReferenceConvention = pret?.ReferenceConvention ?? "",
                PretReportMontantConcours = pret?.MontantConcours ?? 0m,
                PretReportTaux = pret?.Taux ?? 0m,
                PretReportMontantLettrePret = pret?.MontantLettrePret ?? "",
                PretReportAccessoires = pret?.Accessoires ?? "",
                PretReportObjetPret = pret?.ObjetPret ?? "",
                PretReportDateSignaturePret = SafeDate(pret?.DateSignaturePret),
                PretReportDureMoisPret = pret?.DureMoisPret ?? 0,
                PretReportDateDebutRemboursemnt = SafeDate(pret?.DateDebutRemboursemnt),
                PretReportDatePremiereEcheance = SafeDate(pret?.DatePremiereEcheance),
                PretReportMontantPremiereEcheance = pret?.MontantPremiereEcheance ?? 0m,
                PretReportDateDerniereEcheance = SafeDate(pret?.DateDerniereEcheance),
                PretReportMontantDerniereEcheance = pret?.MontantDerniereEcheance ?? 0m,
                PretReportEtatPret = pret?.EtatPret ?? "",

                AutorisationReportNumeroAutorisation = autorisation?.NumeroAutorisation ?? "",
                AutorisationReportDateAutorisation = SafeDate(autorisation?.DateAutorisation),
                AutorisationReportLieuAutorisation = autorisation?.LieuAutorisation ?? "",
                AutorisationReportMontantMaxAutorise = autorisation?.MontantMaxAutorise ?? 0m,
                AutorisationReportConditionsAutorisation = autorisation?.ConditionsAutorisation ?? "",

                ChargeReportTypeCharge = charge?.TypeCharge ?? "",
                ChargeReportMontantCharge = charge?.MontantCharge ?? 0m,
                ChargeReportDateCharge = SafeDate(charge?.DateCharge),
                ChargeReportPayePar = charge?.PayePar ?? "",
                ChargeReportStatutCharge = charge?.StatutCharge ?? "",

                EntiteJuridiqueReportDateActeJuridique = SafeDate(entite?.DateActeJuridique),
                EntiteJuridiqueReportReference = entite?.Reference ?? "",
                EntiteJuridiqueReportNomHuissier = entite?.NomHuissier ?? "",
                EntiteJuridiqueReportVilleHuissier = entite?.VilleHuissier ?? "",
                EntiteJuridiqueReportNumeroDossier = entite?.NumeroDossier ?? "",
                EntiteJuridiqueReportResponsable = entite?.Responsable ?? "",
                EntiteJuridiqueReportFonctionResponsable = entite?.FonctionResponsable ?? "",
                EntiteJuridiqueReportNomDossier = entite?.NomDossier ?? "",
            };
        }

        private static ClientModel? GetClientByRoles(
            List<ClientActModel> links,
            List<ClientModel> clients,
            params string[] acceptedRoles)
        {
            var accepted = acceptedRoles.Select(NormalizeRole).ToHashSet();

            var link = links.FirstOrDefault(x => accepted.Contains(NormalizeRole(x.DescriptionRole)));
            if (link is null) return null;

            return clients.FirstOrDefault(c => c.IdClt == link.IdClt);
        }

        private static string NormalizeRole(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;

            return value.Trim().ToLowerInvariant()
                .Replace("é", "e")
                .Replace("è", "e")
                .Replace("ê", "e")
                .Replace("à", "a")
                .Replace("ù", "u")
                .Replace("ô", "o")
                .Replace("î", "i")
                .Replace("ï", "i")
                .Replace(" ", "")
                .Replace("_", "")
                .Replace("-", "");
        }

        private static string BuildNomComplet(ClientModel? client)
        {
            if (client is null) return "";

            return string.Equals(client.TypeClient, "Personne Morale", StringComparison.OrdinalIgnoreCase)
                ? $"{client.NomRaisonsociale} {client.NomRepresentant}".Trim()
                : $"{client.Nom} {client.Prenoms}".Trim();
        }

        private static DateTime SafeDate(DateTime? value)
            => value ?? DateTime.MinValue;
    }
}
