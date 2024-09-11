using Microsoft.EntityFrameworkCore;
using MovieCards.Domain.Entities;
using MovieCards.Infrastructure.Data;

namespace MovieCards.Infrastructure.Repositories
{
	public class DirectorRepository : IDirectorRepository
	{
		private readonly MovieCardsContext db;

		public DirectorRepository(MovieCardsContext db)
		{
			this.db = db;
		}

		public async Task<Director?> GetByIdAsync(int id)
		{
			return await db.Director.FirstOrDefaultAsync(d => d.Id == id);
		}
	}
}
