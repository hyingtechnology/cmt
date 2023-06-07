using cmt.Models;
using cmt.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmt.Repositories
{
    public class EFKnowledgeRepo : EFGenericRepo<Knowledge>, IKnowledgeRepo
    {
        public EFKnowledgeRepo(cmtEntities dbContext) : base(dbContext)
        {

        }
    }
}