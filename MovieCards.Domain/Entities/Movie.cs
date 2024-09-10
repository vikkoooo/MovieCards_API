using System.ComponentModel.DataAnnotations;

namespace MovieCards.Domain.Entities
{
    public class Movie
    {
        // Props
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is a required field.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the Title is 100 characters.")]
        public string Title { get; set; } = string.Empty; // non nullable
        [Range(0, 10, ErrorMessage = "Rating must be between 0 and 10.")]
        public int Rating { get; set; }
        [Required(ErrorMessage = "Release date is a required field.")]
        public DateTime ReleaseDate { get; set; }
        [MaxLength(1000, ErrorMessage = "Maximum length for the Description is 1000 characters.")]
        public string? Description { get; set; } // nullable

        // Relationships
        public int DirectorId { get; set; } // one to many with director (foreign key)
        public Director Director { get; set; } = null!; // one to many with director (non nullable)
        public ICollection<Actor> Actors { get; set; } = new List<Actor>(); // many to many with actor (non nullable)
        public ICollection<Genre> Genres { get; set; } = new List<Genre>(); // many to many with genre (non nullable)
    }
}
