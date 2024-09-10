namespace MovieCards.Domain.Contracts
{
	public interface IUnitOfWork : IDisposable
	{
		IMovieRepository Movies { get; }
		IDirectorRepository Directors { get; }
		IActorRepository Actors { get; }
		Task<int> CompleteAsync();
	}
}
