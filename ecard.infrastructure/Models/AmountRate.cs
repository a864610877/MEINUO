using System;
using System.ComponentModel.DataAnnotations;

namespace Ecard.Models
{
    public class TokenKeyTypes
    {
        public const int RecoveryPassword = 1;
    }
    public class TemporaryTokenKey
    {
        [Key]
        public int TemporaryTokenKeyId { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public DateTime ExpiredDate { get; set; }
        public int TokenKeyType { get; set; }
    }
   
    public class AccountDependencyTypes
    {
        public const int Manunal = 0;
        public const int BirthDate = 1;
        public const int Weekly = 2;
        public const int Day = 4;
        public const int EveryDay = 8;
    }
    public interface IAccountDependency
    {
        int DependencyType { get; set; }
        string WeekDays { get; set; }
        string Days { get; set; }
        string AccountLevels { get; set; }
    }
}