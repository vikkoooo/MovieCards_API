using System.ComponentModel.DataAnnotations;

namespace MovieCards_API.Model.DTO
{
	public class MovieForPatchDTO
	{
		[MaxLength(100, ErrorMessage = "Maximum length for the Title is 100 characters.")]
		public string? Title { get; set; }
		[Range(0, 10, ErrorMessage = "Rating must be between 0 and 10.")]
		public int? Rating { get; set; }
		public DateTime? ReleaseDate { get; set; } // nullable for partial updates
		[MaxLength(1000, ErrorMessage = "Maximum length for the Description is 1000 characters.")]
		public string? Description { get; set; } // nullable
		public int? DirectorId { get; set; } // nullable, director may not always be updated
		public ICollection<int>? ActorIds { get; set; } // nullable for optional actor updates
		public ICollection<int>? GenreIds { get; set; } // nullable for optional genre updates
	}

}
