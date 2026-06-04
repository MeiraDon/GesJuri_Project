namespace GesCPSI_Project.Reports
{
    public class ActCautionnementReportModel
    {
        // ACTE
        public int ActReportIdActe { get; set; }
        public string ActReportNomTypesActe { get; set; } = string.Empty;
        public DateTime ActReportDateCreation { get; set; }
        public string ActReportStatut { get; set; } = string.Empty;

        // BANQUE
        public string BanqueReportNomRaisonSocialBnq { get; set; } = string.Empty;
        public string BanqueReportSigleBnq { get; set; } = string.Empty;
        public string BanqueReportFormeJuridique { get; set; } = string.Empty;
        public string BanqueReportCapitalSocial { get; set; } = string.Empty;
        public string BanqueReportSiegeSocial { get; set; } = string.Empty;
        public string BanqueReportRegistreCommerce { get; set; } = string.Empty;
        public string BanqueReportSwift { get; set; } = string.Empty;
        public string BanqueReportTelBnq { get; set; } = string.Empty;
        public string BanqueReportEmailBnq { get; set; } = string.Empty;
        public string BanqueReportRepresentantBnq { get; set; } = string.Empty;
        public string BanqueReportFonctionRepresentant { get; set; } = string.Empty;
        public string BanqueReportNomCollaborateur { get; set; } = string.Empty;
        public string BanqueReportVisaCollaborateur { get; set; } = string.Empty;

        // DEBITEUR
        public string DebiteurReportNomComplet { get; set; } = string.Empty;
        public string DebiteurReportNomRaisonsociale { get; set; } = string.Empty;
        public string DebiteurReportNomRepresentant { get; set; } = string.Empty;
        public string DebiteurReportNom { get; set; } = string.Empty;
        public string DebiteurReportPrenoms { get; set; } = string.Empty;
        public string DebiteurReportNumCompte { get; set; } = string.Empty;
        public string DebiteurReportNumRCCM { get; set; } = string.Empty;
        public DateTime DebiteurReportDateNaiss { get; set; }
        public string DebiteurReportLieuNaiss { get; set; } = string.Empty;
        public string DebiteurReportPaysNaiss { get; set; } = string.Empty;
        public string DebiteurReportNationalite { get; set; } = string.Empty;
        public string DebiteurReportAdressecomplet { get; set; } = string.Empty;
        public string DebiteurReportVilleCommune { get; set; } = string.Empty;
        public string DebiteurReportPaysResidn { get; set; } = string.Empty;
        public string DebiteurReportBPClt { get; set; } = string.Empty;
        public string DebiteurReportTelBur { get; set; } = string.Empty;
        public string DebiteurReportTelCel { get; set; } = string.Empty;
        public string DebiteurReportTelDom { get; set; } = string.Empty;
        public string DebiteurReportProfessionClt { get; set; } = string.Empty;
        public string DebiteurReportSituationMatrim { get; set; } = string.Empty;
        public string DebiteurReportNomConjoint { get; set; } = string.Empty;
        public DateTime DebiteurReportDateMariage { get; set; }
        public string DebiteurReportLieuMariage { get; set; } = string.Empty;
        public string DebiteurReportRegimeMatrim { get; set; } = string.Empty;
        public string DebiteurReportTypePieceID { get; set; } = string.Empty;
        public string DebiteurReportNumPieceID { get; set; } = string.Empty;
        public DateTime DebiteurReportDateDelivPiece { get; set; }
        public string DebiteurReportLieuDelivPiece { get; set; } = string.Empty;
        public string DebiteurReportPersonDelivPiece { get; set; } = string.Empty;

        // CAUTION
        public string CautionReportNomComplet { get; set; } = string.Empty;
        public string CautionReportNomRaisonsociale { get; set; } = string.Empty;
        public string CautionReportNomRepresentant { get; set; } = string.Empty;
        public string CautionReportNom { get; set; } = string.Empty;
        public string CautionReportPrenoms { get; set; } = string.Empty;
        public string CautionReportNumCompte { get; set; } = string.Empty;
        public string CautionReportNumRCCM { get; set; } = string.Empty;
        public DateTime CautionReportDateNaiss { get; set; }
        public string CautionReportLieuNaiss { get; set; } = string.Empty;
        public string CautionReportPaysNaiss { get; set; } = string.Empty;
        public string CautionReportNationalite { get; set; } = string.Empty;
        public string CautionReportAdressecomplet { get; set; } = string.Empty;
        public string CautionReportVilleCommune { get; set; } = string.Empty;
        public string CautionReportPaysResidn { get; set; } = string.Empty;
        public string CautionReportBPClt { get; set; } = string.Empty;
        public string CautionReportTelBur { get; set; } = string.Empty;
        public string CautionReportTelCel { get; set; } = string.Empty;
        public string CautionReportTelDom { get; set; } = string.Empty;
        public string CautionReportProfessionClt { get; set; } = string.Empty;
        public string CautionReportSituationMatrim { get; set; } = string.Empty;
        public string CautionReportNomConjoint { get; set; } = string.Empty;
        public DateTime CautionReportDateMariage { get; set; }
        public string CautionReportLieuMariage { get; set; } = string.Empty;
        public string CautionReportRegimeMatrim { get; set; } = string.Empty;
        public string CautionReportTypePieceID { get; set; } = string.Empty;
        public string CautionReportNumPieceID { get; set; } = string.Empty;
        public DateTime CautionReportDateDelivPiece { get; set; }
        public string CautionReportLieuDelivPiece { get; set; } = string.Empty;
        public string CautionReportPersonDelivPiece { get; set; } = string.Empty;

        // Caution — Personne Morale
        public string? CautionReportFormeJuridique { get; set; }
        public string? CautionReportCapitalSocial { get; set; }
        public string? CautionReportSiegeSocial { get; set; }
        public string? CautionReportRegistreCommerce { get; set; }
        public string? CautionReportNumIdentifFiscal { get; set; }
        public string? CautionReportFonctionRepresentant { get; set; }
        public string? CautionReportRefPVDelib { get; set; }
        public DateTime CautionReportDatePVDelib { get; set; }

        // TEMOIN1
        public string Temoin1ReportNomComplet { get; set; } = string.Empty;

        public string Temoin1ReportNomRaisonsociale { get; set; } = string.Empty;
        public string Temoin1ReportNomRepresentant { get; set; } = string.Empty;
        public string Temoin1ReportNom { get; set; } = string.Empty;
        public string Temoin1ReportPrenoms { get; set; } = string.Empty;
        public string Temoin1ReportNumCompte { get; set; } = string.Empty;
        public string Temoin1ReportNumRCCM { get; set; } = string.Empty;
        public DateTime Temoin1ReportDateNaiss { get; set; }
        public string Temoin1ReportLieuNaiss { get; set; } = string.Empty;
        public string Temoin1ReportPaysNaiss { get; set; } = string.Empty;
        public string Temoin1ReportNationalite { get; set; } = string.Empty;
        public string Temoin1ReportAdressecomplet { get; set; } = string.Empty;
        public string Temoin1ReportVilleCommune { get; set; } = string.Empty;
        public string Temoin1ReportPaysResidn { get; set; } = string.Empty;
        public string Temoin1ReportBPClt { get; set; } = string.Empty;
        public string Temoin1ReportTelBur { get; set; } = string.Empty;
        public string Temoin1ReportTelCel { get; set; } = string.Empty;
        public string Temoin1ReportTelDom { get; set; } = string.Empty;
        public string Temoin1ReportProfessionClt { get; set; } = string.Empty;
        public string Temoin1ReportSituationMatrim { get; set; } = string.Empty;
        public string Temoin1ReportNomConjoint { get; set; } = string.Empty;
        public DateTime Temoin1ReportDateMariage { get; set; }
        public string Temoin1ReportLieuMariage { get; set; } = string.Empty;
        public string Temoin1ReportRegimeMatrim { get; set; } = string.Empty;
        public string Temoin1ReportTypePieceID { get; set; } = string.Empty;
        public string Temoin1ReportNumPieceID { get; set; } = string.Empty;
        public DateTime Temoin1ReportDateDelivPiece { get; set; }
        public string Temoin1ReportLieuDelivPiece { get; set; } = string.Empty;
        public string Temoin1ReportPersonDelivPiece { get; set; } = string.Empty;

        // TEMOIN2
        public string Temoin2ReportNomComplet { get; set; } = string.Empty;

        public string Temoin2ReportNomRaisonsociale { get; set; } = string.Empty;
        public string Temoin2ReportNomRepresentant { get; set; } = string.Empty;
        public string Temoin2ReportNom { get; set; } = string.Empty;
        public string Temoin2ReportPrenoms { get; set; } = string.Empty;
        public string Temoin2ReportNumCompte { get; set; } = string.Empty;
        public string Temoin2ReportNumRCCM { get; set; } = string.Empty;
        public DateTime Temoin2ReportDateNaiss { get; set; }
        public string Temoin2ReportLieuNaiss { get; set; } = string.Empty;
        public string Temoin2ReportPaysNaiss { get; set; } = string.Empty;
        public string Temoin2ReportNationalite { get; set; } = string.Empty;
        public string Temoin2ReportAdressecomplet { get; set; } = string.Empty;
        public string Temoin2ReportVilleCommune { get; set; } = string.Empty;
        public string Temoin2ReportPaysResidn { get; set; } = string.Empty;
        public string Temoin2ReportBPClt { get; set; } = string.Empty;
        public string Temoin2ReportTelBur { get; set; } = string.Empty;
        public string Temoin2ReportTelCel { get; set; } = string.Empty;
        public string Temoin2ReportTelDom { get; set; } = string.Empty;
        public string Temoin2ReportProfessionClt { get; set; } = string.Empty;
        public string Temoin2ReportSituationMatrim { get; set; } = string.Empty;
        public string Temoin2ReportNomConjoint { get; set; } = string.Empty;
        public DateTime Temoin2ReportDateMariage { get; set; }
        public string Temoin2ReportLieuMariage { get; set; } = string.Empty;
        public string Temoin2ReportRegimeMatrim { get; set; } = string.Empty;
        public string Temoin2ReportTypePieceID { get; set; } = string.Empty;
        public string Temoin2ReportNumPieceID { get; set; } = string.Empty;
        public DateTime Temoin2ReportDateDelivPiece { get; set; }
        public string Temoin2ReportLieuDelivPiece { get; set; } = string.Empty;
        public string Temoin2ReportPersonDelivPiece { get; set; } = string.Empty;

        // CAUTIONNEMENT
        public string CautionnementReportNomTypesActe { get; set; } = string.Empty;
        public string CautionnementReportNumeroCautionmt { get; set; } = string.Empty;
        public decimal CautionnementReportMontantCautionne { get; set; }
        public string CautionnementReportMontantLettreCautionmt { get; set; } = string.Empty;
        public DateTime CautionnementReportDateSignatureCautionmt { get; set; }
        public DateTime CautionnementReportDateEffetCautionmt { get; set; }
        public DateTime CautionnementReportDateExpirationCautionmt { get; set; }
        public string CautionnementReportLieuSignatureCautionmt { get; set; } = string.Empty;
        public string CautionnementReportEtatCautionmt { get; set; } = string.Empty;
        public string CautionnementReportConditionsRevocationCautionmt { get; set; } = string.Empty;
        public string CautionnementReportObligationsBanque { get; set; } = string.Empty;
        public string CautionnementReportObligationsCaution { get; set; } = string.Empty;
        public string CautionnementReportClauseSubrogation { get; set; } = string.Empty;
        public string CautionnementReportClauseNonNovation { get; set; } = string.Empty;
        public string CautionnementReportElectionDomicile { get; set; } = string.Empty;
        public string CautionnementReportReglementDifferend { get; set; } = string.Empty;

        // PRET
        public string PretReportReferenceConvention { get; set; } = string.Empty;
        public decimal PretReportMontantConcours { get; set; }
        public decimal PretReportTaux { get; set; }

        public string PretReportMontantLettrePret { get; set; } = string.Empty;
        public string PretReportAccessoires { get; set; } = string.Empty;
        public string PretReportObjetPret { get; set; } = string.Empty;
        public DateTime PretReportDateSignaturePret { get; set; }
        public int PretReportDureMoisPret { get; set; }
        public DateTime PretReportDateDebutRemboursemnt { get; set; }
        public DateTime PretReportDatePremiereEcheance { get; set; }
        public decimal PretReportMontantPremiereEcheance { get; set; }
        public DateTime PretReportDateDerniereEcheance { get; set; }
        public decimal PretReportMontantDerniereEcheance { get; set; }
        public string PretReportEtatPret { get; set; } = string.Empty;

        // AUTORISATION
        public string AutorisationReportNumeroAutorisation { get; set; } = string.Empty;
        public DateTime AutorisationReportDateAutorisation { get; set; }
        public string AutorisationReportLieuAutorisation { get; set; } = string.Empty;
        public decimal AutorisationReportMontantMaxAutorise { get; set; }
        public string AutorisationReportConditionsAutorisation { get; set; } = string.Empty;

        // CHARGE
        public string ChargeReportTypeCharge { get; set; } = string.Empty;
        public decimal ChargeReportMontantCharge { get; set; }
        public DateTime ChargeReportDateCharge { get; set; }
        public string ChargeReportPayePar { get; set; } = string.Empty;
        public string ChargeReportStatutCharge { get; set; } = string.Empty;

        // ENTITE JURIDIQUE
        public DateTime EntiteJuridiqueReportDateActeJuridique { get; set; }
        public string EntiteJuridiqueReportReference { get; set; } = string.Empty;
        public string EntiteJuridiqueReportNomHuissier { get; set; } = string.Empty;
        public string EntiteJuridiqueReportVilleHuissier { get; set; } = string.Empty;
        public string EntiteJuridiqueReportNumeroDossier { get; set; } = string.Empty;
        public string EntiteJuridiqueReportResponsable { get; set; } = string.Empty;
        public string EntiteJuridiqueReportFonctionResponsable { get; set; } = string.Empty;
        public string EntiteJuridiqueReportNomDossier { get; set; } = string.Empty;


    }


}
