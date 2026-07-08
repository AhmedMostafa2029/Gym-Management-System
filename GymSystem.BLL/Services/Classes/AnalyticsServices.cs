using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.AnalyticsViewModel;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class AnalyticsServices : IAnalyticsServices
    {

        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AnalyticsViewModel> GetAnalyticsDataAsync(CancellationToken ct = default)
        {
            var sessions = await _unitOfWork.GetRepository<Session>().GetAll(false, ct);

            var totalMember = await _unitOfWork.GetRepository<Member>().CountAsync(ct : ct);
            var totalTrainer = await _unitOfWork.GetRepository<Trainer>().CountAsync(ct  : ct);
            var activeMembers = await _unitOfWork.GetRepository<Membership>().CountAsync(m => m.EndDate > DateTime.Now ,ct);

            return new AnalyticsViewModel
            {
                TotalMembers = totalMember,
                TotalTrainers = totalTrainer,
                ActiveMembers = activeMembers,
                UpcomingSessions = sessions.Count(s => s.StartDate > DateTime.Now),
                OngoingSessions = sessions.Count(s => s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now),
                CompletedSessions = sessions.Count(s => s.EndDate < DateTime.Now)
            };

        }
    }
}
