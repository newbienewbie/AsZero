using AsZero.Core.Entities;

namespace AsZero.Core.Services.Auth
{
    public class CreateUserInput
    {
        public string Account { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserStatus Status { get; set; }
        public string Email { get; set; }
    }

}
