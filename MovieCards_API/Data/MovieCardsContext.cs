using Microsoft.EntityFrameworkCore;
using MovieCards_API.Model.Entities;

namespace MovieCards_API.Data
{
	public class MovieCardsContext : DbContext
	{
		public MovieCardsContext(DbContextOptions<MovieCardsContext> options) : base(options)
		{
		}

		public DbSet<Movie> Movie { get; set; }
		public DbSet<Director> Director { get; set; }
		public DbSet<Actor> Actor { get; set; }
		public DbSet<Genre> Genre { get; set; }
	}
}
