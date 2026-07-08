using AutoMapper;
using GymSystem.BLL.Common;
using GymSystem.BLL.Services.Classes;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.DAL.Repository.Classes
{
    public class MemberServices : IMemberServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentServices _attachmentService;
        private const string MemberImagesFolder = "Files/Images/Members";


        public MemberServices(IUnitOfWork unitOfWork , IMapper mapper , IAttachmentServices attachmentServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentServices;

        }


        public async Task<MemberIndexViewModel> GetAllAsync(MemberFilterViewModel filter, CancellationToken ct = default)
        {
            var query = _unitOfWork
                .GetRepository<Member>()
                .GetQueryable();

            // ================= Search =================

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.Trim();

                query = query.Where(m => m.Name.Contains(search) || m.Email.Contains(search)
                    || m.Phone.Contains(search));
            }
            // ================= City =================

            if (!string.IsNullOrWhiteSpace(filter.City))
            {
                query = query.Where(m =>

                    m.Address.City == filter.City);
            }

            // ================= Gender =================

            if (filter.Gender.HasValue)
            {
                query = query.Where(m =>

                    m.Gender == filter.Gender);
            }

            // ================= Sort =================

            query = query.OrderBy(m => m.Name);

            // ================= Mapping =================

            var memberQuery = query.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Gender = m.Gender.ToString(),
                Photo = m.Photo
            });

            var members =
                await PaginatedList<MemberViewModel>.CreateAsync(
                    memberQuery,
                    filter.Page,
                    filter.PageSize,
                    ct);

            var cities = await GetCitiesAsync(ct);

            return new MemberIndexViewModel
            {
                Filter = filter,
                Members = members,
                Cities = cities
            };
        }

        public async Task<MemberViewModel?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            // Member + membership + plan
            var member = await _unitOfWork.GetRepository<Member>().GetById(id, ct);
            if (member is null)
                return null;

            var memberViewModel = _mapper.Map<MemberViewModel>(member);

            // Membership
            var ActiveMembership = await _unitOfWork.GetRepository<Membership>().FirstOrDefaultAsync(mb => mb.MemberId == id && mb.EndDate > DateTime.Now, false, ct);
            if (ActiveMembership is not null)
            {
                var ActivePlan = await _unitOfWork.GetRepository<Plan>().GetById(ActiveMembership.PlanId, ct);

                memberViewModel.PlanName = ActivePlan?.Name;
                memberViewModel.MembershipStartDate = ActiveMembership.CreatedAt.ToShortDateString();
                memberViewModel.MembershipEndDate = ActiveMembership.EndDate.ToShortDateString();
            }

            return memberViewModel;
        }

        public async Task<HealthRecordViewModel?> GetHealthRecordAsync(int id, CancellationToken ct = default)
        {
            var Record = await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(r => r.MemberId == id, false, ct);

            if (Record is null)
                return null;

            return _mapper.Map<HealthRecordViewModel>(Record);
        }

        public async Task<MemberToUpdateViewModel?> GetForUpdateAsync(int id, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetById(id, ct);

            if (member is null)
                return null;

            return _mapper.Map<MemberToUpdateViewModel>(member);
        }

        public async Task<Result> CreateAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var emailExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email, ct);
            var phoneExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone, ct);

            if (emailExists)
                return Result.Fail("Email already exists.");
            if (phoneExists)
                return Result.Fail("Phone number already exists.");

            var member = _mapper.Map<Member>(model);

            if (model.PhotoFile is null)
                return Result.Fail("Member photo is required.");

            var NewPhotoName = await _attachmentService.UploadAsync(model.PhotoFile.OpenReadStream(), model.PhotoFile.FileName, MemberImagesFolder, ct);

            if (string.IsNullOrEmpty(NewPhotoName)) return Result.Fail("Failed to Photo Member");

            member.Photo = NewPhotoName;
            _unitOfWork.GetRepository<Member>().Add(member);
            var rowEffected = await _unitOfWork.CompleteAsync();

            return rowEffected > 0 ? Result.Ok() : Result.Fail("Failed to Create Member");

        }

        public async Task<Result> UpdateAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var memberRepo = _unitOfWork.GetRepository<Member>();

            var member = await memberRepo.GetById(id, ct);
            if (member is null)
                return Result.NotFound("Member Not Found");

            if (await memberRepo.AnyAsync(m => m.Email == model.Email && m.Id != id))
                return Result.Fail("Email already exists.");
            if (await memberRepo.AnyAsync(m => m.Phone == model.Phone && m.Id != id))
                return Result.Fail("Phone number already exists.");


            member.UpdatedAt = DateTime.Now;

            _mapper.Map(model, member);

            if (model.PhotoFile is not null)
            {
                if (!string.IsNullOrEmpty(member.Photo))
                {
                    _attachmentService.Delete(
                        member.Photo,
                        MemberImagesFolder);
                }

                member.Photo =
                    await _attachmentService.UploadAsync(
                        model.PhotoFile.OpenReadStream(),
                        model.PhotoFile.FileName,
                        MemberImagesFolder,
                        ct);
            }

            memberRepo.Update(member);

            var mapped = await _unitOfWork.CompleteAsync();

            return mapped > 0
                ? Result.Ok()
                : Result.Fail("Failed To Update Member"); ;
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetById(id, ct);
            if (member == null) return Result.NotFound("Not Found");

            var HasFutureSessions = await _unitOfWork.GetRepository<Booking>().AnyAsync(b => b.MemberId == id && b.Session.EndDate > DateTime.Now, ct);
            if (HasFutureSessions)
                return Result.Fail("Member has future bookings and cannot be deleted.");

            _unitOfWork.GetRepository<Member>().Delete(id);

            if (member.Photo is not null)
            {
                _attachmentService.Delete(member.Photo, MemberImagesFolder);
            }
            var mapped = await _unitOfWork.CompleteAsync();

            return mapped > 0
                ? Result.Ok()
                 : Result.Fail("Failed To Delete Member"); ;
        }

        public async Task<IEnumerable<string>> GetCitiesAsync(CancellationToken ct = default)
        {
            return await _unitOfWork
              .GetRepository<Member>()
              .GetQueryable()
              .Select(m => m.Address.City)
              .Distinct()
              .OrderBy(c => c)
              .ToListAsync(ct);
        }
    }
}
