using Microsoft.EntityFrameworkCore;
using MovieCards.Domain.Contracts;
using MovieCards.Domain.Entities;
using MovieCards.Infrastructure.Data;

namespace MovieCards.Infrastructure.Repositories
{
	public class DirectorRepository : IDirectorRepository
	{
		private readonly MovieCardsContext context;

		public DirectorRepository(MovieCardsContext context)
		{
			this.context = context;
		}

		public async Task<IEnumerable<Director>> GetAllDirectorsAsync()
		{
			return await context.Director.ToListAsync();
		}

		public async Task<Director?> GetDirectorByIdAsync(int id)
		{
			return await context.Director.FindAsync(id);
		}

		public async Task AddDirectorAsync(Director director)
		{
			await context.Director.AddAsync(director);
		}

		public void UpdateDirector(Director director)
		{
			context.Director.Update(director);
		}

		public void DeleteDirector(Director director)
		{
			context.Director.Remove(director);
		}

		public async Task<bool> DirectorExistsAsync(int id)
		{
			return await context.Director.AnyAsync(d => d.Id == id);
		}
	}
}
