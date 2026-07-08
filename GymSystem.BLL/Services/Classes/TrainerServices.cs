using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.TrainerViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class TrainerServices : ITrainerServices
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TrainerServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<Result> CreateAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            if (model.DateOfBirth >= DateOnly.FromDateTime(DateTime.Today))
                return Result.Validation("Date Of Birth Must Be In The Past");

            var trainerRepo = _unitOfWork.GetRepository<Trainer>();

            var isEmailExist = await trainerRepo.AnyAsync(
                    t => t.Email == model.Email,
                    ct);

            if (isEmailExist)
                return Result.Fail("Email Already Exists");

            var isPhoneExist =
                    await trainerRepo.AnyAsync(
                        t => t.Phone == model.Phone,
                        ct);

            if (isPhoneExist)
                return Result.Fail("Phone Already Exists");

            var trainer = _mapper.Map<CreateTrainerViewModel, Trainer>(model);
            trainerRepo.Add(trainer);

            var rows = await _unitOfWork.CompleteAsync();

            return rows > 0
                ? Result.Ok()
                : Result.Fail("Failed To Create Trainer");
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
        {
            var trainerRepo = _unitOfWork.GetRepository<Trainer>();

            var trainer = await trainerRepo.GetById(id, ct);

            if (trainer is null)
                return Result.NotFound("Trainer Not Found");

            trainerRepo.Delete(id);

            if (await _unitOfWork.TrainerRepository.HasSessionsAsync(id, ct))
                return Result.Fail("Cannot delete trainer because they have assigned sessions.");

            var rowsAffected = await _unitOfWork.CompleteAsync();

            return rowsAffected > 0
                ? Result.Ok()
                : Result.Fail("Failed To Delete Trainer");
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllAsync(CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();
            var trainers = await repo.GetAll(false,ct);
            trainers = trainers.OrderBy(t => t.Name);

            var Mapped = _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerViewModel>>(trainers);

            return Mapped;
        }

        public async Task<TrainerViewModel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();
            var trainer = await repo.GetById(id, ct);

            if (trainer == null)
                return null;

            var mapped = _mapper.Map<Trainer, TrainerViewModel>(trainer);
            return mapped;
        }

        public async Task<TrainerToUpdateViewModel?> GetForUpdateAsync(int id, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Trainer>();

            var trainer = await repo.GetById(id, ct);

            if (trainer is null)
                return null;

            var mapped = _mapper.Map<Trainer, TrainerToUpdateViewModel>(trainer);
            return mapped;
        }

        public async Task<Result> UpdateAsync(int id, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var trainerRepo = _unitOfWork.GetRepository<Trainer>();

            var trainer = await trainerRepo.GetById(id, ct);

            if (trainer is null)
                return Result.NotFound("Trainer Not Found");


            // Check Email
            var isEmailExist = await trainerRepo.AnyAsync(
                t => t.Email == model.Email && t.Id != id,
                ct);

            if (isEmailExist)
                return Result.Fail("Email Already Exists");


            // Check Phone
            var isPhoneExist = await trainerRepo.AnyAsync(
                t => t.Phone == model.Phone && t.Id != id,
                ct);

            if (isPhoneExist)
                return Result.Fail("Phone Number Already Exists");


            trainer.UpdatedAt = DateTime.Now;

            _mapper.Map(model, trainer);

            trainerRepo.Update(trainer);

            var rowsAffected = await _unitOfWork.CompleteAsync();

            return rowsAffected > 0
                ? Result.Ok()
                : Result.Fail("Failed To Update Trainer");
        }
    }
}
