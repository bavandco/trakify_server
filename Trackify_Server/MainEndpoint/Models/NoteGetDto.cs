using System.ComponentModel.DataAnnotations;

namespace MainEndpoint.Models
{
    public class NoteGetDto
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string Title { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [Range(0, 100)]
        public int? Happiness { get; set; }

        [Range(0, 100)]
        public int? Satisfaction { get; set; }

        [Range(0, 100)]
        public int? Health { get; set; }
    }
}
