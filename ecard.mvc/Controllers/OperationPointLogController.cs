﻿using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.Models.OperationPointLogs;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ecard.Mvc.Controllers
{
    public class OperationPointLogController:Controller
    {
         private readonly IUnityContainer _unityContainer;
         public OperationPointLogController(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
            //_securityHelper = securityHelper;
        }

         [Dependency]
         [NoRender]
         public IOperationPointLogService operationPointLogService { get; set; }

         [Dependency, NoRender]
         public TransactionHelper transaction { get; set; }
        [CheckPermission(Permissions.OperationPointLog)]
         public ActionResult OperationPointLogList(OperationPointLogLists request)
         {
             request.Query();
             return View(request);
         }
        [CheckPermission(Permissions.OperationPointLog)]
         public ActionResult AjaxList(OperationPointLogRequest request)
         {
             var create = _unityContainer.Resolve<OperationPointLogLists>();
             var table = create.AjaxQuery(request);
             return Json(new { tables = table, html = create.pageHtml });
         }

         /// <summary>
         /// 删除积分操作记录
         /// </summary>
         /// <returns></returns>
         [CheckPermission(Permissions.OperationPointLog)]
         public ActionResult Delete(int id)
         {
             ResultMsg result = new ResultMsg();
             var comm = operationPointLogService.GetById(id);
             if (comm != null)
             {
                 operationPointLogService.Delete(comm);
                 result.Code = 1;
                 result.CodeText = "删除成功!";
                 return Json(result);
             }
             else
             {
                 result.Code = 2;
                 result.CodeText = "删除失败!";
                 return Json(result);
             }
         }

       [CheckPermission(Permissions.OperationPointLog)]
         /// <summary>
         /// 批量删除推荐记录
         /// </summary>
         /// <param name="strIds"></param>
         /// <returns></returns>
         public ActionResult Deletes(string strIds)
         {
             ResultMsg result = new ResultMsg();
             int sum = 0;
             if (!string.IsNullOrEmpty(strIds))
             {
                 var commodityIds = strIds.Split(',');

                 transaction.BeginTransaction();
                 for (int i = 0; i < commodityIds.Length; i++)
                 {
                     var comm = operationPointLogService.GetById(Convert.ToInt32(commodityIds[i]));
                     try
                     {
                         sum += operationPointLogService.Delete(comm);
                     }
                     catch (Exception)
                     {
                         result.CodeText = "不好意思,系统异常!";
                         return Json(result);
                     }

                 }
                 transaction.Commit();
                 if (sum == commodityIds.Length)
                 {
                     result.Code = 1;
                     result.CodeText = "删除成功!";

                 }
                 else
                 {
                     result.Code = 2;
                     result.CodeText = "删除失败!";
                 }
                 return Json(result);
             }
             else
             {
                 result.Code = 2;
                 result.CodeText = "请选中您要删除的记录！";
                 return Json(result);
             }

         }
    }
}
