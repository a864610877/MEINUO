using System;
using System.Runtime.InteropServices;
using System.Text;
using Ecard.Models;

namespace Ecard.Services
{
    public class UserAndPasswordAndIKeyAuthenticateService : IAuthenticateService
    {
        [DllImport("iKeyAPI.dll")]
        public extern static void MD5_XOR(byte[] text, int text_len, byte[] key, int key_len, byte[] digest);

        private readonly IMembershipService _membershipService;

        public UserAndPasswordAndIKeyAuthenticateService(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public string Name
        {
            get { return "”√ªß√˚√‹¬Î + Ikey"; }
        }

        public bool ValidateUser(string username, string password, string token, string tokenOnServer)
        {
            var user = _membershipService.GetUserByName(username);
            if (user == null) return false;

            var result = user.State == UserStates.Normal && string.Equals(User.SaltAndHash(password, user.PasswordSalt), user.Password);
            if (result)
            {
                user.LastSignInTime = DateTime.Now;
                if (user.LoginInToken)
                {
                    var key = Encoding.ASCII.GetBytes(user.LoginToken);
                    byte[] buf = new byte[16];
                    MD5_XOR(Encoding.ASCII.GetBytes(tokenOnServer), tokenOnServer.Length, key, key.Length, buf);
                    if (!string.Equals(BitConverter.ToString(buf).Replace("-", ""), token, StringComparison.OrdinalIgnoreCase))
                        return false;
                }
                _membershipService.UpdateUser(user);
                return true;
            }
            return false;
        }
    }
}