using AutoMapper;
using cmt.Areas.Admin.ViewModels;
using cmt.DTOs;
using cmt.Extensions.ValidationError;
using cmt.Services.Interfaces;
using Microsoft.Ajax.Utilities;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace cmt.Areas.Admin.Controllers
{
    public class AuthController : BaseController
    {
        public AuthController(IAuthService authService, ILogService logService, INewsService newsService,ICodeTableService codeTableService)
            : base(authService, logService, newsService, codeTableService)
        {
            _authService.InitialiseIValidationDictionary(new ModelStateWrapper(this.ModelState));
        }

      

        [HttpGet]
        public ActionResult Login()
        {
            _logger.Info("瀏覽");

            return View();
        }

        [HttpPost]
        
        public ActionResult Login(LoginVM loginVM)
        {
            _logger.Info("登入");

            var config = new MapperConfiguration(cfg => cfg.CreateMap<LoginVM, LoginDTO>());
            var mapper = config.CreateMapper();
            var loginDTO = mapper.Map<LoginDTO>(loginVM);

            if (!ModelState.IsValid || !_authService.IsValidLogin(loginDTO))
            {
                return View(loginVM);
            }

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(loginDTO.Account, true, 60);

            //ticket 生成 cookie
            string encTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            httpCookie.HttpOnly = true;
            //httpCookie.Secure = true;

            //cookie 寫入 response
            Response.Cookies.Add(httpCookie);
            _logger.Info("登入成功");

            return RedirectToAction("Exhibition", "News");

        }
    }
}