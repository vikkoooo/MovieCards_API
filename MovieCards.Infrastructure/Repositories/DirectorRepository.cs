using Microsoft.EntityFrameworkCore;
using MovieCards.Domain.Entities;
using MovieCards.Infrastructure.Data;

namespace MovieCards.Infrastructure.Repositories
{
	public class DirectorRepository
	{
		private readonly MovieCardsContext db;

		public DirectorRepository(MovieCardsContext db)
		{
			this.db = db;
		}

		public async Task<IEnumerable<Director>> GetAllDirectorsAsync()
		{
			return await db.Director.ToListAsync();
		}

		public async Task<Director?> GetDirectorByIdAsync(int id)
		{
			return await db.Director.FindAsync(id);
		}

		public async Task<Director?> GetByIdAsync(int id)
		{
			return await db.Director.FirstOrDefaultAsync(d => d.Id == id);
		}

		public async Task AddDirectorAsync(Director director)
		{
			await db.Director.AddAsync(director);
		}

		public void UpdateDirector(Director director)
		{
			db.Director.Update(director);
		}

		public void DeleteDirector(Director director)
		{
			db.Director.Remove(director);
		}

		public async Task<bool> DirectorExistsAsync(int id)
		{
			return await db.Director.AnyAsync(d => d.Id == id);
		}
	}
}
