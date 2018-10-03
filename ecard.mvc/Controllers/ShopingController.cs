using Ecard.Models;
using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.Models.Homes;
using Ecard.Mvc.Models.Shoping;
using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Ecard.Mvc.Controllers
{
    public class ShopingController : Controller
    {
        private readonly IUnityContainer _unityContainer;
        //private readonly SecurityHelper _securityHelper;
        [Dependency, NoRender]
        public ISpecificationService specificationService { get; set; }
        [Dependency, NoRender]
        public ICommodityService commodityService { get; set; }
        [Dependency, NoRender]
        public TransactionHelper transaction { get; set; }
         [Dependency, NoRender]
        public ICommodityCategorysService ICommodityCategorysService { get; set; }

        public ShopingController(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
            //_securityHelper = securityHelper;
        }

        /// <summary>
        /// 商品添加
        /// </summary>
        /// <returns></returns>
        [CheckPermission(Permissions.CommodityAdd)]
        public ActionResult ShopingManager()
        {
            int id=0;
            CreateShoping home = _unityContainer.Resolve<CreateShoping>();
            id = commodityService.GetMaxId()+1;
            var list = specificationService.QueryAll().ToList();
            var selectList = list.Select(x => new SelectListItem { Text = x.Name, Value = x.specificationId.ToString() });
            ViewData["specification"] = selectList;
            var CommodityCategorys = ICommodityCategorysService.GetAll();
            var CommodityCategorysList = CommodityCategorys.Select(x => new SelectListItem { Text = x.name, Value = x.commodityCategoryId.ToString()}).ToList();
            
            ViewData["CommodityCategorysList"] = CommodityCategorysList;
            var model = new EcardModelItem<CreateShoping>(home);
            model.Item.commodityNo = "00000" + id;
            return View(model);
        }

        /// <summary>
        /// 商品列表
        /// </summary>
        /// <returns></returns>
        [CheckPermission(Permissions.Commodity)]
        [Ecard.Services.DashboardItem]
        public ActionResult ShopingList(ListShopings request)
        {

            request.Query();
            return View(request);
        }
        [CheckUserType(typeof(AdminUser))]
        public ActionResult AjaxList(CommodityRequest request)
        {
            var create = _unityContainer.Resolve<ListShopings>();
            var table = create.AjaxQuery(request);
            return Json(new { tables = table, html = create.pageHtml });
        }

        /// <summary>
        /// 编辑商品
        /// </summary>
        /// <returns></returns>
        [CheckPermission(Permissions.CommodityEdit)]
        public ActionResult Edit(int id)
        {
            var model = _unityContainer.Resolve<EditShoping>();
            model.Ready(id);
            var list = specificationService.QueryAll().ToList();
            var selectList = list.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.specificationId.ToString()
            });
            ViewData["specification"] = selectList;
            var CommodityCategorys = ICommodityCategorysService.GetAll();
            var CommodityCategorysList = CommodityCategorys.Select(x => new SelectListItem { Text = x.name, Value = x.commodityCategoryId.ToString(),Selected=x.commodityCategoryId==model.commodityCategoryId }).ToList();
            ViewData["CommodityCategorysList"] = CommodityCategorysList;
            return View(new EcardModelItem<EditShoping>(model));
        }

        /// <summary>
        /// 编辑商品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [CheckPermission(Permissions.CommodityEdit)]
        public ActionResult Edit(EditShoping model)
        {

            return Json(model.Save());
        }
        /// <summary>
        /// 删除商品
        /// </summary>
        /// <returns></returns>
         [CheckPermission(Permissions.CommodityDelete)]
        public ActionResult DeleteShoping(int id)
        {
            ResultMsg result = new ResultMsg();
            var comm = commodityService.GetById(id);
            if (comm != null)
            {
                commodityService.Delete(comm);
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

        /// <summary>
        /// 删除规格
        /// </summary>
        /// <param name="spid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        //public ActionResult DeleteSp(string spid, int id)
        //{
        //    var comm = commodityService.GetById(id);
        //    if (comm != null)
        //    {
        //        var a = comm.specificationId;
        //        var start = (spid + ",").Trim();
        //        var end = "," + spid;
        //        if (a.StartsWith(start))
        //        {
        //            a = a.Substring(spid.Length + 1);
        //        }
        //        else if (a.EndsWith(end.Trim()))
        //        {
        //            a = a.Substring(0, a.Length - (spid.Length + 1));
        //        }
        //        else
        //        {
        //            a = a.Replace("," + spid + ",", ",");
        //        }
        //        comm.specificationId = a;
        //        commodityService.Update(comm);
        //    }
        //    return Json(1);
        //}

        /// <summary>
        ///批量上架
        /// </summary>
        /// <returns></returns>
        [CheckPermission(Permissions.CommodityPutaway)]
        public ActionResult ShoinpsPutaway(string strIds)
        {
            ResultMsg result = new ResultMsg();
            int sum = 0;
            if (!string.IsNullOrEmpty(strIds))
            {
                var commodityIds = strIds.Split(',');

                transaction.BeginTransaction();
                for (int i = 0; i < commodityIds.Length; i++)
                {
                    var comm = commodityService.GetById(Convert.ToInt32(commodityIds[i]));
                    comm.commodityState = CommodityStates.putaway;
                    try
                    {
                        sum += commodityService.Update(comm);
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
                    result.CodeText = sum + "件商品上架成功!";

                }
                else
                {
                    result.Code = 2;
                    result.CodeText = "批量上架失败!";
                }
                return Json(result);
            }
            else
            {
                result.CodeText = "请选中您要上架的商品！";
                return Json(result);
            }
        }
        /// <summary>
        ///批量下架
        /// </summary>
        /// <returns></returns>
        [CheckPermission(Permissions.CommoditySoldout)]
        public ActionResult ShoinpSoldout(string strIds)
        {
            ResultMsg result = new ResultMsg();
            int sum = 0;
            if (!string.IsNullOrEmpty(strIds))
            {
                var commodityIds = strIds.Split(',');

                transaction.BeginTransaction();
                for (int i = 0; i < commodityIds.Length; i++)
                {
                    var comm = commodityService.GetById(Convert.ToInt32(commodityIds[i]));
                    comm.commodityState = CommodityStates.soldOut;
                    try
                    {
                        sum += commodityService.Update(comm);
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

                    result.CodeText = sum + "件商品下架成功!";

                }
                else
                {
                    result.Code = 2;

                    result.CodeText = "批量下架失败!";
                }
                return Json(result);
            }
            else
            {
                result.CodeText = "请选中您要下架的商品！";
                return Json(result);
            }
        }

        /// <summary>
        /// 检验商品的状态：是否已经上、下架
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckShopingSate(string strIds, int State)
        {
            ResultMsg result = new ResultMsg();
            int sum = 0;
            if (!string.IsNullOrEmpty(strIds))
            {
                if (State == CommodityStates.soldOut)
                {
                    var commodityIds = strIds.Split(',');
                    for (int i = 0; i < commodityIds.Length; i++)
                    {
                        var comm = commodityService.GetById(Convert.ToInt32(commodityIds[i]));
                        if (comm != null && comm.commodityState == CommodityStates.putaway)
                        {
                            sum += 1;
                        }
                    }
                    if (sum != 0)
                    {
                        result.Code = 1;
                        result.CodeText = "您选中的商品有" + sum + "件已经上架！";
                        return Json(result);
                    }
                }
                if (State == CommodityStates.putaway)
                {
                    var commodityIds = strIds.Split(',');
                    for (int i = 0; i < commodityIds.Length; i++)
                    {
                        var comm = commodityService.GetById(Convert.ToInt32(commodityIds[i]));
                        if (comm != null && comm.commodityState == CommodityStates.soldOut)
                        {
                            sum += 1;
                        }
                    }
                    if (sum != 0)
                    {
                        result.Code = 1;
                        result.CodeText = "您选中的商品有" + sum + "件已经下架！";
                        return Json(result);
                    }
                }

            }

            if (string.IsNullOrEmpty(strIds) && State == CommodityStates.soldOut)
            {
                result.Code = 1;
                result.CodeText = "请选中您要上架的商品！";
                return Json(result);
            }
            if (string.IsNullOrEmpty(strIds) && State == CommodityStates.putaway)
            {
                result.Code = 1;
                result.CodeText = "请选中您要下架的商品！";
                return Json(result);
            }
            else
            {
                result.Code = 2;
                return Json(result);
            }
        }

        /// <summary>
        /// 批量删除商品
        /// </summary>
        /// <returns></returns>
         [CheckPermission(Permissions.CommodityDelete)]
        public ActionResult DeleteShopings(string strIds)
        {
            ResultMsg result = new ResultMsg();
            int sum = 0;
            if (!string.IsNullOrEmpty(strIds))
            {
                var commodityIds = strIds.Split(',');

                transaction.BeginTransaction();
                for (int i = 0; i < commodityIds.Length; i++)
                {
                    var comm = commodityService.GetById(Convert.ToInt32(commodityIds[i]));
                    try
                    {
                        sum += commodityService.Delete(comm);
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
                result.CodeText = "请选中您要删除的商品！";
                return Json(result);
            }

        }
        /// <summary>
        /// 检验商品编码是否重复
        /// </summary>
        /// <param name="commodityNo"></param>
        /// <returns></returns>
        public ActionResult CheckcommodityNo(string commodityNo)
        {
            var comm = commodityService.GetBycommodityNo(commodityNo);
            if (comm == null)
            {
                return Json(1);
            }
            return Json(2);
        }

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [CheckPermission(Permissions.CommodityAdd)]
        public ActionResult Create(CreateShoping shoping)
        {
            if (string.IsNullOrEmpty(shoping.commodityName))
            {
                return Json("商品名称不能未空！");
            }
            return Json(shoping.Create());
        }

        /// <summary>
        /// 上传商品图片
        /// </summary>
        /// <param name="Filedata"></param>
        /// <returns></returns>
        public ActionResult Upload(HttpPostedFileBase Filedata)
        {
            if (Filedata != null && Filedata.ContentLength > 0)
            {
                //文件上传后的保存根路径 
                string filePath = Server.MapPath("/MicroMalls/CommodityImages/");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string fileName = Path.GetFileName(Filedata.FileName);//获取文件原名
                string fileExtension = Path.GetExtension(fileName);//获取文件扩展名
                string saveFileName = Guid.NewGuid().ToString() + fileExtension;//要保存的文件名称
                int fileSize = Filedata.ContentLength / 1024;

                if (fileSize > 10000)
                {
                    return Json(new { ret = false, message = "文件上传失败，请选择小于10M的图片" });
                }
                else
                {
                    Filedata.SaveAs(filePath + saveFileName);
                    return Json(new { ret = true, FilePath = "/MicroMalls/CommodityImages/" + saveFileName });
                }
            }
            else
            {
                return Json(new { ret = false, message = "请选择要上传的文件" });
            }
        }

        /// <summary>
        /// 删除上传后的商品图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult DeleteUpload(string url)
        {
            string file = System.Web.HttpContext.Current.Server.MapPath(url);
            if (System.IO.File.Exists(file))
            {
                System.IO.File.Delete(file);
                return Json(1);

            }
            return Json(0);
        }
        /// <summary>
        /// 删除编辑商品图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult DeleteImg(string url, int id)
        {
            string file = System.Web.HttpContext.Current.Server.MapPath(url);
            if (System.IO.File.Exists(file))
            {
                if (!string.IsNullOrEmpty(url))
                {
                    var comm = commodityService.GetById(id);
                    if (comm != null && comm.images.Contains(url))
                    {
                        var a = comm.images;
                        var start = (url + ",").Trim();
                        var end = ("," + url).Trim();
                        if (a.StartsWith(start))
                        {
                            a = a.Substring(url.Length + 1);
                        }
                        else if (a.EndsWith(end.Trim()))
                        {
                            a = a.Substring(0, a.Length - (url.Length + 1));
                        }
                        else
                        {
                            a = a.Replace("," + url + ",", ",");
                        }
                        comm.images = a;
                        commodityService.Update(comm);
                    }

                }
                System.IO.File.Delete(file);
                return Json(1);

            }
            return Json(0);
        }

      
    }
}
