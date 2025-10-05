using AutoMapper;

namespace Kykyemeklistesi.Models.UserViewModel
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
             
            CreateMap<RegisterModel, User>();
        }
    }
}
