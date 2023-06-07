using AutoMapper;
using cmt.DTOs;
using cmt.Helper;
using cmt.Services.Interfaces;
using cmt.ViewModels;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace cmt.Controllers
{
    public class KnowledgeController : BaseController
    {
        private IKnowledgeService _knowledgeService;
        public KnowledgeController(IAuthService authService, ILogService logService, INewsService newsService, IKnowledgeService knowledgeService,ICodeTableService codeTableService)
            : base(authService, logService, newsService, codeTableService)
        {
            _knowledgeService = knowledgeService;
        }

        //[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //public ActionResult Domestic(DomesticVM domesticVM, int page = 1)
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

        //    if (string.IsNullOrEmpty(domesticVM.NewSort))
        //    {
        //        domesticVM.NewSort = domesticVM.LastSort;
        //    }
        //    else if (domesticVM.NewSort == domesticVM.LastSort)
        //    {
        //        domesticVM.IsSortAsc = !domesticVM.IsSortAsc;
        //    }
        //    domesticVM.LastSort = domesticVM.NewSort;

        //    var config = new MapperConfiguration(cfg => cfg.CreateMap<DomesticVM, KnowledgeSearchDTO>()
        //                .ForMember(dest => dest.SortKey, opt => opt.MapFrom(src => src.NewSort)));
        //    var mapper = config.CreateMapper();

        //    KnowledgeSearchDTO knowledgeSearchDTO = mapper.Map<KnowledgeSearchDTO>(domesticVM);
        //    knowledgeSearchDTO.Type = 1;

        //    var knowledgeDTOs = _knowledgeService.GetPublicKnowledgeByType(knowledgeSearchDTO);

        //    config = new MapperConfiguration(cfg => cfg.CreateMap<KnowledgeDTO, DomesticVM.Domestic>());
        //    mapper = config.CreateMapper();
        //    domesticVM.Domestics = mapper.Map<List<KnowledgeDTO>, List<DomesticVM.Domestic>>(knowledgeDTOs).ToPagedList(page, pageSize);

        //    return View(domesticVM);
        //}


        //[HttpGet]
        //public ActionResult DomesticDetail(int id = 0)
        //{
        //    _logger.Info("瀏覽");

        //    var knowledgeDTO = _knowledgeService.GetPublicKnowledgeById(new KnowledgeDTO() { Id = id });

        //    var config = new MapperConfiguration(cfg => {
        //        cfg.CreateMap<KnowledgeFileDTO, DomesticDetailVM.Attach>();
        //        cfg.CreateMap<KnowledgeDTO, DomesticDetailVM>();
        //    });
        //    var mapper = config.CreateMapper();
        //    var domesticDetailVM = mapper.Map<KnowledgeDTO, DomesticDetailVM>(knowledgeDTO);
        //    domesticDetailVM.TempId = Guid.NewGuid().ToString("N");

        //    return View(domesticDetailVM);
        //}

        //[AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        //public ActionResult Foreign(ForeignVM foreignVM, int page = 1)
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

        //    if (string.IsNullOrEmpty(foreignVM.NewSort))
        //    {
        //        foreignVM.NewSort = foreignVM.LastSort;
        //    }
        //    else if (foreignVM.NewSort == foreignVM.LastSort)
        //    {
        //        foreignVM.IsSortAsc = !foreignVM.IsSortAsc;
        //    }
        //    foreignVM.LastSort = foreignVM.NewSort;

        //    var config = new MapperConfiguration(cfg => cfg.CreateMap<ForeignVM, KnowledgeSearchDTO>()
        //                .ForMember(dest => dest.SortKey, opt => opt.MapFrom(src => src.NewSort)));
        //    var mapper = config.CreateMapper();

        //    KnowledgeSearchDTO knowledgeSearchDTO = mapper.Map<KnowledgeSearchDTO>(foreignVM);
        //    knowledgeSearchDTO.Type = 2;

        //    var knowledgeDTOs = _knowledgeService.GetPublicKnowledgeByType(knowledgeSearchDTO);

        //    config = new MapperConfiguration(cfg => cfg.CreateMap<KnowledgeDTO, ForeignVM.Foreign>());
        //    mapper = config.CreateMapper();
        //    foreignVM.Foreigns = mapper.Map<List<KnowledgeDTO>, List<ForeignVM.Foreign>>(knowledgeDTOs).ToPagedList(page, pageSize);

        //    return View(foreignVM);
        //}

        //[HttpGet]
        //public ActionResult ForeignDetail(int id = 0)
        //{
        //    _logger.Info("瀏覽");

        //    var knowledgeDTO = _knowledgeService.GetPublicKnowledgeById(new KnowledgeDTO() { Id = id });

        //    var config = new MapperConfiguration(cfg => {
        //        cfg.CreateMap<KnowledgeFileDTO, ForeignDetailVM.Attach>();
        //        cfg.CreateMap<KnowledgeDTO, ForeignDetailVM>();
        //    });
        //    var mapper = config.CreateMapper();
        //    var foreignDetailVM = mapper.Map<KnowledgeDTO, ForeignDetailVM>(knowledgeDTO);
        //    foreignDetailVM.TempId = Guid.NewGuid().ToString("N");

        //    return View(foreignDetailVM);
        //}

        //[HttpGet]
        //public ActionResult GetContentImage(string id, string fileName)
        //{
        //    _logger.Info("下載");

        //    fileName = PathHelper.MakeFilenameValid(fileName);

        //    FileDTO fileDTO = new FileDTO { Id = id, Name = Path.GetFileName(fileName), Type = "ContentImage" };
        //    fileDTO = _knowledgeService.GetFile(fileDTO);

        //    if (fileDTO != null)
        //    {
        //        return File(Path.Combine(fileDTO.Path, fileDTO.Name), fileDTO.ContentType, fileDTO.Name);
        //    }
        //    return null;
        //}

        //[HttpGet]
        //public ActionResult DownloadFile(string id, string fileName)
        //{
        //    _logger.Info("下載");

        //    fileName = PathHelper.MakeFilenameValid(fileName);

        //    FileDTO fileDTO = new FileDTO { Id = id, Name = Path.GetFileName(fileName), Type = "Attach" };
        //    fileDTO = _knowledgeService.GetFile(fileDTO);

        //    if (fileDTO != null)
        //    {
        //        return File(Path.Combine(fileDTO.Path, fileDTO.Name), fileDTO.ContentType, fileDTO.Name);
        //    }
        //    return null;
        //}
    }
}