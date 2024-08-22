using Microsoft.EntityFrameworkCore;
using MovieCards_API.Model.Entities;

namespace MovieCards_API.Data
{
	public class MovieCardsContext : DbContext
	{
		public MovieCardsContext(DbContextOptions<MovieCardsContext> options) : base(options)
		{
		}

		public DbSet<Movie> Movie { get; set; } = default!;
	}
}
