using Curry.DataAccess.Data.Repository.IRepository;
using Microsoft.Ajax.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Curry.DataAccess.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
            protected readonly DbContext Context;
            internal DbSet<T> dbset;
            
        public Repository(DbContext context)
        {
            Context = context;
            this.dbset = context.Set<T>();
        }
        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filer = null, string includeProperties = null)
        {
            IQueryable<T> query = dbset;
            if (filer != null)
            {
                query = query.Where(filer);
            }
            //included properties will be comma separated
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);

                }
            }
            return query.FirstOrDefault();
        }

        public T Get(int id)
        {
            return dbset.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filer = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> query = dbset;
            if(filer !=null)
            {
                query = query.Where(filer);
            }
            //included properties will be comma separated
            if(includeProperties != null)
            {
                foreach(var includeProperty in includeProperties.Split(new char[] { ','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);

                }
            }
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public void Remove(int id)
        {
            T entityToRemove = dbset.Find(id);
            Remove(entityToRemove);
        }
        public void Remove(T entity)
        {
                dbset.Remove(entity);
        }
    }
}
