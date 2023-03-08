namespace Domain.Models.Users
{
    public class UserProfilePicture
    {
        public Guid Id { get; set; }
        public string Src { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
    }
}
