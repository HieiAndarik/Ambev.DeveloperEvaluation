using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth;
using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Ambev.DeveloperEvaluation.Application.Auth;


namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<AuthenticateUserResult, AuthenticateUserResponse>();
    }
}
