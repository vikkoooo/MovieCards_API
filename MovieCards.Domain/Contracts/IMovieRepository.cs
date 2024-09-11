using MovieCards.Domain.Entities;

namespace MovieCards.Infrastructure.Repositories
{
	public interface IMovieRepository
	{
		IQueryable<Movie> GetAllMovies();
		Task<Movie?> GetByIdAsync(int id);
		Task<Movie?> GetByTitleAsync(string title);
		Task AddAsync(Movie movie);
		void DeleteMovie(Movie movie);
		Task SaveChangesAsync();
	}
}