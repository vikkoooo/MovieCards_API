using System.ComponentModel.DataAnnotations;

namespace MovieCards_API.Model.DTO
{
	public record MovieForUpdateDTO
	{
		[Required]
		public int Id { get; init; }
		[Required]
		public string Title { get; init; }
		[Range(0, 10, ErrorMessage = "Rating must be between 0 and 10.")]
		public int Rating { get; init; }
		[Required]
		public DateTime ReleaseDate { get; init; } // non nullable
		public string? Description { get; init; } // nullable
		public int DirectorId { get; init; }
		public ICollection<int>? ActorIds { get; init; } // nullable
		public ICollection<int>? GenreIds { get; init; } // nullable
	}
}
