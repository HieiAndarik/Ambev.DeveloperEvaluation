using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Users
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Id = Guid.Parse(command.Id),
                Username = command.Username,
                Email = command.Email,
                Password = command.Password,
                Role = command.Role,
                Phone = command.Phone,
                Status = command.Status
            };

            return await _userRepository.UpdateAsync(user);
        }
    }
}
