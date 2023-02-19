using AutoMapper;
using Web_Triolingo.ModelDto;
using Web_Triolingo.Model;

namespace Web_Triolingo.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<Triolingo.Core.Entity.Setting, SettingDto>();
            CreateMap<Lesson, LessonDto>();
            CreateMap<User, UserDto>();
        }
    }
}
