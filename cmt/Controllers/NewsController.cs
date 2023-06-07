using AutoMapper;
using cmt.DTOs;
using cmt.Extensions.ValidationError;
using cmt.Helper;
using cmt.Services.Interfaces;
using cmt.ViewModels;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace cmt.Controllers
{
    public class NewsController : BaseController
    {
        private INewsService _newsService;
        public NewsController(IAuthService authService, ILogService logService, INewsService newsService,ICodeTableService codeTableService)
            : base(authService, logService, newsService, codeTableService)
        {
            _newsService = newsService;
            _newsService.InitialiseIValidationDictionary(new ModelStateWrapper(this.ModelState));
        }

        //[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //public ActionResult Exhibition(ExhibitionVM exhibitionVM, int page = 1)
        //{
        //    if (Request.HttpMethod == WebRequestMethods.Http.Post)
        //    {
        //        _logger.Info("查詢");
        //        System.Web.Helpers.AntiForgery.Validate();
        //    }
        //    else
        //    {
        //        _logger.Info("瀏覽");
        //    }

        //    int pageSize = 10;

        //    if (string.IsNullOrEmpty(exhibitionVM.NewSort))
        //    {
        //        exhibitionVM.NewSort = exhibitionVM.LastSort;
        //    }
        //    else if (exhibitionVM.NewSort == exhibitionVM.LastSort)
        //    {
        //        exhibitionVM.IsSortAsc = !exhibitionVM.IsSortAsc;
        //    }
        //    exhibitionVM.LastSort = exhibitionVM.NewSort;

        //    var config = new MapperConfiguration(cfg => cfg.CreateMap<ExhibitionVM, NewsSearchDTO>()
        //                .ForMember(dest => dest.SortKey, opt => opt.MapFrom(src => src.NewSort)));
        //    var mapper = config.CreateMapper();

        //    NewsSearchDTO newsSearchDTO = mapper.Map<NewsSearchDTO>(exhibitionVM);
        //    newsSearchDTO.Type = 1;

        //    var newsDTOs = _newsService.GetPublicNewsByType(newsSearchDTO);

        //    config = new MapperConfiguration(cfg => cfg.CreateMap<NewsDTO, ExhibitionVM.Exhibition>());
        //    mapper = config.CreateMapper();
        //    exhibitionVM.Exhibitions = mapper.Map<List<NewsDTO>, List<ExhibitionVM.Exhibition>>(newsDTOs).ToPagedList(page, pageSize);

        //    return View(exhibitionVM);
        //}

        //[HttpGet]
        //public ActionResult ExhibitionDetail(int id = 0)
        //{
        //    _logger.Info("瀏覽");

        //    var newsDTO = _newsService.GetPublicNewsById(new NewsDTO() { Id = id });

        //    var config = new MapperConfiguration(cfg => {
        //        cfg.CreateMap<NewsFileDTO, ExhibitionDetailVM.Attach>();
        //        cfg.CreateMap<NewsDTO, ExhibitionDetailVM>();
        //    });
        //    var mapper = config.CreateMapper();
        //    var exhibitionDetailVM = mapper.Map<NewsDTO, ExhibitionDetailVM>(newsDTO);
        //    exhibitionDetailVM.TempId = Guid.NewGuid().ToString("N");

        //    return View(exhibitionDetailVM);
        //}


        [Route("Presentation/{n_id}")]
        [HttpGet]
        public ActionResult Presentation(int n_id)
        {
            _logger.Info("瀏覽");
            var newsDTO = _newsService.GetPublicNewsById(new NewsDTO() { Id = n_id });

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NewsFileDTO, PresentationDetailVM.Attach>();
                cfg.CreateMap<NewsDTO, PresentationDetailVM>();
            });
            var mapper = config.CreateMapper();
            var presentationDetailVM = mapper.Map<NewsDTO, PresentationDetailVM>(newsDTO);
            presentationDetailVM.TempId = Guid.NewGuid().ToString("N");

            return View(presentationDetailVM);
        }

        [Route("PresentationPreview")]
        [HttpPost]
        public string Presentation(PresentationDetailVM presentationDetailVM)
        {
            _logger.Info("瀏覽");
            Session["presentationDetailVM"] = presentationDetailVM;
            return "ok";
        }

        [Route("PresentationPreview")]
        [HttpGet]
        public ActionResult Presentation()
        {
            _logger.Info("瀏覽");

            PresentationDetailVM presentationDetailVM = new PresentationDetailVM();
            if (Session["presentationDetailVM"] != null)
            {
                presentationDetailVM = (PresentationDetailVM)Session["presentationDetailVM"];
                presentationDetailVM.TempId = Guid.NewGuid().ToString("N");
                presentationDetailVM.Create_date = DateTime.Now;
                presentationDetailVM.mode = "Preview";
            }

            return View(presentationDetailVM);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Industry(IndustryVM industryVM, int page = 1)
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

            int pageSize = 10;

            if (string.IsNullOrEmpty(industryVM.NewSort))
            {
                industryVM.NewSort = industryVM.LastSort;
            }
            else if (industryVM.NewSort == industryVM.LastSort)
            {
                industryVM.IsSortAsc = !industryVM.IsSortAsc;
            }
            industryVM.LastSort = industryVM.NewSort;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<IndustryVM, NewsSearchDTO>()
                        .ForMember(dest => dest.SortKey, opt => opt.MapFrom(src => src.NewSort)));
            var mapper = config.CreateMapper();

            NewsSearchDTO newsSearchDTO = mapper.Map<NewsSearchDTO>(industryVM);
            newsSearchDTO.Type = 2;

            var newsDTOs = _newsService.GetPublicNewsByType(newsSearchDTO);

            config = new MapperConfiguration(cfg => cfg.CreateMap<NewsDTO, IndustryVM.Industry>());
            mapper = config.CreateMapper();
            industryVM.Industrys = mapper.Map<List<NewsDTO>, List<IndustryVM.Industry>>(newsDTOs).ToPagedList(page, pageSize);

            return View(industryVM);
        }

        [HttpGet]
        public ActionResult IndustryDetail(int id = 0)
        {
            _logger.Info("瀏覽");

            var newsDTO = _newsService.GetPublicNewsById(new NewsDTO() { Id = id });

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NewsFileDTO, IndustryDetailVM.Attach>();
                cfg.CreateMap<NewsDTO, IndustryDetailVM>();
            });
            var mapper = config.CreateMapper();
            var industryDetailVM = mapper.Map<NewsDTO, IndustryDetailVM>(newsDTO);
            industryDetailVM.TempId = Guid.NewGuid().ToString("N");

            return View(industryDetailVM);
        }

        //[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //public ActionResult Knowledge(KnowledgeVM knowledgeVM, int page = 1)
        //{
        //    if (Request.HttpMethod == WebRequestMethods.Http.Post)
        //    {
        //        _logger.Info("查詢");
        //        System.Web.Helpers.AntiForgery.Validate();
        //    }
        //    else
        //    {
        //        _logger.Info("瀏覽");
        //    }

        //    int pageSize = 10;

        //    if (string.IsNullOrEmpty(knowledgeVM.NewSort))
        //    {
        //        knowledgeVM.NewSort = knowledgeVM.LastSort;
        //    }
        //    else if (knowledgeVM.NewSort == knowledgeVM.LastSort)
        //    {
        //        knowledgeVM.IsSortAsc = !knowledgeVM.IsSortAsc;
        //    }
        //    knowledgeVM.LastSort = knowledgeVM.NewSort;

        //    var config = new MapperConfiguration(cfg => cfg.CreateMap<KnowledgeVM, NewsSearchDTO>()
        //                .ForMember(dest => dest.SortKey, opt => opt.MapFrom(src => src.NewSort)));
        //    var mapper = config.CreateMapper();

        //    NewsSearchDTO newsSearchDTO = mapper.Map<NewsSearchDTO>(knowledgeVM);
        //    newsSearchDTO.Type = 3;

        //    var newsDTOs = _newsService.GetPublicNewsByType(newsSearchDTO);

        //    config = new MapperConfiguration(cfg => cfg.CreateMap<NewsDTO, KnowledgeVM.Knowledge>());
        //    mapper = config.CreateMapper();
        //    knowledgeVM.Knowledges = mapper.Map<List<NewsDTO>, List<KnowledgeVM.Knowledge>>(newsDTOs).ToPagedList(page, pageSize);

        //    return View(knowledgeVM);
        //}

        //[HttpGet]
        //public ActionResult KnowledgeDetail(int id = 0)
        //{
        //    _logger.Info("瀏覽");

        //    var newsDTO = _newsService.GetPublicNewsById(new NewsDTO() { Id = id });

        //    var config = new MapperConfiguration(cfg => {
        //        cfg.CreateMap<NewsFileDTO, KnowledgeDetailVM.Attach>();
        //        cfg.CreateMap<NewsDTO, KnowledgeDetailVM>();
        //    });
        //    var mapper = config.CreateMapper();
        //    var knowledgeDetailVM = mapper.Map<NewsDTO, KnowledgeDetailVM>(newsDTO);
        //    knowledgeDetailVM.TempId = Guid.NewGuid().ToString("N");

        //    return View(knowledgeDetailVM);
        //}

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Event(EventVM eventVM, int page = 1)
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

            int pageSize = 10;

            if (string.IsNullOrEmpty(eventVM.NewSort))
            {
                eventVM.NewSort = eventVM.LastSort;
            }
            else if (eventVM.NewSort == eventVM.LastSort)
            {
                eventVM.IsSortAsc = !eventVM.IsSortAsc;
            }
            eventVM.LastSort = eventVM.NewSort;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<EventVM, NewsSearchDTO>()
                        .ForMember(dest => dest.SortKey, opt => opt.MapFrom(src => src.NewSort)));
            var mapper = config.CreateMapper();

            NewsSearchDTO newsSearchDTO = mapper.Map<NewsSearchDTO>(eventVM);
            newsSearchDTO.Type = 4;

            var newsDTOs = _newsService.GetPublicNewsByType(newsSearchDTO);

            config = new MapperConfiguration(cfg => cfg.CreateMap<NewsDTO, EventVM.Event>());
            mapper = config.CreateMapper();
            eventVM.Events = mapper.Map<List<NewsDTO>, List<EventVM.Event>>(newsDTOs).ToPagedList(page, pageSize);

            return View(eventVM);
        }

        [HttpGet]
        public ActionResult EventDetail(int id = 0)
        {
            _logger.Info("瀏覽");

            var newsDTO = _newsService.GetPublicNewsById(new NewsDTO() { Id = id });

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<NewsFileDTO, EventDetailVM.Attach>();
                cfg.CreateMap<NewsDTO, EventDetailVM>();
            });
            var mapper = config.CreateMapper();
            var exhibitionDetailVM = mapper.Map<NewsDTO, EventDetailVM>(newsDTO);
            exhibitionDetailVM.TempId = Guid.NewGuid().ToString("N");

            return View(exhibitionDetailVM);
        }

        [HttpGet]
        public ActionResult GetContentImage(string id, string fileName)
        {
            _logger.Info("下載");

            fileName = PathHelper.MakeFilenameValid(fileName);

            FileDTO fileDTO = new FileDTO { Id = id, Name = Path.GetFileName(fileName), Type = "ContentImage" };
            fileDTO = _newsService.GetFile(fileDTO);

            if (fileDTO != null)
            {
                return File(Path.Combine(fileDTO.Path, fileDTO.Name), fileDTO.ContentType, fileDTO.Name);
            }
            return null;
        }

        [HttpGet]
        public ActionResult DownloadFile(string id, string fileName)
        {
            _logger.Info("下載");

            fileName = PathHelper.MakeFilenameValid(fileName);

            FileDTO fileDTO = new FileDTO { Id = id, Name = Path.GetFileName(fileName), Type = "Attach" };

            fileDTO = _newsService.GetFile(fileDTO);

            if (fileDTO != null)
            {
                return File(Path.Combine(fileDTO.Path, fileDTO.Name), fileDTO.ContentType, fileDTO.Name);
            }

            return null;
        }
    }
}