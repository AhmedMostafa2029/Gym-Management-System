using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Interfaces
{
    public interface IPlanServices
    {
        Task<IEnumerable<PlanViewModel>> GetAllAsync(CancellationToken ct = default);

        Task<PlanViewModel?> GetByIdAsync(int id, CancellationToken ct = default);

        Task<UpdatePlanViewModel?> GetForUpdateAsync(int id, CancellationToken ct = default);


        Task<Result> UpdateAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default);

        Task<Result> ChangeStatusAsync(int id, CancellationToken ct = default);
    }
}
