using System.ComponentModel.DataAnnotations;

namespace MainEndpoint.Models
{
    public class ForgetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
