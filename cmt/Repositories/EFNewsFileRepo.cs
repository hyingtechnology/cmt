using cmt.Models;
using cmt.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmt.Repositories
{

    public class EFNewsFileRepo : EFGenericRepo<NewsFile>, INewsFileRepo
    {
        public EFNewsFileRepo(cmtEntities dbContext) : base(dbContext)
        {

        }
    }
}