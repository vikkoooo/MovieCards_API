using Microsoft.EntityFrameworkCore;
using MovieCards.Domain.Contracts;
using MovieCards.Domain.Entities;
using MovieCards.Infrastructure.Data;

namespace MovieCards.Infrastructure.Repositories
{
	public class MovieRepository : IMovieRepository
	{
		private readonly MovieCardsContext context;

		public MovieRepository(MovieCardsContext context)
		{
			this.context = context;
		}

		public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
		{
			return await context.Movie
				.Include(m => m.Director)
				.Include(m => m.Actors)
				.Include(m => m.Genres)
				.ToListAsync();
		}

		public async Task<Movie?> GetMovieByIdAsync(int id)
		{
			return await context.Movie
				.Include(m => m.Director)
				.Include(m => m.Actors)
				.Include(m => m.Genres)
				.FirstOrDefaultAsync(m => m.Id == id);
		}

		public async Task AddMovieAsync(Movie movie)
		{
			await context.Movie.AddAsync(movie);
		}

		public void UpdateMovie(Movie movie)
		{
			context.Movie.Update(movie);
		}

		public void DeleteMovie(Movie movie)
		{
			context.Movie.Remove(movie);
		}

		public async Task<bool> MovieExistsAsync(int id)
		{
			return await context.Movie.AnyAsync(m => m.Id == id);
		}
	}
}
