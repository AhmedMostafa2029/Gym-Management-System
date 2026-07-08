using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    public class SessionServices : ISessionServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SessionServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate)
                return Result.Validation("End Date Must Be After Start Date");
            if (model.StartDate <= DateTime.Now)
                return Result.Validation("Start Date Must be in the future");

            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();

            var Trainer = await TrainerRepo.GetById(model.TrainerId, ct);

            if (Trainer is null)
                return Result.NotFound("Trainer Not Found");

            var CategorryRepo = _unitOfWork.GetRepository<Category>();

            var Category = await CategorryRepo.GetById(model.CategoryId, ct);

            if (Category is null)
                return Result.NotFound("Categories Not Found");

            var session = _mapper.Map<CreateSessionViewModel, Session>(model);

            var SessionRepo = _unitOfWork.GetRepository<Session>(); // Generic

            SessionRepo.Add(session);
            var rowEffected = await _unitOfWork.CompleteAsync();

            return rowEffected > 0 ? Result.Ok() : Result.Fail("Failed to Create Session");

        }

        public async Task<Result> DeleteSessionAsync(int id, CancellationToken ct = default)
        {
            var repo = _unitOfWork.GetRepository<Session>();
            var session = await repo.GetById(id , ct);

            if (session is null)
                return Result.NotFound("Session Not Found");

            if(session.EndDate >= DateTime.Now)
                return Result.Fail("Cannot delete a session that has not ended yet");

            var bookedSlotsCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(session.Id, ct);
            if (bookedSlotsCount > 0)
                return Result.Fail("Cannot delete a session that has booked slots");

            repo.Delete(id);

            var rowEffected = await _unitOfWork.CompleteAsync();

            return rowEffected > 0 ? Result.Ok() : Result.Fail("Failed to Delete Session");
        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct = default)
        {
            // get all sessions
            // find the trainer for each session
            // find the category for each session

            var Sessions = await _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCatogryAsync(ct);
            Sessions = Sessions.OrderByDescending(s => s.StartDate);

            // Model => ViewModel
            // Faster Automatic Mapping
            var MappedSessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(Sessions);

            foreach (var session in MappedSessions)
            {
                session.AvailableSlots = session.Capacity - (await _unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(session.Id, ct));
            }
            return MappedSessions;
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct = default)
        {
            var Categories = await _unitOfWork.GetRepository<Category>().GetAll(false, ct);

            return _mapper.Map<IEnumerable<Category>, IEnumerable<CategorySelectViewModel>>(Categories);
        }

        public async Task<SessionViewModel?> GetSessionByIdAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetSessionByIdWithTrainerAndCatogryAsync(sessionId, ct);
            if (session is null)
                return null;

            var mappedSession = _mapper.Map<Session, SessionViewModel>(session);

            mappedSession.AvailableSlots = mappedSession.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(mappedSession.Id, ct);

            return mappedSession;


        }

        public async Task<UpdateSessionViewModel?> GetSessionByIdForUpdateAsync(int sessionId, CancellationToken ct = default)
        {
            var Session = await _unitOfWork.SessionRepository.GetById(sessionId, ct);

            if (Session is null)
                return null;

            if (!await IsSessionValidForUpdateAsync(Session, ct))
                return null;

            return _mapper.Map<Session, UpdateSessionViewModel>(Session);

        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default)
        {
            var Trainers = await _unitOfWork.GetRepository<Trainer>().GetAll(false, ct);

            return _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerSelectViewModel>>(Trainers);
        }

        public async Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var SessionRepo = _unitOfWork.GetRepository<Session>();
            var session = await SessionRepo.GetById(id, ct);

            if (session is null)
                return Result.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Cannot update a session that has already started");


            var bookedSlotsCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(session.Id, ct);

            if(bookedSlotsCount > 0)
                return Result.Fail("Cannot update a session that has booked slots");

            if(model.EndDate <= model.StartDate)
                return Result.Validation("End Date Must Be After Start Date");




            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();

            var Trainer = await TrainerRepo.GetById(model.TrainerId, ct);

            if (Trainer is null)
                return Result.NotFound("Trainer Not Found");

            var CategorryRepo = _unitOfWork.GetRepository<Category>();

            var Category = await CategorryRepo.GetById(model.CategoryId, ct);

            if (Category is null)
                return Result.NotFound("Categories Not Found");

            session.UpdatedAt = DateTime.Now; 

            _mapper.Map(model, session);

            SessionRepo.Update(session);
            var rowEffected = await _unitOfWork.CompleteAsync();

            return rowEffected > 0 ? Result.Ok() : Result.Fail("Failed to Update Session");
        }

        private async Task<bool> IsSessionValidForUpdateAsync(Session session, CancellationToken ct)
        {
            if (session.StartDate <= DateTime.Now) return false;
            var bookedSlotsCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(session.Id, ct); 
            
            return bookedSlotsCount == 0;

        }
    }
}
