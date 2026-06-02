using GesCPSI_Project.Models;

namespace GesCPSI_Project.ViewModels
{
    public class CautionnementSaisieViewModel
    {
        public ClientModelViewModel Debiteur { get; set; } = new();
        public ClientModelViewModel Caution { get; set; } = new();
        public CautionnementViewModel Cautionnement { get; set; } = new();
        public PretViewModel Pret { get; set; } = new();
        public AutorisationViewModel Autorisation { get; set; } = new();
        public ChargeViewModel Charge { get; set; } = new();
        public EntiteJurViewModel EntiteJuridique { get; set; } = new();
        public ClientModelViewModel Temoin1 { get; set; } = new();
        public ClientModelViewModel Temoin2 { get; set; } = new();
    }
}
