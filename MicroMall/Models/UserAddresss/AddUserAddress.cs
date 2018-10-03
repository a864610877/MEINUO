using Ecard.Models;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.UserAddresss
{
    public class AddUserAddress
    {
        public int MyAddressId { get; set; }

        public string recipients { get; set; }

        public string moblie { get; set; }

        public string provinceName { get; set; }

        public int cityName { get; set; }

        public string detailedAddress { get; set; }

        public string zipCode { get; set; }
        [Dependency]
        public  IUserAddressService _userAddressService{get;set;}
        [Dependency]
        public IAccountService _accountService { get; set; }
        public void Save(int accountId)
        {
           //var user=_securityHelper.GetCurrentUser();
           //if (user != null)
           //{
               if (MyAddressId > 0)
               {
                   var model = _userAddressService.GetById(MyAddressId);
                   if (model != null)
                   {
                       model.provinceName = provinceName;
                       model.detailedAddress = detailedAddress;
                       model.moblie = moblie;
                       model.cityId = cityName;
                       model.recipients = recipients;
                       model.zipCode = zipCode;
                       _userAddressService.Update(model);
                   }
                   else
                   {
                       UserAddress item = new UserAddress();
                       item.cityId = cityName;
                       item.detailedAddress = detailedAddress;
                       item.moblie = moblie;
                       item.provinceName = provinceName;
                       item.recipients = recipients;
                       item.userId = accountId;
                       item.zipCode = zipCode;
                       _userAddressService.Insert(item);
                   }
               }
               else
               {
                   UserAddress item = new UserAddress();
                   item.cityId = cityName;
                   item.detailedAddress = detailedAddress;
                   item.moblie = moblie;
                   item.provinceName = provinceName;
                   item.recipients = recipients;
                   item.userId = accountId;
                   item.zipCode = zipCode;
                   _userAddressService.Insert(item);
               }
           //}
        }

        public void Ready(int id)
        {
            var item = _userAddressService.GetById(id);
            if (item != null)
            {
                MyAddressId = item.userAddressId;
                recipients = item.recipients;
                moblie = item.moblie;
                provinceName = item.provinceName;
                detailedAddress = item.detailedAddress;
                zipCode = item.zipCode;
            }
        }

        public bool Del(int id)
        {
            var item = _userAddressService.GetById(id);
            if (item != null)
            {
                if (_userAddressService.Delete(item) > 0)
                    return true;
            }
            return false;
        }

        public bool SetDefAddress(int id, int accountId)
        {
            var item = _accountService.GetById(accountId);
            if (item !=null)
            {
                item.defaultAddressId = id;
                _accountService.Update(item);
                return true;
            }
            return false;

        }

        /// <summary>
        /// 修改地址
        /// </summary>
        /// <param name="recipients"></param>
        /// <param name="mobile"></param>
        /// <param name="detailAddress"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool MdyAddress(string recipients, string mobile, string expressArea, string detailAddress, int id)
        {
            var item = _userAddressService.GetById(id);
            if (item != null)
            {
                item.provinceName = expressArea;
                item.recipients = recipients;
                item.moblie = mobile;
                item.detailedAddress = detailAddress;
                
                _userAddressService.Update(item);
                return true;
            }
            return false;
        
        
        }
        
        /// <summary>
        /// 新增地址
        /// </summary>
        /// <param name="recipients"></param>
        /// <param name="mobile"></param>
        /// <param name="detailAddress"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public bool AddAddress(string recipients, string mobile, string expressArea, string detailAddress, int userId)
        {
            //var account = _accountService.GetById(accountId);
            //if (account != null)
            //{
                UserAddress item = new UserAddress();
                item.detailedAddress = detailAddress;
                item.provinceName = expressArea;
                item.moblie = mobile;
                item.recipients = recipients;
                item.userId = userId;
                _userAddressService.Insert(item);
                return true;
            //}
           

        }

    }
}