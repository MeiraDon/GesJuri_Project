namespace GesCPSI_Project.Models
{
    public enum TemplateType
    {
        // Cautionnement spécifique
        CautionnementSpecifiquePhysique = 1,
        CautionnementSpecifiqueMorale = 2,

        // Tout engagement (solde débiteur compte courant)
        ToutEngagementPhysique = 3,
        ToutEngagementMorale = 4,

        // Délégation de loyers
        DelegationLoyersMorale = 5
    }
}
