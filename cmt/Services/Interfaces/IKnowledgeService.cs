using cmt.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace cmt.Services.Interfaces
{
    public interface IKnowledgeService : IService
    {
        List<KnowledgeDTO> GetKnowledgeByType(KnowledgeSearchDTO knowledgeSearchDTO);
        List<KnowledgeDTO> GetPublicKnowledgeByType(KnowledgeSearchDTO knowledgeSearchDTO);
        KnowledgeDTO GetKnowledgeById(KnowledgeDTO knowledgeDTO);
        KnowledgeDTO GetPublicKnowledgeById(KnowledgeDTO knowledgeDTO);
        void ValidateUpdateKnowledge(KnowledgeDTO knowledgeDTO);
        bool UpdateKnowledge(KnowledgeDTO knowledgeDTO);
        bool DeleteKnowledge(KnowledgeDTO knowledgeDTO);
        void UpdateKnowledgeFile(KnowledgeDTO knowledgeDTO);
        void DeletePicture(FileDTO fileDTO);
        void DeleteKnowledgeFile(FileDTO fileDTO);
        void MoveTempKnowledgeFile(KnowledgeDTO knowledgeDTO);
        FileDTO GetFile(FileDTO fileDTO);
        void UploadContentImage(FileDTO fileDTO);
        void SaveFile(string id, string type, string name, HttpPostedFileBase file);
        void DeleteFile(string id, string type, string name);
        void DeleteDirectory(string id);
    }
}

