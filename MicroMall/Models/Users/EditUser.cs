using Ecard.Models;
using Ecard.Services;
using MicroMall.Models.layouts;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroMall.Models.Users
{
    public class EditUser : LayoutModel
    {

        [Dependency]
        public IMembershipService MembershipService { get; set; }
        //[Dependency]
        //public IAccountService AccountService { get; set; }

        public int UserId { get; set; }
        public string Photo { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public int Gender { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Password { get; set; }

        public string PasswordConfirm { get; set; }

        public int ProvinceId { get; set; }

        public int CityId { get; set; }

        public List<SelectModel> Provinces { get; set; }

        public List<SelectModel> Citys { get; set; }
        ///// <summary>
        ///// 赠送积分
        ///// </summary>
        //public decimal presentExp { get; set; }
        ///// <summary>
        ///// 未激活积分（只能用于抵扣）
        ///// </summary>
        //public decimal notActivatePoint { get; set; }
        ///// <summary>
        ///// 激活积分（可用于抵扣、提现）
        ///// </summary>
        //public decimal activatePoint { get; set; }

        public void Ready()
        {
            Load();
        }

        public void GetProvince()
        {
            Provinces = ProvinceService.Query().Select(x => new SelectModel() {Id=x.ProvinceId,Name=x.Name }).ToList();
            Citys = GetCity(UserInformation.provinceId).Select(x => new SelectModel() { Id = x.CityId, Name = x.Name }).ToList();
        }

        public ResultMessage Save()
        {
            try
            {
                var Currentuser = _securityHelper.GetCurrentUser();
                //var user = MembershipService.GetUserById(UserId);
                if (Currentuser != null)
                {
                    var user = Currentuser.CurrentUser;
                    if (Mobile == null)
                        return new ResultMessage() { Code = -1, Msg = "请输入手机号！" };

                    var usermobile = MembershipService.GetByMobile(Mobile);
                    if (usermobile != null)
                    {
                        if (usermobile.UserId != user.UserId)
                            return new ResultMessage() { Code = -1, Msg = "手机号已存在，请更换！" };
                    }
                    user.Photo = Photo;
                    user.Address = Address;
                    user.BirthDate = BirthDate;
                    user.DisplayName = DisplayName;
                    user.Email = Email;
                    user.Gender = Gender;
                    user.Mobile = Mobile;
                    user.Name = Mobile;
                    user.provinceId = ProvinceId;
                    user.cityId = CityId;
                    if (!string.IsNullOrWhiteSpace(Password))
                    {
                        if (!Password.Equals(PasswordConfirm))
                            return new ResultMessage() { Code = -1, Msg = "两次输入密码不一致！" };
                        user.SetPassword(Password);
                    }
                    MembershipService.UpdateUser(user);
                    return new ResultMessage() { Code =0 };
                }
                else
                {
                    return new ResultMessage() { Code = -1, Msg = "请重新登录！" };
                }
            }
            catch(Exception ex)
            {
                 logService.Insert(ex);
                 return new ResultMessage() { Code = -1, Msg = "系统错误！" };
            }
           
        }
    }
}