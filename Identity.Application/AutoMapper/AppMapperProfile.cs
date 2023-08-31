using AutoMapper;
using Identity.Application.DTOs;
using Identity.Domain.Entities;
using OnlineStore.Shared;

namespace Identity.Application.AutoMapper
{
    public class AppMapperProfile : Profile
    {
        public AppMapperProfile()
        {
            CreateMap<RegisterUserDto, ApplicationUser>();
            CreateMap<ApplicationUser, DeleteUserMessageDto>();
        }
    }
}