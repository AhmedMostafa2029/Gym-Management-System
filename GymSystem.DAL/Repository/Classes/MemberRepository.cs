using GymSystem.DAL.Contexts;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Classes
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {

        private readonly GymDbContext _context;

        public MemberRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }

        // impament new Faetures for member repository
        public async Task<Member?> GetMemberWithHealthRecordById(int id)
        {
            var member = await _context.Members
                .Include(m => m.HealthRecord) // Assuming HealthRecord is a navigation property in Member
                .FirstOrDefaultAsync(m => m.Id == id);
                
            return member;
        }

    }
}
