using Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Notes
{
    public class Note
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Text { get; set; }
        public User User { get; set; }
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
