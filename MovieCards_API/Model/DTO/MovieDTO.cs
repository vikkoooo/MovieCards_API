namespace MovieCards_API.Model
{
	public class MovieDTO
	{
		public string Title { get; set; }
		public int Rating { get; set; }
		public DateTime ReleaseDate { get; set; }
		public string Description { get; set; }
		public string DirectorName { get; set; }
		public ICollection<string> ActorNames { get; set; }
		public ICollection<string> GenreNames { get; set; }
	}
}
