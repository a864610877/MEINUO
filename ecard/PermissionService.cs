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
                            new Permission {DisplayName = "�û�����", Category = "�û�", Name = Permissions.User , UserTypes = adminUserType}, 
                            new Permission {DisplayName = "�û��༭", Category = "�û�",Name = Permissions.UserEdit, UserTypes = adminUserType}, 
                            new Permission {DisplayName = "��ɫ����", Category = "��ɫ",Name = Permissions.Role, UserTypes = adminUserType}, 
                            new Permission {DisplayName = "��ɫ�༭", Category = "��ɫ",Name = Permissions.RoleEdit, UserTypes = adminUserType}, 
                            new Permission {DisplayName = "��ɫͣ��", Category = "��ɫ",Name = Permissions.RoleSuspend, UserTypes = adminUserType}, 
                            new Permission {DisplayName = "��ɫ����", Category = "��ɫ",Name = Permissions.RoleSume, UserTypes = adminUserType}, 
                            new Permission {DisplayName = "��ɫɾ��", Category = "��ɫ",Name = Permissions.RoleDelete, UserTypes = adminUserType}, 

                                                                  
                            //new Permission {DisplayName = "�̻�����", Category = "�̻�", Name = Permissions.Shop, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "�̻��༭", Category = "�̻�", Name = Permissions.ShopEdit, UserTypes = adminUserType}, 

                            //new Permission {DisplayName = "�����̹���", Category = "������", Name = Permissions.Distributor, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "�����̱༭", Category = "������", Name = Permissions.DistributorEdit, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "��������ɽ���", Category = "������", Name = Permissions.DistributorBrokerage, UserTypes = adminUserType}, 

                            //new Permission {DisplayName = "�ն˹���",     Category = "�ն�",Name = Permissions.Pos, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "�ն˱༭",     Category = "�ն�",Name = Permissions.PosEdit, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "�ն���Կ�鿴", Category = "�ն�",Name = Permissions.PosDataKey, UserTypes = adminUserType},

                            //new Permission {DisplayName = "�ʻ��������",Category = "�ʻ�����",Name = Permissions.AccountLevel, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "�ʻ�����༭",Category = "�ʻ�����",Name = Permissions.AccountLevelEdit, UserTypes = adminUserType}, 

                            //new Permission {DisplayName = "�ʻ�������",Category = "�ʻ����",Name = Permissions.AccountType, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "�ʻ����༭",Category = "�ʻ����",Name = Permissions.AccountTypeEdit, UserTypes = adminUserType}, 

                            //new Permission {DisplayName = "���ֹ������",Category = "���ֹ���",Name = Permissions.PointPolicy, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "���ֹ���༭",Category = "���ֹ���",Name = Permissions.PointPolicyEdit, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "������־����", Category ="������־",Name = Permissions.PointRebateLog, UserTypes = adminUserType},

                            //new Permission {DisplayName = "��������",Category = "����", Name = Permissions.PointRebate, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "�����༭",Category = "����", Name = Permissions.PointRebateEdit, UserTypes = adminUserType}, 

                            //new Permission {DisplayName = "���ֶһ�����",Category = "���ֶһ�",Name = Permissions.PointGift, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "���ֶһ��༭",Category = "���ֶһ�",Name = Permissions.PointGiftEdit, UserTypes = adminUserType}, 

                            //new Permission {DisplayName = "СƱ�������",Category = "СƱ����",Name = Permissions.PrintTicket, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "СƱ����ɾ��",Category = "СƱ����",Name = Permissions.PrintTicketDelete, UserTypes = adminUserType}, 

                            //new Permission {DisplayName = "����", Category = "����",Name = Permissions.Liquidate, UserTypes = adminUserType},
                            //new Permission {DisplayName = "����", Category = "����",Name = Permissions.Rollback, UserTypes = adminUserType},
                            //new Permission {DisplayName = "�������", Category = "����",Name = Permissions.RollbackApply, UserTypes = adminUserType},

                            new Permission {DisplayName = "��Ʒ����", Category = "��Ʒ",    Name = Permissions.Commodity, UserTypes = adminUserType}, 
                            new Permission {DisplayName = "��Ʒ�༭", Category = "��Ʒ",Name = Permissions.CommodityEdit, UserTypes = adminUserType}, 
                            new Permission {DisplayName = "��Ʒ���", Category = "��Ʒ",Name = Permissions.CommodityAdd, UserTypes = adminUserType},
                            new Permission {DisplayName = "��Ʒɾ��", Category = "��Ʒ",Name = Permissions.CommodityDelete, UserTypes = adminUserType},
                            new Permission {DisplayName = "��Ʒ�ϼ�", Category = "��Ʒ",Name = Permissions.CommodityPutaway, UserTypes = adminUserType},
                            new Permission {DisplayName = "��Ʒ�¼�", Category = "��Ʒ",Name = Permissions.CommoditySoldout, UserTypes = adminUserType},
                            new Permission{DisplayName="��������",Category="����",Name=Permissions.OrderList,UserTypes=adminUserType},
                            new Permission{DisplayName="�˿����",Category="����",Name=Permissions.ApplyRefundOrderList,UserTypes=adminUserType},
                            new Permission {DisplayName = "������", Category = "���",Name = Permissions.Ads, UserTypes = adminUserType},
                            new Permission {DisplayName = "��ӹ��", Category = "���",Name = Permissions.AdsCreate, UserTypes = adminUserType},
                            new Permission {DisplayName = "ɾ�����", Category = "���",Name = Permissions.AdsDelete, UserTypes = adminUserType},
                            new Permission {DisplayName = "�༭���", Category = "���",Name = Permissions.AdsEdit, UserTypes = adminUserType},
                            new Permission {DisplayName = "����ϼ�", Category = "���",Name = Permissions.AdsPutaway, UserTypes = adminUserType},
                            new Permission {DisplayName = "����¼�", Category = "���",Name = Permissions.AdsSoldout, UserTypes = adminUserType},
                            new Permission {DisplayName = "���۹���", Category = "����",Name = Permissions.ListReview, UserTypes = adminUserType},
                            new Permission {DisplayName = "��Ա����",    Category = "��Ա",Name = Permissions.Account, UserTypes = adminUserType},
                            new Permission {DisplayName = "���ּ�¼",    Category = "��Ա",Name = Permissions.OperationPointLog, UserTypes = adminUserType},
                            new Permission {DisplayName = "���ֹ���", Category = "����",Name = Permissions.ListWithdraw, UserTypes = adminUserType}, 
                            new Permission {DisplayName = "�������", Category = "����",Name = Permissions.WithdrawAudit, UserTypes = adminUserType}, 

                             new Permission {DisplayName = "�����б�", Category = "����",Name = Permissions.ListArticles, UserTypes = adminUserType},
                             new Permission {DisplayName = "��������", Category = "����",Name = Permissions.ArticlesCreate, UserTypes = adminUserType},
                             new Permission {DisplayName = "�༭����", Category = "����",Name = Permissions.EditArticles, UserTypes = adminUserType},
                             new Permission {DisplayName = "ɾ������", Category = "����",Name = Permissions.DeleteArticles, UserTypes = adminUserType},
                           
                             new Permission {DisplayName = "�����б�", Category = "����",Name = Permissions.ListGrades, UserTypes = adminUserType},
                             new Permission {DisplayName = "��Ӽ���", Category = "����",Name = Permissions.GradesCreate, UserTypes = adminUserType},
                             new Permission {DisplayName = "�༭����", Category = "����",Name = Permissions.EditGrades, UserTypes = adminUserType},
                             new Permission {DisplayName = "ɾ������", Category = "����",Name = Permissions.DeleteGrades, UserTypes = adminUserType},

                             new Permission {DisplayName = "���۱���", Category = "����",Name = Permissions.ListCommoditySales, UserTypes = adminUserType},
                             //new Permission {DisplayName = "���ֹ���", Category = "����",Name = Permissions.OperationPointLog, UserTypes = adminUserType},
                            //new Permission {DisplayName = "��������", Category = "����",    Name = Permissions.Order, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "�����޸�", Category = "����",Name = Permissions.OrderEdit, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "��������", Category = "����",    Name = Permissions.OrderCarry, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "��ɶ���", Category = "����",Name = Permissions.OrderComplete, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "ע������", Category = "����",Name = Permissions.OrderSuspend, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "����ע���Ķ���", Category = "����",    Name = Permissions.OrderResume, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "��������", Category = "����",Name = Permissions.OrderCreate, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "ɾ������", Category = "����",Name = Permissions.OrderDelete, UserTypes = adminUserType}, 
                           
                            //new Permission {DisplayName = "֧������",Category = "֧������",Name = Permissions.DealWay, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "֧�������༭",Category = "֧������",Name = Permissions.DealWayEdit, UserTypes = adminUserType}, 

                            //new Permission {DisplayName = "�ֽ���־����",Category = "�ֽ�",Name = Permissions.CashDealLog, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "�ֽ���־�༭",Category = "�ֽ�",Name = Permissions.CashDealLogEdit, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "�ֽ�ͳ�ƹ���",Category = "�ֽ�",Name = Permissions.CashDealLogSummary, UserTypes = adminUserType},
                            //new Permission {DisplayName = "�ֽ�ͳ�ƴ���",Category = "�ֽ�",Name = Permissions.CashDealLogDone, UserTypes = adminUserType},  

                            //new Permission {DisplayName = "ϵͳ��־",      Category = "��־",Name = Permissions.Log, UserTypes = adminUserType},
                            //new Permission {DisplayName = "ϵͳ������־",  Category = "��־",Name = Permissions.SystemDealLog, UserTypes = adminUserType},
                            //new Permission {DisplayName = "��Ա���׼�¼",  Category = "��־",Name = Permissions.DealLog, UserTypes = adminUserType},
                            //new Permission {DisplayName = "�̻����׼�¼",  Category = "��־",Name = Permissions.ShopDealLog, UserTypes = adminUserType},
                            //new Permission {DisplayName = "Ԥ��Ȩ��ѯ",    Category = "��־",Name = Permissions.AccountPrePayList, UserTypes = adminUserType},
                            //new Permission {DisplayName = "Ԥ��Ȩǿ�����",Category = "��־",Name = Permissions.AccountPrePayDone, UserTypes = adminUserType},
                            //new Permission {DisplayName = "Ԥ��Ȩǿ��ȡ��",Category = "��־",Name = Permissions.AccountPrePayCancel, UserTypes = adminUserType},

                            //new Permission {DisplayName = "��ֵ���",Category = "���",Name = Permissions.TaskRecharging, UserTypes = adminUserType},
                            //new Permission {DisplayName = "���ö�����",Category = "���",Name = Permissions.TaskLimitAmount, UserTypes = adminUserType},
                             
                            //new Permission {DisplayName = "�ʻ���ѯ",    Category = "��Ա����",Name = Permissions.AccountQuery, UserTypes = adminUserType},
                            //new Permission {DisplayName = "�ʻ��޿���ѯ",Category = "��Ա����",Name = Permissions.AccountQueryWithUserInfo, UserTypes = adminUserType},
                            //new Permission {DisplayName = "�ʻ���ѯ����",Category = "��Ա����",Name = Permissions.AccountQueryWithoutToken, UserTypes = adminUserType},
                            //new Permission {DisplayName = "��ʼ��",      Category = "��Ա����",Name = Permissions.AccountInit, UserTypes = adminUserType},
                            //new Permission {DisplayName = "ɾ����",    Category = "��Ա����",Name = Permissions.AccountDelete, UserTypes = adminUserType},
                            //new Permission {DisplayName = "�����ʻ�",    Category = "��Ա����",Name = Permissions.AccountReport, UserTypes = adminUserType},
                            //new Permission {DisplayName = "�ƿ�",    Category = "��Ա����",        Name = Permissions.AccountCreate, UserTypes = adminUserType}, 
                            //new Permission {DisplayName = "���",Category = "��Ա����", Name = Permissions.AccountApprove, UserTypes = adminUserType},
                            //new Permission {DisplayName = "��Ա���ſ�", Category = "��Ա����",Name = Permissions.AccountOpen, UserTypes = adminUserType},
                            //new Permission {DisplayName = "��������",Category = "��Ա����", Name = Permissions.AccountOpens, UserTypes = adminUserType},
                            //new Permission {DisplayName = "�˿�",Category = "��Ա����",Name = Permissions.AccountClose, UserTypes = adminUserType},
                            //new Permission {DisplayName = "����",Category = "��Ա����",Name = Permissions.AccountChangeName, UserTypes = adminUserType},
                            //new Permission {DisplayName = "��ʧ",Category = "��Ա����",Name = Permissions.AccountSuspend, UserTypes = adminUserType},
                            //new Permission {DisplayName = "�༭��Ա����",Category = "��Ա����", Name = Permissions.AccountOwner, UserTypes = adminUserType},
                            //new Permission {DisplayName = "ȡ����ʧ",Category = "��Ա����", Name = Permissions.AccountResume, UserTypes = adminUserType},
                            //new Permission {DisplayName = "����",Category = "��Ա����",  Name = Permissions.AccountRebate, UserTypes = adminUserType},
                            //new Permission {DisplayName = "�һ���Ʒ",Category = "��Ա����",  Name = Permissions.AccountGift, UserTypes = adminUserType},
                            //new Permission {DisplayName = "���߽���",Category = "��Ա����", Name = Permissions.AccountPay, UserTypes = shopType},
                            //new Permission {DisplayName = "���߳���", Category = "��Ա����",Name = Permissions.AccountCancelPay, UserTypes = shopType},
                            //new Permission {DisplayName = "�鿴�ʻ���ʼ������",Category = "��Ա����", Name = Permissions.AccountInitPassword, UserTypes = adminUserType},
                            //new Permission {DisplayName = "�鿴�ʻ�ʶ����", Category = "��Ա����",Name = Permissions.AccountToken, UserTypes = adminUserType},
                            //new Permission {DisplayName = "����", Category = "��Ա����",Name = Permissions.AccountRenew, UserTypes = adminUserType},
                            //new Permission {DisplayName = "��ֵ", Category = "��Ա����",Name = Permissions.AccountRecharge, UserTypes = adminUserType},
                            //new Permission {DisplayName = "����γ�ֵ", Category = "��Ա����",Name = Permissions.AreaRecharing, UserTypes = adminUserType},
                            //new Permission {DisplayName = "���ö��",      Category = "��Ա����",Name = Permissions.AccountLimit, UserTypes = adminUserType},
                            //new Permission {DisplayName = "����Ʊ",Category = "��Ա����", Name = Permissions.SystemDealLogOpenReceipt, UserTypes = adminUserType},
                            //new Permission {DisplayName = "������ֵ",Category = "��Ա����", Name = Permissions.SystemDealLogCloseRecharging, UserTypes = adminUserType},
                            //new Permission {DisplayName = "�޸��ʻ�����", Category = "��Ա����",Name = Permissions.AccountChangePassword, UserTypes = adminUserType},
                            //new Permission {DisplayName = "����U��", Category = "��Ա����",Name = Permissions.CreateDog, UserTypes = adminUserType},
                            new Permission {DisplayName = "ϵͳ����", Category = "ϵͳά��", Name = Permissions.SystemSettings, UserTypes = adminUserType},
                            //new Permission {DisplayName = "ϵͳ��־", Category = "ϵͳά��",  Name = Permissions.ReportLogList, UserTypes = adminUserType},
                            //new Permission {DisplayName = "��Ա����ƽ̨", Category = "ϵͳά��",  Name = Permissions.SystemMessagePanelAccount, UserTypes = adminUserType},
                            // new Permission {DisplayName = "��Ա���Ų�������", Category = "ϵͳά��",  Name = Permissions.SmsSeting, UserTypes = adminUserType},


                            //new Permission {DisplayName = "��Ա�����±�",        Category = "����",   Name = Permissions.ReportAccountMonth, UserTypes = adminUserType},
                            //new Permission {DisplayName = "�̻�����ͳ�Ʊ���",    Category = "����",  Name = Permissions.ReportShopDeals, UserTypes = adminUserType},
                            //new Permission {DisplayName = "�̻�����ͳ�Ʊ���",    Category = "����",  Name = Permissions.ReportShopDealAccountType, UserTypes = adminUserType},
                            //new Permission {DisplayName = "��Ա����ͳ�Ʊ���",    Category = "����",  Name = Permissions.ReportAccountDeals, UserTypes = adminUserType},
                            //new Permission {DisplayName = "����������ϸ����",    Category = "����",  Name = Permissions.ReportAccountDealsList, UserTypes = adminUserType},
                            //new Permission {DisplayName = "ϵͳ���׻��ܱ���",    Category = "����",  Name = Permissions.ReportSystemDealLogDay, UserTypes = adminUserType},
                            //new Permission {DisplayName = "ϵͳ�����û�ͳ�Ʊ���",Category = "����",  Name = Permissions.ReportSystemDealLogByUser, UserTypes = adminUserType},
                            //new Permission {DisplayName = "��Ա�����ܱ���",      Category = "����",    Name = Permissions.ReportAccounts, UserTypes = adminUserType},
                            //new Permission {DisplayName = "��Ա�������ܱ���",  Category = "����",  Name = Permissions.ReportAccounts2, UserTypes = adminUserType},
                            //new Permission {DisplayName = "��Ա�������ܱ���",  Category = "����",  Name = Permissions.ReportExpiredAccounts, UserTypes = adminUserType},
                            //new Permission {DisplayName = "Ԥ��Ȩͳ�Ʊ���",      Category = "����",  Name = Permissions.ReportPrepayList, UserTypes = adminUserType},
                            //new Permission {DisplayName = "��Ա�������ܱ���",  Category = "����",  Name = Permissions.ReportAccountDealsForAccount, UserTypes = adminUserType},
                            //new Permission {DisplayName = "��ֵ���ܱ���",        Category = "����",  Name = Permissions.ReportRecharging, UserTypes = adminUserType},
                            //new Permission {DisplayName = "��ֵ��ϸ����",        Category = "����",  Name = Permissions.ReportRechargingList, UserTypes = adminUserType},
                            //new Permission {DisplayName = "�ۿ�ͳ�Ʊ���",        Category = "����",  Name = Permissions.ReportSaleAccount, UserTypes = adminUserType},
                            //new Permission {DisplayName = "�ۿ���ϸ����",        Category = "����",  Name = Permissions.ReportSaleAccountList, UserTypes = adminUserType},
                            //new Permission {DisplayName = "ϵͳ��Ӫ�������",    Category = "����",  Name = Permissions.ReportAllSystemSummary, UserTypes = adminUserType},
                            new Permission {DisplayName = "��ɱ����", Category = "��ɱ",Name = Permissions.SecondKillSet, UserTypes = adminUserType},
                            new Permission {DisplayName = "��ɱ��Ʒ����", Category = "��ɱ",Name = Permissions.ManageSecondKillCommoditys, UserTypes = adminUserType},
                           
                        
                        }).AsQueryable();
            return from x in query
                   where x.UserTypes == null || x.UserTypes.ToList().IndexOf(userType) >= 0
                   select x;
        }

        #endregion
    }
}