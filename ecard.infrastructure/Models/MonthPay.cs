using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ecard.Models;

namespace Oxite.Model
{
    #region MonthPay
    /// <summary>
    /// This object represents the properties and methods of a MonthPay.
    /// </summary>
    public partial class MonthPay
    {

        #region Public Properties
        public int MonthPayId { get; set; }

        public decimal PayAmount { get; set; }

        public int Month { get; set; }

        public decimal Amount { get; set; }

        public int AccountId { get; set; }

        public string AccountName { get; set; }

        public int State { get; set; }
        public int AccountType { get; set; }
        #endregion

    }
    #endregion 

    
    public partial class MonthPay : IEntityBase
    {
        public MonthPay()
        {
            State = States.Normal;
        }
        public int Id
        {
            get
            {
                return MonthPayId;
            }
            set
            {
                MonthPayId = value;
            }
        }
    }
}
