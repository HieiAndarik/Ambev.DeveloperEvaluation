using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUsers;

/// <summary>
/// Configuração do AutoMapper para mapeamento de usuário
/// </summary>
public sealed class GetUsersProfile : Profile
{
    public GetUsersProfile()
    {
        CreateMap<User, UserDto>();
    }
}