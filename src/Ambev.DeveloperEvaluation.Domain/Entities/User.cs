using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserStatus Status { get; set; } 
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public void Activate()
        {
            Status = UserStatus.Active;
        }
        public void Suspend()
        {
            Status = UserStatus.Suspended;
        }
        public (bool IsValid, List<string> Errors) Validate()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(Username))
                errors.Add("Username is required");

            if (string.IsNullOrWhiteSpace(Password))
                errors.Add("Password is required");

            if (string.IsNullOrWhiteSpace(Email))
                errors.Add("Email is required");

            return (errors.Count == 0, errors);
        }
    }
}
