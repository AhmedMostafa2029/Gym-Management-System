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
    public class TrainerRepository : GenericRepository<Trainer>, ITrainerRepository
    {
        private readonly GymDbContext _context;
        public TrainerRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> HasSessionsAsync(int trainerId, CancellationToken ct = default)
        {
            return await _context.Sessions
                        .AnyAsync(s => s.TrainerId == trainerId, ct);
        }
    }
}
