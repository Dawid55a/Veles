using AutoMapper;
using VelesAPI.Extensions;
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
                    .MapFrom(src => src.Group.Name))
            .ForMember(dest => dest.Nick,
                opt => opt
                    .MapFrom(src =>
                        src.User.UserGroups.FirstOrDefault(ug =>
                            ug.UserId == src.User.Id && ug.GroupId == src.Group.Id)!.UserGroupNick));
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Nicks,
                opt => opt
                    .MapFrom(src => src.GetUserNicks()));
        CreateMap<Group, GroupDto>()
            .ForMember(dest => dest.Owner,
                opt => opt
                    .MapFrom(
                        src =>
                            src.UserGroups.FirstOrDefault(ug => ug.GroupId == src.Id && ug.Role == Roles.Owner)!.User
                                .UserName));
    }
}
