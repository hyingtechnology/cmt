using cmt.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web;

namespace cmt.Repositories
{
    public class EFGenericRepo<T> : IRepository<T>
        where T : class
    {
        private DbContext Context { get; set; }

        public EFGenericRepo(DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("context");
            }
            Context = dbContext;
        }

        public void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            Context.Entry<T>(entity).State = EntityState.Deleted;
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return Context.Set<T>().FirstOrDefault(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return Context.Set<T>().AsQueryable();
        }
    }
}