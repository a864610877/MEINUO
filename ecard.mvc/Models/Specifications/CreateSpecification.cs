using Ecard.Models;
using Ecard;
using Ecard.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Ecard.Requests;

namespace Ecard.Mvc.Models.Specifications
{
    public class CreateSpecification : Specification
    {
        public string value { get; set; }
        [Dependency]
        public ISpecificationService specificationService { get; set; }
        [Dependency]
        public ISpecificationDetailService specificationDetailService { get; set; }
        [Dependency]
        public TransactionHelper TransactionHelper { get; set; }
        public void Create()
        {
            TransactionHelper.BeginTransaction();
            Specification sp = new Specification();
            sp.Name = this.Name;
            sp.remark = this.remark;
            sp.Type = this.Type;
            sp.showType = this.showType;
            sp.submitTime = DateTime.Now;
            sp.selectType = selectType;
            specificationService.Insert(sp);

            //获取插入的自增ID
            var s = specificationService.QueryIdentity().ToString();
            var arr = this.value.Split(',');
            for (int i = 0; i < arr.Length; i++)
            {
                SpecificationDetail spdetail = new SpecificationDetail();
                spdetail.specificationId =Convert.ToInt32( s);
                spdetail.describe = arr[i].Split('/')[1];
                spdetail.value = arr[i].Split('/')[0];
                
                spdetail.submitTime = DateTime.Now;
                specificationDetailService.Insert(spdetail);
            }
            TransactionHelper.Commit();
        }
    }


}
