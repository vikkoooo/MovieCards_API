﻿namespace MovieCards_API.Model.Entities
{
	public class Genre
	{
		// Props
		public int Id { get; set; }
		public string Name { get; set; }

		// Relationships
		public ICollection<Movie> Movies { get; set; } // many to many with movie
	}
}
