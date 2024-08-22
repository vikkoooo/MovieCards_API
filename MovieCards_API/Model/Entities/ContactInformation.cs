namespace MovieCards_API.Model.Entities
{
	public class ContactInformation
	{
		// Props
		public int Id { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }

		// Relationships
		public int DirectorId { get; set; } // one to one with Director
		public Director Director { get; set; } // one to one with Director
	}
}
