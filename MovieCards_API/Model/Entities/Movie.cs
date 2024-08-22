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
		public int DirectorId { get; set; } // one to many with director
		public Director Director { get; set; } // one to many with director
		public ICollection<Actor> Actors { get; set; } // many to many with actor
		public ICollection<Genre> Genres { get; set; } // many to many with genre
	}
}
