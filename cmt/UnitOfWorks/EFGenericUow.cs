using cmt.Extensions.ValidationError;
using cmt.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace cmt.UnitOfWorks
{
    public class EFGenericUow<T> : IUnitOfWork
        where T : DbContext
    {
        private T Context { get; set; }
        public EFGenericUow(T dbContext)
        {
            Context = dbContext;
        }

        public void SaveChanges()
        {
            var errors = Context.GetValidationErrors();
            if (!errors.Any())
            {
                Context.SaveChanges();
            }
            else
            {
                throw new DatabaseValidationErrors(errors);
            }
        }
    }
}