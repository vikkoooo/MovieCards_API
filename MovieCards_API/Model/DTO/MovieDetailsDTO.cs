namespace MovieCards_API.Model.DTO
{
	public record MovieDetailsDTO
	{
		// movie
		public string Title { get; init; }
		public int Rating { get; init; }
		public DateTime ReleaseDate { get; init; }
		public string Description { get; init; }

		// director
		public string DirectorName { get; init; }
		public DateTime DirectorDateOfBirth { get; init; }

		// director contact info
		public string DirectorEmail { get; init; }
		public string DirectorPhoneNumber { get; init; }

		// actors & genres
		public ICollection<string> ActorNames { get; init; }
		public ICollection<string> GenreNames { get; init; }
	}
}
