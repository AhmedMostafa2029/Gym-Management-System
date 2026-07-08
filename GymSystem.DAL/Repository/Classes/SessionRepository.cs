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
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _context;
        public SessionRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCatogryAsync(CancellationToken ct = default)
        {
            var sessions = _context.Sessions
                .AsNoTracking()
                .Include(s => s.Trainer)
                .Include(s => s.Category);

            return await sessions.ToListAsync(ct);
        }

        public async Task<Session?> GetSessionByIdWithTrainerAndCatogryAsync(int sessionid, CancellationToken ct = default)
        {
            var session = _context.Sessions
                .Include(s => s.Trainer)
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.Id == sessionid);

            return await session;
        }

        public Task<int> GetCountOfBookedSlotAsync(int sessionId, CancellationToken ct)
        {
            return _context.Bookings.AsNoTracking().CountAsync(b => b.SessionId == sessionId);
        }
    }
}
