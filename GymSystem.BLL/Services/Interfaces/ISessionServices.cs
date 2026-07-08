using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface ISessionServices
    {
        Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default);
        Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct = default);
        Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default);

        Task<SessionViewModel?> GetSessionByIdAsync(int sessionId, CancellationToken ct = default);
        Task<UpdateSessionViewModel?> GetSessionByIdForUpdateAsync(int sessionId, CancellationToken ct = default);



        // Post ViewModels -> Models -> DB
        Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);
        Task<Result> UpdateSessionAsync(int id,UpdateSessionViewModel model, CancellationToken ct = default);
        Task<Result> DeleteSessionAsync(int id, CancellationToken ct = default);



    }
}
