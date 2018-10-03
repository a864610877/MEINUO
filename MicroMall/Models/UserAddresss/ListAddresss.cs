using Ecard;
using Ecard.Requests;
using Ecard.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.UserAddresss
{
    public class ListAddresss : ResultMessage
    {
        public List<ListAddress> List { get; set; }
        [Dependency, NoRender]
        public IUserAddressService IUserAddressService { get; set; }

        [Dependency, NoRender]
        public IAccountService IAccountService { get; set; }
        public void Query(int userId)
        {
            UserAddressRequest request = new UserAddressRequest();
            request.UserId = userId;
            request.PageSize = 50;
            var query = IUserAddressService.GetByUserId(request);
            //int defId = 0;
            //var GetdefId = IAccountService.GetById(userId);
            //if (GetdefId != null)
            //{
            //    defId = GetdefId.defaultAddressId;
            //}
            if (query != null && query.ModelList != null)
            {
                List = query.ModelList.Select(x => new ListAddress()
                {
                    moblie = x.moblie,
                    provinceName = x.provinceName,
                    recipients = x.recipients,
                    userAddressId = x.userAddressId,
                    detailedAddress = x.detailedAddress,
                    cityId=x.cityId,
                    provinceId=x.provinceId,
                    cityName=x.cityName
                    //defaultAddressId = x.userAddressId == defId ? defId : 0
                }).ToList();
            }
        }
    }

    public class ListAddress
    {
        public int userAddressId { get; set; }

        public int provinceId { get; set; }

        public int cityId { get; set; }

        public string recipients { get; set; }

        public string moblie { get; set; }

        public string provinceName { get; set; }

        public string cityName { get; set; }
        public string detailedAddress { get; set; }

        public int defaultAddressId { get; set; }
    }
}