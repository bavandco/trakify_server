﻿using System.ComponentModel.DataAnnotations;

namespace MainEndpoint.Models
{
    public class NoteUpdateDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Text { get; set; }

        [Range(0, 100)]
        public int Happiness { get; set; }

        [Range(0, 100)]
        public int Satisfaction { get; set; }

        [Range(0, 100)]
        public int Health { get; set; }
    }
}