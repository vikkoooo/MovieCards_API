using System.ComponentModel.DataAnnotations;

namespace MovieCards_API.Model.DTO
{
	public record MovieForCreationDTO
	{
		[Required]
		public string Title { get; init; }
		[Required]
		public int Rating { get; init; }
		[Required]
		public string ReleaseDate { get; init; }
		[Required]
		public string Description { get; init; }
		[Required]
		public int DirectorId { get; init; }
		[Required]
		public ICollection<int> ActorIds { get; init; }
		[Required]
		public ICollection<int> GenreIds { get; init; }
	}
}
