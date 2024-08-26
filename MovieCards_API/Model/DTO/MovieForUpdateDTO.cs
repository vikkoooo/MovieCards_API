namespace MovieCards_API.Model.DTO
{
	public class MovieForUpdateDTO
	{
		public string? Title { get; set; }
		public int Rating { get; set; }
		public string? ReleaseDate { get; set; }
		public string? Description { get; set; }
		public int DirectorId { get; set; }
		public ICollection<int>? ActorIds { get; set; }
		public ICollection<int>? GenreIds { get; set; }
	}
}
