using AutoMapper;
using TheCompleteProject.ModelsAndDto_s.Authentication;
using TheCompleteProject.ModelsAndDto_s.DbModels;
using TheCompleteProject.ModelsAndDto_s.Dtos;

namespace TheCompleteProject.Service.MappingProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Users, UserDtos>().ReverseMap();
            CreateMap<Users, LoginRequestDto>().ReverseMap();
        }
    }
}
