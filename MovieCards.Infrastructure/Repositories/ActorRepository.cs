using Microsoft.EntityFrameworkCore;
using MovieCards.Domain.Entities;
using MovieCards.Infrastructure.Data;

namespace MovieCards.Infrastructure.Repositories
{
	public class ActorRepository : IActorRepository
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
	}
}
