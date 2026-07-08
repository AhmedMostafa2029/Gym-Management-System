using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity , new()
    {
        Task<IEnumerable<TEntity>> GetAll(bool isTracked, CancellationToken ct = default);

        Task<TEntity?> GetById(int? id, CancellationToken ct = default);
        void Add(TEntity entity);
        void Update(TEntity entity);

        void Delete(int id);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate ,bool isTracked = false, CancellationToken ct = default);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null, CancellationToken ct = default);

        IQueryable<TEntity> GetQueryable(bool tracked = false);
    }
}
