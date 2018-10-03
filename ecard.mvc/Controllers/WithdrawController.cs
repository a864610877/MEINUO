using Ecard.Models;
using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.Models.Withdraws;
using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ecard.Mvc.Controllers
{
    public class WithdrawController : Controller
    {


        private readonly IUnityContainer _unityContainer;
        public WithdrawController(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }
        [Dependency]
        [NoRender]
        public IWithdrawService withdrawService { get; set; }
        [Dependency]
        [NoRender]
        public IMembershipService MembershipService { get; set; }
        [Dependency]
        [NoRender]
        public IAccountService IAccountService { get; set; }
        [CheckPermission(Permissions.ListWithdraw)]
        public ActionResult List(ListWithdraws request)
        {
            request.Query();
            return View(request);
        }
        [CheckPermission(Permissions.ListWithdraw)]
        public ActionResult AjaxList(WithdrawRequest request)
        {
            var create = _unityContainer.Resolve<ListWithdraws>();
            var table = create.AjaxQuery(request);
            return Json(new { tables = table, html = create.pageHtml });
        }

        /// <summary>                                                                                                   
        /// 审核页面                                                                                                    
        /// </summary>                                                                                                  
        /// <param name="id"></param>                                                                                   
        /// <returns></returns>                                                                                         
        public ActionResult Audit(int id)
        {
            var request = new WithdrawRequest();
            var withdraws = withdrawService.GetById(id);
            WithdrawModel model = new WithdrawModel();
            if (withdraws != null)
            {
                var user = IAccountService.GetByUserId(withdraws.userId); //MembershipService.GetUserById(withdraws.userId);
                model.amount = withdraws.amount;
                model.DisplayName = user.name;
                //model.Email = "";                                                                                     
                //model.Gender = user.gender;//user.gender==null?0:user.gender;                                         
                //model.Mobile=user.Mobile;                                                                             
                model.Operator = withdraws.Operator;
                model.point = withdraws.point;
                model.remark = withdraws.remark;
                model.state = withdraws.state;
                model.submitTime = withdraws.submitTime;
                model.withdrawId = withdraws.withdrawId;
            }
            //var wdith = withdraws.ModelList.Where(x => x.withdrawId == id).FirstOrDefault();                          
            return View(new EcardModelItem<WithdrawModel>(model));
        }

        /// <summary>                                                                                                   
        /// 审核                                                                                                        
        /// </summary>                                                                                                  
        /// <param name="id"></param>                                                                                   
        /// <returns></returns>                                                                                         
        [CheckPermission(Permissions.WithdrawAudit)]
        [HttpPost]
        public ActionResult AuditWithdraw(int id, string remark)
        {
            ResultMsg result = new ResultMsg();
            try
            {
                var request = _unityContainer.Resolve<ListWithdraws>();
                result = request.Agree(id, remark);
                return Json(result);
            }
            catch (Exception)
            {

                result.Code = 2;
                result.CodeText = "系统错误！";
                return Json(result);
            }

        }
        [CheckPermission(Permissions.WithdrawAudit)]
        public ActionResult AuditWithdraws(string strIds)
        {
            var request = _unityContainer.Resolve<ListWithdraws>();
            ResultMsg result = new ResultMsg();
            int sum = 0;
            if (!string.IsNullOrEmpty(strIds))
            {
                var commodityIds = strIds.Split(',');
                for (int i = 0; i < commodityIds.Length; i++)
                {
                    //var comm = //ReviewService.GetById(Convert.ToInt32(commodityIds[i]));                             
                    //comm.State = ReviewStates.Show; //CommodityStates.putaway;                                        
                    result = request.Agree(Convert.ToInt32(commodityIds[i]), "");
                    if (result.Code == 0)
                        sum += 1;
                }
                if (sum == commodityIds.Length)
                {
                    result.Code = 1;
                    result.CodeText = sum + "条申请审核成功!";

                }
                else
                {
                    result.Code = 2;
                    result.CodeText = "审核失败!";
                }
                return Json(result);
            }
            else
            {
                result.CodeText = "请选中您要审核的申请！";
                return Json(result);
            }
        }

        /// <summary>                                                                                                   
        /// 不同意审核                                                                                                  
        /// </summary>                                                                                                  
        /// <param name="id"></param>                                                                                   
        /// <returns></returns>                                                                                         
        [CheckPermission(Permissions.WithdrawAudit)]
        [HttpPost]
        public ActionResult NotAuditWithdraw(int id, string remark)
        {
            ResultMsg result = new ResultMsg();
            try
            {
                var request = _unityContainer.Resolve<ListWithdraws>();
                result = request.NotAgree(id, remark);
                return Json(result);
            }
            catch (Exception)
            {

                result.Code = 2;
                result.CodeText = "系统错误！";
                return Json(result);
            }
        }


        [CheckPermission(Permissions.WithdrawAudit)]
        public ActionResult NotAuditWithdraws(string strIds)
        {
            var request = _unityContainer.Resolve<ListWithdraws>();
            ResultMsg result = new ResultMsg();
            int sum = 0;
            if (!string.IsNullOrEmpty(strIds))
            {
                var commodityIds = strIds.Split(',');
                for (int i = 0; i < commodityIds.Length; i++)
                {
                    //var comm = //ReviewService.GetById(Convert.ToInt32(commodityIds[i]));                             
                    //comm.State = ReviewStates.Show; //CommodityStates.putaway;                                        
                    result = request.NotAgree(Convert.ToInt32(commodityIds[i]), "");
                    if (result.Code == 0)
                        sum += 1;
                }
                if (sum == commodityIds.Length)
                {
                    result.Code = 1;
                    result.CodeText = sum + "条申请审核成功!";

                }
                else
                {
                    result.Code = 2;
                    result.CodeText = "审核失败!";
                }
                return Json(result);
            }
            else
            {
                result.CodeText = "请选中您要审核的申请！";
                return Json(result);
            }
        }

    }
}
