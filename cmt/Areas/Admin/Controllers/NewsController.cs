using AutoMapper;
using cmt.Areas.Admin.ViewModels;
using cmt.DTOs;
using cmt.Extensions.CustomFilter;
using cmt.Extensions.ValidationError;
using cmt.Helper;
using cmt.Models;
using cmt.Services;
using cmt.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using X.PagedList;

namespace cmt.Areas.Admin.Controllers
{
    [Authorize]
    public class NewsController : BaseController
    {
        private ICodeTableService _codeTableService;
        private INewsService _newsService;
        public NewsController(IAuthService authService, ILogService logService, INewsService newsService,ICodeTableService codeTableService)
            : base(authService, logService, newsService, codeTableService)
        {
            _codeTableService = codeTableService;
            _newsService = newsService;
            _newsService.InitialiseIValidationDictionary(new ModelStateWrapper(this.ModelState));
        }

        #region 後臺管理
        [Authorize(Roles = "admin")]
        public ActionResult ManagerCenter()
        {
            _logger.Info("瀏覽");
            ViewData["role"] = getRoles();



            return View();
        }
        #endregion

        #region 線上展覽
        [Authorize(Roles = "admin/news/presentation")]
        
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Presentation(PresentationVM presentationVM, int page = 1)
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

            if (string.IsNullOrEmpty(presentationVM.NewSort))
            {
                presentationVM.NewSort = presentationVM.LastSort;
            }
            else if (presentationVM.NewSort == presentationVM.LastSort)
            {
                presentationVM.IsSortAsc = !presentationVM.IsSortAsc;
            }
            presentationVM.LastSort = presentationVM.NewSort;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<PresentationVM, NewsSearchDTO>()
                        .ForMember(dest => dest.SortKey, opt => opt.MapFrom(src => src.NewSort)));
            var mapper = config.CreateMapper();

            NewsSearchDTO newsSearchDTO = mapper.Map<NewsSearchDTO>(presentationVM);
            newsSearchDTO.Type = 5;

            var newsDTOs = _newsService.GetNewsByType(newsSearchDTO);

            config = new MapperConfiguration(cfg => cfg.CreateMap<NewsDTO, PresentationVM.Presentation>());
            mapper = config.CreateMapper();
            presentationVM.Presentations = mapper.Map<List<NewsDTO>, List<PresentationVM.Presentation>>(newsDTOs).ToPagedList(page, pageSize);

            return View(presentationVM);
        }

        [HttpGet]
        public ActionResult PresentationEdit(int id = 0)
        {
            _logger.Info("瀏覽");

            var newsDTO = _newsService.GetNewsById(new NewsDTO() { Id = id });

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NewsFileDTO, PresentationEditVM.Attach>();
                cfg.CreateMap<NewsDTO, PresentationEditVM>();
            });
            var mapper = config.CreateMapper();
            var presentationEditVM = mapper.Map<NewsDTO, PresentationEditVM>(newsDTO);
            presentationEditVM.TempId = Guid.NewGuid().ToString("N");
            if (id == 0)
            {
                presentationEditVM.Start_date = DateTime.Today;
            }
            presentationEditVM.codeTables = _codeTableService.GetCodeTableByType("01");
            var Identity = User.IsInRole("IEK");
            ViewData["IEK"] = Identity;
            return View(presentationEditVM);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult PresentationEdit(PresentationEditVM presentationEditVM)
        {
            _logger.Info("修改");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PresentationEditVM, NewsDTO>()
                .ForMember(dest => dest.Picture, opt =>
                {
                    opt.MapFrom(src => src.PictureFile == null ? src.Picture : new FileInfo(src.PictureFile.FileName).Name);
                });
            });

            var mapper = config.CreateMapper();
            var newsDTO = mapper.Map<PresentationEditVM, NewsDTO>(presentationEditVM);
            newsDTO.Type = 5;
            newsDTO.Creator = User.Identity.Name;
            newsDTO.Create_date = DateTime.Now;
            newsDTO.Modifier = User.Identity.Name;
            newsDTO.Modified_date = DateTime.Now;
             
            if (!ModelState.IsValid || !_newsService.UpdateNews(newsDTO))
            {
                var Identity = User.IsInRole("IEK");
                ViewData["IEK"] = Identity;
                return View(presentationEditVM);
            }

            return RedirectToAction("Presentation");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PresentationDelete(int Id)
        {
            _logger.Info("刪除");

            if (!ModelState.IsValid || !_newsService.DeleteNews(new NewsDTO() { Id = Id }))
            {
                return RedirectToAction("Presentation");
            }
            return RedirectToAction("Presentation");
        }

        #endregion

        #region 展覽公告
        [AccessDeniedAuthorizeAttribute(Roles = "admin/news/exhibition")]
        [Authorize(Roles = "admin/news/exhibition")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Exhibition(ExhibitionVM exhibitionVM, int page = 1)
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

            if (string.IsNullOrEmpty(exhibitionVM.NewSort))
            {
                exhibitionVM.NewSort = exhibitionVM.LastSort;
            }
            else if (exhibitionVM.NewSort == exhibitionVM.LastSort)
            {
                exhibitionVM.IsSortAsc = !exhibitionVM.IsSortAsc;
            }
            exhibitionVM.LastSort = exhibitionVM.NewSort;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<ExhibitionVM, NewsSearchDTO>()
                        .ForMember(dest => dest.SortKey, opt => opt.MapFrom(src => src.NewSort)));
            var mapper = config.CreateMapper();

            NewsSearchDTO newsSearchDTO = mapper.Map<NewsSearchDTO>(exhibitionVM);
            newsSearchDTO.Type = 1;

            var newsDTOs = _newsService.GetNewsByType(newsSearchDTO);

            config = new MapperConfiguration(cfg => cfg.CreateMap<NewsDTO, ExhibitionVM.Exhibition>());
            mapper = config.CreateMapper();
            exhibitionVM.Exhibitions = mapper.Map<List<NewsDTO>, List<ExhibitionVM.Exhibition>>(newsDTOs).ToPagedList(page, pageSize);

            return View(exhibitionVM);
        }

        [HttpGet]
        public ActionResult ExhibitionEdit(int id = 0)
        {
            _logger.Info("瀏覽");

            var newsDTO = _newsService.GetNewsById(new NewsDTO() { Id = id });

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NewsFileDTO, ExhibitionEditVM.Attach>();
                cfg.CreateMap<NewsDTO, ExhibitionEditVM>();
            });
            var mapper = config.CreateMapper();
            var exhibitionEditVM = mapper.Map<NewsDTO, ExhibitionEditVM>(newsDTO);
            exhibitionEditVM.TempId = Guid.NewGuid().ToString("N");
            if (id == 0)
            {
                exhibitionEditVM.Start_date = DateTime.Today;
            }

            return View(exhibitionEditVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExhibitionEdit(ExhibitionEditVM exhibitionEditVM)
        {
            _logger.Info("修改");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ExhibitionEditVM, NewsDTO>()
                .ForMember(dest => dest.Picture, opt =>
                {
                    opt.MapFrom(src => src.PictureFile == null ? src.Picture : new FileInfo(src.PictureFile.FileName).Name);
                });
            });

            var mapper = config.CreateMapper();
            var newsDTO = mapper.Map<ExhibitionEditVM, NewsDTO>(exhibitionEditVM);
            newsDTO.Type = 1;
            newsDTO.Creator = User.Identity.Name;
            newsDTO.Create_date = DateTime.Now;
            newsDTO.Modifier = User.Identity.Name;
            newsDTO.Modified_date = DateTime.Now;

            if (!ModelState.IsValid || !_newsService.UpdateNews(newsDTO))
            {
                return View(exhibitionEditVM);
            }

            return RedirectToAction("Exhibition");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExhibitionDelete(int Id)
        {
            _logger.Info("刪除");

            if (!ModelState.IsValid || !_newsService.DeleteNews(new NewsDTO() { Id = Id }))
            {
                return RedirectToAction("Exhibition");
            }
            return RedirectToAction("Exhibition");
        }
        #endregion

        #region 產業新聞
        [AccessDeniedAuthorizeAttribute(Roles = "admin/news/industry")]
        [Authorize(Roles = "admin/news/industry")]
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

            var newsDTOs = _newsService.GetNewsByType(newsSearchDTO);

            config = new MapperConfiguration(cfg => cfg.CreateMap<NewsDTO, IndustryVM.Industry>());
            mapper = config.CreateMapper();
            industryVM.Industries = mapper.Map<List<NewsDTO>, List<IndustryVM.Industry>>(newsDTOs).ToPagedList(page, pageSize);

            return View(industryVM);
        }

        [HttpGet]
        public ActionResult IndustryEdit(int id = 0)
        {
            _logger.Info("瀏覽");

            var newsDTO = _newsService.GetNewsById(new NewsDTO() { Id = id });

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NewsFileDTO, IndustryEditVM.Attach>();
                cfg.CreateMap<NewsDTO, IndustryEditVM>();
            });
            var mapper = config.CreateMapper();
            var industryEditVM = mapper.Map<NewsDTO, IndustryEditVM>(newsDTO);
            industryEditVM.TempId = Guid.NewGuid().ToString("N");
            if (id == 0)
            {
                industryEditVM.Start_date = DateTime.Today;
            }

            return View(industryEditVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IndustryEdit(IndustryEditVM industryEditVM)
        {
            _logger.Info("修改");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<IndustryEditVM, NewsDTO>()
                .ForMember(dest => dest.Picture, opt =>
                {
                    opt.MapFrom(src => src.PictureFile == null ? src.Picture : new FileInfo(src.PictureFile.FileName).Name);
                });
            });

            var mapper = config.CreateMapper();
            var newsDTO = mapper.Map<IndustryEditVM, NewsDTO>(industryEditVM);
            newsDTO.Type = 2;
            newsDTO.Creator = User.Identity.Name;
            newsDTO.Create_date = DateTime.Now;
            newsDTO.Modifier = User.Identity.Name;
            newsDTO.Modified_date = DateTime.Now;

            if (!ModelState.IsValid || !_newsService.UpdateNews(newsDTO))
            {
                return View(industryEditVM);
            }

            return RedirectToAction("Industry");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IndustryDelete(int Id)
        {
            _logger.Info("刪除");

            if (!ModelState.IsValid || !_newsService.DeleteNews(new NewsDTO() { Id = Id }))
            {
                return RedirectToAction("Event");
            }
            return RedirectToAction("Event");
        }

        #endregion

        #region 新知上架
        [AccessDeniedAuthorizeAttribute(Roles = "admin/news/knowledge")]
        [Authorize(Roles = "admin/news/knowledge")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Knowledge(KnowledgeVM knowledgeVM, int page = 1)
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

            if (string.IsNullOrEmpty(knowledgeVM.NewSort))
            {
                knowledgeVM.NewSort = knowledgeVM.LastSort;
            }
            else if (knowledgeVM.NewSort == knowledgeVM.LastSort)
            {
                knowledgeVM.IsSortAsc = !knowledgeVM.IsSortAsc;
            }
            knowledgeVM.LastSort = knowledgeVM.NewSort;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<KnowledgeVM, NewsSearchDTO>()
                        .ForMember(dest => dest.SortKey, opt => opt.MapFrom(src => src.NewSort)));
            var mapper = config.CreateMapper();

            NewsSearchDTO newsSearchDTO = mapper.Map<NewsSearchDTO>(knowledgeVM);
            newsSearchDTO.Type = 3;

            var newsDTOs = _newsService.GetNewsByType(newsSearchDTO);

            config = new MapperConfiguration(cfg => cfg.CreateMap<NewsDTO, KnowledgeVM.Knowledge>());
            mapper = config.CreateMapper();
            knowledgeVM.Knowledges = mapper.Map<List<NewsDTO>, List<KnowledgeVM.Knowledge>>(newsDTOs).ToPagedList(page, pageSize);

            return View(knowledgeVM);
        }

        [HttpGet]
        public ActionResult KnowledgeEdit(int id = 0)
        {
            _logger.Info("瀏覽");

            var newsDTO = _newsService.GetNewsById(new NewsDTO() { Id = id });

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NewsFileDTO, KnowledgeEditVM.Attach>();
                cfg.CreateMap<NewsDTO, KnowledgeEditVM>();
            });
            var mapper = config.CreateMapper();
            var knowledgeEditVM = mapper.Map<NewsDTO, KnowledgeEditVM>(newsDTO);
            knowledgeEditVM.TempId = Guid.NewGuid().ToString("N");
            if (id == 0)
            {
                knowledgeEditVM.Start_date = DateTime.Today;
            }

            return View(knowledgeEditVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KnowledgeEdit(KnowledgeEditVM knowledgeEditVM)
        {
            _logger.Info("修改");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<KnowledgeEditVM, NewsDTO>()
                .ForMember(dest => dest.Picture, opt =>
                {
                    opt.MapFrom(src => src.PictureFile == null ? src.Picture : new FileInfo(src.PictureFile.FileName).Name);
                });
            });

            var mapper = config.CreateMapper();
            var newsDTO = mapper.Map<KnowledgeEditVM, NewsDTO>(knowledgeEditVM);
            newsDTO.Type = 3;
            newsDTO.Creator = User.Identity.Name;
            newsDTO.Create_date = DateTime.Now;
            newsDTO.Modifier = User.Identity.Name;
            newsDTO.Modified_date = DateTime.Now;

            if (!ModelState.IsValid || !_newsService.UpdateNews(newsDTO))
            {
                return View(knowledgeEditVM);
            }

            return RedirectToAction("Knowledge");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KnowledgeDelete(int Id)
        {
            _logger.Info("刪除");

            if (!ModelState.IsValid || !_newsService.DeleteNews(new NewsDTO() { Id = Id }))
            {
                return RedirectToAction("Knowledge");
            }
            return RedirectToAction("Knowledge");
        }

        #endregion

        #region 活動紀實
        [AccessDeniedAuthorizeAttribute(Roles = "admin/news/event")]
        [Authorize(Roles = "admin/news/event")]
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

            var newsDTOs = _newsService.GetNewsByType(newsSearchDTO);

            config = new MapperConfiguration(cfg => cfg.CreateMap<NewsDTO, EventVM.Event>());
            mapper = config.CreateMapper();
            eventVM.Events = mapper.Map<List<NewsDTO>, List<EventVM.Event>>(newsDTOs).ToPagedList(page, pageSize);

            return View(eventVM);
        }

        [HttpGet]
        public ActionResult EventEdit(int id = 0)
        {
            _logger.Info("瀏覽");

            var newsDTO = _newsService.GetNewsById(new NewsDTO() { Id = id });

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NewsFileDTO, EventEditVM.Attach>();
                cfg.CreateMap<NewsDTO, EventEditVM>();
            });
            var mapper = config.CreateMapper();
            var exhibitionEditVM = mapper.Map<NewsDTO, EventEditVM>(newsDTO);
            exhibitionEditVM.TempId = Guid.NewGuid().ToString("N");
            if (id == 0)
            {
                exhibitionEditVM.Start_date = DateTime.Today;
            }

            return View(exhibitionEditVM);
        }

        [HttpPost]
        
        public ActionResult EventEdit(EventEditVM eventEditVM)
        {
            _logger.Info("修改");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EventEditVM, NewsDTO>()
                .ForMember(dest => dest.Picture, opt =>
                {
                    opt.MapFrom(src => src.PictureFile == null ? null : new FileInfo(src.PictureFile.FileName).Name);
                });
            });

            var mapper = config.CreateMapper();
            var newsDTO = mapper.Map<EventEditVM, NewsDTO>(eventEditVM);
            newsDTO.Type = 4;
            newsDTO.Creator = User.Identity.Name;
            newsDTO.Create_date = DateTime.Now;
            newsDTO.Modifier = User.Identity.Name;
            newsDTO.Modified_date = DateTime.Now;

            if (!ModelState.IsValid || !_newsService.UpdateNews(newsDTO))
            {
                return View(eventEditVM);
            }

            return RedirectToAction("Event");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EventDelete(int Id)
        {
            _logger.Info("刪除");

            if (!ModelState.IsValid || !_newsService.DeleteNews(new NewsDTO() { Id = Id }))
            {
                return RedirectToAction("Event");
            }
            return RedirectToAction("Event");
        }

        #endregion

        #region 事件操作
        [AccessDeniedAuthorizeAttribute(Roles = "admin,adminUser")]
        [Authorize(Roles = "admin,adminUser")]
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

        [HttpPost]
        [ValidateAntiForgeryTokenHeader]
        public ActionResult UploadContentImage(string id)
        {
            _logger.Info("上傳");

            FileDTO fileDTO = new FileDTO { Id = id, File = Request.Files[0] };
            _newsService.UploadContentImage(fileDTO);

            string url = Url.Action("GetContentImage", "News", new { Area = "", id = id, fileName = new FileInfo(fileDTO.File.FileName).Name }, Request.Url.Scheme);
            return Json(new { location = url });
        }

        [HttpGet]
        public ActionResult DownloadPicture(string id, string fileName)
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

        [HttpPost]
        [ValidateAntiForgeryTokenHeader]
        public ActionResult DeletePicture(string id, string fileName)
        {
            _logger.Info("刪除");

            fileName = PathHelper.MakeFilenameValid(fileName);

            FileDTO fileDTO = new FileDTO { Id = id, Name = Path.GetFileName(fileName), Type = "Picture" };
            _newsService.DeletePicture(fileDTO);

            return Json(new { success = true, responseText = "刪除成功!" });
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

        [HttpPost]
        [ValidateAntiForgeryTokenHeader]
        public ActionResult DeleteFile(string id, string fileName)
        {
            _logger.Info("刪除");

            fileName = PathHelper.MakeFilenameValid(fileName);

            FileDTO fileDTO = new FileDTO { Id = id, Name = Path.GetFileName(fileName), Type = "Attach" };
            _newsService.DeleteNewsFile(fileDTO);

            return Json(new { success = true, responseText = "刪除成功!" });
        }

        #endregion
    }
}