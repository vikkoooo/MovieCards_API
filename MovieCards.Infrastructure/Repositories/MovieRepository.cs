using Microsoft.EntityFrameworkCore;
using MovieCards.Domain.Entities;
using MovieCards.Infrastructure.Data;

namespace MovieCards.Infrastructure.Repositories
{
	public class MovieRepository
	{
		private readonly MovieCardsContext db;

		public MovieRepository(MovieCardsContext db)
		{
			this.db = db;
		}

		public IQueryable<Movie> GetAllMovies()
		{
			return db.Movie.AsQueryable();
		}

		public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
		{
			return await db.Movie
				.Include(m => m.Director)
				.Include(m => m.Actors)
				.Include(m => m.Genres)
				.ToListAsync();
		}

		public async Task<Movie?> GetMovieByIdAsync(int id)
		{
			return await db.Movie
				.Include(m => m.Director)
				.Include(m => m.Actors)
				.Include(m => m.Genres)
				.FirstOrDefaultAsync(m => m.Id == id);
		}

		public async Task<Movie?> GetByIdAsync(int id)
		{
			return await db.Movie
				.Include(m => m.Director)
				.Include(m => m.Actors)
				.Include(m => m.Genres)
				.FirstOrDefaultAsync(m => m.Id == id);
		}

		public async Task<Movie?> GetByTitleAsync(string title)
		{
			return await db.Movie.FirstOrDefaultAsync(m => m.Title == title);
		}

		public async Task AddMovieAsync(Movie movie)
		{
			await db.Movie.AddAsync(movie);
		}

		public async Task AddAsync(Movie movie)
		{
			await db.Movie.AddAsync(movie);
		}

		public void UpdateMovie(Movie movie)
		{
			db.Movie.Update(movie);
		}

		public void DeleteMovie(Movie movie)
		{
			db.Movie.Remove(movie);
		}

		public async Task<bool> MovieExistsAsync(int id)
		{
			return await db.Movie.AnyAsync(m => m.Id == id);
		}

		public async Task SaveChangesAsync()
		{
			await db.SaveChangesAsync();
		}
	}
}
