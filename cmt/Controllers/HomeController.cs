using AutoMapper;
using cmt.DTOs;
using cmt.Helper;
using cmt.Models;
using cmt.Services.Interfaces;
using cmt.ViewModels;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using System.Web.UI;

namespace cmt.Controllers
{
    public class HomeController : BaseController
    {
        private INewsService _newsService;
        public HomeController(IAuthService authService, ILogService logService, INewsService newsService,ICodeTableService codeTableService)
            : base(authService, logService, newsService, codeTableService)
        {
            _newsService = newsService;
        }

       
        [HttpGet]
        public ActionResult Index()
        {
            _logger.Info("瀏覽");

            var indexVM = new IndexVM();

            List<NewsDTO> newsDTOs;
            MapperConfiguration config;
            IMapper mapper;
            newsDTOs = _newsService.GetPublicTopNewsByType(new NewsSearchDTO() { Type = 0 }, 3);
            config = new MapperConfiguration(cfg => cfg.CreateMap<NewsDTO, IndexVM.News>());
            mapper = config.CreateMapper();
            indexVM.Sliders = mapper.Map<List<NewsDTO>, List<IndexVM.News>>(newsDTOs);

            newsDTOs = _newsService.GetPublicTopNewsByType(new NewsSearchDTO() { Type = 2 }, 3);
            config = new MapperConfiguration(cfg => cfg.CreateMap<NewsDTO, IndexVM.News>());
            mapper = config.CreateMapper();
            indexVM.Industrys = mapper.Map<List<NewsDTO>, List<IndexVM.News>>(newsDTOs);

            newsDTOs = _newsService.GetPublicTopNewsByType(new NewsSearchDTO() { Type = 3 }, 1);
            config = new MapperConfiguration(cfg => cfg.CreateMap<NewsDTO, IndexVM.News>());
            mapper = config.CreateMapper();
            indexVM.Knowledges = mapper.Map<List<NewsDTO>, List<IndexVM.News>>(newsDTOs);

            newsDTOs = _newsService.GetPublicTopNewsByType(new NewsSearchDTO() { Type = 4 }, 1);
            config = new MapperConfiguration(cfg => cfg.CreateMap<NewsDTO, IndexVM.News>());
            mapper = config.CreateMapper();
            indexVM.Events = mapper.Map<List<NewsDTO>, List<IndexVM.News>>(newsDTOs);

            return View(indexVM);
        }

        [HttpGet]
        public ActionResult PageIdea()
        {
            _logger.Info("瀏覽");

            return View();
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult SiteSearch()
        {
            if (Request.HttpMethod == WebRequestMethods.Http.Post)
            {
                _logger.Info("查詢");
                System.Web.Helpers.AntiForgery.Validate();
            }
            else
            {
                _logger.Info("瀏覽");
            }

            return View();
        }

        [HttpGet]
        public ActionResult PrivacyPolicy()
        {
            _logger.Info("瀏覽");

            return View();
        }

        [HttpGet]
        public ActionResult CyberSecurityPolicy()
        {
            _logger.Info("瀏覽");

            return View();
        }

        [HttpGet]
        public ActionResult Sitemap()
        {
            _logger.Info("瀏覽");

            return View();
        }

        [HttpGet]
        public ActionResult GetNewsPicture(string id, string fileName)
        {
            _logger.Info("下載");

            fileName = PathHelper.MakeFilenameValid(fileName);

            FileDTO fileDTO = new FileDTO { Id = id, Name = Path.GetFileName(fileName), Type = "Picture" };
            fileDTO = _newsService.GetFile(fileDTO);

            if (fileDTO != null)
            {
                return File(Path.Combine(fileDTO.Path, fileDTO.Name), fileDTO.ContentType, fileDTO.Name);
            }
            return null;
        }

        public ActionResult SingOut()
        {
            FormsAuthentication.SignOut();

            return Redirect(ConfigurationManager.AppSettings["LoginOut"].ToString());
        }

     
    }
}