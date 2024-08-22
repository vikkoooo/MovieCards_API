namespace MovieCards_API.Model.Entities
{
	public class Movie
	{
		// Props
		public int Id { get; set; }
		public string Title { get; set; }
		public int Rating { get; set; }
		public DateTime ReleaseDate { get; set; }
		public string Description { get; set; }

		// Relationships
		public int DirectorId { get; set; }
		public Director Director { get; set; }
	}
}
