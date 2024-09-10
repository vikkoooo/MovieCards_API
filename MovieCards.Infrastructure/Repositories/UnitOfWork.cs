using MovieCards.Domain.Contracts;
using MovieCards.Infrastructure.Data;

namespace MovieCards.Infrastructure.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly MovieCardsContext context;

		public UnitOfWork(MovieCardsContext context)
		{
			this.context = context;
			Movies = new MovieRepository(this.context);
			Directors = new DirectorRepository(this.context);
			Actors = new ActorRepository(this.context);
		}

		public IMovieRepository Movies { get; private set; }
		public IDirectorRepository Directors { get; private set; }
		public IActorRepository Actors { get; private set; }

		public async Task<int> CompleteAsync()
		{
			return await context.SaveChangesAsync();
		}

		public void Dispose()
		{
			context.Dispose();
		}
	}
}
