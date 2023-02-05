using AutoMapper;
using Web_Triolingo.ModelDto;
using Web_Triolingo.Models;

namespace Web_Triolingo.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Setting, SettingDto>();
        }
    }
}
