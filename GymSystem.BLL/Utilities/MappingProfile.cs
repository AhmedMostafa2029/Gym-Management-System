using AutoMapper;
using GymSystem.BLL.ViewModels.BookingViewModels;
using GymSystem.BLL.ViewModels.MembershipViewModels;
using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.BLL.ViewModels.PlanViewModels;
using GymSystem.BLL.ViewModels.SessionViewModels;
using GymSystem.BLL.ViewModels.TrainerViewModels;
using GymSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add your mapping configurations here
            // For example:
            // CreateMap<SourceModel, DestinationModel>();

            MapSession();
            MapTrainer();
            MapMember();
            MapPlan();

            MapMembership();
            MapBooking();

        }

        private void MapSession()
        {
            //CreateMap<Session, SessionViewModel>().ReverseMap();
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.TrainerName, opt => opt.MapFrom(src => src.Trainer.Name))
                .ForMember(dest => dest.AvailableSlots, opt => opt.Ignore()).ReverseMap();

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();

        }

        private void MapTrainer()
        {

            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForPath(d => d.Address.City,
                         opt => opt.MapFrom(s => s.City))
                .ForPath(d => d.Address.Street,
                         opt => opt.MapFrom(s => s.Street))
                .ForPath(d => d.Address.BuildingNumber,
                         opt => opt.MapFrom(s => s.BuildingNumber));

            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(d => d.Address,
                    opt => opt.MapFrom(s =>
                        $"{s.Address.City}, {s.Address.Street}, {s.Address.BuildingNumber}"))
                .ForMember(d => d.DateOfBirth,
                    opt => opt.MapFrom(s => s.DateOfBirth.ToString()))
                .ForMember(d => d.Gender,
                    opt => opt.MapFrom(s => s.Gender.ToString()))
                .ForMember(d => d.Specialty,
                    opt => opt.MapFrom(s => s.Specialty.ToString()));

            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ReverseMap();

        }

        private void MapMember()
        {
            CreateMap<CreateMemberViewModel, Member>()
                .ForPath(d => d.Address.City,
                    opt => opt.MapFrom(s => s.City))

                .ForPath(d => d.Address.Street,
                    opt => opt.MapFrom(s => s.Street))

                .ForPath(d => d.Address.BuildingNumber,
                    opt => opt.MapFrom(s => s.BuildingNumber))

                .ForPath(d => d.HealthRecord.Height,
                    opt => opt.MapFrom(s => s.HealthRecordViewModel.Height))

                .ForPath(d => d.HealthRecord.Weight,
                    opt => opt.MapFrom(s => s.HealthRecordViewModel.Weight))

                .ForPath(d => d.HealthRecord.BloodType,
                    opt => opt.MapFrom(s => s.HealthRecordViewModel.BloodType))

                .ForPath(d => d.HealthRecord.Note,
                    opt => opt.MapFrom(s => s.HealthRecordViewModel.Note));

            CreateMap<Member, MemberViewModel>()
                .ForMember(d => d.DateOfBirth,
                    opt => opt.MapFrom(s =>
                        s.DateOfBirth.ToString("dd/MM/yyyy")))

                .ForMember(d => d.Gender,
                    opt => opt.MapFrom(s =>
                        s.Gender.ToString()))

                .ForMember(d => d.Address,
                    opt => opt.MapFrom(s =>
                        $"{s.Address.BuildingNumber}, {s.Address.Street}, {s.Address.City}"));

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(d => d.City,
                    opt => opt.MapFrom(s => s.Address.City))

                .ForMember(d => d.Street,
                    opt => opt.MapFrom(s => s.Address.Street))

                .ForMember(d => d.BuildingNumber,
                    opt => opt.MapFrom(s => s.Address.BuildingNumber))
                .ReverseMap()
                .ForPath(d => d.Address.City,
                    opt => opt.MapFrom(s => s.City))

                .ForPath(d => d.Address.Street,
                    opt => opt.MapFrom(s => s.Street))

                .ForPath(d => d.Address.BuildingNumber,
                    opt => opt.MapFrom(s => s.BuildingNumber));

            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();


        }

        private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>().ReverseMap();
            CreateMap<Plan, UpdatePlanViewModel>().ReverseMap();
        }

        private void MapMembership()
        {
            CreateMap<Plan, PlanSelectListViewModel>();

            CreateMap<Member, MemberSelectListViewModel>();

            CreateMap<CreateMemberShipViewModel, Membership>();

            CreateMap<Membership, MemberShipViewModel>()
                .ForMember(d => d.MemberName,
                    opt => opt.MapFrom(s => s.Member.Name))
                .ForMember(d => d.PlanName,
                    opt => opt.MapFrom(s => s.Plan.Name))
                .ForMember(d => d.StartDate,
                    opt => opt.MapFrom(s => s.CreatedAt));

            CreateMap<Membership, MemberShipForMemberViewModel>()
                .ForMember(d => d.MemberName,
                    opt => opt.MapFrom(s => s.Member.Name))
                .ForMember(d => d.PlanName,
                    opt => opt.MapFrom(s => s.Plan.Name))
                .ForMember(d => d.StartDate,
                    opt => opt.MapFrom(s => s.CreatedAt))
                .ForMember(d => d.Status,
                    opt => opt.MapFrom(s => s.Status));

        }

        private void MapBooking()
        {
            CreateMap<CreateBookingViewModel, Booking>();

            CreateMap<Booking, MemberForSessionViewModel>()
                .ForMember(d => d.MemberName,
                    opt => opt.MapFrom(s => s.Member.Name))
                .ForMember(d => d.BookingDate,
                    opt => opt.MapFrom(s => s.CreatedAt.ToString("dd/MM/yyyy")));
        }

    }
}
