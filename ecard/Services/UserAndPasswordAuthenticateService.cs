using System;
using System.Text;
using Ecard.Models;

namespace Ecard.Services
{
    public class UserAndPasswordAuthenticateService : IAuthenticateService
    {
        private readonly IMembershipService _membershipService;

        public UserAndPasswordAuthenticateService(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public string Name
        {
            get { return "”√ªß√˚ + √‹¬Î"; }
        }

        public bool ValidateUser(string username, string password, string token, string tokenOnServer)
        {
            var user = _membershipService.GetUserByName(username);
            if (user == null) return false;

            var result = user.State == UserStates.Normal && string.Equals(User.SaltAndHash(password, user.PasswordSalt), user.Password);
            if (result)
            {
                user.LastSignInTime = DateTime.Now; 
                _membershipService.UpdateUser(user);
                return true;
            }
            return false;
        }
    }
}