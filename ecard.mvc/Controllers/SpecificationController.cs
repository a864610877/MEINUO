using Ecard.Mvc.ActionFilters;
using Ecard.Mvc.Models.Specifications;
using Ecard.Services;
using Microsoft.Practices.Unity;
using Moonlit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ecard.Mvc.Controllers
{
    public class SpecificationController:Controller
    {
         private readonly IUnityContainer _unityContainer;
         [Dependency, NoRender]
         public ISpecificationService specificationService { get; set; }
         [Dependency, NoRender]
         public ISpecificationDetailService specificationDetailService { get; set; }
         public SpecificationController(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        /// <summary>
        /// 添加规格
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
         [HttpPost]
         public ActionResult Create(CreateSpecification request) 
         {
             
             request.Create();
             var list = specificationService.QueryAll().ToList();
             var idNamePairs = list.Select(x => new IdNamePair() { Key = x.specificationId, Name = x.Name });
             return Json(idNamePairs);
         }

        /// <summary>
        /// 检验规格名称是否已存在
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
         public ActionResult CheckName(string Name) 
         {
             var sp=specificationService.QueryAll().ToList().Where(x=>x.Name==Name).FirstOrDefault();
             if (sp==null)
             {
                 return Json(1);

             }
             else
             {
                 return Json(2);
             }
         }
        /// <summary>
        /// 根据规格编号获取所有明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
         public ActionResult GetSpecificationDetails(int id) 
         {
            var specification= specificationService.GetById(id);
            var SpecificationDetails = specificationDetailService.GetByspecificationId(id).ToList(); 
            if (specification.Type==1)
            {
                return Json(new { sp = SpecificationDetails,type=1 });
            }
            else
            {
                return Json(new { sp = SpecificationDetails, type = 2 });

            }
         }
    }
}
