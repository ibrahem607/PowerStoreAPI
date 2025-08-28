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
            // Map DTO to Entity (for Create/Update)
            CreateMap<CreateMainAreaDto, MainArea>();
            CreateMap<UpdateMainAreaDto, MainArea>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // Ignore nulls on update

            // Map Entity to DTO (for Get)
            CreateMap<MainArea, MainAreaResponseDto>();
        }
    }
}
