namespace MovieCards_API.Model.DTO
{
	public class MovieDetailsDTO
	{
		// movie
		public string Title { get; set; }
		public int Rating { get; set; }
		public DateTime ReleaseDate { get; set; }
		public string Description { get; set; }

		// director
		public string DirectorName { get; set; }
		public DateTime DirectorDateOfBirth { get; set; }

		// director contact info
		public string DirectorEmail { get; set; }
		public string DirectorPhoneNumber { get; set; }

		// actors & genres
		public ICollection<string> ActorNames { get; set; }
		public ICollection<string> GenreNames { get; set; }
	}
}
