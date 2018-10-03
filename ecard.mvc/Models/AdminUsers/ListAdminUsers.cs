using System.Collections.Generic;
using System.Linq;
using Ecard.Infrastructure;
using Ecard.Models;
using Ecard.Mvc.ViewModels;
using Ecard.Services;
using Microsoft.Practices.Unity;
using Moonlit;
using Moonlit.Text;
using System.Web.Mvc;
using System;

namespace Ecard.Mvc.Models.AdminUsers
{
    public class ListAdminUsers : EcardModelListRequest<ListAdminUser>
    {
        public ListAdminUsers()
        {
            OrderBy = "UserId";
        }

        private string _name;
        public string Name
        {
            get { return _name.TrimSafty(); }
            set { _name = value; }
        }

        private string _displayName;
        public string DisplayName
        {
            get { return _displayName.TrimSafty(); }
            set { _displayName = value; }
        }

        private string _email;
        public string Email
        {
            get { return _email.TrimSafty(); }
            set { _email = value; }
        }

        public IEnumerable<ActionMethodDescriptor> GetToolbarActions()
        {
            yield return new ActionMethodDescriptor("Create", null);
            yield return new ActionMethodDescriptor("Suspends", null);
            yield return new ActionMethodDescriptor("Resumes", null);
            yield return new ActionMethodDescriptor("Deletes", null);
            yield return new ActionMethodDescriptor("Export", null, new { export = "EXCEL" });

        }
        public IEnumerable<ActionMethodDescriptor> GetItemToobalActions(ListAdminUser user)
        {
            yield return new ActionMethodDescriptor("Edit", null, new { id = user.UserId });
            //测试添加
            //yield return new ActionMethodDescriptor("Delete", null);
            //-----------
            if (user.InnerObject.State == UserStates.Normal && !user.InnerObject.BuildIn)
            {
                yield return new ActionMethodDescriptor("Suspend", null, new { id = user.UserId });
                yield return new ActionMethodDescriptor("CreateDog", "User", new { id = user.UserId });
            }
            if (user.InnerObject.State == UserStates.Invalid && !user.InnerObject.BuildIn)
                yield return new ActionMethodDescriptor("Resume", null, new { id = user.UserId });
            if (!user.InnerObject.BuildIn)
                yield return new ActionMethodDescriptor("Delete", null, new { id = user.UserId });
        }
        private Bounded _state;
        public Bounded State
        {
            get
            {
                if (_state == null)
                {
                    _state = Bounded.Create<Ecard.Models.AdminUser>("State", UserStates.Normal);
                }
                return _state;
            }
            set { _state = value; }
        }
        [NoRender]
        [Dependency]
        public IMembershipService MembershipService { get; set; }
        public ResultMsg Delete(int id)
        {
            ResultMsg msg=new ResultMsg();
            try
            {
                var oldUser = MembershipService.GetUserById(id);

                AdminUser adminUser = oldUser as AdminUser;
                if (adminUser != null && !adminUser.BuildIn)
                {
                    MembershipService.DeleteUser(oldUser);
                    Logger.LogWithSerialNo(LogTypes.AdminUserDelete, SerialNoHelper.Create(), id, adminUser.Name);
                   // AddMessage("delete.success", adminUser.Name);
                    msg.Code = 1;
                    msg.CodeText = "删除用户 " + adminUser.DisplayName + " 成功";
                }
                else
                {
                    msg.CodeText = "不好意思,没有找到用户";
                }
                return msg;
            }
            catch (Exception ex)
            {
                msg.CodeText = "不好意思,系统异常";
                Logger.Error("停用用户", ex);
                return msg;
            }
        }

        public ResultMsg Suspend(int id)
        {
               ResultMsg msg=new ResultMsg();
               try
               {
                   var oldUser = MembershipService.GetUserById(id);
                       AdminUser adminUser = oldUser as AdminUser;
                       if (adminUser != null && !adminUser.BuildIn && adminUser.State == UserStates.Normal)
                       {
                           adminUser.State = UserStates.Invalid;
                           MembershipService.UpdateUser(adminUser);
                           Logger.LogWithSerialNo(LogTypes.AdminUserSuspend, SerialNoHelper.Create(), id, adminUser.Name);
                           //AddMessage("suspend.success", adminUser.Name);
                           msg.Code = 1;
                           msg.CodeText = "停用用户 " + adminUser.DisplayName + " 成功";
                       } 
                       else
                       {
                           msg.CodeText = "不好意思,没有找到用户";
                       }
                       return msg;
               }
               catch (Exception ex)
               {
                   msg.CodeText = "不好意思,系统异常";
                   Logger.Error("停用用户", ex);
                   return msg;
               }
        }
        public ResultMsg Resume(int id)
        {
            ResultMsg msg = new ResultMsg();
            try
            {
                var oldUser = MembershipService.GetUserById(id);

                AdminUser adminUser = oldUser as AdminUser;
                if (adminUser != null && !adminUser.BuildIn && adminUser.State == UserStates.Invalid)
                {
                    adminUser.State = UserStates.Normal;
                    MembershipService.UpdateUser(adminUser);
                    Logger.LogWithSerialNo(LogTypes.AdminUserResume, SerialNoHelper.Create(), id, adminUser.Name);
                    //AddMessage("resume.success", adminUser.Name);
                    msg.Code = 1;
                    msg.CodeText = "启用用户 " + adminUser.DisplayName + " 成功";
                }
                else
                {
                    msg.CodeText = "不好意思,没有找到用户";
                }
                return msg;
            }
            catch (Exception ex)
            {
                msg.CodeText = "不好意思,系统异常";
                Logger.Error("停用用户", ex);
                return msg;
            }
        }
        //获取用户数据
        public void Query( out string pageHtml)
        {
            pageHtml = string.Empty;
            UserRequest userRequest = new UserRequest();
            if (userRequest.PageIndex == null || userRequest.PageIndex <= 0)
            {
                userRequest.PageIndex = 1;
            }
            if (userRequest.PageSize == null || userRequest.PageSize <= 0)
            {
                userRequest.PageSize = 10;
            }
            if (!string.IsNullOrWhiteSpace(Name))
                userRequest.NameWith = Name;

            if (!string.IsNullOrWhiteSpace(DisplayName))
                userRequest.DisplayNameWith = DisplayName;

            if (!string.IsNullOrWhiteSpace(Email))
                userRequest.EmailWith = Email;

            if (State.Key != UserStates.All)
                userRequest.State = State;
            var query = MembershipService.QueryUsers<AdminUser>(userRequest);
            List = query.ToList(this, x => new ListAdminUser(x));
            //var query = MembershipService.NewQueryUser(userRequest);
            //List = query.ModelList.ToList(this, x => new ListAdminUser(x));
            if (query != null)
            {
                pageHtml = MvcPage.AjaxPager((int)userRequest.PageIndex, (int)userRequest.PageSize, List.Count);
            }
        }
        public List<ListAdminUser> AjaxGet(UserRequest request, out string pageHtml)
        {
            pageHtml = string.Empty;
            if (request.PageIndex == null || request.PageIndex <= 0)
            {
                request.PageIndex = 1;
            }
            if (request.PageSize == null || request.PageSize <= 0)
            {
                request.PageSize = 10;
            }
            var _tables = MembershipService.NewQueryUser(request);
            // var _tables = MembershipService.QueryUsers<AdminUser>(request);
            var datas = _tables.ModelList.Select(x => new ListAdminUser(x)).ToList();
            foreach (var item in datas)
            {
                bool issale = false;
                if (item.InnerObject.IsSale.HasValue)
                {
                    if (bool.TryParse(item.InnerObject.IsSale.ToString(), out  issale))
                    {
                        if (issale)
                        {
                            item.IsSaleStr = "是";
                        }
                        else 
                        {
                            item.IsSaleStr = "否";
                        }
                    } 
                }
                item.boor += "<a href='#' onclick=OperatorThis('Edit','/AdminUser/Edit/" + item.UserId + "') class='tablelink'>编辑 </a> ";
                if (item.InnerObject.State == UserStates.Normal && !item.InnerObject.BuildIn)
                {
                    item.boor += "<a href='#' onclick=OperatorThis('Suspend','/AdminUser/Suspend/" + item.UserId + "') class='tablelink'>停用 </a> ";
                    item.boor += "<a href='#' onclick=OperatorThis('CreateDog','/User/CreateDog/" + item.UserId + "') class='tablelink'>创建U盾 </a> ";
                }
                if (item.InnerObject.State == UserStates.Invalid && !item.InnerObject.BuildIn)
                    item.boor += "<a href='#' onclick=OperatorThis('Resume','/AdminUser/Resume/" + item.UserId + "') class='tablelink'>启用 </a> "; 
                if (!item.InnerObject.BuildIn)
                    item.boor += "<a href='#' onclick=OperatorThis('Delete','/AdminUser/Delete/" + item.UserId + "') class='tablelink'>删除 </a> ";
            }
            if (_tables != null)
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, _tables.TotalCount);
            return datas; 
        }
    }

}