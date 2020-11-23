using Application.Activities.DTOs;
using AutoMapper;
using Domain;

namespace Application.Activities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Activity, ActivityDto>();
            CreateMap<UserActivity, AttendeeDto>()
                .ForMember(dst => dst.Username, opt => opt.MapFrom(src => src.AppUser.UserName))
                .ForMember(dst => dst.DisplayName, opt => opt.MapFrom(src => src.AppUser.DisplayName));
        }
    }
}