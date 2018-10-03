namespace Ecard.Models
{
    public class LogTypes
    {
        /// <summary>
        /// 删除商品
        /// </summary>
        public const int DeleteGood = 50;
        /// <summary>
        /// 添加商品
        /// </summary>
        public const int AddGood = 51;
        /// <summary>
        /// 编辑商品
        /// </summary>
        public const int EditGood = 52;
        public const int SuspendGood = 53;
        public const int ResumeGood = 54;


        /// <summary>
        /// 删除订单
        /// </summary>
        public const int DeleteOrder = 55;
        /// <summary>
        /// 添加订单
        /// </summary>
        public const int AddOrder = 56;
        /// <summary>
        /// 编辑订单
        /// </summary>
        public const int EditOrder = 57;

        /// <summary>
        /// 停用终端
        /// </summary>
        public const int PosSuspend = 22;
        /// <summary>
        /// 启用终端
        /// </summary>
        public const int PosResume = 23;
        /// <summary>
        /// 创建终端
        /// </summary>
        public const int PosCreate = 24;
        /// <summary>
        /// 编辑终端
        /// </summary>
        public const int PosEdit = 25;
        /// <summary>
        /// 删除终端
        /// </summary>
        public const int PosDelete = 26;
        /// <summary>
        /// 导出终端
        /// </summary>
        public const int PosExport = 28;

        /// <summary>
        /// 停用用户
        /// </summary>
        public const int ShopSuspend = 12;
        /// <summary>
        /// 启用用户
        /// </summary>
        public const int ShopResume = 13;
        /// <summary>
        /// 创建用户
        /// </summary>
        public const int ShopCreate = 14;
        /// <summary>
        /// 编辑用户
        /// </summary>
        public const int ShopEdit = 15;
        /// <summary>
        /// 删除用户
        /// </summary>
        public const int ShopDelete = 16;
        /// <summary>
        /// 导出用户
        /// </summary>
        public const int ShopExport = 18;


        /// <summary>
        /// 停用经销商
        /// </summary>
        public const int DistributorSuspend = 42;
        /// <summary>
        /// 启用经销商
        /// </summary>
        public const int DistributorResume = 43;
        /// <summary>
        /// 创建经销商
        /// </summary>
        public const int DistributorCreate = 44;
        /// <summary>
        /// 编辑经销商
        /// </summary>
        public const int DistributorEdit = 45;
        /// <summary>
        /// 删除经销商
        /// </summary>
        public const int DistributorDelete = 46;
        /// <summary>
        /// 导出经销商
        /// </summary>
        public const int DistributorExport = 48;

        /// <summary>
        /// 会员制狗
        /// </summary>
        public const int CreateDog = 30;

        /// <summary>
        /// 停用用户
        /// </summary>
        public const int AdminUserSuspend = 92;
        /// <summary>
        /// 启用用户
        /// </summary>
        public const int AdminUserResume = 93;
        /// <summary>
        /// 创建用户
        /// </summary>
        public const int AdminUserCreate = 94;
        /// <summary>
        /// 编辑用户
        /// </summary>
        public const int AdminUserEdit = 95;
        /// <summary>
        /// 删除用户
        /// </summary>
        public const int AdminUserDelete = 96;
        /// <summary>
        /// 导出用户
        /// </summary>
        public const int AdminUserExport = 98;


        /// <summary>
        /// 停用角色
        /// </summary>
        public const int RoleSuspend = 102;
        /// <summary>
        /// 启用角色
        /// </summary>
        public const int RoleResume = 103;
        /// <summary>
        /// 创建角色
        /// </summary>
        public const int RoleCreate = 104;
        /// <summary>
        /// 编辑角色
        /// </summary>
        public const int RoleEdit = 105;
        /// <summary>
        /// 删除角色
        /// </summary>
        public const int RoleDelete = 106;


        /// <summary>
        /// 停用用户级别
        /// </summary>
        public const int AccountLevelSuspend = 112;
        /// <summary>
        /// 启用用户级别
        /// </summary>
        public const int AccountLevelResume = 113;
        /// <summary>
        /// 创建用户级别
        /// </summary>
        public const int AccountLevelCreate = 114;
        /// <summary>
        /// 编辑用户级别
        /// </summary>
        public const int AccountLevelEdit = 115;
        /// <summary>
        /// 删除用户级别
        /// </summary>
        public const int AccountLevelDelete = 116;
        /// <summary>
        /// 导出用户级别
        /// </summary>
        public const int AccountLevelExport = 118;


        /// <summary>
        /// 停用积分规则
        /// </summary>
        public const int PointPolicySuspend = 122;
        /// <summary>
        /// 启用积分规则
        /// </summary>
        public const int PointPolicyResume = 123;
        /// <summary>
        /// 创建积分规则
        /// </summary>
        public const int PointPolicyCreate = 124;
        /// <summary>
        /// 编辑积分规则
        /// </summary>
        public const int PointPolicyEdit = 125;
        /// <summary>
        /// 删除积分规则
        /// </summary>
        public const int PointPolicyDelete = 126;
        /// <summary>
        /// 导出积分规则
        /// </summary>
        public const int PointPolicyExport = 128;



        /// <summary>
        /// 停用返利
        /// </summary>
        public const int PointRebateSuspend = 132;
        /// <summary>
        /// 启用返利
        /// </summary>
        public const int PointRebateResume = 133;
        /// <summary>
        /// 创建返利
        /// </summary>
        public const int PointRebateCreate = 134;
        /// <summary>
        /// 编辑返利
        /// </summary>
        public const int PointRebateEdit = 135;
        /// <summary>
        /// 删除返利
        /// </summary>
        public const int PointRebateDelete = 136;
        /// <summary>
        /// 导出返利
        /// </summary>
        public const int PointRebateExport = 138;
        /// <summary>
        /// 获取返利
        /// </summary>
        public const int RebateGet = 139;



        /// <summary>
        /// 停用帐户类型
        /// </summary>
        public const int AccountTypeSuspend = 142;
        /// <summary>
        /// 启用帐户类型
        /// </summary>
        public const int AccountTypeResume = 143;
        /// <summary>
        /// 创建帐户类型
        /// </summary>
        public const int AccountTypeCreate = 144;
        /// <summary>
        /// 编辑帐户类型
        /// </summary>
        public const int AccountTypeEdit = 145;
        /// <summary>
        /// 删除帐户类型
        /// </summary>
        public const int AccountTypeDelete = 146;
        /// <summary>
        /// 导出帐户类型
        /// </summary>
        public const int AccountTypeExport = 148;


        /// <summary>
        /// 停用帐户类型
        /// </summary>
        public const int AmountRateSuspend = 152;
        /// <summary>
        /// 启用帐户类型
        /// </summary>
        public const int AmountRateResume = 153;
        /// <summary>
        /// 创建帐户类型
        /// </summary>
        public const int AmountRateCreate = 154;
        /// <summary>
        /// 编辑帐户类型
        /// </summary>
        public const int AmountRateEdit = 155;
        /// <summary>
        /// 删除帐户类型
        /// </summary>
        public const int AmountRateDelete = 156;
        /// <summary>
        /// 导出帐户类型
        /// </summary>
        public const int AmountRateExport = 158;
        /// <summary>
        /// 结算查询
        /// </summary>
        public const int LiquidateLoad = 166;
        /// <summary>
        /// 结算清算
        /// </summary>
        public const int LiquidateDone = 167;
        /// <summary>
        /// 导出清算
        /// </summary>
        public const int LiquidateExport = 168;
        /// <summary>
        /// 刷新清算
        /// </summary>
        public const int LiquidateRefresh = 169;


        /// <summary>
        /// 停用返利
        /// </summary>
        public const int PointGiftSuspend = 172;
        /// <summary>
        /// 启用返利
        /// </summary>
        public const int PointGiftResume = 173;
        /// <summary>
        /// 创建返利
        /// </summary>
        public const int PointGiftCreate = 174;
        /// <summary>
        /// 编辑返利
        /// </summary>
        public const int PointGiftEdit = 175;
        /// <summary>
        /// 删除返利
        /// </summary>
        public const int PointGiftDelete = 176;
        /// <summary>
        /// 导出返利
        /// </summary>
        public const int PointGiftExport = 178;

        public const int PointGiftGet = 109;

        /// <summary>
        /// 停用商品类型
        /// </summary>
        public const int CommoditySuspend = 182;
        /// <summary>
        /// 启用商品类型
        /// </summary>
        public const int CommodityResume = 183;
        /// <summary>
        /// 创建商品类型
        /// </summary>
        public const int CommodityCreate = 184;
        /// <summary>
        /// 编辑商品类型
        /// </summary>
        public const int CommodityEdit = 185;
        /// <summary>
        /// 删除商品类型
        /// </summary>
        public const int CommodityDelete = 186;
        /// <summary>
        /// 导出商品类型
        /// </summary>
        public const int CommodityExport = 188;
        /// <summary>
        /// 删除小票打印
        /// </summary>
        public const int PrintTicketDelete = 196;
        /// <summary>
        /// 打印小票
        /// </summary>
        public const int PrintTicketPrint = 199;

        /// <summary>
        /// 停用交易类型
        /// </summary>
        public const int DealWaySuspend = 212;

        /// <summary>
        /// 启用交易类型
        /// </summary>
        public const int DealWayResume = 213;

        /// <summary>
        /// 创建交易类型
        /// </summary>
        public const int DealWayCreate = 214;

        /// <summary>
        /// 编辑交易类型
        /// </summary>
        public const int DealWayEdit = 215;

        /// <summary>
        /// 删除交易类型
        /// </summary>
        public const int DealWayDelete = 216;

        /// <summary>
        /// 导出交易类型
        /// </summary>
        public const int DealWayExport = 218;

        /// <summary>
        /// 停用短信模板
        /// </summary>
        public const int MessageTemplateSuspend = 222;

        /// <summary>
        /// 启用短信模板
        /// </summary>
        public const int MessageTemplateResume = 223;

        /// <summary>
        /// 创建短信模板
        /// </summary>
        public const int MessageTemplateCreate = 224;

        /// <summary>
        /// 编辑短信模板
        /// </summary>
        public const int MessageTemplateEdit = 225;

        /// <summary>
        /// 删除短信模板
        /// </summary>
        public const int MessageTemplateDelete = 226;

        /// <summary>
        /// 导出短信模板
        /// </summary>
        public const int MessageTemplateExport = 228;

         
        /// <summary>
        /// 停用现金日志
        /// </summary>
        public const int CashDealLogSuspend = 242;

        /// <summary>
        /// 启用现金日志
        /// </summary>
        public const int CashDealLogResume = 243;

        /// <summary>
        /// 创建现金日志
        /// </summary>
        public const int CashDealLogCreate = 244;

        /// <summary>
        /// 编辑现金日志
        /// </summary>
        public const int CashDealLogEdit = 245;

        /// <summary>
        /// 删除现金日志
        /// </summary>
        public const int CashDealLogDelete = 246;

        /// <summary>
        /// 导出现金日志
        /// </summary>
        public const int CashDealLogExport = 248;
        /// <summary>
        /// 员工现金还款
        /// </summary>
        public const int CashDealLogDone = 249;
        /// <summary>
        /// 初始化卡
        /// </summary>
        public const int AccountInit = 501;
        /// <summary>
        /// 初始化卡
        /// </summary>
        public const int AccountCreate = 502;
        /// <summary>
        /// 审核卡
        /// </summary>
        public const int AccountApprove = 503;
        /// <summary>
        /// 开卡
        /// </summary>
        public const int AccountOpen = 504;
        /// <summary>
        /// 充值
        /// </summary>
        public const int AccountRecharge = 505;
        /// <summary>
        /// 延期
        /// </summary>
        public const int AccountRenew = 506;
        /// <summary>
        /// 删除卡
        /// </summary>
        public const int AccountDelete = 507;
        /// <summary>
        /// 换卡
        /// </summary>
        public const int AccountChangeName = 508;
        /// <summary>
        /// 退卡
        /// </summary>
        public const int AccountClose = 509;
        /// <summary>
        /// 换卡
        /// </summary>
        public const int AccountChangePassword = 510;
        /// <summary>
        /// 会员档案修改
        /// </summary>
        public const int AccountOwner = 511;
        /// <summary>
        /// 会员转帐
        /// </summary>
        public const int AccountTransfer = 512;
        /// <summary>
        /// 停用
        /// </summary>
        public const int AccountSuspend = 522;
        /// <summary>
        /// 启用
        /// </summary>
        public const int AccountResume = 523;
        /// <summary>
        /// 返利
        /// </summary>
        public const int AccountRebate = 524;
        /// <summary>
        /// 兑换奖品
        /// </summary>
        public const int AccountDoGift = 525;
        /// <summary>
        /// 兑换奖品
        /// </summary>
        public const int AccountLimit = 526;
        /// <summary>
        /// 导出卡
        /// </summary>
        public const int AccountExport = 528;
        /// <summary>
        /// 强制预授权完成
        /// </summary>
        public const int DonePrePayForce = 600;
        /// <summary>
        /// 强制撤消预授权
        /// </summary>
        public const int CancelPrePayForce = 601;
        /// <summary>
        /// 帐户查询
        /// </summary>
        public const int AccountQuery = 602;

        /// <summary>
        /// 查询用户信息
        /// </summary>
        public const int AccountQueryWithUserInfo = 603;
        /// <summary>
        /// 不用 token 查询帐户信息
        /// </summary>
        public const int AccountQueryWithoutToken = 604;
        /// <summary>
        /// 批准充值
        /// </summary>
        public const int ApproveRecharging = 401;
        /// <summary>
        /// 拒绝充值
        /// </summary>
        public const int RefuseRecharging = 402;
        /// <summary>
        /// 批准信用额度
        /// </summary>
        public const int ApproveLimitAmount = 411;
        /// <summary>
        /// 拒绝信用额度
        /// </summary>
        public const int RefuseLimitAmount = 412;

        /// <summary>
        /// 编辑个人信息
        /// </summary>
        public const int EditProfile = 900;
        /// <summary>
        /// 系统设置
        /// </summary>
        public const int SystemSettings = 1000;
        /// <summary>
        /// 系统交易日志导出
        /// </summary>
        public const int SystemDealLogExport = 1001;
        /// <summary>
        /// 系统交易开发票
        /// </summary>
        public const int SystemDealLogOpenReceipt = 1002;
        /// <summary>
        /// 撤消充值
        /// </summary>
        public const int SystemDealLogCloseRecharging = 1003;
        /// <summary>
        /// 系统中回滚交易
        /// </summary>
        public const int DealLogRollback = 1004;
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        public const int SendSmsCode = 1005;

        public const int RecoveryPassword = 1006;
        public static int RegisterAccountUser = 1007;

        public const int Pay = 2001;
        public const int CancelPay = 2002;
        public const int PrePay = 2003;
        public const int DonePrePay = 2004;
        public const int CancelPrePay = 2005;
        public const int CancelDonePrePay = 2006;
        public const int UnPay = 2007;

        /// <summary>
        /// 删除秒杀商品
        /// </summary>
        public const int SecondKillCommoditysDelete = 9001;

    }
}