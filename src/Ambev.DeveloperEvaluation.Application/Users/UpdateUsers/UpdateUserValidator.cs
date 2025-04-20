using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUsers
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Username).NotEmpty().MinimumLength(3);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Phone).NotEmpty().MinimumLength(10);
            RuleFor(x => x.Role).IsInEnum();
            RuleFor(x => x.Status).IsInEnum();
        }
    }
}
