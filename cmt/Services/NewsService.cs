using AutoMapper;
using cmt.DTOs;
using cmt.Models;
using cmt.Repositories.Interfaces;
using cmt.Services.Interfaces;
using cmt.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace cmt.Services
{
    public class NewsService : GenericService, INewsService
    {
        private readonly ICmtUow _cmtUow;
        private readonly INewsRepo _newsRepo;
        private readonly INewsFileRepo _newsFileRepo;
        private readonly ICodeTableRepo _codeTableRepo;


        public NewsService(ICmtUow cmtUow, INewsRepo newsRepo, INewsFileRepo newsFileRepo, ICodeTableRepo codeTableRepo)
        {
            _cmtUow = cmtUow;
            _newsRepo = newsRepo;
            _newsFileRepo = newsFileRepo;
            _codeTableRepo = codeTableRepo;
        }
        public List<NewsDTO> GetNewsByCodeTableType(string CT_type)
        {
            var codeTable = _codeTableRepo.GetAll().Where(x => x.CT_type == CT_type);

            var news = _newsRepo.GetAll().Where(x => codeTable.Where(a=>a.CT_value == x.n_parent_type).Count()>0 && x.n_is_public
           )
                                .ToList();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.RecognizePrefixes("n_");
                cfg.CreateMap<News, NewsDTO>();
            });
            var mapper = config.CreateMapper();
            var model = mapper.Map<List<News>, List<NewsDTO>>(news);

            return model.Where(x =>  Common.DateTimeCompareByToday(x.Start_date) && Common.DateTimeCompareByEndTime(x.End_date)).ToList();
        }

        

        public List<NewsDTO> GetNewsByType(NewsSearchDTO newsSearchDTO)
        {
            if (String.IsNullOrEmpty(newsSearchDTO.SortKey))
            {
                newsSearchDTO.SortKey = "n_create_date";
                newsSearchDTO.IsSortAsc = false;
            }
            else
            {
                newsSearchDTO.SortKey = "n_" + newsSearchDTO.SortKey.ToLower();
            }

            if (!String.IsNullOrEmpty(newsSearchDTO.Keywords))
            {
                newsSearchDTO.Keywords = newsSearchDTO.Keywords.ToUpper();
            }

            var news = _newsRepo.GetAll().Where(x => x.n_type == newsSearchDTO.Type)
                                .OrderBy(newsSearchDTO.SortKey + (newsSearchDTO.IsSortAsc ? "" : " descending"))
                                .ToList();

            var config = new MapperConfiguration(cfg =>
                {
                    cfg.RecognizePrefixes("n_");
                    cfg.CreateMap<News, NewsDTO>();
                });
            var mapper = config.CreateMapper();
            return mapper.Map<List<News>, List<NewsDTO>>(news);
        }

        public List<NewsDTO> GetPublicNewsByType(NewsSearchDTO newsSearchDTO)
        {
            if (String.IsNullOrEmpty(newsSearchDTO.SortKey))
            {
                newsSearchDTO.SortKey = "n_start_date";
                newsSearchDTO.IsSortAsc = false;
            }
            else
            {
                newsSearchDTO.SortKey = "n_" + newsSearchDTO.SortKey.ToLower();
            }

            if (!String.IsNullOrEmpty(newsSearchDTO.Keywords))
            {
                newsSearchDTO.Keywords = newsSearchDTO.Keywords.ToUpper();
            }

            var today = DateTime.Now.Date;
            var news = _newsRepo.GetAll()
                .Where
                (
                    x => x.n_type == newsSearchDTO.Type &&
                    x.n_is_public &&
                    (x.n_start_date == null || today >= DbFunctions.TruncateTime(x.n_start_date)) &&
                    (x.n_end_date == null || today <= DbFunctions.TruncateTime(x.n_end_date))
                )
                .OrderBy(newsSearchDTO.SortKey + (newsSearchDTO.IsSortAsc ? "" : " descending"))
                .ToList();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.RecognizePrefixes("n_");
                cfg.CreateMap<News, NewsDTO>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<List<News>, List<NewsDTO>>(news);
        }

        public List<NewsDTO> GetPublicTopNewsByType(NewsSearchDTO newsSearchDTO, int quantity)
        {
            if (String.IsNullOrEmpty(newsSearchDTO.SortKey))
            {
                newsSearchDTO.SortKey = "n_start_date";
                newsSearchDTO.IsSortAsc = false;
            }
            else
            {
                newsSearchDTO.SortKey = "n_" + newsSearchDTO.SortKey.ToLower();
            }

            if (!String.IsNullOrEmpty(newsSearchDTO.Keywords))
            {
                newsSearchDTO.Keywords = newsSearchDTO.Keywords.ToUpper();
            }

            var today = DateTime.Now.Date;
            var news = _newsRepo.GetAll()
                .Where
                (
                    x => (newsSearchDTO.Type == 0 || x.n_type == newsSearchDTO.Type) &&
                    x.n_is_public &&
                    (x.n_start_date == null || today >= DbFunctions.TruncateTime(x.n_start_date)) &&
                    (x.n_end_date == null || today <= DbFunctions.TruncateTime(x.n_end_date))
                )
                .OrderBy(newsSearchDTO.SortKey + (newsSearchDTO.IsSortAsc ? "" : " descending")).Take(quantity)
                .ToList();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.RecognizePrefixes("n_");
                cfg.CreateMap<News, NewsDTO>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<List<News>, List<NewsDTO>>(news);
        }

        public NewsDTO GetNewsById(NewsDTO newsDTO)
        {
            var news = _newsRepo.Get(x => x.n_id == newsDTO.Id);

            var config = new MapperConfiguration(cfg =>
                {
                    cfg.RecognizePrefixes("n_");
                    cfg.CreateMap<News, NewsDTO>();
                });
            var mapper = config.CreateMapper();
            mapper.Map(news, newsDTO);

            var newsFiles = _newsFileRepo.GetAll().Where(x => x.nf_n_id == newsDTO.Id).ToList();
            config = new MapperConfiguration(cfg =>
            {
                cfg.RecognizePrefixes("nf_");
                cfg.CreateMap<NewsFile, NewsFileDTO>();
            });
            mapper = config.CreateMapper();
            newsDTO.Attaches = mapper.Map<List<NewsFile>, List<NewsFileDTO>>(newsFiles);
            return newsDTO;
        }

        public NewsDTO GetPublicNewsById(NewsDTO newsDTO)
        {
            var today = DateTime.Now.Date;
            var news = _newsRepo.Get
                (
                    x => x.n_id == newsDTO.Id &&
                    x.n_is_public &&
                    (x.n_start_date == null || today >= DbFunctions.TruncateTime(x.n_start_date)) &&
                    (x.n_end_date == null || today <= DbFunctions.TruncateTime(x.n_end_date))
                );

            var config = new MapperConfiguration(cfg =>
            {
                cfg.RecognizePrefixes("n_");
                cfg.CreateMap<News, NewsDTO>();
            });
            var mapper = config.CreateMapper();
            mapper.Map(news, newsDTO);

            var newsFiles = _newsFileRepo.GetAll().Where(x => x.nf_n_id == newsDTO.Id).ToList();
            config = new MapperConfiguration(cfg =>
            {
                cfg.RecognizePrefixes("nf_");
                cfg.CreateMap<NewsFile, NewsFileDTO>();
            });
            mapper = config.CreateMapper();
            newsDTO.Attaches = mapper.Map<List<NewsFile>, List<NewsFileDTO>>(newsFiles);
            return newsDTO;
        }

        public void ValidateUpdateNews(NewsDTO newsDTO)
        {
            string[] validPicture = { ".tiff", ".tif", ".gif", ".bmp", ".png", ".jpeg", ".jpg" };
            string[] validAttach = { ".tiff", ".tif", ".gif", ".bmp", ".png", ".jpeg", ".jpg",
                                     ".doc", ".docx", ".xls", ".xlsx", ".xlsm", ".ppt", ".pptx",
                                     ".odt", ".fodt", ".ods", ".fods", ".odp", ".fodp", ".odg",".fodg",".odf",
                                     ".txt", ".pdf", ".zip", ".rar", ".7z", ".csv", ".json", ".xml"};

            if (newsDTO.PictureFile != null && newsDTO.PictureFile.ContentLength > 0)
            {
                if (!validPicture.Contains(Path.GetExtension(newsDTO.PictureFile.FileName).ToLower()))
                {
                    ValidationDictionary.AddError("PictureFile", $"檔案類型錯誤。");
                }
            }

            foreach(var attachFile in newsDTO.AttachFiles)
            {
                if(attachFile != null && attachFile.ContentLength > 0)
                {
                    if (!validAttach.Contains(Path.GetExtension(attachFile.FileName).ToLower()))
                    {
                        ValidationDictionary.AddError("AttachFiles", $"檔案類型錯誤。");
                    }
                }
            }
        }

        public bool UpdateNews(NewsDTO newsDTO)
        {
            ValidateUpdateNews(newsDTO);

            if (ValidationDictionary.IsValid)
            {
                News news = null;

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.RecognizeDestinationPrefixes("n_");
                    cfg.CreateMap<NewsDTO, News>()
                    .ForMember(dest => dest.n_creator, opt => opt.Condition(src => src.Id == 0))
                    .ForMember(dest => dest.n_create_date, opt => opt.Condition(src => src.Id == 0))
                    .ForMember(dest => dest.n_picture, opt => opt.Condition(src => src.PictureFile != null));
                });
                var mapper = config.CreateMapper();


                if (newsDTO.Id == 0)
                {
                    news = new News();
                    mapper.Map(newsDTO, news);
                    _newsRepo.Add(news);
                    SaveChanges(_cmtUow);

                    SaveFile(newsDTO.TempId, "Picture", newsDTO.Picture, newsDTO.PictureFile);

                    //更新圖片路徑，將TempID替換為正式ID
                    news.n_content = news.n_content?.Replace(newsDTO.TempId, news.n_id.ToString());

                    newsDTO.Id = news.n_id;
                    MoveTempNewsFile(newsDTO);
                }
                else
                {
                    news = _newsRepo.Get(x => x.n_id == newsDTO.Id);
                    mapper.Map(newsDTO, news);
                    SaveFile(newsDTO.Id.ToString(), "Picture", newsDTO.Picture, newsDTO.PictureFile);
                }
                UpdateNewsFile(newsDTO);
                SaveChanges(_cmtUow);
            }
            return ValidationDictionary.IsValid;
        }

        public bool DeleteNews(NewsDTO newsDTO)
        {
            if (ValidationDictionary.IsValid)
            {
                var news = _newsRepo.Get(x => x.n_id == newsDTO.Id);
                _newsRepo.Delete(news);

                var newsFiles = _newsFileRepo.GetAll().Where(x => x.nf_n_id == newsDTO.Id);
                foreach (var newsFile in newsFiles)
                {
                    _newsFileRepo.Delete(newsFile);
                }

                SaveChanges(_cmtUow);

                DeleteDirectory(newsDTO.Id.ToString());
            }
            return ValidationDictionary.IsValid;
        }

        public void UpdateNewsFile(NewsDTO newsDTO)
        {
            if (newsDTO.AttachFiles.Any())
            {
                foreach (var attach in newsDTO.AttachFiles)
                {
                    if (attach != null && attach.ContentLength > 0)
                    {
                        _newsFileRepo.Add(new NewsFile()
                        {
                            nf_name = new FileInfo(attach.FileName).Name,
                            nf_n_id = newsDTO.Id,
                            nf_creator = newsDTO.Creator,
                            nf_create_date = newsDTO.Create_date,
                            nf_modifier = newsDTO.Modifier,
                            nf_modified_date = newsDTO.Modified_date
                        });
                        SaveFile(newsDTO.Id.ToString(), "Attach", attach.FileName, attach);
                    }
                }
            }
        }

        public void DeletePicture(FileDTO fileDTO)
        {
            var news = _newsRepo.Get(x => x.n_id.ToString() == fileDTO.Id);
            if (news != null)
            {
                news.n_picture = null;
                SaveChanges(_cmtUow);
            }

            DeleteFile(fileDTO.Id, "Picture", fileDTO.Name);
        }

        public void DeleteNewsFile(FileDTO fileDTO)
        {
            var newsFile = _newsFileRepo.Get(x => x.nf_id.ToString() == fileDTO.Id);
            if (newsFile != null)
            {
                _newsFileRepo.Delete(newsFile);
                SaveChanges(_cmtUow);
            }

            DeleteFile(fileDTO.Id, "Attach", fileDTO.Name);
        }

        public void MoveTempNewsFile(NewsDTO newsDTO)
        {
            string source = $"{_uploadFileDir}\\Temp\\News\\{newsDTO.TempId}\\";
            string target = $"{_uploadFileDir}\\News\\{newsDTO.Id}\\";

            if (Directory.Exists(source))
            {
                DirectoryInfo di = new DirectoryInfo(target);
                if (!di.Parent.Exists)
                {
                    Directory.CreateDirectory(di.Parent.FullName);
                }
                Directory.Move(source, target);
            }
        }

        public FileDTO GetFile(FileDTO fileDTO)
        {
            // && System.IO.Directory.Exists(fileDTO.Path)
            if (!String.IsNullOrEmpty(fileDTO.Id) && !String.IsNullOrEmpty(fileDTO.Name))
            {
                if (fileDTO.Type == "ContentImage" || fileDTO.Type == "Picture")
                {
                    fileDTO.ContentType = MimeMapping.GetMimeMapping(fileDTO.Name);
                }
                else 
                {
                    fileDTO.ContentType = "application/force-download";
                }

                int i;
                //是否為暫存檔案
                if (int.TryParse(fileDTO.Id, out i))
                {
                    fileDTO.Path = $"{_uploadFileDir}\\News\\{fileDTO.Id}\\{fileDTO.Type}\\";
                }
                else
                {
                    fileDTO.Path = $"{_uploadFileDir}\\Temp\\News\\{fileDTO.Id}\\{fileDTO.Type}\\";
                }

                if (Directory.EnumerateFiles(fileDTO.Path, fileDTO.Name).Any())
                {
                    return fileDTO;
                }

            }
            return null;
        }

        public void UploadContentImage(FileDTO fileDTO)
        {
            HttpPostedFileBase uploadFile = fileDTO.File;

            SaveFile(fileDTO.Id, "ContentImage", Path.Combine(Guid.NewGuid().ToString("N"), new FileInfo(uploadFile?.FileName).Name), fileDTO.File);
        }

        public void SaveFile(string id, string type, string name, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var path = string.Empty;
                var fullPath = string.Empty;

                int i;
                if (int.TryParse(id, out i))
                {
                    path = $"{_uploadFileDir}\\News\\{id}\\{type}\\";
                }
                else
                {
                    path = $"{_uploadFileDir}\\Temp\\News\\{id}\\{type}\\";
                }

                fullPath = Path.Combine(path, new FileInfo(name).Name);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                file.SaveAs(fullPath);
            }
        }

        public void DeleteFile(string id, string type, string name)
        {
            if (name != null)
            {
                var path = string.Empty;
                var fullPath = string.Empty;

                int i;
                if (int.TryParse(id, out i))
                {
                    path = $"{_uploadFileDir}\\News\\{id}\\{type}\\";
                }
                else
                {
                    path = $"{_uploadFileDir}\\Temp\\News\\{id}\\{type}\\";
                }

                fullPath = Path.Combine(path, name);

                if (Directory.EnumerateFiles(path, name).Any())
                {
                    File.Delete(fullPath);
                }
            }
        }

        public void DeleteDirectory(string id)
        {
            var path = string.Empty;

            int i;
            if (int.TryParse(id, out i))
            {
                path = $"{_uploadFileDir}\\News\\{id}\\";
            }
            else
            {
                path = $"{_uploadFileDir}\\Temp\\News\\{id}\\";
            }

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public DateTime? GetLastModifiedDate()
        {

            return _newsRepo.GetAll().Where(x => x.n_is_public == true).OrderByDescending(x => x.n_start_date).FirstOrDefault()?.n_start_date;
        }
    }
}