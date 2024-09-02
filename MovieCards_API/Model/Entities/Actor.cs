using System.ComponentModel.DataAnnotations;

namespace MovieCards_API.Model.Entities
{
	public class Actor
	{
		// Props
		public int Id { get; set; }
		[Required(ErrorMessage = "Name is a required field.")]
		[MaxLength(100, ErrorMessage = "Maximum length for the Name is 100 characters.")]
		public string Name { get; set; } = string.Empty; // non nullable
		[Required(ErrorMessage = "Date of Birth is a required field.")]
		public DateTime DateOfBirth { get; set; }

		// Relationships
		public ICollection<Movie> Movies { get; set; } = new List<Movie>(); // many to many with movie (non nullable)
	}
}
