using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        public Task<List<Booking>> GetBySessionIdAsync(int sessionId, CancellationToken ct = default);
    }
}
