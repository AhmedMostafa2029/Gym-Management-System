using GymSystem.DAL.Contexts;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {

        private readonly GymDbContext _context;

        public GenericRepository(GymDbContext context)
        {
            _context = context;
        }
        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate, ct);

        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            return predicate is null ? _context.Set<TEntity>().AsNoTracking().CountAsync(ct) : _context.Set<TEntity>().AsNoTracking().CountAsync(predicate, ct);
        }


        public void Delete(int id)
        {
            var entity = _context.Set<TEntity>().FirstOrDefault(e => e.Id == id);
            if (entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
            }
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool isTracked = false, CancellationToken ct = default)
        {
            var entities = isTracked ? _context.Set<TEntity>() : _context.Set<TEntity>().AsNoTracking();

            return await entities.FirstOrDefaultAsync(predicate, ct);
        }

        public async Task<IEnumerable<TEntity>> GetAll(bool isTracked, CancellationToken ct = default)
        {
            var entities = isTracked ? _context.Set<TEntity>() : _context.Set<TEntity>().AsNoTracking();
            return await entities.ToListAsync();
        }

        public async Task<TEntity?> GetById(int? id, CancellationToken ct = default)
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(p => p.Id == id);
            return entity;
        }

        public IQueryable<TEntity> GetQueryable(bool tracked = false)
        {
            return tracked
                ? _context.Set<TEntity>()
                : _context.Set<TEntity>().AsNoTracking();
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

    }
}
