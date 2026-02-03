using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimpleKanbanBoards.DataAccess.Repository.IRepository
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes
        );

        Task<T> GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            params Expression<Func<T, object>>[] includes
        );

        Task<bool> Exist(Expression<Func<T, bool>> filter = null);

        Task AddAsync(T entity);

        void Remove(T entity);

        void Update(T entity);
    }
}
