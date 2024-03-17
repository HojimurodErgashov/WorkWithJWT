using AutoMapper;
using Entities.DTO.User;
using Entities.Model;

namespace CrudLearn.Profilies
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User , UserDTO>();
            CreateMap<UserCreateDTO , User>();
            CreateMap<UserDTO, User>();
        }
    }
}
