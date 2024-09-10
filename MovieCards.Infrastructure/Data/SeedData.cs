using Bogus;
using Microsoft.EntityFrameworkCore;
using MovieCards.Domain.Entities;
using MovieCards.Infrastructure.Data;

namespace MovieCards_API.Data
{
	public class SeedData
	{
		private static Faker faker = new Faker("sv");

		public static async Task InitAsync(MovieCardsContext context)
		{
			// check if data already exists
			if (await context.Movie.AnyAsync())
			{
				return;
			}

			var directors = GenerateDirectors(5);
			await context.AddRangeAsync(directors);

			var actors = GenerateActors(5);
			await context.AddRangeAsync(actors);

			var genres = GenerateGenres(5);
			await context.AddRangeAsync(genres);

			var movies = GenerateMovies(5, directors, actors, genres);
			await context.AddRangeAsync(movies);

			await context.SaveChangesAsync();
		}

		private static List<Director> GenerateDirectors(int n)
		{
			var directors = new List<Director>(n);

			for (int i = 0; i < n; i++)
			{
				var director = new Director
				{
					Name = faker.Name.FullName(),
					DateOfBirth = new DateTime(faker.Random.Int(1920, 2024), faker.Random.Int(1, 12), faker.Random.Int(1, 28))
				};

				director.ContactInformation = GenerateContactInformation();

				directors.Add(director);
			}
			return directors;
		}

		private static List<Actor> GenerateActors(int n)
		{
			var actors = new List<Actor>(n);

			for (int i = 0; i < n; i++)
			{
				var actor = new Actor
				{
					Name = faker.Name.FullName(),
					DateOfBirth = new DateTime(faker.Random.Int(1920, 2024), faker.Random.Int(1, 12), faker.Random.Int(1, 28))
				};
				actors.Add(actor);
			}
			return actors;
		}

		private static List<Genre> GenerateGenres(int n)
		{
			var genres = new List<Genre>(n);

			for (int i = 0; i < n; i++)
			{
				var genre = new Genre
				{
					Name = faker.Music.Genre(),
				};
				genres.Add(genre);
			}
			return genres;
		}

		private static IEnumerable<Movie> GenerateMovies(int n, List<Director> directors, List<Actor> actors, List<Genre> genres)
		{
			var movies = new List<Movie>(n);

			for (int i = 0; i < n; i++)
			{
				var movie = new Movie
				{
					Title = faker.Vehicle.Model(),
					Rating = faker.Random.Int(1, 10),
					ReleaseDate = new DateTime(faker.Random.Int(1920, 2024), 1, 1),
					Description = faker.Lorem.Sentences(),

					Director = faker.Random.ListItem(directors),
					Actors = faker.Random.ListItems(actors, faker.Random.Int(2, 5)).ToList(),
					Genres = faker.Random.ListItems(genres, faker.Random.Int(1, 3)).ToList(),
				};

				movies.Add(movie);
			}

			return movies;
		}

		private static ContactInformation GenerateContactInformation()
		{
			var contactInfo = new ContactInformation
			{
				Email = faker.Internet.Email(),
				PhoneNumber = faker.Phone.PhoneNumber()
			};

			return contactInfo;
		}
	}
}
