using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Interfaces
{
    public interface ITrainerRepository : IGenericRepository<Trainer>
    {
        Task<bool> HasSessionsAsync(int trainerId, CancellationToken ct = default);
    }
}
