using GesCPSI_Project.Services;

public interface IEmail
{
    Task<Result<bool>> SendAsync(string to, string subject, string htmlBody);
}