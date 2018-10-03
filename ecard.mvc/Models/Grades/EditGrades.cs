using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecard.Mvc.Models.Grades
{
    public class EditGrades : EcardModelListRequest<Ecard.Models.Genders>
    {
        [NoRender]
        public int gradeId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 略缩图
        /// </summary>
        public decimal sale { get; set; }
        

        [Dependency]
        [NoRender]
        public IGradesService IGradesService { get; set; }

        public void Ready(int id)
        {
            var item = IGradesService.GetById(id);
            if (item != null)
            {
                this.gradeId = item.gradeId;
                this.name = item.name;
                this.sale = item.sale;
            }
        }
        public ResultMsg Save()
        {
            ResultMsg result = new ResultMsg();
            var ad = IGradesService.GetById(this.gradeId);
            if (ad != null)
            {
                ad.name = this.name;
                ad.sale = this.sale;
                IGradesService.Update(ad);
                result.Code = 0;
                result.CodeText = "修改成功!";
                return result;
            }
            else
            {
                result.Code = -1;
                result.CodeText = "修改失败!";
                return result;
            }
        }
    }
}
