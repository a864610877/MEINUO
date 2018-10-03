using Ecard;
using Ecard.Models;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Withdraws
{
    public class OperationWithdraw:layouts.LayoutModel
    {
        [Dependency, NoRender]
        public IWithdrawService WithdrawService { get; set; }
         [Dependency, NoRender]
        public IAccountService AccountService { get; set; }
        [Dependency, NoRender]
         public IOperationPointLogService OperationPointLogService { get; set; }
       
         public decimal point { get; set; }

         public decimal presentExp { get; set; }

         public string grade { get; set; }
        public void Ready(int userId)
        {
           // Load();
            
            var account = AccountService.GetByUserId(userId);
            if (account != null)
            {
                point = account.activatePoint;
                presentExp = account.presentExp;
                grade = AccountGrade.GetName(account.grade);
            }

        }

        public ResultMessage Save(int userId)
        {
            if (point <= 0)
                return new ResultMessage() { Code = -1, Msg = "提现积分必须大于0" };
            Account = AccountService.GetByUserId(userId);
            if (Account == null)
                return new ResultMessage() { Code = -1, Msg = "请重新登录" };
            if(Account.activatePoint<point)
                return new ResultMessage() { Code = -1, Msg = "积分余额不足" };
            if (string.IsNullOrWhiteSpace(Account.openID))
                return new ResultMessage() { Code = -1, Msg = "你还未授权，请重新登录" };
            TransactionHelper.BeginTransaction();
            Withdraw model = new Withdraw();
            model.openId = Account.openID;
            model.point = point;
            model.state = WithdrawStates.notaudit;
            model.submitTime = DateTime.Now;
            model.userId = Account.userId;
            model.amount = point;
            WithdrawService.Insert(model);
            OperationPointLog log = new OperationPointLog();
            log.account = "activatePoint";
            log.point = -point;
            log.remark = "提现";
            log.submitTime = DateTime.Now;
            log.userId = Account.userId;
            OperationPointLogService.Insert(log);
            Account.activatePoint -= point;
            //Account.withdrawPoint += point;
            AccountService.Update(Account);
            TransactionHelper.Commit();
            return new ResultMessage(){Code=0};

        }

        
    }
}