namespace MovieCards_API.Model.DTO
{
	public record MovieDTO
	{
		public string Title { get; init; }
		public int Rating { get; init; }
		public DateTime ReleaseDate { get; init; }
		public string Description { get; init; }
		public string DirectorName { get; init; }
		public ICollection<string> ActorNames { get; init; }
		public ICollection<string> GenreNames { get; init; }
	}
}
