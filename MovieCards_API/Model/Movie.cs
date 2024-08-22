namespace MovieCards_API.Model
{
	public class Movie
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public int Rating { get; set; }
		public DateTime ReleaseDate { get; set; }
		public string Description { get; set; }
	}
}
