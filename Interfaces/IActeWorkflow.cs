using GesCPSI_Project.Models;

namespace GesCPSI_Project.Interfaces
{
    public class WorkflowResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public ActeStatut? NewStatut { get; set; }

        public static WorkflowResult Ok(ActeStatut newStatut)
            => new() { Success = true, NewStatut = newStatut };

        public static WorkflowResult Fail(string error)
            => new() { Success = false, ErrorMessage = error };
    }
    public interface IActeWorkflow
    {
        /// <summary>
        /// L'agent soumet son acte pour validation.
        /// Conditions : statut = Brouillon ou Rejete.
        /// </summary>
        Task<WorkflowResult> SoumettreAsync(int acteId, int agentId);

        /// <summary>
        /// Le responsable valide l'acte.
        /// Conditions : statut = EnAttenteValidation, et validateur != agent saisisseur.
        /// </summary>
        Task<WorkflowResult> ValiderAsync(int acteId, int validateurId);

        /// <summary>
        /// Le responsable rejette l'acte avec un motif.
        /// Conditions : statut = EnAttenteValidation.
        /// </summary>
        Task<WorkflowResult> RejeterAsync(int acteId, int validateurId, string motif);

        /// <summary>
        /// Archive un acte validé (après signature + scan).
        /// Conditions : statut = Valide.
        /// </summary>
        Task<WorkflowResult> ArchiverAsync(int acteId, int adminId);

        /// <summary>
        /// Re-ouvre un acte rejeté pour modification (le repasse en Brouillon).
        /// Conditions : statut = Rejete, et user = agent qui l'a saisi (ou admin).
        /// </summary>
        Task<WorkflowResult> RouvrirAsync(int acteId, int userId);

        /// <summary>
        /// Compte les actes en attente de validation (pour le badge du NavMenu).
        /// </summary>
        Task<int> CountEnAttenteAsync();
        Task LogCreationAsync(int acteId, int userId);
    }
}
