using AutoMapper;
using Ordering.Application.DTOs;
using Ordering.Domain.Entities;

namespace Ordering.Application.AutoMapper
{
    public class AppMapperProfile : Profile
    {
        public AppMapperProfile()
        {
            CreateMap<CreateOrderDto, Order>();
        }
    }
}