using Microsoft.EntityFrameworkCore;

namespace MovieCards_API.Data
{
	public class MovieCardsContext : DbContext
	{
		public MovieCardsContext(DbContextOptions<MovieCardsContext> options) : base(options)
		{
		}

		public DbSet<MovieCards_API.Model.Movie> Movie { get; set; } = default!;
	}
}
