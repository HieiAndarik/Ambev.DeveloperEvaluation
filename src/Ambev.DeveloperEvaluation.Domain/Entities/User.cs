using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class User
    {
        public User(string id, string username, string password, UserStatus status, string email, string phone, UserRole role)
        {
            Id = id;
            Username = username;
            Password = password;
            Status = status;
            Email = email;
            Phone = phone;
            Role = role;
        }
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserStatus Status { get; set; } 
        public string Email { get; set; }
        public string Phone { get; set; }
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
