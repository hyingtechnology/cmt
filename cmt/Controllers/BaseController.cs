using cmt.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cmt.Controllers
{
    public class BaseController : Controller
    {
        public readonly IAuthService _authService;
        public readonly ILogService _logService;
        public readonly INewsService _newsService;
        public readonly ICodeTableService _codeTableService;
        protected ILogger _logger;

        protected BaseController(IAuthService authService, ILogService logService, INewsService newsService,ICodeTableService codeTableService)
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
                ViewBag.isManager = ((System.Web.Security.FormsIdentity)((System.Security.Principal.GenericPrincipal)User).Identity).Ticket.UserData != string.Empty;
            }
            ViewBag.isLogin = !string.IsNullOrEmpty(name);

            /*
             新增大分類做法
            1.增加codetable
            2.增加  ViewBag.menuModel
            3.增加   ViewBag.menuModel6
            4.去chtml 增加
            5.把分類加過去就出現了
             */

            ViewBag.menuModel = menuModel.Where(x=> 
            ( x.parent_type =="03" && x.Title.Trim() != _codeTableService.Get_Desc("01", x.parent_type)) ||
            ( x.parent_type == "04" && x.Title.Trim() != _codeTableService.Get_Desc("01", x.parent_type)) ||
            (x.parent_type == "05" && x.Title.Trim() != _codeTableService.Get_Desc("01", x.parent_type)) ||
             (x.parent_type == "06" && x.Title.Trim() != _codeTableService.Get_Desc("01", x.parent_type))).ToList() ;

            ViewBag.menuModel3 = menuModel.Where(x => x.parent_type == "03" && x.Title.Trim() == _codeTableService.Get_Desc("01",x.parent_type)).ToList();
            ViewBag.menuModel4 = menuModel.Where(x => x.parent_type == "04" && x.Title.Trim() == _codeTableService.Get_Desc("01", x.parent_type)).ToList();
            ViewBag.menuModel5 = menuModel.Where(x => x.parent_type == "05" && x.Title.Trim() == _codeTableService.Get_Desc("01", x.parent_type)).ToList();
            ViewBag.menuModel6 = menuModel.Where(x => x.parent_type == "06" && x.Title.Trim() == _codeTableService.Get_Desc("01", x.parent_type)).ToList();

            ViewBag.PageViewCount = _logService.GetPageViewCount();
            ViewBag.LastUpdatedDate = _newsService.GetLastModifiedDate()?.ToString("yyyy/MM/dd");

            ViewBag.NickName = User.Identity.Name;
        }
    }
}