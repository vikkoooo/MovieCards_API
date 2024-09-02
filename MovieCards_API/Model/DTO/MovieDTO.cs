namespace MovieCards_API.Model.DTO
{
	public record MovieDTO
	{
		public string Title { get; init; } = string.Empty; // non nullable
		public int Rating { get; init; }
		public DateTime ReleaseDate { get; init; }
		public string? Description { get; init; } // nullable
		public string DirectorName { get; init; } = string.Empty; // non nullable
		public ICollection<string> ActorNames { get; init; } = new List<string>(); // non nullable
		public ICollection<string> GenreNames { get; init; } = new List<string>(); // non nullable
	}
}
