using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.Domain.Entities.Mongo;

namespace Catalog.Application.AutoMapper
{
    public class AppMapperProfile : Profile
    {
        public AppMapperProfile()
        {
            CreateMap<InputCategoryDto, Category>();
            CreateMap<Category, OutputCategoryDto>();
            CreateMap<InputProductDto, Product>();
            CreateMap<Product, OutputProductDto>();
            CreateMap<CreateReportDto, Report>();
        }
    }
} 