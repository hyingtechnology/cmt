using cmt.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cmt.Controllers
{
    public class ExhibitionController : BaseController
    {
        private INewsService _newsService;
        public ExhibitionController(IAuthService authService, ILogService logService, INewsService newsService,ICodeTableService codeTableService)
            : base(authService, logService, newsService, codeTableService)
        {
            _newsService = newsService;
        }

        

        [HttpGet]
        public ActionResult PageExhibitionNormal()
        {
            _logger.Info("瀏覽");

            return View();
        }

        [HttpGet]
        public ActionResult PageExhibitionNormal01()
        {
            _logger.Info("瀏覽");

            return View();
        }

        [HttpGet]
        public ActionResult PageExhibitionNormal02()
        {
            _logger.Info("瀏覽");

            return View();
        }

        [HttpGet]
        public ActionResult PageExhibitionNormal03()
        {
            _logger.Info("瀏覽");

            return View();
        }

        [HttpGet]
        public ActionResult PageExhibitionNormal04()
        {
            _logger.Info("瀏覽");

            return View();
        }

        [HttpGet]
        public ActionResult PageExhibitionNormal05()
        {
            _logger.Info("瀏覽");

            return View();
        }

        [HttpGet]
        public ActionResult PageExhibitionNormal06()
        {
            _logger.Info("瀏覽");

            return View();
        }

        [HttpGet]
        public ActionResult IntelliTech()
        {
            _logger.Info("瀏覽");

            return View();
        }

        [HttpGet]
        public ActionResult ChemicalTrading()
        {
            _logger.Info("瀏覽");

            return View();
        }


    }
}