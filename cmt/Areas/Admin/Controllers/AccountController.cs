using cmt.Areas.Admin.Helper;
using cmt.Services;
using cmt.Services.Interfaces;
using cmt.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cmt.Areas.Admin.Controllers
{
   
    public class AccountController : BaseController
    {
        IAccountService _accountService;
        public AccountController(IAuthService authService, ILogService logService, INewsService newsService, ICodeTableService codeTableService, IAccountService accountService)
            : base(authService, logService, newsService, codeTableService)
        {
            _accountService = accountService;
        }

        [AccessDeniedAuthorizeAttribute(Roles = "admin,adminUser")]
        [Authorize(Roles = "admin,adminUser")]
        public ActionResult MemberEdit(string Guid)
        {
            UserLoginVM models = new UserLoginVM();
            if (!string.IsNullOrEmpty(Guid))
            {
                models = _accountService.GetUser(Guid);
            }
            
            return View(models);
        }

        [AccessDeniedAuthorizeAttribute(Roles = "admin,adminUser")]
        [Authorize(Roles = "admin,adminUser")]
        [HttpPost] 
        public ActionResult MemberEdit(UserLoginVM userLoginVM)
        {
          
            if (!ModelState.IsValid)
            {
                
                return View(userLoginVM);
            }
            else
            {
                var userAll = _accountService.GetUserAll();
                var item = userAll.Where(x => x.UserGuid == userLoginVM.UserGuid);
                var EmailExisted = userAll.Where(x => x.UserEmail == userLoginVM.UserEmail).Count() > 0;
                var AccountExisted = userAll.Where(x => x.UserAccount == userLoginVM.UserAccount).Count() > 0;
                var Idno = IdnoHelper.CheckIdno(userLoginVM.UserAccount);
                if ((string.IsNullOrEmpty(userLoginVM.UserGuid) && EmailExisted) || 
                    (!string.IsNullOrEmpty(userLoginVM.UserGuid) && item.Count()>0 && item.First().UserEmail != userLoginVM.UserEmail && EmailExisted))
                {
                    userLoginVM.Error = "E-mail 已存在";
                }
                if (string.IsNullOrEmpty(userLoginVM.UserGuid) && AccountExisted)
                {
                    userLoginVM.Error = "帳號 已存在";
                }
                if (Idno)
                {
                    userLoginVM.Error = "不可使用身分證字號作為帳號";
                }

                if (string.IsNullOrEmpty(userLoginVM.Error))
                {

                    //新增的話
                    if (string.IsNullOrEmpty(userLoginVM.UserGuid))
                    {
                        var pwd = "default_" + Guid.NewGuid().ToString("N");
                        userLoginVM.UserGuid = Guid.NewGuid().ToString();
                        userLoginVM.CreateTime = DateTime.Now.ToString("yyyy-MM-dd");
                        userLoginVM.UserPassWord = Helper.Hash.SHA1(pwd);
                        Helper.MailHepler mailHepler = new MailHepler();
                        mailHepler.SendPwd(userLoginVM.UserEmail, pwd);
                    }
                    else
                    {
                        userLoginVM.UserPassWord = "";
                    }

                    _accountService.UpdateUser(userLoginVM);

                    return RedirectToAction("MemberList");
                }
                else
                {
                    return View(userLoginVM);
                }
            }
        }
        [AccessDeniedAuthorizeAttribute(Roles = "admin,adminUser")]
        [Authorize(Roles = "admin,adminUser")]
        [HttpPost]
        public ActionResult MemberDelete(string Guid)
        {
            _accountService.DeleteUser(Guid);

            return RedirectToAction("MemberList");
        }
        [AccessDeniedAuthorizeAttribute(Roles = "admin/account/memberlist")]
        [Authorize(Roles = "admin/account/memberlist")]
        public ActionResult MemberList()
        {
           var models = _accountService.GetUserAll();
            return View(models);
        }

        public ActionResult Denied()
        {
            return View();
        }


    }
}