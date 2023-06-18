using Domain.Models.Users;

namespace MainEndpoint.Models
{
    public class RegisterModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GoogleAuthCode { get; set; }
        public UserGender Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
    }
}
