namespace Domain.Models.Users
{
    public class UserToken
    {
        public int Id { get; set; }
        public string TokenHash { get; set; }
        public DateTime TokenExp { get; set; }
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExp { get; set; }
    }
}
