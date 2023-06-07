using AutoMapper;
using cmt.Areas.Admin.ViewModels;
using cmt.DTOs;
using cmt.Extensions.CustomFilter;
using cmt.Extensions.ValidationError;
using cmt.Helper;
using cmt.Services;
using cmt.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace cmt.Areas.Admin.Controllers
{
    [AccessDeniedAuthorizeAttribute(Roles = "admin/knowledge,admin/knowledge/domestic,admin/knowledge/foreign")]
    [Authorize(Roles = "admin/knowledge,admin/knowledge/domestic,admin/knowledge/foreign")]
    public class KnowledgeController : BaseController
    {
        private IKnowledgeService _knowledgeService;
        public KnowledgeController(IAuthService authService, ILogService logService, INewsService newsService, IKnowledgeService knowledgeService, ICodeTableService codeTableService)
            : base(authService, logService, newsService, codeTableService)
        {
            _knowledgeService = knowledgeService;
            _knowledgeService.InitialiseIValidationDictionary(new ModelStateWrapper(this.ModelState));
        }
        [Authorize(Roles = "admin/knowledge/domestic")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Domestic(DomesticVM domesticVM, int page = 1)
        {
            _logger.Info("瀏覽");

            if (Request.HttpMethod == WebRequestMethods.Http.Post)
            {
                System.Web.Helpers.AntiForgery.Validate();
            }

            int pageSize = 10;

            if (string.IsNullOrEmpty(domesticVM.NewSort))
            {
                domesticVM.NewSort = domesticVM.LastSort;
            }
            else if (domesticVM.NewSort == domesticVM.LastSort)
            {
                domesticVM.IsSortAsc = !domesticVM.IsSortAsc;
            }
            domesticVM.LastSort = domesticVM.NewSort;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<DomesticVM, KnowledgeSearchDTO>()
                        .ForMember(dest => dest.SortKey, opt => opt.MapFrom(src => src.NewSort)));
            var mapper = config.CreateMapper();

            KnowledgeSearchDTO knowledgeSearchDTO = mapper.Map<KnowledgeSearchDTO>(domesticVM);
            knowledgeSearchDTO.Type = 1;

            var knowledgeDTOs = _knowledgeService.GetKnowledgeByType(knowledgeSearchDTO);

            config = new MapperConfiguration(cfg => cfg.CreateMap<KnowledgeDTO, DomesticVM.Domestic>());
            mapper = config.CreateMapper();
            domesticVM.Domestics = mapper.Map<List<KnowledgeDTO>, List<DomesticVM.Domestic>>(knowledgeDTOs).ToPagedList(page, pageSize);

            return View(domesticVM);
        }

        [HttpGet]
        public ActionResult DomesticEdit(int id = 0)
        {
            _logger.Info("瀏覽");

            var knowledgeDTO = _knowledgeService.GetKnowledgeById(new KnowledgeDTO() { Id = id });

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<KnowledgeFileDTO, DomesticEditVM.Attach>();
                cfg.CreateMap<KnowledgeDTO, DomesticEditVM>();
            });
            var mapper = config.CreateMapper();
            var domesticEditVM = mapper.Map<KnowledgeDTO, DomesticEditVM>(knowledgeDTO);
            domesticEditVM.TempId = Guid.NewGuid().ToString("N");
            if (id == 0)
            {
                domesticEditVM.Start_date = DateTime.Today;
            }

            return View(domesticEditVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DomesticEdit(DomesticEditVM domesticEditVM)
        {
            _logger.Info("修改");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DomesticEditVM, KnowledgeDTO>()
                .ForMember(dest => dest.Picture, opt =>
                {
                    opt.MapFrom(src => src.PictureFile == null ? src.Picture : new FileInfo(src.PictureFile.FileName).Name);
                });
            });
            
            var mapper = config.CreateMapper();
            var knowledgeDTO = mapper.Map<DomesticEditVM, KnowledgeDTO>(domesticEditVM);
            knowledgeDTO.Type = 1;
            knowledgeDTO.Creator = User.Identity.Name;
            knowledgeDTO.Create_date = DateTime.Now;
            knowledgeDTO.Modifier = User.Identity.Name;
            knowledgeDTO.Modified_date = DateTime.Now;

            if (!ModelState.IsValid || !_knowledgeService.UpdateKnowledge(knowledgeDTO))
            {
                return View(domesticEditVM);
            }

            return RedirectToAction("Domestic");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DomesticDelete(int Id)
        {
            _logger.Info("瀏覽");

            if (!ModelState.IsValid || !_knowledgeService.DeleteKnowledge(new KnowledgeDTO() { Id = Id }))
            {
                return RedirectToAction("Domestic");
            }
            return RedirectToAction("Domestic");
        }

        [Authorize(Roles = "admin/knowledge/foreign")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Foreign(ForeignVM foreignVM, int page = 1)
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

            if (string.IsNullOrEmpty(foreignVM.NewSort))
            {
                foreignVM.NewSort = foreignVM.LastSort;
            }
            else if (foreignVM.NewSort == foreignVM.LastSort)
            {
                foreignVM.IsSortAsc = !foreignVM.IsSortAsc;
            }
            foreignVM.LastSort = foreignVM.NewSort;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<ForeignVM, KnowledgeSearchDTO>()
                        .ForMember(dest => dest.SortKey, opt => opt.MapFrom(src => src.NewSort)));
            var mapper = config.CreateMapper();

            KnowledgeSearchDTO knowledgeSearchDTO = mapper.Map<KnowledgeSearchDTO>(foreignVM);
            knowledgeSearchDTO.Type = 2;

            var knowledgeDTOs = _knowledgeService.GetKnowledgeByType(knowledgeSearchDTO);

            config = new MapperConfiguration(cfg => cfg.CreateMap<KnowledgeDTO, ForeignVM.Foreign>());
            mapper = config.CreateMapper();
            foreignVM.Foreigns = mapper.Map<List<KnowledgeDTO>, List<ForeignVM.Foreign>>(knowledgeDTOs).ToPagedList(page, pageSize);

            return View(foreignVM);
        }

        [HttpGet]
        public ActionResult ForeignEdit(int id = 0)
        {
            _logger.Info("瀏覽");

            var knowledgeDTO = _knowledgeService.GetKnowledgeById(new KnowledgeDTO() { Id = id });

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<KnowledgeFileDTO, ForeignEditVM.Attach>();
                cfg.CreateMap<KnowledgeDTO, ForeignEditVM>();
            });
            var mapper = config.CreateMapper();
            var foreignEditVM = mapper.Map<KnowledgeDTO, ForeignEditVM>(knowledgeDTO);
            foreignEditVM.TempId = Guid.NewGuid().ToString("N");
            if (id == 0)
            {
                foreignEditVM.Start_date = DateTime.Today;
            }

            return View(foreignEditVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForeignEdit(ForeignEditVM foreignEditVM)
        {
            _logger.Info("修改");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ForeignEditVM, KnowledgeDTO>()
                .ForMember(dest => dest.Picture, opt =>
                {
                    opt.MapFrom(src => src.PictureFile == null ? src.Picture : new FileInfo(src.PictureFile.FileName).Name);
                });
            });

            var mapper = config.CreateMapper();
            var knowledgeDTO = mapper.Map<ForeignEditVM, KnowledgeDTO>(foreignEditVM);
            knowledgeDTO.Type = 2;
            knowledgeDTO.Creator = User.Identity.Name;
            knowledgeDTO.Create_date = DateTime.Now;
            knowledgeDTO.Modifier = User.Identity.Name;
            knowledgeDTO.Modified_date = DateTime.Now;

            if (!ModelState.IsValid || !_knowledgeService.UpdateKnowledge(knowledgeDTO))
            {
                return View(foreignEditVM);
            }

            return RedirectToAction("Foreign");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForeignDelete(int Id)
        {
            _logger.Info("刪除");

            if (!ModelState.IsValid || !_knowledgeService.DeleteKnowledge(new KnowledgeDTO() { Id = Id }))
            {
                return RedirectToAction("Foreign");
            }
            return RedirectToAction("Foreign");
        }

        [HttpGet]
        public ActionResult GetContentImage(string id, string fileName)
        {
            _logger.Info("下載");

            fileName = PathHelper.MakeFilenameValid(fileName);

            FileDTO fileDTO = new FileDTO { Id = id, Name = Path.GetFileName(fileName), Type = "ContentImage" };
            fileDTO = _knowledgeService.GetFile(fileDTO);

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
            _knowledgeService.UploadContentImage(fileDTO);

            string url = Url.Action("GetContentImage", "Knowledge", new { Area = "", id = id, fileName = new FileInfo(fileDTO.File.FileName).Name }, Request.Url.Scheme);
            return Json(new { location = url });
        }

        [HttpGet]
        public ActionResult DownloadPicture(string id, string fileName)
        {
            _logger.Info("下載");

            fileName = PathHelper.MakeFilenameValid(fileName);

            FileDTO fileDTO = new FileDTO { Id = id, Name = Path.GetFileName(fileName), Type = "Picture" };
            fileDTO = _knowledgeService.GetFile(fileDTO);

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
            _knowledgeService.DeletePicture(fileDTO);

            return Json(new { success = true, responseText = "刪除成功!" });
        }

        [HttpGet]
        public ActionResult DownloadFile(string id, string fileName)
        {
            _logger.Info("下載");

            fileName = PathHelper.MakeFilenameValid(fileName);

            FileDTO fileDTO = new FileDTO { Id = id, Name = Path.GetFileName(fileName), Type = "Attach" };
            fileDTO = _knowledgeService.GetFile(fileDTO);

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
            _knowledgeService.DeleteKnowledgeFile(fileDTO);

            return Json(new { success = true, responseText = "刪除成功!" });
        }
    }
}