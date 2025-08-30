using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PowerStore.Core.DTOs.MainAreaDtos;
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
            CreateMap<MainArea, SubAreaResponseDto>();
        }
    }
}
