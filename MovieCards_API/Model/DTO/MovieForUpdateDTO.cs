namespace MovieCards_API.Model.DTO
{
	public record MovieForUpdateDTO
	{
		public int Id { get; init; }
		public string? Title { get; init; }
		public int Rating { get; init; }
		public string? ReleaseDate { get; init; }
		public string? Description { get; init; }
		public int DirectorId { get; init; }
		public ICollection<int>? ActorIds { get; init; }
		public ICollection<int>? GenreIds { get; init; }
	}
}
