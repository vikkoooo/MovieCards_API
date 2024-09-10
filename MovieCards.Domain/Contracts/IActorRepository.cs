using MovieCards.Domain.Entities;

namespace MovieCards.Domain.Contracts
{
	public interface IActorRepository
	{
		Task<IEnumerable<Actor>> GetAllActorsAsync();
		Task<Actor?> GetActorByIdAsync(int id);
		Task AddActorAsync(Actor actor);
		void UpdateActor(Actor actor);
		void DeleteActor(Actor actor);
		Task<bool> ActorExistsAsync(int id);
	}
}
