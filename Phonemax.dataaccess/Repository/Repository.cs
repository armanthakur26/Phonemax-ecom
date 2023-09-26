using Microsoft.EntityFrameworkCore;
using Phonemax.dataaccess.Data;
using Phonemax.dataaccess.Repository.Irepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Phonemax.dataaccess.Repository
{
    public class Repository<T> : Irepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbset;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            dbset=_context.Set<T>();
            
        }
        public void Add(T entity)
        {
            dbset.Add(entity);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> Filter = null, string includeproperties = null)
        {

            IQueryable<T> query = dbset;
            if (Filter != null)
                query = query.Where(Filter);
            if (includeproperties != null)
            {
                foreach (var includeProp in includeproperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();

        }

        public T Get(int id)
        {
            return dbset.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> Filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeproperties = null)
        {
            IQueryable<T> query = dbset;
            if (Filter != null)
                query = query.Where(Filter);
            if (includeproperties != null)
            {
                foreach (var includeProp in includeproperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            if (orderBy != null)
                return orderBy(query).ToList();
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbset.Remove(entity);
        }

        public void Remove(int id)
        {
            var entity = dbset.Find(id);
            dbset.Remove(entity);
            //  or
            //var entity=Get(id);
            // Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbset.RemoveRange(entity);
        }

        public void Update(T entity)
        {
            _context.ChangeTracker.Clear();
            dbset.Update(entity);
        }
    }
}
