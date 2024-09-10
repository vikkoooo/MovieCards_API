using System.ComponentModel.DataAnnotations;

namespace MovieCards_API.Model.DTO
{
	public record GenreDTO
	{
		[Required(ErrorMessage = "Name is a required field.")]
		[MaxLength(50, ErrorMessage = "Maximum length for the Name is 50 characters.")]
		public string Name { get; set; } = string.Empty; // non nullable
	}
}
