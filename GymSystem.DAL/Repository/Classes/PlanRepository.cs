using GymSystem.DAL.Contexts;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Classes
{
    public class PlanRepository : GenericRepository<Plan>, IPlanRepository
    {
        private readonly GymDbContext _context;

        public PlanRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> HasActiveMembershipsAsync(int planId, CancellationToken ct = default)
        {
            return await _context.Memberships
                    .AnyAsync(m =>
                        m.PlanId == planId &&
                        m.EndDate > DateTime.Now,
                        ct);
        }

        // Implement any specific methods for PlanRepository if needed

    }
}
