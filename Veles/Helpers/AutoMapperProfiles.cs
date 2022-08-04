using AutoMapper;
using VelesLibrary.DbModels;
using VelesLibrary.DTOs;

namespace VelesAPI.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<User, TokenDto>();
        CreateMap<Message, NewMessageDto>()
            .ForMember(dest => dest.User,
                opt => opt
                    .MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.Group,
                opt => opt
                    .MapFrom(src => src.Group.Name));
        CreateMap<User, UserDto>();
    }
}
