using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.JuMeiMallIndex
{
    public class AddressListModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int userAddressId { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string detailedAddress { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string moblie { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public string recipients { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string zipCode { get; set; }

        /// <summary>
        /// 省市地区
        /// </summary>

        public string ProvinceName { get; set; }



    }

    /// <summary>
    /// 地址列表
    /// </summary>
    public class AddressListsModel
    {

        public AddressListsModel()
        {
            AddressList = new List<AddressListModel>();

        }

        /// <summary>
        /// 地址列表
        /// </summary>
        public List<AddressListModel> AddressList { get; set; }

        /// <summary>
        /// 默认地址Id
        /// </summary>
        public int defaultAddressId { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string defaultDetailedAddress { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string defaultMoblie { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public string defaultRecipients { get; set; }

        /// <summary>
        /// 省市地区
        /// </summary>

        public string defaultProvinceName { get; set; }


    }
}