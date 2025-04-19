using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Users;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.DTOs;

namespace Ambev.DeveloperEvaluation.Application.Mappings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateUserRequest, User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

            CreateMap<UpdateUserRequest, User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

            CreateMap<User, UserResponse>();

        }
    }
}
