using MovieCards.Domain.Entities;

namespace MovieCards.Domain.Contracts
{
	public interface IMovieRepository
	{
		Task<IEnumerable<Movie>> GetAllMoviesAsync();
		Task<Movie?> GetMovieByIdAsync(int id);
		Task AddMovieAsync(Movie movie);
		void UpdateMovie(Movie movie);
		void DeleteMovie(Movie movie);
		Task<bool> MovieExistsAsync(int id);
	}
}
