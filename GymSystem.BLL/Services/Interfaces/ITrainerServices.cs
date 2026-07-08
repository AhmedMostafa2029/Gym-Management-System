using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface ITrainerServices
    {
        Task<IEnumerable<TrainerViewModel>> GetAllAsync(CancellationToken ct = default);

        Task<TrainerViewModel?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<TrainerToUpdateViewModel?> GetForUpdateAsync(int id, CancellationToken ct = default);



        Task<Result> CreateAsync(CreateTrainerViewModel model, CancellationToken ct = default);

        Task<Result> UpdateAsync(int id, TrainerToUpdateViewModel model, CancellationToken ct = default);

        Task<Result> DeleteAsync(int id, CancellationToken ct = default);
    }
}
