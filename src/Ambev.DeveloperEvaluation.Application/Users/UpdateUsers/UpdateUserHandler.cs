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
            var user = await _userRepository.GetByIdAsync(command.Id, cancellationToken);
            if (user == null) return false;

            user.FirstName = command.Firstname;
            user.LastName = command.Lastname;
            user.Username = command.Username;
            user.Email = command.Email;
            user.Password = command.Password;
            user.Role = command.Role;
            user.Phone = command.Phone;
            user.Status = command.Status;

            return await _userRepository.UpdateAsync(user);
        }
    }
}
