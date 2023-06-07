using cmt.Models;
using cmt.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmt.Repositories
{
    public class EFCodeTableRepo : EFGenericRepo<CodeTable>, ICodeTableRepo
    {
        public EFCodeTableRepo(cmtEntities dbContext) : base(dbContext)
        {

        }
    }
}