using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace cmt.Repositories.Interfaces
{
    public interface IRepository<T>
            where T : class
    {
        void Add(T entity);

        void Delete(T entity);

        T Get(Expression<Func<T, bool>> predicate);

        IQueryable<T> GetAll();

    }
}
