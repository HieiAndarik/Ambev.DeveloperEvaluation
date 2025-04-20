using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Users;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;

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

            CreateMap<AuthenticateUserRequest, AuthenticateUserCommand>();

        }
    }
}
