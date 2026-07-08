using GymSystem.DAL.Models;
using GymSystem.DAL.Repository.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Interfaces
{
    public interface IMembershipRepository : IGenericRepository<Membership>
    {
        Task<List<Membership>> GetAllMembershipsWithMemberAndPlanAsync(Expression<Func<Membership, bool>>? predicate = null, CancellationToken ct = default);
    }
}
