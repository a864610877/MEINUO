namespace Ecard.Services
{
    public class NonePasswordService : IPasswordService
    {
        public string Name
        {
            get { return "Õ¯“≥ ‰»Î"; }
        }

        public void Decrypto(string password, string passwordConfirm, out string password1, out string password2)
        {
            password1 = password;
            password2 = passwordConfirm;
        }

        public string Decrypto(string password)
        {
            return password;
        }
    }
}