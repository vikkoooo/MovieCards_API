using System.ComponentModel.DataAnnotations;

namespace MovieCards_API.Model.DTO
{
	public record ActorDTO
	{
		[Required(ErrorMessage = "Name is a required field.")]
		[MaxLength(100, ErrorMessage = "Maximum length for the Name is 100 characters.")]
		public string Name { get; set; } = string.Empty; // non nullable
		[Required(ErrorMessage = "Date of Birth is a required field.")]
		public DateTime DateOfBirth { get; set; }
	}
}
