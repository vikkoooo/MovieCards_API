using Microsoft.EntityFrameworkCore;
using MovieCards.Domain.Contracts;
using MovieCards.Domain.Entities;
using MovieCards.Infrastructure.Data;

namespace MovieCards.Infrastructure.Repositories
{
	public class ActorRepository : IActorRepository
	{
		private readonly MovieCardsContext context;

		public ActorRepository(MovieCardsContext context)
		{
			this.context = context;
		}

		public async Task<IEnumerable<Actor>> GetAllActorsAsync()
		{
			return await context.Actor.ToListAsync();
		}

		public async Task<Actor?> GetActorByIdAsync(int id)
		{
			return await context.Actor.FindAsync(id);
		}

		public async Task AddActorAsync(Actor actor)
		{
			await context.Actor.AddAsync(actor);
		}

		public void UpdateActor(Actor actor)
		{
			context.Actor.Update(actor);
		}

		public void DeleteActor(Actor actor)
		{
			context.Actor.Remove(actor);
		}

		public async Task<bool> ActorExistsAsync(int id)
		{
			return await context.Actor.AnyAsync(a => a.Id == id);
		}
	}
}
