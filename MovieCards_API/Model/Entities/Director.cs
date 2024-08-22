namespace MovieCards_API.Model.Entities
{
	public class Director
	{
		// Props
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime DateOfBirth { get; set; }

		// Relationships
		public ICollection<Movie> Movies { get; set; } // one to many with movie
	}
}
