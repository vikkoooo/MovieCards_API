using System.ComponentModel.DataAnnotations;

namespace MovieCards_API.Model.DTO
{
	public class MovieForCreationDTO
	{
		[Required]
		public string Title { get; set; }
		[Required]
		public int Rating { get; set; }
		[Required]
		public string ReleaseDate { get; set; }
		[Required]
		public string Description { get; set; }
		[Required]
		public int DirectorId { get; set; }
		[Required]
		public ICollection<int> ActorIds { get; set; }
		[Required]
		public ICollection<int> GenreIds { get; set; }
	}
}
