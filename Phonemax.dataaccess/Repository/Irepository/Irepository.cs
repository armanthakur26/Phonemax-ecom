using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Phonemax.dataaccess.Repository.Irepository
{
    public interface Irepository<T>where T : class
    {
        void Add(T entity);// save code
        void Remove(T entity);// delete code
        void Remove(int id);//delete code by id
        void RemoveRange(IEnumerable<T> entity);//for multipal data delete
        T Get(int id);//find code
        void Update(T entity);
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> Filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeproperties = null);
        T FirstOrDefault(
                      Expression<Func<T, bool>> Filter = null,
                      string includeproperties = null);
    }
}
