using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Ecard.Models
{
    /// <summary>
    /// 商品规格表
    /// </summary>
    public class Specification
    {
        [Key]
        public int specificationId { get; set; }
        /// <summary>
        /// 规格名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 规格类型 1文字类型 2图片类型
        /// </summary>
        [Bounded(typeof(Types))]
        public int Type { get; set; }
        /// <summary>
        /// 显示方式 1平铺类型 2 下拉类型
        /// </summary>
       [Bounded(typeof(ShowTypes))]
        public int showType { get; set; }
        /// <summary>
        /// 选择方式 1单选 2多选
        /// </summary>
        [Bounded(typeof(SelectTypes))]
        public int selectType { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime submitTime { get; set; }

        ///Matthew - 20170420 start
        /// <summary>
        /// 对应商品ID
        /// </summary>
      ///  public int commodityId { get; set; }
        ///Matthew - 20170420 end
    }

    public class Types
    {
        /// <summary>
        /// 文字类型
        /// </summary>
        public const int text = 1;
        /// <summary>
        /// 图片类型
        /// </summary>
        public const int image = 2;
    }

    public class ShowTypes
    {
        /// <summary>
        /// 平铺类型
        /// </summary>
        public const int tiled = 1;
        /// <summary>
        /// 下拉类型
        /// </summary>
        public const int dropDown = 2;
    }

    public class SelectTypes
    {
        /// <summary>
        /// 单选
        /// </summary>
        public const int radio = 1;
        /// <summary>
        /// 多选
        /// </summary>
        public const int checkBox = 2;
    }
}
