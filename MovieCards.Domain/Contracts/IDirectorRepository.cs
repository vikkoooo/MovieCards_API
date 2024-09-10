using MovieCards.Domain.Entities;

namespace MovieCards.Domain.Contracts
{
	public interface IDirectorRepository
	{
		Task<IEnumerable<Director>> GetAllDirectorsAsync();
		Task<Director?> GetDirectorByIdAsync(int id);
		Task AddDirectorAsync(Director director);
		void UpdateDirector(Director director);
		void DeleteDirector(Director director);
		Task<bool> DirectorExistsAsync(int id);
	}
}
