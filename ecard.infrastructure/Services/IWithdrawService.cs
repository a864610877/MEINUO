using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Services
{
    public interface IWithdrawService
    {
        int Insert(Withdraw item);
        int Update(Withdraw item);
        int Delete(Withdraw item);

        Withdraw GetById(int id);

        DataTables<WithdrawModel> Query(WithdrawRequest request);
        DataTables<Withdraw> Query(UserWithdrawRequest request, string openId);

        decimal GetUserIdPoint(int userId);
    }

    public class WithdrawRequest : PageRequest
    {

        public string Operator { get; set; }
        public int? state { get; set; }

        public int? UserId { get; set; }

        public DateTime? startTime { get; set; }

        public DateTime? endTime { get; set; }

    }

    public class UserWithdrawRequest : PageRequest
    {


    }
}
