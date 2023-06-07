using cmt.Models;
using cmt.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace cmt.Repositories
{
    public class EFUserRepo : EFGenericRepo<User>, IUserRepo
    {
        public EFUserRepo(cmtEntities dbContext) : base(dbContext)
        {

        }
    }
}