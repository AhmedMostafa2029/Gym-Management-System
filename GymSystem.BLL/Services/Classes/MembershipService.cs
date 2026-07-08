using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembershipViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class MembershipService(IUnitOfWork unitOfWork, IMapper mapper) : IMembershipService
    {
        public async Task<Result> CreateMembershipAsync(CreateMemberShipViewModel model, CancellationToken ct = default)
        {
            var memberExists = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Id == model.MemberId, ct);
            if (!memberExists) return Result.NotFound("Member not found.");

            var plan = await unitOfWork.GetRepository<Plan>().GetById(model.PlanId, ct);
            if (plan is null) return Result.NotFound("Plan not found.");
            if (!plan.IsActive) return Result.Fail("Plan is not active.");


            var hasActive = await unitOfWork.MembershipRepository
                .AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > DateTime.Now, ct);
            if (hasActive) return Result.Fail("Member already has an active membership.");

            var entity = new Membership
            {
                MemberId = model.MemberId,
                PlanId = plan.Id,
                CreatedAt = DateTime.Now,
                EndDate = (model.StartDate ?? DateTime.Now).AddDays(plan.Duration),
            };

            unitOfWork.MembershipRepository.Add(entity);
            var result = await unitOfWork.CompleteAsync();
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Create New Membership");
        }

        public async Task<Result> DeleteActiveMembershipAsync(int memberId, CancellationToken ct = default)
        {
            var active = await unitOfWork.MembershipRepository.FirstOrDefaultAsync(
                m => m.MemberId == memberId && m.EndDate > DateTime.Now, ct: ct);

            if (active is null) return Result.NotFound("No active membership for this member.");

            unitOfWork.MembershipRepository.Delete(active.Id);
            var result = await unitOfWork.CompleteAsync();
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Delete Membership");
        }

        public async Task<IEnumerable<MemberShipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default)
        {
            var memberships = await unitOfWork.MembershipRepository
                .GetAllMembershipsWithMemberAndPlanAsync(m => m.EndDate > DateTime.Now, ct);
            return mapper.Map<IEnumerable<MemberShipViewModel>>(memberships);
        }

        public async Task<IEnumerable<PlanSelectListViewModel>> GetPlansForDropDownAsync(CancellationToken ct = default)
        {
            var plans = await unitOfWork.GetRepository<Plan>().GetAll(false, ct);
            return mapper.Map<IEnumerable<PlanSelectListViewModel>>(plans);
        }

        public async Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(CancellationToken ct = default)
        {
            var members = await unitOfWork.GetRepository<Member>().GetAll(false, ct);
            return mapper.Map<IEnumerable<MemberSelectListViewModel>>(members);
        }
    }
}
