using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Ecard.Mvc.Controllers;
using Ecard.Services;
using Moonlit.Security;

namespace Ecard.Mvc
{
    // delete for publish source code
    public class SLE902rPasswordService : IPasswordService
    {
        private readonly RandomCodeHelper _randomCodeHelper;

        public SLE902rPasswordService(RandomCodeHelper randomCodeHelper)
        {
            _randomCodeHelper = randomCodeHelper;
        }

        public string Name
        {
            get { return "SLE902R √‹¬Îº¸≈Ã"; }
        }

        public void Decrypto(string password, string passwordConfirm, out string password1, out string password2)
        {
            password1 = "";
            password2 = "";
            PasswordToken token = _randomCodeHelper.GetObject<PasswordToken>(RandomCodeNames.PasswordToken);
            if (!string.IsNullOrEmpty(password))
                password1 = DecryptoPassword(password, token);
            if (!string.IsNullOrEmpty(passwordConfirm))
                password2 = DecryptoPassword(passwordConfirm, token);
        }

        public string Decrypto(string password)
        {
            PasswordToken token = _randomCodeHelper.GetObject<PasswordToken>(RandomCodeNames.PasswordToken);
            return DecryptoPassword(password, token);
        }

        private static string DecryptoPassword(string password, PasswordToken token)
        {
            var bytes = HexToString(password);
            var d = token.Rsa.BlockDecrypt(bytes.ToArray());
            string realPassword = Encoding.ASCII.GetString(d);
            if (!realPassword.StartsWith(token.ChallengeData))
                throw new Exception("√‹‘ø≤ª∆•≈‰");
            return realPassword.Substring(token.ChallengeData.Length);
        }

        private static byte[] HexToString(string password)
        {
            byte[] bytes = new byte[password.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(password.Substring(i * 2, 2), NumberStyles.HexNumber);
            }
            return bytes;
        }
    }
}