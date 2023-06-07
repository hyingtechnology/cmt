using cmt.Models;
using cmt.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmt.Repositories
{
    public class EFLogRepo : EFGenericRepo<Log>, ILogRepo
    {
        public EFLogRepo(cmtEntities dbContext) : base(dbContext)
        {

        }
    }
}