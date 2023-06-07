using cmt.Models;
using cmt.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmt.Repositories
{
    public class EFNewsRepo : EFGenericRepo<News>, INewsRepo
    {
        public EFNewsRepo(cmtEntities dbContext) : base(dbContext)
        {

        }
    }
}