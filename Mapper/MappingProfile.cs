using AutoMapper;
using Ecommerse_shoes_backend.Data.Dto;
using Ecommerse_shoes_backend.Data.Models;

namespace Ecommerse_shoes_backend.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User,UserDto>().ReverseMap();
            CreateMap<User,AdminDto>().ReverseMap();
        }
    }
}
