namespace GesCPSI_Project.Services
{
    public class ActeRefreshService
    {
        public event Action? OnRefreshRequested;

        public void RequestRefresh() => OnRefreshRequested?.Invoke();
    }
}
