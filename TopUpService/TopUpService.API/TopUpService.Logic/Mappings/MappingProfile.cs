using AutoMapper;
using TopUpService.Database.Entities;
using TopUpService.DTO.Request;
using TopUpService.DTO.Response;

namespace TopUpService.Logic.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TopUpRequest, TopUpTransaction>();
            CreateMap<AddBeneficiaryRequest, Beneficiary>();
            CreateMap<Beneficiary, BeneficiaryDetails>();
        }
    }
}
