using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        public Task<int> CompleteAsync();
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();

        public ISessionRepository SessionRepository { get; }
        public IBookingRepository BookingRepository { get; }
        public IMembershipRepository MembershipRepository { get; }


        public ITrainerRepository TrainerRepository { get; }
        public IPlanRepository PlanRepository { get; }

    }
}
