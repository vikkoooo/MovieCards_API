using MovieCards.Domain.Entities;

namespace MovieCards.Infrastructure.Repositories
{
	public interface IActorRepository
	{
		Task<List<Actor>> GetActorsByIdsAsync(IEnumerable<int> actorIds);
	}
}