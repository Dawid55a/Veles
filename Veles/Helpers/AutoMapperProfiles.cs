using AutoMapper;
using VelesLibrary.DbModels;
using VelesLibrary.DTOs;

namespace VelesAPI.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<User, UserDto>();
    }
}
