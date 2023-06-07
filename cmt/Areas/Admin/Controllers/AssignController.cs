using cmt.Areas.Admin.ViewModels;
using cmt.Services;
using cmt.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cmt.Areas.Admin.Controllers
{
    [AccessDeniedAuthorizeAttribute(Roles = "admin/assign/memberlist")]
    [Authorize(Roles = "admin/assign/memberlist")]
    public class AssignController : BaseController
    {
        string api = ConfigurationManager.AppSettings["MemberAPI"];
        ICodeTableService _codeTableService;
        IAccountService _accountService;
        IAssignService _assignService;
        public AssignController(IAuthService authService, ILogService logService, INewsService newsService, ICodeTableService codeTableService, IAccountService accountService, IAssignService assignService)
            : base(authService, logService, newsService, codeTableService)
        {
            _assignService = assignService;
            _codeTableService = codeTableService;
            _accountService = accountService;
        }

       
        public ActionResult MemberList()
        {
            var Role = _assignService.getRoleAll();
            var Agent = _assignService.getAgentAll();
            var models = _accountService.GetUserAll().Where(x=>x.IsEnabled).ToList();

            models.ForEach(x =>
            {
                var r = Role.Where(a =>  a.assign_UserId == x.UserGuid);
                var t = Agent.Where(a =>  a.assign_UserId == x.UserGuid);
                x.RoleName = "---";
                x.AgentName = "---";
                if (r.Count() > 0)
                {
                    x.RoleName = r.First().roleName;
                }
                if (t.Count() > 0)
                {
                    x.AgentName = t.First().Agent_userName;
                }


            });

            return View(models);
        }


        public ActionResult Role(string Guid)
        {
            var _role = _assignService.getRole(Guid);

            RoleVM role = new RoleVM();

            role.codeTables = _codeTableService.GetCodeTableByType("02");
            role.Users = _accountService.GetUser(Guid);
            if (_role.Count() > 0)
            {
                role.codeTables.ForEach(x =>
                {

                    var m = _role.Where(a => a.assign_Role == x.Value);
                    x.Selected = m.Count() > 0;

                });
            }
            return View(role);
        }

        public ActionResult Agent(string Guid)
        {
            //自己被設為代理人
            var Agent = _assignService.getAgentAll().Where(x=>x.assign_AgentUserId == Guid);
            var _agent = _assignService.getAgent(Guid);

            List<SelectListItem> codeTables = new List<SelectListItem>();
            AgentVM role = new AgentVM();
            var models = _accountService.GetUserAll().Where(x => 
            x.IsEnabled 
            && x.UserGuid != Guid
           

            ).ToList();

            SelectListItem item = new SelectListItem();
            item.Value = "";
            item.Text = "未指派";
            codeTables.Add(item);

            foreach (var m in models)
            {
               

                item = new SelectListItem();
                item.Value = m.UserGuid;
                item.Text = $"{m.UserNickname}-({m.UserEmail})";

                if (_agent.Count() > 0)
                {
                    var _agent_models = _agent.Where(a => a.assign_AgentUserId == m.UserGuid);
                    item.Selected = _agent_models.Count() > 0;
                }
                //不可以設對方為代理人
                item.Disabled = Agent.Where(a => a.assign_UserId == m.UserGuid).Count() > 0;

                codeTables.Add(item);
            }

            role.codeTables = codeTables;
            role.Users = _accountService.GetUser(Guid);

            return View(role);
        }

        public ActionResult RoleEdit(RoleEditVM vM)
        {
            _assignService.setRole(vM, User.Identity.Name);

            return RedirectToAction("MemberList");
        }

        public ActionResult AgentEdit(AgentEditVM vM)
        {
            _assignService.setAgent(vM, User.Identity.Name);

            return RedirectToAction("MemberList");
        }
    }
}