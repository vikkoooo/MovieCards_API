namespace MovieCards_API.Model.DTO
{
	public record MovieDetailsDTO
	{
		// movie
		public string Title { get; init; } = string.Empty; // non nullable
		public int Rating { get; init; }
		public DateTime ReleaseDate { get; init; }
		public string? Description { get; init; } // nullable

		// director
		public string DirectorName { get; init; } = string.Empty; // non nullable
		public DateTime DirectorDateOfBirth { get; init; }

		// director contact info
		public string DirectorEmail { get; init; } = string.Empty; // non nullable
		public string DirectorPhoneNumber { get; init; } = string.Empty; // non nullable

		// actors & genres
		public ICollection<string> ActorNames { get; init; } = new List<string>(); // non nullable
		public ICollection<string> GenreNames { get; init; } = new List<string>(); // non nullable
	}
}
