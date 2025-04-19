using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUsers;

public sealed class GetUsersHandler : IRequestHandler<GetUsersQuery, GetUsersResult>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public GetUsersHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<GetUsersResult> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var allUsers = await _repository.GetAllAsync(cancellationToken);

        var ordered = request.Order?.ToLower() switch
        {
            "username" => allUsers.OrderBy(u => u.Username),
            "email" => allUsers.OrderBy(u => u.Email),
            _ => allUsers
        };

        var paged = ordered
            .Skip((request.Page - 1) * request.Size)
            .Take(request.Size)
            .ToList();

        return new GetUsersResult
        {
            Users = _mapper.Map<IEnumerable<UserDto>>(paged),
            TotalCount = allUsers.Count()
        };
    }
}