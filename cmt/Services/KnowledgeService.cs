using AutoMapper;
using cmt.DTOs;
using cmt.Extensions.ValidationError.Interface;
using cmt.Repositories.Interfaces;
using cmt.Services.Interfaces;
using cmt.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using cmt.Models;
using System.IO;
using System.Data.Entity;

namespace cmt.Services
{
    public class KnowledgeService : GenericService, IKnowledgeService
    {
        private readonly ICmtUow _cmtUow;
        private readonly IKnowledgeRepo _knowledgeRepo;
        private readonly IKnowledgeFileRepo _knowledgeFileRepo;

        public KnowledgeService(ICmtUow cmtUow, IKnowledgeRepo knowledgeRepo, IKnowledgeFileRepo knowledgeFileRepo)
        {
            _cmtUow = cmtUow;
            _knowledgeRepo = knowledgeRepo;
            _knowledgeFileRepo = knowledgeFileRepo;
        }

        public List<KnowledgeDTO> GetKnowledgeByType(KnowledgeSearchDTO knowledgeSearchDTO)
        {
            if (String.IsNullOrEmpty(knowledgeSearchDTO.SortKey))
            {
                knowledgeSearchDTO.SortKey = "k_create_date";
                knowledgeSearchDTO.IsSortAsc = false;
            }
            else
            {
                knowledgeSearchDTO.SortKey = "k_" + knowledgeSearchDTO.SortKey.ToLower();
            }

            if (!String.IsNullOrEmpty(knowledgeSearchDTO.Keywords))
            {
                knowledgeSearchDTO.Keywords = knowledgeSearchDTO.Keywords.ToUpper();
            }

            var knowledge = _knowledgeRepo.GetAll().Where(x => x.k_type == knowledgeSearchDTO.Type)
                                .OrderBy(knowledgeSearchDTO.SortKey + (knowledgeSearchDTO.IsSortAsc ? "" : " descending"))
                                .ToList();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.RecognizePrefixes("k_");
                cfg.CreateMap<Knowledge, KnowledgeDTO>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<List<Knowledge>, List<KnowledgeDTO>>(knowledge);
        }

        public List<KnowledgeDTO> GetPublicKnowledgeByType(KnowledgeSearchDTO knowledgeSearchDTO)
        {
            if (String.IsNullOrEmpty(knowledgeSearchDTO.SortKey))
            {
                knowledgeSearchDTO.SortKey = "k_start_date";
                knowledgeSearchDTO.IsSortAsc = false;
            }
            else
            {
                knowledgeSearchDTO.SortKey = "k_" + knowledgeSearchDTO.SortKey.ToLower();
            }

            if (!String.IsNullOrEmpty(knowledgeSearchDTO.Keywords))
            {
                knowledgeSearchDTO.Keywords = knowledgeSearchDTO.Keywords.ToUpper();
            }

            var today = DateTime.Now.Date;
            var knowledge = _knowledgeRepo.GetAll()
                .Where
                (
                    x => x.k_type == knowledgeSearchDTO.Type &&
                    x.k_is_public &&
                    (x.k_start_date == null || today >= DbFunctions.TruncateTime(x.k_start_date)) &&
                    (x.k_end_date == null || today <= DbFunctions.TruncateTime(x.k_end_date))
                )
                .OrderBy(knowledgeSearchDTO.SortKey + (knowledgeSearchDTO.IsSortAsc ? "" : " descending"))
                .ToList();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.RecognizePrefixes("k_");
                cfg.CreateMap<Knowledge, KnowledgeDTO>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<List<Knowledge>, List<KnowledgeDTO>>(knowledge);
        }

        public KnowledgeDTO GetKnowledgeById(KnowledgeDTO knowledgeDTO)
        {
            var knowledge = _knowledgeRepo.Get(x => x.k_id == knowledgeDTO.Id);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.RecognizePrefixes("k_");
                cfg.CreateMap<Knowledge, KnowledgeDTO>();
            });
            var mapper = config.CreateMapper();
            mapper.Map(knowledge, knowledgeDTO);

            var knowledgeFiles = _knowledgeFileRepo.GetAll().Where(x => x.kf_k_id == knowledgeDTO.Id).ToList();
            config = new MapperConfiguration(cfg =>
            {
                cfg.RecognizePrefixes("kf_");
                cfg.CreateMap<KnowledgeFile, KnowledgeFileDTO>();
            });
            mapper = config.CreateMapper();
            knowledgeDTO.Attaches = mapper.Map<List<KnowledgeFile>, List<KnowledgeFileDTO>>(knowledgeFiles);
            return knowledgeDTO;
        }

        public KnowledgeDTO GetPublicKnowledgeById(KnowledgeDTO knowledgeDTO)
        {
            var today = DateTime.Now.Date;
            var knowledge = _knowledgeRepo.Get
                (
                    x => x.k_id == knowledgeDTO.Id &&
                    x.k_is_public &&
                    (x.k_start_date == null || today >= DbFunctions.TruncateTime(x.k_start_date)) &&
                    (x.k_end_date == null || today <= DbFunctions.TruncateTime(x.k_end_date))
                );

            var config = new MapperConfiguration(cfg =>
            {
                cfg.RecognizePrefixes("k_");
                cfg.CreateMap<Knowledge, KnowledgeDTO>();
            });
            var mapper = config.CreateMapper();
            mapper.Map(knowledge, knowledgeDTO);

            var knowledgeFiles = _knowledgeFileRepo.GetAll().Where(x => x.kf_k_id == knowledgeDTO.Id).ToList();
            config = new MapperConfiguration(cfg =>
            {
                cfg.RecognizePrefixes("kf_");
                cfg.CreateMap<KnowledgeFile, KnowledgeFileDTO>();
            });
            mapper = config.CreateMapper();
            knowledgeDTO.Attaches = mapper.Map<List<KnowledgeFile>, List<KnowledgeFileDTO>>(knowledgeFiles);
            return knowledgeDTO;
        }

        public void ValidateUpdateKnowledge(KnowledgeDTO knowledgeDTO)
        {
            string[] validPicture = { ".tiff", ".tif", ".gif", ".bmp", ".png", ".jpeg", ".jpg" };
            string[] validAttach = { ".tiff", ".tif", ".gif", ".bmp", ".png", ".jpeg", ".jpg",
                                     ".doc", ".docx", ".xls", ".xlsx", ".xlsm", ".ppt", ".pptx",
                                     ".odt", ".fodt", ".ods", ".fods", ".odp", ".fodp", ".odg",".fodg",".odf",
                                     ".txt", ".pdf", ".zip", ".rar", ".7z", ".csv", ".json", ".xml"};

            if (knowledgeDTO.PictureFile != null && knowledgeDTO.PictureFile.ContentLength > 0)
            {
                if (!validPicture.Contains(Path.GetExtension(knowledgeDTO.PictureFile.FileName).ToLower()))
                {
                    ValidationDictionary.AddError("PictureFile", $"檔案類型錯誤。");
                }
            }

            foreach (var attachFile in knowledgeDTO.AttachFiles)
            {
                if (attachFile != null && attachFile.ContentLength > 0)
                {
                    if (!validAttach.Contains(Path.GetExtension(attachFile.FileName).ToLower()))
                    {
                        ValidationDictionary.AddError("AttachFiles", $"檔案類型錯誤。");
                    }
                }
            }
        }

        public bool UpdateKnowledge(KnowledgeDTO knowledgeDTO)
        {
            ValidateUpdateKnowledge(knowledgeDTO);

            if (ValidationDictionary.IsValid)
            {
                Knowledge knowledge = null;

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.RecognizeDestinationPrefixes("k_");
                    cfg.CreateMap<KnowledgeDTO, Knowledge>()
                    .ForMember(dest => dest.k_creator, opt => opt.Condition(src => src.Id == 0))
                    .ForMember(dest => dest.k_create_date, opt => opt.Condition(src => src.Id == 0))
                    .ForMember(dest => dest.k_picture, opt => opt.Condition(src => src.PictureFile != null));
                });
                var mapper = config.CreateMapper();


                if (knowledgeDTO.Id == 0)
                {
                    knowledge = new Knowledge();
                    mapper.Map(knowledgeDTO, knowledge);
                    _knowledgeRepo.Add(knowledge);
                    SaveChanges(_cmtUow);

                    SaveFile(knowledgeDTO.TempId, "Picture", knowledgeDTO.Picture, knowledgeDTO.PictureFile);

                    //更新圖片路徑，將TempID替換為正式ID
                    knowledge.k_content = knowledge.k_content?.Replace(knowledgeDTO.TempId, knowledge.k_id.ToString());

                    knowledgeDTO.Id = knowledge.k_id;
                    MoveTempKnowledgeFile(knowledgeDTO);
                }
                else
                {
                    knowledge = _knowledgeRepo.Get(x => x.k_id == knowledgeDTO.Id);
                    mapper.Map(knowledgeDTO, knowledge);
                    SaveFile(knowledgeDTO.Id.ToString(), "Picture", knowledgeDTO.Picture, knowledgeDTO.PictureFile);
                }
                UpdateKnowledgeFile(knowledgeDTO);
                SaveChanges(_cmtUow);
            }
            return ValidationDictionary.IsValid;
        }

        public bool DeleteKnowledge(KnowledgeDTO knowledgeDTO)
        {
            if (ValidationDictionary.IsValid)
            {
                var knowledge = _knowledgeRepo.Get(x => x.k_id == knowledgeDTO.Id);
                _knowledgeRepo.Delete(knowledge);

                var knowledgeFiles = _knowledgeFileRepo.GetAll().Where(x => x.kf_k_id == knowledgeDTO.Id);
                foreach (var knowledgeFile in knowledgeFiles)
                {
                    _knowledgeFileRepo.Delete(knowledgeFile);
                }

                SaveChanges(_cmtUow);

                DeleteDirectory(knowledgeDTO.Id.ToString());
            }
            return ValidationDictionary.IsValid;
        }

        public void UpdateKnowledgeFile(KnowledgeDTO knowledgeDTO)
        {
            if (knowledgeDTO.AttachFiles.Any())
            {
                foreach (var attach in knowledgeDTO.AttachFiles)
                {
                    if (attach != null && attach.ContentLength > 0)
                    {
                        _knowledgeFileRepo.Add(new KnowledgeFile()
                        {
                            kf_name = new FileInfo(attach.FileName).Name,
                            kf_k_id = knowledgeDTO.Id,
                            kf_creator = knowledgeDTO.Creator,
                            kf_create_date = knowledgeDTO.Create_date,
                            kf_modifier = knowledgeDTO.Modifier,
                            kf_modified_date = knowledgeDTO.Modified_date
                        });
                        SaveFile(knowledgeDTO.Id.ToString(), "Attach", attach.FileName, attach);
                    }
                }
            }
        }

        public void DeletePicture(FileDTO fileDTO)
        {
            var knowledge = _knowledgeRepo.Get(x => x.k_id.ToString() == fileDTO.Id);
            if (knowledge != null)
            {
                knowledge.k_picture = null;
                SaveChanges(_cmtUow);
            }

            DeleteFile(fileDTO.Id, "Picture", fileDTO.Name);
        }

        public void DeleteKnowledgeFile(FileDTO fileDTO)
        {
            var knowledgeFile = _knowledgeFileRepo.Get(x => x.kf_id.ToString() == fileDTO.Id);
            if (knowledgeFile != null)
            {
                _knowledgeFileRepo.Delete(knowledgeFile);
                SaveChanges(_cmtUow);
            }

            DeleteFile(fileDTO.Id, "Attach", fileDTO.Name);
        }

        public void MoveTempKnowledgeFile(KnowledgeDTO knowledgeDTO)
        {
            string source = $"{_uploadFileDir}\\Temp\\Knowledge\\{knowledgeDTO.TempId}\\";
            string target = $"{_uploadFileDir}\\Knowledge\\{knowledgeDTO.Id}\\";

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
                    fileDTO.Path = $"{_uploadFileDir}\\Knowledge\\{fileDTO.Id}\\{fileDTO.Type}\\";
                }
                else
                {
                    fileDTO.Path = $"{_uploadFileDir}\\Temp\\Knowledge\\{fileDTO.Id}\\{fileDTO.Type}\\";
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
                    path = $"{_uploadFileDir}\\Knowledge\\{id}\\{type}\\";
                }
                else
                {
                    path = $"{_uploadFileDir}\\Temp\\Knowledge\\{id}\\{type}\\";
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
                    path = $"{_uploadFileDir}\\Knowledge\\{id}\\{type}\\";
                }
                else
                {
                    path = $"{_uploadFileDir}\\Temp\\Knowledge\\{id}\\{type}\\";
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
                path = $"{_uploadFileDir}\\Knowledge\\{id}\\";
            }
            else
            {
                path = $"{_uploadFileDir}\\Temp\\Knowledge\\{id}\\";
            }

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
    }
}