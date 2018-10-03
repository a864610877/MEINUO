using System.Linq;
using Ecard.Models;

namespace Ecard
{
    public class PermissionService : IPermissionService
    {
        #region IPermissionRepository Members

        public IQueryable<Permission> QueryPermissions(UserType userType)//UserType userType
        {
            var adminUserType = new[] { UserType.AdminUser, };
            var adminShopType = new[] { UserType.AdminUser, UserType.ShopUser, };
            var shopType = new[] { UserType.AdminUser, UserType.ShopUser, };
            var query = (new[]
                        {
                            new Permission {DisplayName = "用户管理", Category = "用户", Name = Permissions.User , UserTypes = adminUserType}, 
                            new Permission {DisplayName = "用户编辑", Category = "用户",Name = Permissions.UserEdit, UserTypes = adminUserType}, 
                            new Permission {DisplayName = "角色管理", Category = "角色",Name = Permissions.Role, UserTypes = adminUserType}, 
                            new Permission {DisplayName = "角色编辑", Category = "角色",Name = Permissions.RoleEdit, UserTypes = adminUserType}, 
                            new Permission {DisplayName = "角色停用", Category = "角色",Name = Permissions.RoleSuspend, UserTypes = adminUserType}, 
                            new Permission {DisplayName = "角色启用", Category = "角色",Name = Permissions.RoleSume, UserTypes = adminUserType}, 
                            new Permission {DisplayName = "角色删除", Category = "角色",Name = Permissions.RoleDelete, UserTypes = adminUserType}, 

                                                                  
                            //new Permission {DisplayName = "商户管理", Category = "商户", Name = Permissions.Shop, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "商户编辑", Category = "商户", Name = Permissions.ShopEdit, UserTypes = adminUserType}, 

                            //new Permission {DisplayName = "经销商管理", Category = "经销商", Name = Permissions.Distributor, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "经销商编辑", Category = "经销商", Name = Permissions.DistributorEdit, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "经销商提成结算", Category = "经销商", Name = Permissions.DistributorBrokerage, UserTypes = adminUserType}, 

                            //new Permission {DisplayName = "终端管理",     Category = "终端",Name = Permissions.Pos, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "终端编辑",     Category = "终端",Name = Permissions.PosEdit, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "终端密钥查看", Category = "终端",Name = Permissions.PosDataKey, UserTypes = adminUserType},

                            //new Permission {DisplayName = "帐户级别管理",Category = "帐户级别",Name = Permissions.AccountLevel, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "帐户级别编辑",Category = "帐户级别",Name = Permissions.AccountLevelEdit, UserTypes = adminUserType}, 

                            //new Permission {DisplayName = "帐户类别管理",Category = "帐户类别",Name = Permissions.AccountType, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "帐户类别编辑",Category = "帐户类别",Name = Permissions.AccountTypeEdit, UserTypes = adminUserType}, 

                            //new Permission {DisplayName = "积分规则管理",Category = "积分规则",Name = Permissions.PointPolicy, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "积分规则编辑",Category = "积分规则",Name = Permissions.PointPolicyEdit, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "积分日志管理", Category ="积分日志",Name = Permissions.PointRebateLog, UserTypes = adminUserType},

                            //new Permission {DisplayName = "返利管理",Category = "返利", Name = Permissions.PointRebate, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "返利编辑",Category = "返利", Name = Permissions.PointRebateEdit, UserTypes = adminUserType}, 

                            //new Permission {DisplayName = "积分兑换管理",Category = "积分兑换",Name = Permissions.PointGift, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "积分兑换编辑",Category = "积分兑换",Name = Permissions.PointGiftEdit, UserTypes = adminUserType}, 

                            //new Permission {DisplayName = "小票任务管理",Category = "小票任务",Name = Permissions.PrintTicket, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "小票任务删除",Category = "小票任务",Name = Permissions.PrintTicketDelete, UserTypes = adminUserType}, 

                            //new Permission {DisplayName = "清算", Category = "清算",Name = Permissions.Liquidate, UserTypes = adminUserType},
                            //new Permission {DisplayName = "冲正", Category = "冲正",Name = Permissions.Rollback, UserTypes = adminUserType},
                            //new Permission {DisplayName = "申请冲正", Category = "冲正",Name = Permissions.RollbackApply, UserTypes = adminUserType},

                            new Permission {DisplayName = "商品管理", Category = "商品",    Name = Permissions.Commodity, UserTypes = adminUserType}, 
                            new Permission {DisplayName = "商品编辑", Category = "商品",Name = Permissions.CommodityEdit, UserTypes = adminUserType}, 
                            new Permission {DisplayName = "商品添加", Category = "商品",Name = Permissions.CommodityAdd, UserTypes = adminUserType},
                            new Permission {DisplayName = "商品删除", Category = "商品",Name = Permissions.CommodityDelete, UserTypes = adminUserType},
                            new Permission {DisplayName = "商品上架", Category = "商品",Name = Permissions.CommodityPutaway, UserTypes = adminUserType},
                            new Permission {DisplayName = "商品下架", Category = "商品",Name = Permissions.CommoditySoldout, UserTypes = adminUserType},
                            new Permission{DisplayName="订单管理",Category="订单",Name=Permissions.OrderList,UserTypes=adminUserType},
                            new Permission{DisplayName="退款管理",Category="订单",Name=Permissions.ApplyRefundOrderList,UserTypes=adminUserType},
                            new Permission {DisplayName = "广告管理", Category = "广告",Name = Permissions.Ads, UserTypes = adminUserType},
                            new Permission {DisplayName = "添加广告", Category = "广告",Name = Permissions.AdsCreate, UserTypes = adminUserType},
                            new Permission {DisplayName = "删除广告", Category = "广告",Name = Permissions.AdsDelete, UserTypes = adminUserType},
                            new Permission {DisplayName = "编辑广告", Category = "广告",Name = Permissions.AdsEdit, UserTypes = adminUserType},
                            new Permission {DisplayName = "广告上架", Category = "广告",Name = Permissions.AdsPutaway, UserTypes = adminUserType},
                            new Permission {DisplayName = "广告下架", Category = "广告",Name = Permissions.AdsSoldout, UserTypes = adminUserType},
                            new Permission {DisplayName = "评论管理", Category = "评论",Name = Permissions.ListReview, UserTypes = adminUserType},
                            new Permission {DisplayName = "会员管理",    Category = "会员",Name = Permissions.Account, UserTypes = adminUserType},
                            new Permission {DisplayName = "积分记录",    Category = "会员",Name = Permissions.OperationPointLog, UserTypes = adminUserType},
                            new Permission {DisplayName = "提现管理", Category = "提现",Name = Permissions.ListWithdraw, UserTypes = adminUserType}, 
                            new Permission {DisplayName = "提现审核", Category = "提现",Name = Permissions.WithdrawAudit, UserTypes = adminUserType}, 

                             new Permission {DisplayName = "文章列表", Category = "文章",Name = Permissions.ListArticles, UserTypes = adminUserType},
                             new Permission {DisplayName = "发布文章", Category = "文章",Name = Permissions.ArticlesCreate, UserTypes = adminUserType},
                             new Permission {DisplayName = "编辑文章", Category = "文章",Name = Permissions.EditArticles, UserTypes = adminUserType},
                             new Permission {DisplayName = "删除文章", Category = "文章",Name = Permissions.DeleteArticles, UserTypes = adminUserType},
                           
                             new Permission {DisplayName = "级别列表", Category = "级别",Name = Permissions.ListGrades, UserTypes = adminUserType},
                             new Permission {DisplayName = "添加级别", Category = "级别",Name = Permissions.GradesCreate, UserTypes = adminUserType},
                             new Permission {DisplayName = "编辑级别", Category = "级别",Name = Permissions.EditGrades, UserTypes = adminUserType},
                             new Permission {DisplayName = "删除级别", Category = "级别",Name = Permissions.DeleteGrades, UserTypes = adminUserType},

                             new Permission {DisplayName = "销售报表", Category = "报表",Name = Permissions.ListCommoditySales, UserTypes = adminUserType},
                             //new Permission {DisplayName = "积分管理", Category = "积分",Name = Permissions.OperationPointLog, UserTypes = adminUserType},
                            //new Permission {DisplayName = "订单管理", Category = "订单",    Name = Permissions.Order, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "订单修改", Category = "订单",Name = Permissions.OrderEdit, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "订单派送", Category = "订单",    Name = Permissions.OrderCarry, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "完成订单", Category = "订单",Name = Permissions.OrderComplete, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "注销订单", Category = "订单",Name = Permissions.OrderSuspend, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "启用注销的订单", Category = "订单",    Name = Permissions.OrderResume, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "新增订单", Category = "订单",Name = Permissions.OrderCreate, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "删除订单", Category = "订单",Name = Permissions.OrderDelete, UserTypes = adminUserType}, 
                           
                            //new Permission {DisplayName = "支付渠道",Category = "支付渠道",Name = Permissions.DealWay, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "支付渠道编辑",Category = "支付渠道",Name = Permissions.DealWayEdit, UserTypes = adminUserType}, 

                            //new Permission {DisplayName = "现金日志管理",Category = "现金",Name = Permissions.CashDealLog, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "现金日志编辑",Category = "现金",Name = Permissions.CashDealLogEdit, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "现金统计管理",Category = "现金",Name = Permissions.CashDealLogSummary, UserTypes = adminUserType},
                            //new Permission {DisplayName = "现金统计创建",Category = "现金",Name = Permissions.CashDealLogDone, UserTypes = adminUserType},  

                            //new Permission {DisplayName = "系统日志",      Category = "日志",Name = Permissions.Log, UserTypes = adminUserType},
                            //new Permission {DisplayName = "系统交易日志",  Category = "日志",Name = Permissions.SystemDealLog, UserTypes = adminUserType},
                            //new Permission {DisplayName = "会员交易记录",  Category = "日志",Name = Permissions.DealLog, UserTypes = adminUserType},
                            //new Permission {DisplayName = "商户交易记录",  Category = "日志",Name = Permissions.ShopDealLog, UserTypes = adminUserType},
                            //new Permission {DisplayName = "预授权查询",    Category = "日志",Name = Permissions.AccountPrePayList, UserTypes = adminUserType},
                            //new Permission {DisplayName = "预授权强制完成",Category = "日志",Name = Permissions.AccountPrePayDone, UserTypes = adminUserType},
                            //new Permission {DisplayName = "预授权强制取消",Category = "日志",Name = Permissions.AccountPrePayCancel, UserTypes = adminUserType},

                            //new Permission {DisplayName = "充值审核",Category = "审核",Name = Permissions.TaskRecharging, UserTypes = adminUserType},
                            //new Permission {DisplayName = "信用额度审核",Category = "审核",Name = Permissions.TaskLimitAmount, UserTypes = adminUserType},
                             
                            //new Permission {DisplayName = "帐户查询",    Category = "会员事务",Name = Permissions.AccountQuery, UserTypes = adminUserType},
                            //new Permission {DisplayName = "帐户无卡查询",Category = "会员事务",Name = Permissions.AccountQueryWithUserInfo, UserTypes = adminUserType},
                            //new Permission {DisplayName = "帐户查询在线",Category = "会员事务",Name = Permissions.AccountQueryWithoutToken, UserTypes = adminUserType},
                            //new Permission {DisplayName = "初始化",      Category = "会员事务",Name = Permissions.AccountInit, UserTypes = adminUserType},
                            //new Permission {DisplayName = "删除卡",    Category = "会员事务",Name = Permissions.AccountDelete, UserTypes = adminUserType},
                            //new Permission {DisplayName = "导出帐户",    Category = "会员事务",Name = Permissions.AccountReport, UserTypes = adminUserType},
                            //new Permission {DisplayName = "制卡",    Category = "会员事务",        Name = Permissions.AccountCreate, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "审核",Category = "会员事务", Name = Permissions.AccountApprove, UserTypes = adminUserType},
                            //new Permission {DisplayName = "会员卡放卡", Category = "会员事务",Name = Permissions.AccountOpen, UserTypes = adminUserType},
                            //new Permission {DisplayName = "批量开卡",Category = "会员事务", Name = Permissions.AccountOpens, UserTypes = adminUserType},
                            //new Permission {DisplayName = "退卡",Category = "会员事务",Name = Permissions.AccountClose, UserTypes = adminUserType},
                            //new Permission {DisplayName = "换卡",Category = "会员事务",Name = Permissions.AccountChangeName, UserTypes = adminUserType},
                            //new Permission {DisplayName = "挂失",Category = "会员事务",Name = Permissions.AccountSuspend, UserTypes = adminUserType},
                            //new Permission {DisplayName = "编辑会员档案",Category = "会员事务", Name = Permissions.AccountOwner, UserTypes = adminUserType},
                            //new Permission {DisplayName = "取消挂失",Category = "会员事务", Name = Permissions.AccountResume, UserTypes = adminUserType},
                            //new Permission {DisplayName = "返利",Category = "会员事务",  Name = Permissions.AccountRebate, UserTypes = adminUserType},
                            //new Permission {DisplayName = "兑换奖品",Category = "会员事务",  Name = Permissions.AccountGift, UserTypes = adminUserType},
                            //new Permission {DisplayName = "在线交易",Category = "会员事务", Name = Permissions.AccountPay, UserTypes = shopType},
                            //new Permission {DisplayName = "在线撤消", Category = "会员事务",Name = Permissions.AccountCancelPay, UserTypes = shopType},
                            //new Permission {DisplayName = "查看帐户初始化密码",Category = "会员事务", Name = Permissions.AccountInitPassword, UserTypes = adminUserType},
                            //new Permission {DisplayName = "查看帐户识别码", Category = "会员事务",Name = Permissions.AccountToken, UserTypes = adminUserType},
                            //new Permission {DisplayName = "延期", Category = "会员事务",Name = Permissions.AccountRenew, UserTypes = adminUserType},
                            //new Permission {DisplayName = "充值", Category = "会员事务",Name = Permissions.AccountRecharge, UserTypes = adminUserType},
                            //new Permission {DisplayName = "号码段充值", Category = "会员事务",Name = Permissions.AreaRecharing, UserTypes = adminUserType},
                            //new Permission {DisplayName = "信用额度",      Category = "会员事务",Name = Permissions.AccountLimit, UserTypes = adminUserType},
                            //new Permission {DisplayName = "开发票",Category = "会员事务", Name = Permissions.SystemDealLogOpenReceipt, UserTypes = adminUserType},
                            //new Permission {DisplayName = "撤消充值",Category = "会员事务", Name = Permissions.SystemDealLogCloseRecharging, UserTypes = adminUserType},
                            //new Permission {DisplayName = "修改帐户密码", Category = "会员事务",Name = Permissions.AccountChangePassword, UserTypes = adminUserType},
                            //new Permission {DisplayName = "创建U盾", Category = "会员事务",Name = Permissions.CreateDog, UserTypes = adminUserType},
                            new Permission {DisplayName = "系统配置", Category = "系统维护", Name = Permissions.SystemSettings, UserTypes = adminUserType},
                            //new Permission {DisplayName = "系统日志", Category = "系统维护",  Name = Permissions.ReportLogList, UserTypes = adminUserType},
                            //new Permission {DisplayName = "会员短信平台", Category = "系统维护",  Name = Permissions.SystemMessagePanelAccount, UserTypes = adminUserType},
                            // new Permission {DisplayName = "会员短信参数设置", Category = "系统维护",  Name = Permissions.SmsSeting, UserTypes = adminUserType},


                            //new Permission {DisplayName = "会员交易月报",        Category = "报表",   Name = Permissions.ReportAccountMonth, UserTypes = adminUserType},
                            //new Permission {DisplayName = "商户交易统计报表",    Category = "报表",  Name = Permissions.ReportShopDeals, UserTypes = adminUserType},
                            //new Permission {DisplayName = "商户消费统计报表",    Category = "报表",  Name = Permissions.ReportShopDealAccountType, UserTypes = adminUserType},
                            //new Permission {DisplayName = "会员交易统计报表",    Category = "报表",  Name = Permissions.ReportAccountDeals, UserTypes = adminUserType},
                            //new Permission {DisplayName = "店铺消费明细报表",    Category = "报表",  Name = Permissions.ReportAccountDealsList, UserTypes = adminUserType},
                            //new Permission {DisplayName = "系统交易汇总报表",    Category = "报表",  Name = Permissions.ReportSystemDealLogDay, UserTypes = adminUserType},
                            //new Permission {DisplayName = "系统交易用户统计报表",Category = "报表",  Name = Permissions.ReportSystemDealLogByUser, UserTypes = adminUserType},
                            //new Permission {DisplayName = "会员卡汇总报表",      Category = "报表",    Name = Permissions.ReportAccounts, UserTypes = adminUserType},
                            //new Permission {DisplayName = "会员卡余额汇总报表",  Category = "报表",  Name = Permissions.ReportAccounts2, UserTypes = adminUserType},
                            //new Permission {DisplayName = "会员卡余额汇总报表",  Category = "报表",  Name = Permissions.ReportExpiredAccounts, UserTypes = adminUserType},
                            //new Permission {DisplayName = "预授权统计报表",      Category = "报表",  Name = Permissions.ReportPrepayList, UserTypes = adminUserType},
                            //new Permission {DisplayName = "会员卡余额汇总报表",  Category = "报表",  Name = Permissions.ReportAccountDealsForAccount, UserTypes = adminUserType},
                            //new Permission {DisplayName = "充值汇总报表",        Category = "报表",  Name = Permissions.ReportRecharging, UserTypes = adminUserType},
                            //new Permission {DisplayName = "充值明细报表",        Category = "报表",  Name = Permissions.ReportRechargingList, UserTypes = adminUserType},
                            //new Permission {DisplayName = "售卡统计报表",        Category = "报表",  Name = Permissions.ReportSaleAccount, UserTypes = adminUserType},
                            //new Permission {DisplayName = "售卡明细报表",        Category = "报表",  Name = Permissions.ReportSaleAccountList, UserTypes = adminUserType},
                            //new Permission {DisplayName = "系统运营情况汇总",    Category = "报表",  Name = Permissions.ReportAllSystemSummary, UserTypes = adminUserType},
                            new Permission {DisplayName = "秒杀设置", Category = "秒杀",Name = Permissions.SecondKillSet, UserTypes = adminUserType},
                            new Permission {DisplayName = "秒杀商品管理", Category = "秒杀",Name = Permissions.ManageSecondKillCommoditys, UserTypes = adminUserType},
                           
                        
                        }).AsQueryable();
            return from x in query
                   where x.UserTypes == null || x.UserTypes.ToList().IndexOf(userType) >= 0
                   select x;
        }

        #endregion
    }
}