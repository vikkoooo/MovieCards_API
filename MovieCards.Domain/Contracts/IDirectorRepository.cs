using MovieCards.Domain.Entities;

namespace MovieCards.Infrastructure.Repositories
{
	public interface IDirectorRepository
	{
		Task<Director?> GetByIdAsync(int id);
	}
}