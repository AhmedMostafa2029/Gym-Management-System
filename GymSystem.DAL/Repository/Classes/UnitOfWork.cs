using GymSystem.DAL.Contexts;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _context;
        private readonly Dictionary<string, object> _Repos = [];
        public UnitOfWork(GymDbContext context)
        {
            _context = context;
            SessionRepository = new SessionRepository(_context);
            TrainerRepository = new TrainerRepository(_context);
            PlanRepository = new PlanRepository(_context);
            MembershipRepository = new MembershipRepository(_context);
            BookingRepository = new BookingRepository(_context);
        }

        public ISessionRepository SessionRepository { get; }
        public IBookingRepository BookingRepository { get; }
        public IMembershipRepository MembershipRepository { get; }

        public ITrainerRepository TrainerRepository { get; }

        public IPlanRepository PlanRepository { get; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var TypeName = typeof(TEntity).Name; // String  Key
            if(_Repos.TryGetValue(TypeName, out object oldRepo))
            {
                return (IGenericRepository<TEntity>)oldRepo;
            }
            var newRepo = new GenericRepository<TEntity>(_context);
            _Repos[TypeName] = newRepo;

            return newRepo;
        }
    }
}
