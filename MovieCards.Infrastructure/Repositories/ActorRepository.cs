using Microsoft.EntityFrameworkCore;
using MovieCards.Domain.Entities;
using MovieCards.Infrastructure.Data;

namespace MovieCards.Infrastructure.Repositories
{
	public class ActorRepository
	{
		private readonly MovieCardsContext db;

		public ActorRepository(MovieCardsContext db)
		{
			this.db = db;
		}

		public async Task<List<Actor>> GetActorsByIdsAsync(IEnumerable<int> actorIds)
		{
			return await db.Actor.Where(a => actorIds.Contains(a.Id)).ToListAsync();
		}

		public async Task<IEnumerable<Actor>> GetAllActorsAsync()
		{
			return await db.Actor.ToListAsync();
		}

		public async Task<Actor?> GetActorByIdAsync(int id)
		{
			return await db.Actor.FindAsync(id);
		}

		public async Task AddActorAsync(Actor actor)
		{
			await db.Actor.AddAsync(actor);
		}

		public void UpdateActor(Actor actor)
		{
			db.Actor.Update(actor);
		}

		public void DeleteActor(Actor actor)
		{
			db.Actor.Remove(actor);
		}

		public async Task<bool> ActorExistsAsync(int id)
		{
			return await db.Actor.AnyAsync(a => a.Id == id);
		}
	}
}
