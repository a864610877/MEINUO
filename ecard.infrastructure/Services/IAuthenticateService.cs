namespace Ecard.Services
{
    public interface IAuthenticateService
    {
        string Name { get; }
        bool ValidateUser(string username, string password, string token, string tokenOnServer);
    }
}