using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class PlanServices : IPlanServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PlanServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<Result> ChangeStatusAsync(int id, CancellationToken ct = default)
        {
            var planRepo = _unitOfWork.GetRepository<Plan>();

            var plan = await planRepo.GetById(id, ct);

            if (plan is null)
                return Result.NotFound("Plan not found.");


            // Business Rule:
            // Active plan cannot be deactivated
            // while it has active memberships.

            if (plan.IsActive)
            {
                var hasActiveMemberships =
                    await _unitOfWork.PlanRepository
                        .HasActiveMembershipsAsync(id, ct);

                if (hasActiveMemberships)
                {
                    return Result.Fail(
                        "Plan cannot be deactivated because it has active memberships.");
                }
            }


            // Toggle Status

            plan.IsActive = !plan.IsActive;

            plan.UpdatedAt = DateTime.Now;


            var rowsAffected =
                await _unitOfWork.CompleteAsync();


            return rowsAffected > 0
                ? Result.Ok()
                : Result.Fail("Failed to change plan status.");
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllAsync(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAll(false, ct);

            plans = plans.OrderBy(p => p.Name);

            return _mapper.Map<IEnumerable<PlanViewModel>>(plans);
        }

        public async Task<PlanViewModel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetById(id, ct);

            if (plan is null)
                return null;

            return _mapper.Map<PlanViewModel>(plan);
        }

        public async Task<UpdatePlanViewModel?> GetForUpdateAsync(int id, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetById(id, ct);

            if (plan is null)
                return null;

            return _mapper.Map<UpdatePlanViewModel>(plan);
        }

        public async Task<Result> UpdateAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var planRepo = _unitOfWork.GetRepository<Plan>();

            var plan = await planRepo.GetById(id, ct);

            if (plan is null)
                return Result.NotFound("Plan not found.");

            var isNameExist = await planRepo.AnyAsync(
                p => p.Name == model.Name && p.Id != id,
                ct);

            if (isNameExist)
                return Result.Fail("Plan name already exists.");

            plan.UpdatedAt = DateTime.Now;

            _mapper.Map(model, plan);

            planRepo.Update(plan);

            var rowsAffected = await _unitOfWork.CompleteAsync();

            return rowsAffected > 0
                ? Result.Ok()
                : Result.Fail("Failed to update plan.");
        }
    }
}
