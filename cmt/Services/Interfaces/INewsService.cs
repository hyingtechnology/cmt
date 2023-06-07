using cmt.DTOs;
using cmt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace cmt.Services.Interfaces
{
    public interface INewsService : IService
    {
        List<NewsDTO> GetNewsByCodeTableType(string CT_type);
        List<NewsDTO> GetNewsByType(NewsSearchDTO newsSearchDTO);
        List<NewsDTO> GetPublicNewsByType(NewsSearchDTO newsSearchDTO);
        List<NewsDTO> GetPublicTopNewsByType(NewsSearchDTO newsSearchDTO, int quantity);
        NewsDTO GetNewsById(NewsDTO newsDTO);
        NewsDTO GetPublicNewsById(NewsDTO newsDTO);
        void ValidateUpdateNews(NewsDTO newsDTO);
        bool UpdateNews(NewsDTO newsDTO);
        bool DeleteNews(NewsDTO newsDTO);
        void UpdateNewsFile(NewsDTO newsDTO);
        void DeletePicture(FileDTO fileDTO);
        void DeleteNewsFile(FileDTO fileDTO);
        void MoveTempNewsFile(NewsDTO newsDTO);
        FileDTO GetFile(FileDTO fileDTO);
        void UploadContentImage(FileDTO fileDTO);
        void SaveFile(string id, string type, string name, HttpPostedFileBase file);
        void DeleteFile(string id, string type, string name);
        void DeleteDirectory(string id);
        DateTime? GetLastModifiedDate();
    }
}
