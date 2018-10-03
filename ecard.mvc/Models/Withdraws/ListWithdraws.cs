using Ecard.Models;
using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using WxPayAPI;

namespace Ecard.Mvc.Models.Withdraws
{
    public class ListWithdraws : EcardModelListRequest<ListWithdraw>
    {
        [Dependency]
        [NoRender]
        public SecurityHelper SecurityHelper { get; set; }
        [Dependency]
        [NoRender]
        public IOperationPointLogService OperationPointLogService { get; set; }
        [Dependency]
        [NoRender]
        public IAccountService AccountService { get; set; }
        [Dependency]
        [NoRender]
        public IOperationAmountLogsService OperationAmountLogsService { get; set; }
        [Dependency]
        [NoRender]
        public IMessagesService IMessagesService { get; set; }

        [Dependency, NoRender]
        public TransactionHelper transaction { get; set; }


        public string Operator { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        private Bounded _state;

        public Bounded state
        {
            get
            {
                if (_state == null)
                    _state = Bounded.Create<Withdraw>("state", WithdrawStates.notaudit);
                return _state;
            }
            set { _state = value; }
        }

        public DateTime? startTime { get; set; }

        public DateTime? endTime { get; set; }

        [Dependency]
        [NoRender]
        public IWithdrawService withdrawService { get; set; }
        public IEnumerable<ActionMethodDescriptor> GetItemToobalActions(ListWithdraw item)
        {
            yield return new ActionMethodDescriptor("Audit", null, new { id = item.withdrawId });
        }


        public void Query()
        {
            var request = new WithdrawRequest();
            request.state = WithdrawStates.notaudit;
            var query = withdrawService.Query(request);
            if (query != null)
            {
                List = query.ModelList.Select(x => new ListWithdraw(x)).ToList();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, query.TotalCount);
            }
            else
            {
                List = new List<ListWithdraw>();
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, 0);
            }
        }


        public List<ListWithdraw> AjaxQuery(WithdrawRequest request)
        {
            if (request.state == WithdrawStates.All)
                request.state = null;
            var data = new List<ListWithdraw>();
            var query = withdrawService.Query(request);
            if (query != null)
            {
                data = query.ModelList.Select(x => new ListWithdraw(x)).ToList();
                foreach (var item in data)
                {
                    item.boor += "<a href='#' onclick=OperatorThis('Audit','/Withdraw/Audit/" + item.withdrawId + "') class='tablelink'>详情 &nbsp;</a> ";
                }
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, query.TotalCount);

            }
            else
            {
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, 0);
            }
            return data;
        }


        public ResultMsg Agree(int id, string remark)
        {
            //ResultMsg result = new ResultMsg();
            var user = SecurityHelper.GetCurrentUser();
            if (user == null)
                return new ResultMsg() { Code = -1, CodeText = "您还没登录" };
            var withdraw = withdrawService.GetById(id);
            if (withdraw != null&&withdraw.state==WithdrawStates.notaudit)
            {
                //JsApiPay jsApiPay = new JsApiPay();
                //jsApiPay.openid = withdraw.openId;
                //jsApiPay.total_fee = (int)(withdraw.amount * 100);
                try
                {
                    //var orderNo = WxPayApi.GenerateOutTradeNo();
                    //WxPayData unifiedResult = jsApiPay.GetUnifiedTransferResult(orderNo, "", "提现");
                    //if (!unifiedResult.GetValue("return_code").Equals("SUCCESS"))
                    //{
                    //    WxPayAPI.Log.Error(this.GetType().ToString(), "UnifiedTransfer response error!");
                    //    return new ResultMsg() { Code = -1, CodeText = "审核失败" };
                    //}
                    //if (!unifiedResult.IsSet("payment_no"))//(!unifiedResult.IsSet("mch_appid") || !unifiedResult.IsSet("mchid"))
                    //{
                    //    WxPayAPI.Log.Error(this.GetType().ToString(), "UnifiedTransfer response error!");
                    //    return new ResultMsg() { Code = -1, CodeText = "审核失败" };
                    //}
                    withdraw.state = WithdrawStates.success;
                    withdraw.orderNo = "";
                    withdraw.Operator = user.CurrentUser.Name;
                    withdraw.remark = remark;
                    withdrawService.Update(withdraw);
                    //var account = AccountService.GetByUserId(withdraw.userId);
                    //if (!string.IsNullOrWhiteSpace(account.openID))
                    //{
                    //    var message = new Fz_Messages();
                    //    message.accountId = account.accountId;
                    //    message.openId = account.openID;
                    //    message.state = MessagesState.staySend;
                    //    message.submitTime = DateTime.Now;
                    //    message.msg = account.name + ",恭喜您的提现已审核通过"; ;
                    //    message.keyword1 = withdraw.amount.ToString();
                    //    message.keyword2 = "微信提现";
                    //    message.keyword3 = withdraw.submitTime.ToString();
                    //    message.keyword4 = "审核通过";
                    //    message.keyword5 = DateTime.Now.ToString();
                    //    message.msgType = MsgType.withdrawReview;
                    //    IMessagesService.Insert(message);
                    //}
                    return new ResultMsg() { Code = 0 };
                }
                catch (Exception ex)
                {
                    WxPayAPI.Log.Error(this.GetType().ToString(), ex.Message.ToString());
                    return new ResultMsg() { Code = -1,CodeText=ex.Message.ToString() };
                }
            }
            return new ResultMsg() { Code = -1,CodeText="申请不存在或者已审核" };
        }

        public ResultMsg NotAgree(int id, string remark)
        {
            var tran= transaction.BeginTransaction();
            try
            {
                var user = SecurityHelper.GetCurrentUser();
                if (user == null)
                    return new ResultMsg() { Code = -1, CodeText = "您还没登录" };
                var withdraw = withdrawService.GetById(id);
                if (withdraw != null && withdraw.state == WithdrawStates.notaudit)
                {
                    var account = AccountService.GetByUserId(withdraw.userId);
                    if (account != null)
                    {
                        //account.amount += withdraw.amount;
                        account.activatePoint += withdraw.point;
                        withdraw.state = WithdrawStates.failure;
                        withdraw.remark = remark;
                        withdraw.Operator = user.CurrentUser.Name;
                        fz_OperationAmountLogs log = new fz_OperationAmountLogs();
                        log.userId = withdraw.userId;
                        log.submitTime = DateTime.Now;
                        log.category = OperationAmountCategory.cancelwithdraw;
                        log.amount = withdraw.point;
                        log.type = OperationAmountType.Income;
                        //OperationPointLog log = new OperationPointLog();
                        //log.account = "activatePoint";
                        //log.point = withdraw.point;
                        //log.remark = "提现失败";
                        //log.submitTime = DateTime.Now;
                        //log.userId = withdraw.userId;
                        
                        AccountService.Update(account);
                        OperationAmountLogsService.Insert(log);
                        withdrawService.Update(withdraw);
                        //if (!string.IsNullOrWhiteSpace(account.openID))
                        //{
                        //    var message = new Fz_Messages();
                        //    message.accountId = account.accountId;
                        //    message.openId = account.openID;
                        //    message.state = MessagesState.staySend;
                        //    message.submitTime = DateTime.Now;
                        //    message.msg = remark;
                        //    message.keyword1 = withdraw.amount.ToString();
                        //    message.keyword2 = "微信提现";
                        //    message.keyword3 = withdraw.submitTime.ToString();
                        //    message.keyword4 = "审核不通过";
                        //    message.keyword5 = DateTime.Now.ToString();
                        //    message.msgType = MsgType.withdrawReview;
                        //    IMessagesService.Insert(message);
                        //}
                        transaction.Commit();
                        return new ResultMsg() { Code = 0 };
                    }
                }
                return new ResultMsg() { Code = -1, CodeText = "申请不存在或者已审核" };
            }
            catch (Exception ex)
            {
                WxPayAPI.Log.Error(this.GetType().ToString(), ex.Message.ToString());
                return new ResultMsg() { Code = -1, CodeText = ex.Message };
            }
            finally
            {
                tran.Dispose();
            }
        }
    }
}
