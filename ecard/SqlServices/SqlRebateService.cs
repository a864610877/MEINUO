using Ecard.Models;
using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.SqlServices
{
    public class SqlRebateService : IRebateService
    {
         private readonly DatabaseInstance _databaseInstance;
         private readonly IAccountService IAccountService;
         private readonly ISiteService ISiteService;
         private readonly IOperationAmountLogsService IOperationAmountLogsService;
         private readonly ILog4netService ILog4netService;
         private const string TableName = "fz_Rebate";

         public SqlRebateService(DatabaseInstance databaseInstance, IAccountService IAccountService, ISiteService ISiteService
             , IOperationAmountLogsService IOperationAmountLogsService, ILog4netService ILog4netService)
         {
             _databaseInstance = databaseInstance;
             this.ISiteService = ISiteService;
             this.IAccountService = IAccountService;
             this.IOperationAmountLogsService = IOperationAmountLogsService;
             this.ILog4netService = ILog4netService;
         }
        public int Insert(Models.fz_Rebate item)
        {
            return _databaseInstance.Insert(item, TableName);
        }

        public decimal GetRebateAmount(int accountId)
        {
            decimal amount = 0;
            string sql = "select sum(reateAmount) as amount from fz_Rebate where accountId=@accountId and state=1";
            var list = new QueryObject<AmountModel>(_databaseInstance, sql, new { accountId = accountId });
            if (list != null&&list.FirstOrDefault()!=null)
            {
                amount = list.FirstOrDefault().amount;
            }
            return amount;
        }

        public Account SJ(Account account,Site site)
        {
            if (account.grade == AccountGrade.Member || account.grade == AccountGrade.Manager)
            {
                //通过积分升级
                if (account.presentExp >= site.ThreePoint)
                {
                    account.grade = AccountGrade.GoldMedalManager;
                }
                else if (account.presentExp >= site.TwoPoint)
                {
                    account.grade = AccountGrade.Manager;

                }
                //通过人数升级
                if (account.grade == AccountGrade.Member || account.grade == AccountGrade.Manager)
                {
                    string sql = @"select COUNT(t.tatol) as tatol  from (
select userId, COUNT(1) as tatol from fz_Orders where userId in (select userId from fz_Accounts where salerId=@accountId) and amount>@amount and isFirs=1 and orderState in (2,3,4,8)  group by userId) t
";
                    var total = _databaseInstance.ExecuteScalar(sql, new { accountId = account.accountId, amount = site.minAmount });
                    if (total != null)
                    {
                        if (int.Parse(total.ToString()) >= site.ThreePeople)
                        {
                            account.grade = AccountGrade.GoldMedalManager;
                        }
                        else if (int.Parse(total.ToString()) >= site.TwoPeople)
                        {
                            account.grade = AccountGrade.Manager;
                        }
                    }
                }
            }
            return account;
        }
        /// <summary>
        /// 推荐升级
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public int TJSJ(Account account)
        {
            int tatol = IAccountService.GetSalerCount(account.accountId);
            var grade = account.grade;
            if (account.grade == AccountGrade.Member)
            {
                if (tatol >= 5)
                    grade = AccountGrade.Manager;
                if (tatol >= 10)
                    grade = AccountGrade.GoldMedalManager;
            }
            else if (account.grade == AccountGrade.Manager)
            {
                if (tatol >= 5)
                    grade = AccountGrade.GoldMedalManager;
            }
            return grade;
        }
        public bool Rebate(int orderId)
        {
             var Transaction = _databaseInstance.BeginTransaction();
            //try
            //{
               WxPayAPI.Log.Debug("Ecard.SqlServices.SqlRebateService.Rebate", "开始返利");
                var order = _databaseInstance.GetById<Order>("fz_Orders", orderId);
                if (order == null)
                    return false;
                if (order.orderState != OrderStates.complete)
                    return false;
                string sql = "select  * from fz_OrderDetails where OrderId=@OrderId";
                var orderDeal = new QueryObject<OrderDetail>(_databaseInstance, sql, new { OrderId = orderId });
                if (orderDeal == null)
                    return false;
                if (orderDeal.ToList().Count <= 0)
                    return false;
                var account = IAccountService.GetById(order.userId);
                if (account == null)
                    return false;
                Account one = IAccountService.GetById(account.salerId);
                Account two = null;
                Account three = null;
                if (one != null)
                {
                    //WxPayAPI.Log.Debug("Ecard.SqlServices.SqlRebateService.Rebate", "第一层："+one.name);
                    two = IAccountService.GetById(one.salerId);
                }
                if (two != null)
                    three = IAccountService.GetById(two.salerId);
                var site = ISiteService.Query(null).FirstOrDefault();
                if (site == null)
                    return false;
                foreach (var item in orderDeal.ToList())
                {
                    if (one != null)
                    {
                        var oneAmount = item.amount * site.OneRebate;
                       // WxPayAPI.Log.Debug("Ecard.SqlServices.SqlRebateService.Rebate", "第一层金额：" + oneAmount.ToString());
                        if (oneAmount > 0)
                        {
                            one.amount += oneAmount;
                            var fzRebate = new fz_Rebate();
                            fzRebate.accountId = one.accountId;
                            fzRebate.orderDetailId = item.orderDetailId;
                            fzRebate.payAmount = item.amount;
                            fzRebate.reateAmount = oneAmount;
                            fzRebate.reateRatio = site.OneRebate;
                            fzRebate.state = RebateState.normal;
                            fzRebate.submitTime = DateTime.Now;
                            fzRebate.type = RebateType.one;
                            fzRebate.rebateId = _databaseInstance.Insert(fzRebate, "fz_Rebate");
                            if (fzRebate.accountId > 0)
                            {
                                var operationAmountLog = new fz_OperationAmountLogs();
                                operationAmountLog.amount = oneAmount;
                                operationAmountLog.source = fzRebate.rebateId.ToString();
                                operationAmountLog.userId = fzRebate.accountId;
                                operationAmountLog.type = OperationAmountType.Income;
                                operationAmountLog.category = OperationAmountCategory.rebate;
                                operationAmountLog.submitTime = DateTime.Now;
                                if (_databaseInstance.Insert(operationAmountLog, "fz_OperationAmountLogs") <= 0)
                                    return false;
                            }
                            else
                            {
                                return false;
                            }
                            //WxPayAPI.Log.Debug("Ecard.SqlServices.SqlRebateService.Rebate", "第一层返利成功：" + fzRebate.rebateId.ToString());
                        }
                    }
                    if (two != null)
                    {
                        var twoAmount = item.amount * site.TwoRebate;
                        if (twoAmount > 0)
                        {
                            two.amount += twoAmount;
                            var fzRebate = new fz_Rebate();
                            fzRebate.accountId = two.accountId;
                            fzRebate.orderDetailId = item.orderDetailId;
                            fzRebate.payAmount = item.amount;
                            fzRebate.reateAmount = twoAmount;
                            fzRebate.reateRatio = site.TwoRebate;
                            fzRebate.state = RebateState.normal;
                            fzRebate.submitTime = DateTime.Now;
                            fzRebate.type = RebateType.two;
                            fzRebate.rebateId = _databaseInstance.Insert(fzRebate, "fz_Rebate");
                            if (fzRebate.accountId > 0)
                            {
                                var operationAmountLog = new fz_OperationAmountLogs();
                                operationAmountLog.amount = twoAmount;
                                operationAmountLog.source = fzRebate.rebateId.ToString();
                                operationAmountLog.userId = fzRebate.accountId;
                                operationAmountLog.type = OperationAmountType.Income;
                                operationAmountLog.category = OperationAmountCategory.rebate;
                                operationAmountLog.submitTime = DateTime.Now;
                                if (_databaseInstance.Insert(operationAmountLog, "fz_OperationAmountLogs") <= 0)
                                    return false;
                            }
                            else
                            {
                                return false;
                            }
                        }
                      
                    }

                    if (three != null)
                    {
                        var threeAmount = item.amount * site.ThreeRebate;
                        if (threeAmount > 0)
                        {
                            three.amount += threeAmount;
                            var fzRebate = new fz_Rebate();
                            fzRebate.accountId = three.accountId;
                            fzRebate.orderDetailId = item.orderDetailId;
                            fzRebate.payAmount = item.amount;
                            fzRebate.reateAmount = threeAmount;
                            fzRebate.reateRatio = site.ThreeRebate;
                            fzRebate.state = RebateState.normal;
                            fzRebate.submitTime = DateTime.Now;
                            fzRebate.type = RebateType.three;
                            fzRebate.rebateId = _databaseInstance.Insert(fzRebate, "fz_Rebate");
                            if (fzRebate.accountId > 0)
                            {
                                var operationAmountLog = new fz_OperationAmountLogs();
                                operationAmountLog.amount = threeAmount;
                                operationAmountLog.source = fzRebate.rebateId.ToString();
                                operationAmountLog.userId = fzRebate.accountId;
                                operationAmountLog.type = OperationAmountType.Income;
                                operationAmountLog.category = OperationAmountCategory.rebate;
                                operationAmountLog.submitTime = DateTime.Now;
                                if (_databaseInstance.Insert(operationAmountLog, "fz_OperationAmountLogs") <= 0)
                                    return false;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    item.IsRebate = true;
                    if (_databaseInstance.Update(item, "fz_OrderDetails") <= 0)
                        return false;
                }
                if (one != null)
                    _databaseInstance.Update(one, "fz_Accounts");
                if(two!=null)
                    _databaseInstance.Update(two, "fz_Accounts");
                if(three!=null)
                    _databaseInstance.Update(three, "fz_Accounts");

                Transaction.Commit();
                Transaction.Dispose();
                return true;
            //}
            //catch (Exception ex)
            //{
            //    ILog4netService.Insert(ex);
            //    Transaction.Rollback();
            //    return false;
            //}
            //finally
            //{
            //    Transaction.Dispose();
            //}
        }
        public bool Rebate2(int orderId)
        {
            var Transaction = _databaseInstance.BeginTransaction();
            try
            {
                WxPayAPI.Log.Debug("Ecard.SqlServices.SqlRebateService.Rebate", orderId+"开始返利");
                var order = _databaseInstance.GetById<Order>("fz_Orders", orderId);
                if (order == null)
                {
                    Transaction.Rollback();
                    return false;
                }
                if (order.orderState != OrderStates.paid&&order.IsRebate==false)
                {
                    Transaction.Rollback();
                    return false;
                }
                var site = ISiteService.Query(null).FirstOrDefault();
                if (site == null)
                {
                    Transaction.Rollback();
                    return false;
                }
                decimal accountRate = 0;
                var account =IAccountService.GetByUserId(order.userId);
                if (account == null)
                {
                    Transaction.Rollback();
                    return false;
                }
                if (account.grade == AccountGrade.not)
                {
                    account.grade = AccountGrade.Member;
                    _databaseInstance.Update(account, "fz_Accounts");
                }
                else
                {
                    if (account.grade == AccountGrade.Member)
                        accountRate = site.OneRebate;
                    else if(account.grade==AccountGrade.Manager)
                        accountRate = site.TwoRebate;
                    else if(account.grade==AccountGrade.GoldMedalManager)
                        accountRate = site.ThreeRebate;
                    decimal accountPoint = order.point * accountRate;
                    if (accountPoint > 0)
                    {
                        account.activatePoint += accountPoint;
                        account.presentExp += accountPoint;
                        var fzRebate = new fz_Rebate();
                        fzRebate.accountId = account.accountId;
                        fzRebate.orderDetailId = order.orderId;
                        fzRebate.payAmount = order.point;
                        fzRebate.reateAmount = accountPoint;
                        fzRebate.reateRatio = accountRate;
                        fzRebate.state = RebateState.normal;
                        fzRebate.submitTime = DateTime.Now;
                        fzRebate.type = RebateType.zero;
                        fzRebate.rebateId = _databaseInstance.Insert(fzRebate, "fz_Rebate");
                        if (fzRebate.rebateId > 0)
                        {
                            var operationAmountLog = new fz_OperationAmountLogs();
                            operationAmountLog.amount = accountPoint;
                            operationAmountLog.source = fzRebate.rebateId.ToString();
                            operationAmountLog.userId = fzRebate.accountId;
                            operationAmountLog.type = OperationAmountType.Income;
                            operationAmountLog.category = OperationAmountCategory.rebate;
                            operationAmountLog.submitTime = DateTime.Now;
                            if (_databaseInstance.Insert(operationAmountLog, "fz_OperationAmountLogs") <= 0)
                            {
                                Transaction.Rollback();
                                return false;
                            }
                        }
                        else
                        {
                            Transaction.Rollback();
                            return false;
                        }

                        #region 升级
                        SJ(account, site);
                        #endregion
                        _databaseInstance.Update(account, "fz_Accounts");
                    }
                }
                Account one = IAccountService.GetById(account.salerId);
                Account two = null;
                decimal twoRebat = 0;
                Account three = null;
                //上级必须是会员以上才给返利
                if (one != null&&one.grade!=AccountGrade.not)
                {
                    #region 一级返利
                    decimal rebate = 0;
                    if (one.grade == AccountGrade.Manager)
                    {
                        rebate = site.TwoRebate - accountRate;
                        two = IAccountService.GetById(one.salerId);
                        if (two != null && two.grade == AccountGrade.GoldMedalManager)
                            twoRebat = site.ThreeRebate - accountRate-rebate; //> site.TwoRebate ? accountRate : site.TwoRebate;
                    }
                    else if (one.grade == AccountGrade.GoldMedalManager)
                    {
                        rebate = site.ThreeRebate-accountRate;
                    } 
                    else
                    {
                        rebate = site.OneRebate - accountRate;
                        two = IAccountService.GetById(one.salerId);
                        if (two!=null&&two.grade == AccountGrade.GoldMedalManager)
                            twoRebat = site.ThreeRebate - accountRate - rebate;
                        else if (two != null && two.grade == AccountGrade.Manager)
                            twoRebat = site.TwoRebate - accountRate - rebate;
                    }
                    var onePoint = rebate * order.point;
                    if (onePoint > 0)
                    {
                        one.presentExp += onePoint;
                        one.activatePoint += onePoint;
                        var fzRebate = new fz_Rebate();
                        fzRebate.accountId = one.accountId;
                        fzRebate.orderDetailId = order.orderId;
                        fzRebate.payAmount = order.point;
                        fzRebate.reateAmount = onePoint;
                        fzRebate.reateRatio = rebate;
                        fzRebate.state = RebateState.normal;
                        fzRebate.submitTime = DateTime.Now;
                        fzRebate.type = RebateType.one;
                        fzRebate.rebateId = _databaseInstance.Insert(fzRebate, "fz_Rebate");
                        if (fzRebate.rebateId > 0)
                        {
                            var operationAmountLog = new fz_OperationAmountLogs();
                            operationAmountLog.amount = onePoint;
                            operationAmountLog.source = fzRebate.rebateId.ToString();
                            operationAmountLog.userId = fzRebate.accountId;
                            operationAmountLog.type = OperationAmountType.Income;
                            operationAmountLog.category = OperationAmountCategory.rebate;
                            operationAmountLog.submitTime = DateTime.Now;
                            if (_databaseInstance.Insert(operationAmountLog, "fz_OperationAmountLogs") <= 0)
                            {
                                Transaction.Rollback();
                                return false;
                            }
                        }
                        else
                        {
                            Transaction.Rollback();
                            return false;
                        }
                        //
                        #region 升级
                        SJ(one, site);
                        #endregion
                        _databaseInstance.Update(one, "fz_Accounts");
                        WxPayAPI.Log.Debug("Ecard.SqlServices.SqlRebateService.Rebate", orderId + "一级返利结束");
                    }
                    #endregion
                }
                if (two != null && twoRebat > 0)
                {
                    #region 二级返利
                    var twoPoint = order.point * twoRebat;
                    if (twoPoint > 0)
                    {
                        two.presentExp += twoPoint;
                        two.activatePoint += twoPoint;
                        var fzRebate = new fz_Rebate();
                        fzRebate.accountId = two.accountId;
                        fzRebate.orderDetailId = order.orderId;
                        fzRebate.payAmount = order.point;
                        fzRebate.reateAmount = twoPoint;
                        fzRebate.reateRatio = twoRebat;
                        fzRebate.state = RebateState.normal;
                        fzRebate.submitTime = DateTime.Now;
                        fzRebate.type = RebateType.two;
                        fzRebate.rebateId = _databaseInstance.Insert(fzRebate, "fz_Rebate");
                        if (fzRebate.rebateId > 0)
                        {
                            var operationAmountLog = new fz_OperationAmountLogs();
                            operationAmountLog.amount = twoPoint;
                            operationAmountLog.source = fzRebate.rebateId.ToString();
                            operationAmountLog.userId = fzRebate.accountId;
                            operationAmountLog.type = OperationAmountType.Income;
                            operationAmountLog.category = OperationAmountCategory.rebate;
                            operationAmountLog.submitTime = DateTime.Now;
                            if (_databaseInstance.Insert(operationAmountLog, "fz_OperationAmountLogs") <= 0)
                            {
                                Transaction.Rollback();
                                return false;
                            }
                        }
                        else
                        {
                            Transaction.Rollback();
                            return false;
                        }
                        _databaseInstance.Update(two, "fz_Accounts");
                        WxPayAPI.Log.Debug("Ecard.SqlServices.SqlRebateService.Rebate", orderId + "二级返利结束");
                    }
                    #endregion
                }
                order.IsRebate = true;
                _databaseInstance.Update(order, "fz_Orders");
                WxPayAPI.Log.Debug("Ecard.SqlServices.SqlRebateService.Rebate", orderId + "返利结束");
                Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Transaction.Rollback();
                WxPayAPI.Log.Debug("Ecard.SqlServices.SqlRebateService.Rebate", "返利出错：" + ex.Message);
                return false;
            }
            finally
            {
                Transaction.Dispose();
            }
            
        }

        public bool Rebate3(int orderId)
        {
            var Transaction = _databaseInstance.BeginTransaction();
            try
            {
                WxPayAPI.Log.Debug("Ecard.SqlServices.SqlRebateService.Rebate3", orderId + "开始返利");
                var order = _databaseInstance.GetById<Order>("fz_Orders", orderId);
                if (order == null)
                {
                    Transaction.Rollback();
                    return false;
                }
                if (order.orderState != OrderStates.paid && order.IsRebate == false)
                {
                    Transaction.Rollback();
                    return false;
                }
                string sql = "select * from fz_Accounts where userId=@userId";
                var account = _databaseInstance.Query<Account>(sql, new { userId = order.userId }).FirstOrDefault();
                if (account == null)
                {
                    Transaction.Rollback();
                    return false;
                }
                order.IsRebate = true;
                _databaseInstance.Update(order, "fz_Orders");
                #region 自己返利
                decimal accountRate = 0;//自己返利比例
                if (account.grade == AccountGrade.not)
                    accountRate = 0;
                else if (account.grade == AccountGrade.Member)
                    accountRate = 0.5m;
                else if (account.grade == AccountGrade.Manager)
                    accountRate = 0.6m;
                else if (account.grade == AccountGrade.GoldMedalManager)
                    accountRate = 0.7m;
                decimal accountPoint = (order.point * accountRate);
                if (accountPoint > 0)
                {
                    account.activatePoint += accountPoint;
                    account.presentExp += accountPoint;
                    _databaseInstance.Update(account, "fz_Accounts");
                    var fzRebate = new fz_Rebate();
                    fzRebate.accountId = account.accountId;
                    fzRebate.orderDetailId = order.orderId;
                    fzRebate.payAmount = order.point;
                    fzRebate.reateAmount = accountPoint;
                    fzRebate.reateRatio = accountRate;
                    fzRebate.state = RebateState.normal;
                    fzRebate.submitTime = DateTime.Now;
                    fzRebate.type = RebateType.gw;
                    fzRebate.rebateId = _databaseInstance.Insert(fzRebate, "fz_Rebate");
                    if (fzRebate.rebateId > 0)
                    {
                        var operationAmountLog = new fz_OperationAmountLogs();
                        operationAmountLog.amount = accountPoint;
                        operationAmountLog.source = fzRebate.rebateId.ToString();
                        operationAmountLog.userId = fzRebate.accountId;
                        operationAmountLog.type = OperationAmountType.Income;
                        operationAmountLog.category = OperationAmountCategory.rebate;
                        operationAmountLog.submitTime = DateTime.Now;
                        if (_databaseInstance.Insert(operationAmountLog, "fz_OperationAmountLogs") <= 0)
                        {
                            Transaction.Rollback();
                            return false;
                        }
                    }
                    else
                    {
                        Transaction.Rollback();
                        return false;
                    }
                }
                #endregion
                #region 直推返利
                Account one = IAccountService.GetById(account.salerId);//直推
                if (one == null)
                {
                    WxPayAPI.Log.Debug("Ecard.SqlServices.SqlRebateService.Rebate3", orderId + "返利结束");
                    Transaction.Commit();
                    return true;
                }
                decimal oneRate = 0.2m;
                if (accountRate == 0)
                {
                    if (one.grade == AccountGrade.not)
                        oneRate = 0;
                    else if (one.grade == AccountGrade.Member)
                        oneRate = 0.5m;
                    else if (one.grade == AccountGrade.GoldMedalManager)
                        oneRate = 0.6m;
                    else if (one.grade == AccountGrade.GoldMedalManager)
                        oneRate = 0.7m;
                }
                decimal onePoint = oneRate * order.point;
                if (onePoint > 0)
                {
                    one.activatePoint += onePoint;
                    one.presentExp += onePoint;
                    _databaseInstance.Update(one, "fz_Accounts");
                    var fzRebate = new fz_Rebate();
                    fzRebate.accountId = one.accountId;
                    fzRebate.orderDetailId = order.orderId;
                    fzRebate.payAmount = order.point;
                    fzRebate.reateAmount = onePoint;
                    fzRebate.reateRatio = oneRate;
                    fzRebate.state = RebateState.normal;
                    fzRebate.submitTime = DateTime.Now;
                    fzRebate.type = RebateType.gw;
                    fzRebate.rebateId = _databaseInstance.Insert(fzRebate, "fz_Rebate");
                    if (fzRebate.rebateId > 0)
                    {
                        var operationAmountLog = new fz_OperationAmountLogs();
                        operationAmountLog.amount = onePoint;
                        operationAmountLog.source = fzRebate.rebateId.ToString();
                        operationAmountLog.userId = fzRebate.accountId;
                        operationAmountLog.type = OperationAmountType.Income;
                        operationAmountLog.category = OperationAmountCategory.rebate;
                        operationAmountLog.submitTime = DateTime.Now;
                        if (_databaseInstance.Insert(operationAmountLog, "fz_OperationAmountLogs") <= 0)
                        {
                            Transaction.Rollback();
                            return false;
                        }
                    }
                    else
                    {
                        Transaction.Rollback();
                        return false;
                    }
                }
                #endregion
                #region 间推返利返利
                Account two = IAccountService.GetById(one.salerId);//直推
                if (two == null)
                {
                    WxPayAPI.Log.Debug("Ecard.SqlServices.SqlRebateService.Rebate3", orderId + "返利结束");
                    Transaction.Commit();
                    return true;
                }
                decimal twoRate = 0.1m;
                if (two.grade == AccountGrade.not)//如果不是会员则没有返利
                    twoRate = 0;
                decimal twoPoint = twoRate * order.point;
                if (twoPoint > 0)
                {
                    two.activatePoint += twoPoint;
                    two.presentExp += twoPoint;
                    _databaseInstance.Update(two, "fz_Accounts");
                    var fzRebate = new fz_Rebate();
                    fzRebate.accountId = two.accountId;
                    fzRebate.orderDetailId = order.orderId;
                    fzRebate.payAmount = order.point;
                    fzRebate.reateAmount = twoPoint;
                    fzRebate.reateRatio = twoRate;
                    fzRebate.state = RebateState.normal;
                    fzRebate.submitTime = DateTime.Now;
                    fzRebate.type = RebateType.gw;
                    fzRebate.rebateId = _databaseInstance.Insert(fzRebate, "fz_Rebate");
                    if (fzRebate.rebateId > 0)
                    {
                        var operationAmountLog = new fz_OperationAmountLogs();
                        operationAmountLog.amount = twoPoint;
                        operationAmountLog.source = fzRebate.rebateId.ToString();
                        operationAmountLog.userId = fzRebate.accountId;
                        operationAmountLog.type = OperationAmountType.Income;
                        operationAmountLog.category = OperationAmountCategory.rebate;
                        operationAmountLog.submitTime = DateTime.Now;
                        if (_databaseInstance.Insert(operationAmountLog, "fz_OperationAmountLogs") <= 0)
                        {
                            Transaction.Rollback();
                            return false;
                        }
                    }
                    else
                    {
                        Transaction.Rollback();
                        return false;
                    }
                }
                #endregion
                Transaction.Commit();
                WxPayAPI.Log.Debug("Ecard.SqlServices.SqlRebateService.Rebate3", orderId + "返利结束");
                return true;
            }
            catch (Exception ex)
            {
                Transaction.Rollback();
                WxPayAPI.Log.Debug("Ecard.SqlServices.SqlRebateService.Rebate3", "返利出错：" + ex.Message);
                return false;
            }
            finally
            {
                Transaction.Dispose();
            }
            
        }

        public bool Rebate4(int orderId)
        {
            var Transaction = _databaseInstance.BeginTransaction();
            WxPayAPI.Log.Debug("Ecard.SqlServices.SqlRebateService.Rebate4", orderId + "开始返利");
            var order = _databaseInstance.GetById<PayOrder>("PayOrder", orderId);
            if (order == null)
            {
                Transaction.Rollback();
                return false;
            }
            if (order.orderState != OrderStates.paid && order.IsRebate == false)
            {
                Transaction.Rollback();
                return false;
            }
            string item = order.item;
            string sql = "select * from fz_Accounts where userId=@userId";
            var account = _databaseInstance.Query<Account>(sql, new { userId = order.userId }).FirstOrDefault();
            if (account == null)
            {
                Transaction.Rollback();
                return false;
            }
            order.IsRebate = true;
            _databaseInstance.Update(order, "PayOrder");
            #region 一级返利
            decimal oneAmount = 0;//一级返利金额
            var one = IAccountService.GetById(account.salerId);//推荐人
            if (one == null)
            {
                WxPayAPI.Log.Debug("Ecard.SqlServices.SqlRebateService.Rebate4", orderId + "返利结束");
                Transaction.Commit();
                return true;
            }
            if (item == PayOrderItems.member)//购买会员
            {
                if (one.grade == AccountGrade.Member)//推荐人是会员
                    oneAmount = 48;
                else if (one.grade == AccountGrade.Manager)//推荐人是店长
                    oneAmount = 53;
                else if (one.grade == AccountGrade.GoldMedalManager)//推荐人是店主
                    oneAmount = 59;
            }
            else if (item == PayOrderItems.shopowner)//购买店长
            {
                if (one.grade == AccountGrade.Member)//推荐人是会员
                    oneAmount = 96;
                else if (one.grade == AccountGrade.Manager)//推荐人是店长
                    oneAmount = 107;
                else if (one.grade == AccountGrade.GoldMedalManager)//推荐人是店主
                    oneAmount = 119;
            }
            else if (item == PayOrderItems.shopkeeper)//购买店主
            {
                if (one.grade == AccountGrade.Member)//推荐人是会员
                    oneAmount = 143;
                else if (one.grade == AccountGrade.Manager)//推荐人是店长
                    oneAmount = 160;
                else if (one.grade == AccountGrade.GoldMedalManager)//推荐人是店主
                    oneAmount = 179;
            }
            else
            {
                 
            }
            if (oneAmount > 0)
            {
                one.grade=TJSJ(one);
                one.activatePoint += oneAmount;
                one.presentExp += oneAmount;
                _databaseInstance.Update(one, "fz_Accounts");
                var fzRebate = new fz_Rebate();
                fzRebate.accountId = one.accountId;
                fzRebate.orderDetailId = order.Id;
                fzRebate.payAmount = order.amount;
                fzRebate.reateAmount = oneAmount;
                fzRebate.reateRatio = 0;
                fzRebate.state = RebateState.normal;
                fzRebate.submitTime = DateTime.Now;
                fzRebate.type = RebateType.tj;
                fzRebate.rebateId = _databaseInstance.Insert(fzRebate, "fz_Rebate");
                if (fzRebate.rebateId > 0)
                {
                    var operationAmountLog = new fz_OperationAmountLogs();
                    operationAmountLog.amount = oneAmount;
                    operationAmountLog.source = fzRebate.rebateId.ToString();
                    operationAmountLog.userId = fzRebate.accountId;
                    operationAmountLog.type = OperationAmountType.Income;
                    operationAmountLog.category = OperationAmountCategory.rebate;
                    operationAmountLog.submitTime = DateTime.Now;
                    if (_databaseInstance.Insert(operationAmountLog, "fz_OperationAmountLogs") <= 0)
                    {
                        Transaction.Rollback();
                        return false;
                    }
                }
                else
                {
                    Transaction.Rollback();
                    return false;
                }
            }
            #endregion
            #region 二级返利
            decimal twoAmount = 0;//二级返利金额
            var two = IAccountService.GetById(one.salerId);
            if (two == null)
            {
                WxPayAPI.Log.Debug("Ecard.SqlServices.SqlRebateService.Rebate4", orderId + "返利结束");
                Transaction.Commit();
                return true;
            }
            if (item == PayOrderItems.member)
            {
                if (two.grade == AccountGrade.Member)
                    twoAmount = 18;
                else if (two.grade == AccountGrade.Manager)
                    twoAmount = 20;
                else if (two.grade == AccountGrade.GoldMedalManager)
                    twoAmount = 23;
            }
            else if (item == PayOrderItems.shopowner)
            {
                if (two.grade == AccountGrade.Member)
                    twoAmount = 25;
                else if (two.grade == AccountGrade.Manager)
                    twoAmount = 30;
                else if (two.grade == AccountGrade.GoldMedalManager)
                    twoAmount = 48;
            }
            else if (item == PayOrderItems.shopkeeper)
            {
                if (one.grade == AccountGrade.Member)
                    oneAmount = 36;
                else if (one.grade == AccountGrade.Manager)
                    oneAmount = 46;
                else if (one.grade == AccountGrade.GoldMedalManager)
                    oneAmount = 70;
            }
            else
            {

            }
            if (twoAmount > 0)
            {
                two.activatePoint += twoAmount;
                two.presentExp += twoAmount;
                _databaseInstance.Update(two, "fz_Accounts");
                var fzRebate = new fz_Rebate();
                fzRebate.accountId = two.accountId;
                fzRebate.orderDetailId = order.Id;
                fzRebate.payAmount = order.amount;
                fzRebate.reateAmount = twoAmount;
                fzRebate.reateRatio = 0;
                fzRebate.state = RebateState.normal;
                fzRebate.submitTime = DateTime.Now;
                fzRebate.type = RebateType.tj;
                fzRebate.rebateId = _databaseInstance.Insert(fzRebate, "fz_Rebate");
                if (fzRebate.rebateId > 0)
                {
                    var operationAmountLog = new fz_OperationAmountLogs();
                    operationAmountLog.amount = twoAmount;
                    operationAmountLog.source = fzRebate.rebateId.ToString();
                    operationAmountLog.userId = fzRebate.accountId;
                    operationAmountLog.type = OperationAmountType.Income;
                    operationAmountLog.category = OperationAmountCategory.rebate;
                    operationAmountLog.submitTime = DateTime.Now;
                    if (_databaseInstance.Insert(operationAmountLog, "fz_OperationAmountLogs") <= 0)
                    {
                        Transaction.Rollback();
                        return false;
                    }
                }
                else
                {
                    Transaction.Rollback();
                    return false;
                }
            }
            #endregion
            #region 三级返利
            decimal threeAmount = 0;
            var three = IAccountService.GetById(two.salerId);
            if (three == null)
            {
                WxPayAPI.Log.Debug("Ecard.SqlServices.SqlRebateService.Rebate4", orderId + "返利结束");
                Transaction.Commit();
                return true;
            }
            if (three.grade == AccountGrade.Member)
                threeAmount = 2;
            else if (three.grade == AccountGrade.Manager)
                threeAmount = 4;
            else if (three.grade == AccountGrade.GoldMedalManager)
                threeAmount = 5;
            if (threeAmount > 0)
            {
                three.activatePoint += threeAmount;
                three.presentExp += threeAmount;
                _databaseInstance.Update(three, "fz_Accounts");
                var fzRebate = new fz_Rebate();
                fzRebate.accountId = three.accountId;
                fzRebate.orderDetailId = order.Id;
                fzRebate.payAmount = order.amount;
                fzRebate.reateAmount = threeAmount;
                fzRebate.reateRatio = 0;
                fzRebate.state = RebateState.normal;
                fzRebate.submitTime = DateTime.Now;
                fzRebate.type = RebateType.tj;
                fzRebate.rebateId = _databaseInstance.Insert(fzRebate, "fz_Rebate");
                if (fzRebate.rebateId > 0)
                {
                    var operationAmountLog = new fz_OperationAmountLogs();
                    operationAmountLog.amount = threeAmount;
                    operationAmountLog.source = fzRebate.rebateId.ToString();
                    operationAmountLog.userId = fzRebate.accountId;
                    operationAmountLog.type = OperationAmountType.Income;
                    operationAmountLog.category = OperationAmountCategory.rebate;
                    operationAmountLog.submitTime = DateTime.Now;
                    if (_databaseInstance.Insert(operationAmountLog, "fz_OperationAmountLogs") <= 0)
                    {
                        Transaction.Rollback();
                        return false;
                    }
                }
                else
                {
                    Transaction.Rollback();
                    return false;
                }
            }
            #endregion
            Transaction.Commit();
            WxPayAPI.Log.Debug("Ecard.SqlServices.SqlRebateService.Rebate4", orderId + "返利结束");
            return true;
        }
    }
}
