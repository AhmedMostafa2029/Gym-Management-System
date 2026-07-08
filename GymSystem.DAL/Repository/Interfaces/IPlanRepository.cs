using GymSystem.DAL.Models;
using GymSystem.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Interfaces
{
    public interface IPlanRepository : IGenericRepository<Plan>
    {
        // New Faetures
        Task<bool> HasActiveMembershipsAsync(int planId,CancellationToken ct = default);

    }
}
