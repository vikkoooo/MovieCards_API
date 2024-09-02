using System.ComponentModel.DataAnnotations;

namespace MovieCards_API.Model.Entities
{
	public class Genre
	{
		// Props
		public int Id { get; set; }
		[Required(ErrorMessage = "Name is a required field.")]
		[MaxLength(50, ErrorMessage = "Maximum length for the Name is 50 characters.")]
		public string Name { get; set; } = string.Empty; // non nullable

		// Relationships
		public ICollection<Movie> Movies { get; set; } = new List<Movie>(); // many to many with movie (non nullable)
	}
}
