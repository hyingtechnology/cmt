using cmt.Models;
using cmt.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmt.Repositories
{
    public class EFKnowledgeFileRepo : EFGenericRepo<KnowledgeFile>, IKnowledgeFileRepo
    {
        public EFKnowledgeFileRepo(cmtEntities dbContext) : base(dbContext)
        {

        }
    }
}