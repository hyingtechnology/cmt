using cmt.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cmt.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        public readonly IAuthService _authService;
        public readonly ILogService _logService;
        public readonly INewsService _newsService;
        public readonly ICodeTableService _codeTableService;
        protected ILogger _logger;

        protected BaseController(IAuthService authService, ILogService logService, INewsService newsService, ICodeTableService codeTableService)
        {
            _authService = authService;
            _logService = logService;
            _newsService = newsService;
            _codeTableService = codeTableService;
            _logger = LogManager.GetLogger(GetType().FullName);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var menuModel = _newsService.GetNewsByCodeTableType("01");

            ViewBag.isManager = false;
            ViewBag.isLogin = false;
            var name = User.Identity.Name;
            if (User != null && !string.IsNullOrEmpty(name))
            {
                ViewBag.isManager = getRoles() == "admin";
            }
            ViewBag.isLogin = !string.IsNullOrEmpty(name);

            ViewBag.menuModel = menuModel;

            ViewBag.PageViewCount = _logService.GetPageViewCount();
            ViewBag.LastUpdatedDate = _newsService.GetLastModifiedDate()?.ToString("yyyy/MM/dd");
            ViewBag.NickName = User.Identity.Name;
        }
        /// <summary>
        /// 取得身份
        /// </summary>
        /// <returns></returns>
        public string getRoles()
        {
            return ((System.Web.Security.FormsIdentity)((System.Security.Principal.GenericPrincipal)User).Identity).Ticket.UserData;
        }
    }
}