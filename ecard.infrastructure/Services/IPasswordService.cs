namespace Ecard.Services
{
    public interface IPasswordService
    {
        string Name { get; }
        void Decrypto(string password, string passwordConfirm, out string password1, out string password2);
        string Decrypto(string password);
    }
}