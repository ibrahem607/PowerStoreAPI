using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PowerStore.Core.DTOs.CategoryDtos;
using PowerStore.Core.DTOs.MainAreaDtos;
using PowerStore.Core.DTOs.ProductDtos;
using PowerStore.Core.DTOs.SubAreaDtos;
using PowerStore.Core.DTOs.UsersDtos;
using PowerStore.Core.Entities;

namespace PowerStore.Service.MappingProfiles
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            CreateMap<CreateSubAreaDto, SubArea>();
            CreateMap<UpdateSubAreaDto, SubArea>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<SubArea, SubAreaResponseDto>()
                .ForMember(dest => dest.MainAreaName, opt =>
                    opt.MapFrom(src => src.MainArea != null ? src.MainArea.Name : string.Empty));
            
            
            
            
            
            // Map DTO to Entity (for Create/Update)
            CreateMap<CreateMainAreaDto, MainArea>();
            CreateMap<UpdateMainAreaDto, MainArea>()
             .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // Ignore nulls on update

            // Map Entity to DTO (for Get)
            CreateMap<MainArea, MainAreaResponseDto>();

            //category
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Category, CategoryResponseDto>();
            CreateMap<Category, CategoryWithProductsDto>()
                .ForMember(dest => dest.ProductsCount, opt => opt.MapFrom(src => src.Products != null ? src.Products.Count : 0))
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products != null ? src.Products : new List<Product>()));

            CreateMap<Product, CategoryProductDto>();


            // Add to existing ServiceMappingProfile
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Product, ProductResponseDto>()
                .ForMember(dest => dest.CategoryName, opt =>
                    opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
                .ForMember(dest => dest.ProfitMargin, opt => opt.Ignore()); // Calculated in service

            CreateMap<Product, ProductDetailDto>()
                .ForMember(dest => dest.CategoryName, opt =>
                    opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
                .ForMember(dest => dest.CategoryDescription, opt =>
                    opt.MapFrom(src => src.Category != null ? $"Category: {src.Category.Name}" : string.Empty))
                .ForMember(dest => dest.ProfitMargin, opt => opt.Ignore()); // Calculated in service



            // Add to existing ServiceMappingProfile
            CreateMap<AddUserDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType));

            CreateMap<UpdateUserDto, ApplicationUser>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ApplicationUser, ReturnUserDto>();
        }

    }
}
