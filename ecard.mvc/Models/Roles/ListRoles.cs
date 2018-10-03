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

namespace Ecard.Mvc.Models.Roles
{
    public class ListRoles : EcardModelListRequest<ListRole>
    {
        public ListRoles()
        {
            OrderBy = "RoleId";
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
        [NoRender, Dependency]
        public SecurityHelper SecurityHelper { get; set; }
        [Hidden]
        public string name { get; set; }
        public IEnumerable<ActionMethodDescriptor> GetToolbarActions()
        {
            yield return new ActionMethodDescriptor("Create", null);
            yield return new ActionMethodDescriptor("Suspends", null);
            yield return new ActionMethodDescriptor("Resumes", null);
            yield return new ActionMethodDescriptor("Deletes", null);

        }
        public IEnumerable<ActionMethodDescriptor> GetItemToobalActions(ListRole user)
        {
            yield return new ActionMethodDescriptor("Edit", null, new { id = user.RoleId });
            if (user.InnerObject.State == UserStates.Normal && !user.InnerObject.BuildIn)
                yield return new ActionMethodDescriptor("Suspend", null, new { id = user.RoleId });
            if (user.InnerObject.State == UserStates.Invalid && !user.InnerObject.BuildIn)
                yield return new ActionMethodDescriptor("Resume", null, new { id = user.RoleId });
            if (!user.InnerObject.BuildIn)
                yield return new ActionMethodDescriptor("Delete", null, new { id = user.RoleId });
        }
        private Bounded _state;
        public Bounded State
        {
            get
            {
                if (_state == null)
                {
                    _state = Bounded.Create<Ecard.Models.Role>("State", UserStates.Normal);
                }
                return _state;
            }
            set { _state = value; }
        }

        public ResultMsg Delete(int id)
        {
              ResultMsg msg=new ResultMsg();
              try
              {
                  var role = MembershipService.GetRoleById(id);

                  if (role != null && !role.BuildIn)
                  {
                      MembershipService.DeleteRole(role);
                      Logger.LogWithSerialNo(LogTypes.RoleDelete, SerialNoHelper.Create(), id, role.Name);
                     // AddMessage("delete.success", role.Name);
                      msg.Code = 1;
                      msg.CodeText = "删除角色 " + role.DisplayName + " 成功";
                  }
                  else
                  {
                      msg.CodeText = "不好意思,没有找到角色";
                  }
                  return msg;
              }
              catch (Exception ex)
              {
                  msg.CodeText = "不好意思,系统异常";
                  Logger.Error("删除角色", ex);
                  return msg;
              }
        }

         
        [Dependency]
        [NoRender]
        public IMembershipService MembershipService { get; set; }

        //停用
        public ResultMsg Suspend(int id)
        {
            ResultMsg msg=new ResultMsg();
            try
            {
                var oldRole = MembershipService.GetRoleById(id);
                if (oldRole != null && !oldRole.BuildIn)
                {
                    oldRole.State = RoleStates.Invalid;
                    MembershipService.UpdateRole(oldRole);

                    Logger.LogWithSerialNo(LogTypes.RoleSuspend, SerialNoHelper.Create(), id, oldRole.Name);
                    // AddMessage("suspend.success", oldRole.Name);
                    msg.Code = 1;
                    msg.CodeText = "停用角色 " + oldRole.DisplayName + " 成功";
                }
                else
                {
                    msg.CodeText = "不好意思,没有找到角色";
                }
                return msg;
            }
            catch(Exception ex)
            {
                msg.CodeText = "不好意思,系统异常";
                Logger.Error("停用角色", ex);
                return msg;
            }
        }
        // 启用角色
        public ResultMsg Resume(int id)
        {
            ResultMsg msg = new ResultMsg();
            try
            {

                var role = MembershipService.GetRoleById(id);
                if (role != null && !role.BuildIn)
                {
                    role.State = RoleStates.Normal;
                    MembershipService.UpdateRole(role);
                    Logger.LogWithSerialNo(LogTypes.RoleResume, SerialNoHelper.Create(), id, role.Name);
                    msg.Code = 1;
                    msg.CodeText = "启用角色 " + role.DisplayName + " 成功";
                    // AddMessage("resume.success", role.Name);
                }
                else
                {
                    msg.CodeText = "不好意思,没有找到角色";
                }
                return msg;
            } 
            catch(Exception ex)
            {
                msg.CodeText = "不好意思,系统异常";
                Logger.Error("启用角色", ex);
                return msg;
            }
        }

        public void Query(out string pageHtml)
        {
            pageHtml = string.Empty;
            var roleRequest = new RoleRequest();

            if (roleRequest.PageIndex == null || roleRequest.PageIndex <= 0)
            {
                roleRequest.PageIndex = 1;
            }
            if (roleRequest.PageSize == null || roleRequest.PageSize <= 0)
            {
                roleRequest.PageSize = 10;
            }
            if (!string.IsNullOrWhiteSpace(Name))
                roleRequest.NameWith = Name;

            if (!string.IsNullOrWhiteSpace(DisplayName))
                roleRequest.DisplayNameWith = DisplayName;

            if (State != UserStates.All)
                roleRequest.State = State;
            var _tables = MembershipService.GetRoles(roleRequest);
            List = _tables.ModelList.Select(x=>new ListRole(x)).ToList();
            if (_tables != null)
            {
               // TotalCount = _tables.TotalCount;
                pageHtml = MvcPage.AjaxPager((int)roleRequest.PageIndex, (int)roleRequest.PageSize, _tables.TotalCount);
            }
            //return  _tables;
        }
        public List<ListRole> AjaxGet(RoleRequest request,out string pageHtml)
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
            var _tables = MembershipService.GetRoles(request);
            var datas = _tables.ModelList.Select(x => new ListRole(x)).ToList();
            foreach (var item in datas)
            {

                if (this.SecurityHelper.HasPermission("roleedit"))
                item.boor += "<a href='#' onclick=OperatorThis('edit','/Role/Edit/" + item.RoleId + "') class='tablelink'>编辑 </a> ";
                if (item.InnerObject.State == UserStates.Normal && !item.InnerObject.BuildIn && this.SecurityHelper.HasPermission("rolesuspend"))
                    item.boor += "<a href='#' onclick=OperatorThis('Suspend','/Role/Suspend/" + item.RoleId + "') class='tablelink'>停用 </a> ";
                //else
                //    item.boor += 0+ ",";
                if (item.InnerObject.State == UserStates.Invalid && !item.InnerObject.BuildIn && this.SecurityHelper.HasPermission("roleresume"))
                    item.boor += "<a href='#' onclick=OperatorThis('Resume','/Role/Resume/" + item.RoleId + "') class='tablelink'>启用 </a> ";

                if (!item.InnerObject.BuildIn && this.SecurityHelper.HasPermission("roledelete"))
                    item.boor += "<a href='#' onclick=OperatorThis('Delete','/Role/Delete/" + item.RoleId + "') class='tablelink'>删除 </a> ";
            }
            if (_tables != null)
                pageHtml = MvcPage.AjaxPager((int)request.PageIndex, (int)request.PageSize, _tables.TotalCount);
            return datas;
        }
    }

}