
using cmt.App_Start;
using cmt.Controllers;
using cmt.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace cmt
{
    
    public class MvcApplication : System.Web.HttpApplication
    {


        string CookieDomainName = ConfigurationManager.AppSettings["CookieDomainName"].ToString();
        string CookiePath = ConfigurationManager.AppSettings["CookiePath"].ToString();
        protected void Application_Start()
        {

            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceScopeModule.SetServiceProvider(services.BuildServiceProvider());


            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;

            AutofacConfig.Register();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

         //   DependencyResolver.SetResolver(new ServiceProviderDependencyResolver());
        }

        protected void Application_Error()
        {
            // Sets 404 HTTP exceptions to be handled via IIS (behavior is specified in the "httpErrors" section in the Web.config file)
            var error = Server.GetLastError();
            if ((error as HttpException)?.GetHttpCode() == 404)
            {
                Server.ClearError();
                Response.StatusCode = 404;
            }


            var exception = Server.GetLastError();
            var httpException = exception as HttpException;
            Response.Clear();
            Server.ClearError();
            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = "Error";
            routeData.Values["exception"] = exception;
            Response.ContentType = "text/html";
            Response.StatusCode = 500;
            if (httpException != null)
            {
                Response.StatusCode = httpException.GetHttpCode();
                switch (Response.StatusCode)
                {
                    case 404:
                        routeData.Values["action"] = "PageNotFound";
                        break;
                }
            }

            //IController errorsController = new cmt.Controllers.ErrorController();
            //var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
            //errorsController.Execute(rc);

        }

         

        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            Cookie_Security();
        }

        void Cookie_Security()
        {
            // Iterate through any cookies found in the Response object.
            //if (HttpContext.Current.Response.Cookies.Count > 0)
            //{
            //    foreach (string s in HttpContext.Current.Response.Cookies.AllKeys)
            //    {
            //        HttpContext.Current.Response.Cookies[s].Secure = true;
            //        HttpContext.Current.Response.Cookies[s].SameSite = SameSiteMode.Strict;
            //        HttpContext.Current.Response.Cookies[s].HttpOnly = true;
            //    }
            //}
        }

        public static string PersistKeysToFileSystem = ConfigurationManager.AppSettings["PersistKeysToFileSystem"].ToString();

        private void ConfigureServices(IServiceCollection services)
        {
            
           

            services.AddScoped<ScopedThing>();
            services.AddTransient<HomeController>();

          
            services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.Events.OnSigningOut = BuildSigningOut;
                options.Events.OnRedirectToLogin = BuildRedirectToLogin;
                options.Cookie.Name = Common.CookieName;
                options.Cookie.Path = "/";
                options.Cookie.HttpOnly = true;
                options.Cookie.Domain = CookieDomainName;
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.None;
                options.DataProtectionProvider = DataProtectionProvider.Create(new DirectoryInfo(PersistKeysToFileSystem));
                //因為有設定OnRedirectToLogin 的原因所以，改理解為目前要導回的頁面
                options.SlidingExpiration = true;
                options.LoginPath = "/home/index";
            }).AddCookie();


            services.AddDataProtection()
                 .UseCryptographicAlgorithms(
                      new AuthenticatedEncryptorConfiguration()
                      {
                          EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                          ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                      }
                  )
             .PersistKeysToFileSystem(new DirectoryInfo(PersistKeysToFileSystem))
             .SetApplicationName(Common.ApplicationName).
             SetDefaultKeyLifetime(TimeSpan.FromDays(90));

            services.AddSession(options =>
            {
          

                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.Name = Common.SessionName;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
                options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                options.CookieHttpOnly = true;

            });

            // 指定Session儲存方式:分發記憶體快取
            services.AddDistributedMemoryCache();

        }

       

        /// <summary>
        /// 注销，引导跳转认证中心登录页面
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task BuildSigningOut(CookieSigningOutContext context)
        {
            
            var returnUrl = new UriBuilder
            {
                Host = context.Request.Host.Host,
                Port = context.Request.Host.Port ?? 80,
            };
            var redirectUrl = new UriBuilder
            {
                Host = CookieDomainName,
                Path = "/" + CookiePath + "/home/Login",
                Query = Microsoft.AspNetCore.Http.QueryString.Create(context.Options.ReturnUrlParameter, returnUrl.Uri.ToString()).Value
            };
            context.Response.Redirect(redirectUrl.Uri.ToString());
            return Task.CompletedTask;
        }


        /// <summary>
        /// 登入自動跳轉在這調整
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task BuildRedirectToLogin(RedirectContext<CookieAuthenticationOptions> context)
        {
            var currentUrl = new UriBuilder(context.RedirectUri);
            var returnUrl = new UriBuilder
            {
                Host = currentUrl.Host,
                Port = currentUrl.Port,
                Path = currentUrl.Path
            };
            var redirectUrl = new UriBuilder
            {
                Host = currentUrl.Host,
                Path = "/" + CookiePath + "/home/Login",
                Query = Microsoft.AspNetCore.Http.QueryString.Create(context.Options.ReturnUrlParameter, returnUrl.Uri.ToString()).Value
            };
            context.Response.Redirect(redirectUrl.Uri.ToString());
            return Task.CompletedTask;
        }
        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            Login();
        }

        string UnicodeToString(string srcText)
        {
            string dst = "";
            string src = srcText;
            int len = srcText.Length / 6;

            for (int i = 0; i <= len - 1; i++)
            {
                string str = "";
                str = src.Substring(0, 6).Substring(2);
                src = src.Substring(6);
                byte[] bytes = new byte[2];
                bytes[1] = byte.Parse(int.Parse(str.Substring(0, 2), System.Globalization.NumberStyles.HexNumber).ToString());
                bytes[0] = byte.Parse(int.Parse(str.Substring(2, 2), System.Globalization.NumberStyles.HexNumber).ToString());
                dst += Encoding.Unicode.GetString(bytes);
            }
            return dst;
        }

        protected void Application_AuthenticateRequest(Object sender,EventArgs e)
        {
            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null || authCookie.Value == "")
            {
                return;
            }
            FormsAuthenticationTicket authTicket = null;
            try
            {
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            }
            catch
            {
                return;
            }
            string[] roles = authTicket.UserData.Split(new char[] { ',' });
            if (Context.User != null)
            {
                Context.User = new System.Security.Principal.GenericPrincipal(Context.User.Identity,roles);
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

            Login();



          //  HttpCookie authCookie = Request.Cookies[Common.CookieName];
            //if (authCookie != null)
            //{

            //    try
            //    {

            //        var provider = DataProtectionProvider.Create(new DirectoryInfo(PersistKeysToFileSystem));

            //        //Get a data protector to use with either approach
            //        var dataProtector = provider.CreateProtector("Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware", "Cookies", "v2");


            //        //Get the decrypted cookie as plain text
            //        UTF8Encoding specialUtf8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
            //        byte[] protectedBytes = Base64UrlTextEncoder.Decode(authCookie.Value);
            //        byte[] plainBytes = dataProtector.Unprotect(protectedBytes);
            //        string plainText = specialUtf8Encoding.GetString(plainBytes);


            //       // if (plainText.IndexOf("管理者") >= 0)
            //       // {

            //            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket( 1,
            // "admin",
            // DateTime.Now,
            // DateTime.Now.AddYears(1),
            //false,
            // String.Join("|", "admin"));
            //            string encTicket = FormsAuthentication.Encrypt(ticket);
            //            HttpCookie httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            //            httpCookie.HttpOnly = true;
            //            //httpCookie.Secure = true;
            //            //cookie 寫入 response
            //            Response.Cookies.Add(httpCookie);


            //      //  }


            //}
            //catch(Exception ex)
            //{

            //}
        }

            
      

    void Login()
        {
            HttpCookie authCookie = Request.Cookies[Common.CookieName];
            if (authCookie != null)
            {

                try
                {

                    var provider = DataProtectionProvider.Create(new DirectoryInfo(PersistKeysToFileSystem));

                    //Get a data protector to use with either approach
                    var dataProtector = provider.CreateProtector("Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationMiddleware", "Cookies", "v2");


                    //Get the decrypted cookie as plain text
                    UTF8Encoding specialUtf8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
                    byte[] protectedBytes = Base64UrlTextEncoder.Decode(authCookie.Value);
                    byte[] plainBytes = dataProtector.Unprotect(protectedBytes);
                    string plainText = Regex.Replace(specialUtf8Encoding.GetString(plainBytes), @"[\W_]+", "|").Replace("||", "");


                    /*
                     |Cookies|Cookies|default|Nickname|王大明|Company|管理者|ProjectName|填海造陸|Type|A|http|schemas|microsoft|com|ws|2008|06|identity|claims|role|A|Dhttp|schemas|microsoft|com|ws|2008|06|identity|claims|serialnumber|D5284574|A920|4A45|A630|8B75E567D5EE|issued|Thu|08|Jul|2021|06|52|26|GMT|expires|Thu|22|Jul|2021|06|52|26|GMT
                     
                     */
                    var Data = plainText.Split('|');
                    var Nickname = "";
                    string Role = "";
                    bool isFind_serialnumber = false;
                    string serialnumber = "";
                    string account = "";
                    if (Data.Length >= 3)
                    {
                        account = Data[3];
                    }
                    for (int i = 0; i < Data.Length; i++)
                    {
                        var d = Data[i];

                        if (isFind_serialnumber && d.ToLower()!= "issued")
                        {
                            if (!string.IsNullOrEmpty(serialnumber)) serialnumber += "-";
                             serialnumber += Data[i];
                        }

                        switch (d.ToLower())
                        {
                            case "role":
                                Role = Data[i + 1];
                                break;
                            case "nickname":
                                Nickname = Data[i + 1];
                                break;
                            case "serialnumber":
                                isFind_serialnumber = true;
                                break;
                            case "issued":
                                isFind_serialnumber = false;
                                break;
                        }

                        
                    }

                    AccountService accountService = new AccountService();
                    var identityUserVMs = accountService.GetAuthorityUserPage(serialnumber,"/cmt");



                    string admin = "";
                    if (identityUserVMs.Count() > 0)
                    {
                        admin = "admin";
                        foreach(var m in identityUserVMs)
                        {
                            admin += ",";
                            string[] tokens = m.ToLower().Split(new[] { "/cmt/" }, StringSplitOptions.None);
                            if (tokens.Count() >1)
                            {
                                admin += tokens[1].ToLower();
                            }
                            else
                            {
                                admin += "/cmt";
                            }
                        }

                    }
                    else
                    {
                        admin = "Guest";
                    }
                    // Session["NlogUser"] = serialnumber;

                    var cookie = HttpContext.Current.Request.Cookies["NlogUser"] ?? new HttpCookie("NlogUser");
                    cookie.HttpOnly = true;
                    cookie.Path = "/NlogUser";
                    cookie.Value = account;
                    HttpContext.Current.Response.Cookies.Add(cookie);

                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                         Nickname,
                         DateTime.Now,
                         DateTime.Now.AddYears(1),
                        false,
                         admin);
                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    HttpCookie httpCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    httpCookie.HttpOnly = true;
                    httpCookie.Secure = true;
                    //cookie 寫入 response
                    Response.Cookies.Add(httpCookie);
                }
                catch (Exception ex)
                {
                    //HttpCookie httpCookie = new HttpCookie("test", ex.Message);
                    //httpCookie.HttpOnly = true;
                    //httpCookie.Secure = true;
                    ////cookie 寫入 response
                    //Response.Cookies.Add(httpCookie);
                }
            }
        }
  

       

      

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Headers.Remove("X-AspNet-Version");
            response.Headers.Remove("x-aspnetmvc-version");
            response.Headers.Remove("Server");
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
             string sidCookieName = "__RequestVerificationToken";
          //  string sidCookieName = Common.CookieName;
            if (Request.Cookies[sidCookieName] != null)
            {
                HttpCookie sidCookie = Response.Cookies[sidCookieName];
                sidCookie.Value = Session.SessionID;
                sidCookie.HttpOnly = true;
                sidCookie.Secure = true;
                sidCookie.SameSite = SameSiteMode.Strict;
                sidCookie.Path = "/";
            }

          
            Cookie_Security();

        }
    }
    public class ScopedThing : IDisposable
    {
        public ScopedThing()
        {

        }
        public void Dispose()
        {
        }
    }

    internal class ServiceProviderDependencyResolver : IDependencyResolver
    {
        public object GetService(Type serviceType)
        {
            if (HttpContext.Current?.Items[typeof(IServiceScope)] is IServiceScope scope)
            {
                return scope.ServiceProvider.GetService(serviceType);
            }

            throw new InvalidOperationException("IServiceScope not provided");
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (HttpContext.Current?.Items[typeof(IServiceScope)] is IServiceScope scope)
            {
                return scope.ServiceProvider.GetServices(serviceType);
            }

            throw new InvalidOperationException("IServiceScope not provided");
        }
    }

    internal class ServiceScopeModule : IHttpModule
    {
        private static ServiceProvider _serviceProvider;

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += Context_BeginRequest;
            context.EndRequest += Context_EndRequest;
        }

        private void Context_EndRequest(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            if (context.Items[typeof(IServiceScope)] is IServiceScope scope)
            {
                scope.Dispose();
            }
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            context.Items[typeof(IServiceScope)] = _serviceProvider.CreateScope();
        }

        public static void SetServiceProvider(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


    }

}
