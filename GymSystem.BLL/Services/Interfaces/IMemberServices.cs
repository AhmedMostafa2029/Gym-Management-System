using GymSystem.BLL.Common;
using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repository.Interfaces
{
    public interface IMemberServices
    {
        // Get Models -> ViewModels -> View
        Task<MemberIndexViewModel> GetAllAsync(MemberFilterViewModel filter , CancellationToken ct = default);
        Task<MemberViewModel?> GetByIdAsync(int id, CancellationToken ct = default);
        
        Task<HealthRecordViewModel?> GetHealthRecordAsync(int id, CancellationToken ct = default);
        Task<MemberToUpdateViewModel?> GetForUpdateAsync(int id, CancellationToken ct = default);

        Task<IEnumerable<string>> GetCitiesAsync(CancellationToken ct = default);

        // Post ViewModels -> Models -> DB
        Task<Result> CreateAsync(CreateMemberViewModel model, CancellationToken ct = default);
        Task<Result> UpdateAsync(int id , MemberToUpdateViewModel model , CancellationToken ct = default);
        Task<Result> DeleteAsync(int id, CancellationToken ct = default);


    }
}
