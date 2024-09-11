using Microsoft.EntityFrameworkCore;
using MovieCards.Domain.Entities;
using MovieCards.Infrastructure.Data;

namespace MovieCards.Infrastructure.Repositories
{
	public class GenreRepository
	{
		private readonly MovieCardsContext db;

		public GenreRepository(MovieCardsContext db)
		{
			this.db = db;
		}

		// Get genres by list of IDs
		public async Task<List<Genre>> GetGenresByIdsAsync(IEnumerable<int> genreIds)
		{
			return await db.Genre.Where(g => genreIds.Contains(g.Id)).ToListAsync();
		}
	}
}
