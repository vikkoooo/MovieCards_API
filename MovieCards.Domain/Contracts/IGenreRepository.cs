using MovieCards.Domain.Entities;

namespace MovieCards.Infrastructure.Repositories
{
	public interface IGenreRepository
	{
		Task<List<Genre>> GetGenresByIdsAsync(IEnumerable<int> genreIds);
	}
}