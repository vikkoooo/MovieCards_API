using Bogus;
using Microsoft.EntityFrameworkCore;

namespace MovieCards_API.Data
{
	public class SeedData
	{
		private static Faker faker = new Faker("sv");

		internal static async Task InitAsync(MovieCardsContext context)
		{
			// check if data already exists
			if (await context.Movie.AnyAsync())
			{
				return;
			}

			var directors = GenerateDirectors(10);
			await context.AddRangeAsync(directors);
			var actors = GenerateActors(10);
			await context.AddRangeAsync(actors);
			var genres = GenerateGenres(10);
			await context.AddRangeAsync(genres);
			var movies = GenerateMovies(20, directors, actors, genres);
			await context.AddRangeAsync(movies);

			await context.SaveChangesAsync();
		}

		private static object[] GenerateDirectors(int v)
		{
			throw new NotImplementedException();
		}

		private static object[] GenerateActors(int v)
		{
			throw new NotImplementedException();
		}

		private static object[] GenerateGenres(int v)
		{
			throw new NotImplementedException();
		}

		private static object[] GenerateMovies(int v, object[] directors, object[] actors, object[] genres)
		{
			throw new NotImplementedException();
		}
	}
}
