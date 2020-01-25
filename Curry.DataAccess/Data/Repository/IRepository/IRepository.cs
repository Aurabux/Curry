using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Curry.DataAccess.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //get object by id
        T Get(int id);
        // get all objects as Ienumerable
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filer = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null);

        //get the first or default
        T GetFirstOrDefault(
          Expression<Func<T, bool>> filer = null,
          string includeProperties = null);

        //Add
        void Add(T entity);
        //Remove(id)
        void Remove(int id);
        //Remove(obj)
        void Remove(T entity);
    }
}
