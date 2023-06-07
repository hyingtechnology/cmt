using cmt.Models;
using cmt.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmt.UnitOfWorks
{
    public class EFCmtUow : EFGenericUow<cmtEntities>, ICmtUow
    {
        public EFCmtUow(cmtEntities dbContext) : base(dbContext)
        {

        }
    }
}