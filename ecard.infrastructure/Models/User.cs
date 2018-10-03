using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Moonlit;
using Moonlit.Validations;

namespace Ecard.Models
{
    public class ShopUser : User
    {
        [Bounded(typeof(ShopRoles))]
        public int ShopRole { get; set; }
        public int ShopId { get; set; }
    }
    /// <summary>
    /// 经销商用户？用户的类型
    /// </summary>
    public class DistributorUser : User
    {
       public int DistributorId { get; set; }
    }

    public class AccountUser : User
    {
        public DateTime? SignOnTime { get; set; }
    }

    public class AdminUser : User
    {
        public bool? IsSale { get; set; }
    }

    public class Genders
    {
        public const int All = Globals.All;
        public const int Male = 1;
        public const int Female = 2;
    }
    public enum UserType
    {
        AdminUser,
        ShopUser,
        AccountUser,
        DistributorUser,
    }
    [Discriminator("AdminUser", typeof(AdminUser))]
    [Discriminator("ShopUser", typeof(ShopUser))]
    [Discriminator("AccountUser", typeof(AccountUser))]
    [Discriminator("DistributorUser", typeof(DistributorUser))]
    public class User : INamedEntity, IKeyObject
    {
        protected User()
        {
            Email = "";
            Password = "";
            PasswordSalt = "";
            DisplayName = "";
            State = States.Normal;
            Mobile = "";
            LoginToken = SaltAndHash(Guid.NewGuid().ToString(), DateTime.Now.ToString());
        }

        [Bounded(typeof(Genders))]
        public int? Gender { get; set; }

        public DateTime? LastSignInTime { get; set; }
        public string IdentityCard { get; set; }
        public string Address { get; set; }

        [Key]
        public int UserId { get; set; }
        /// <summary>
        /// 是否绑定过手机
        /// </summary>
        public bool IsMobileAvailable { get; set; }
        [StringLength(40)]
        [RegularExpression(@"^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$",ErrorMessage="邮箱的格式不对")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        [Bounded(typeof(UserStates))]
        public int State { get; set; }

        public DateTime? BirthDate { get; set; }
        public bool BuildIn { get; set; }

        [RegularExpression(@"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)",ErrorMessage="输入的必须是手机号码和电话号码")]
        public string Mobile { get; set; }
        [RegularExpression(@"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)", ErrorMessage = "输入的必须是手机号码和电话号码")]
        public string Mobile2 { get; set; }
        [RegularExpression(@"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)", ErrorMessage = "输入的必须是手机号码和电话号码")]
        public string PhoneNumber { get; set; }
        [RegularExpression(@"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)", ErrorMessage = "输入的必须是手机号码和电话号码")]
        public string PhoneNumber2 { get; set; }

        public string LoginToken { get; set; }
        public bool LoginInToken { get; set; }

        public int provinceId { get; set; }

        public int cityId { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        #region IKeyObject Members

        public int Id
        {
            get { return UserId; }
        }

        #endregion

        #region INamedEntity Members

        /// <summary>
        /// 用户名
        /// </summary>
        [StringLength(20)]
        public string Name { get; set; }

        [StringLength(40)]
        public string DisplayName { get; set; }

        #endregion

        #region IUser Members

        public string Photo { get; set; }

        public IEnumerable<IRole> GetRoles()
        {
            return Roles.ToList();
        }

        #endregion
        /// <summary>
        /// 密码加密
        /// </summary>
        /// <param name="password"></param>
        public void SetPassword(string password)
        {
            PasswordSalt = Guid.NewGuid().ToString("N").Substring(0, 8);
            Password = SaltAndHash(password, PasswordSalt);
        }
        public string GetPassword(string password)
        {
            return SaltAndHash(password, PasswordSalt);
        }

        public static string SaltAndHash(string rawString, string salt)
        {
            byte[] salted = Encoding.UTF8.GetBytes(string.Concat(rawString, salt));

            SHA256 hasher = new SHA256Managed();
            byte[] hashed = hasher.ComputeHash(salted);

            return Convert.ToBase64String(hashed);
        }
    }
    
}