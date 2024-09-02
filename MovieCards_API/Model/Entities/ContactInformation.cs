using System.ComponentModel.DataAnnotations;

namespace MovieCards_API.Model.Entities
{
	public class ContactInformation
	{
		// Props
		public int Id { get; set; }
		[Required(ErrorMessage = "Email is a required field.")]
		[EmailAddress(ErrorMessage = "Invalid Email Address format.")]
		public string Email { get; set; } = string.Empty; // non nullable
		[Phone(ErrorMessage = "Invalid Phone Number format.")]
		public string PhoneNumber { get; set; } = string.Empty; // non nullable

		// Relationships
		public int DirectorId { get; set; } // one to one with Director (foreign key)
		public Director Director { get; set; } = null!; // one to one with Director (non nullable)
	}
}
