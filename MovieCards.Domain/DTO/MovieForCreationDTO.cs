using System.ComponentModel.DataAnnotations;

namespace MovieCards_API.Model.DTO
{
	public record MovieForCreationDTO
	{
		[Required(ErrorMessage = "Title is a required field.")]
		[MaxLength(100, ErrorMessage = "Maximum length for the Title is 100 characters.")]
		public string Title { get; init; } = string.Empty; // non nullable
		[Range(0, 10, ErrorMessage = "Rating must be between 0 and 10.")]
		public int Rating { get; init; }
		[Required(ErrorMessage = "Release date is a required field.")]
		public DateTime ReleaseDate { get; init; }
		[MaxLength(1000, ErrorMessage = "Maximum length for the Description is 1000 characters.")]
		public string? Description { get; init; } // nullable
		[Required(ErrorMessage = "DirectorId is a required field.")]
		public int DirectorId { get; init; }
		[Required(ErrorMessage = "ActorIds are required.")]
		public ICollection<int> ActorIds { get; init; } = new List<int>(); // non nullable
		[Required(ErrorMessage = "GenreIds are required.")]
		public ICollection<int> GenreIds { get; init; } = new List<int>(); // non nullable
	}
}
