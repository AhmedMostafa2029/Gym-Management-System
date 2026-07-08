using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Interfaces
{
    public interface ISessionRepository: IGenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCatogryAsync(CancellationToken ct = default);

        Task<Session?> GetSessionByIdWithTrainerAndCatogryAsync(int sessionid, CancellationToken ct = default);

        Task<int> GetCountOfBookedSlotAsync(int sessionId, CancellationToken ct);

    }
}
