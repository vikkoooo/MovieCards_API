using Microsoft.EntityFrameworkCore;
using MovieCards_API.Model.DTO;
using MovieCards_API.Model.Entities;

namespace MovieCards_API.Data
{
	public class ValidationService
	{
		private readonly MovieCardsContext db;

		public ValidationService(MovieCardsContext db)
		{
			this.db = db;
		}

		public async Task<bool> IsDuplicateActorAsync(string name, DateTime dateOfBirth)
		{
			bool result = await db.Actor.AnyAsync(a => a.Name == name && a.DateOfBirth == dateOfBirth);

			return result;
		}

		public bool IsReleaseDateValid(DateTime releaseDate)
		{
			if (releaseDate <= DateTime.Now)
				return true;

			return false;
		}

		public async Task<bool> IsTitleUniqueAsync(string title, int? movieId = null)
		{
			bool result = !await db.Movie.AnyAsync(m => m.Title == title && (!movieId.HasValue || m.Id != movieId.Value));

			return result;
		}

		public bool IsRatingValid(int rating)
		{
			if (rating >= 1 && rating <= 10)
				return true;

			return false;
		}

		private bool HasDuplicateActors(IEnumerable<int> actorIds)
		{
			var distinctActorIds = actorIds.Distinct().ToList();
			return distinctActorIds.Count != actorIds.Count();
		}

		private bool HasDuplicateGenres(IEnumerable<int> genreIds)
		{
			var distinctGenreIds = genreIds.Distinct().ToList();
			return distinctGenreIds.Count != genreIds.Count();
		}

		public async Task<(bool IsValid, string Message)> ValidateMovieForCreationAsync(MovieForCreationDTO movieCreateDto)
		{
			var director = await db.Director.FindAsync(movieCreateDto.DirectorId);
			if (director == null)
			{
				return (false, "Director not found");
			}

			if (HasDuplicateActors(movieCreateDto.ActorIds))
			{
				return (false, "Duplicate actors are not allowed in the same movie");
			}

			var actors = await db.Actor.Where(a => movieCreateDto.ActorIds.Contains(a.Id)).ToListAsync();
			if (actors.Count != movieCreateDto.ActorIds.Count)
			{
				return (false, "Actors not found");
			}

			if (HasDuplicateGenres(movieCreateDto.GenreIds))
			{
				return (false, "Duplicate genres are not allowed in the same movie");
			}

			var genres = await db.Genre.Where(g => movieCreateDto.GenreIds.Contains(g.Id)).ToListAsync();
			if (genres.Count != movieCreateDto.GenreIds.Count)
			{
				return (false, "Genres not found");
			}

			if (!IsReleaseDateValid(movieCreateDto.ReleaseDate))
			{
				return (false, "Release date cannot be in the future");
			}

			if (!await IsTitleUniqueAsync(movieCreateDto.Title))
			{
				return (false, "Movie title must be unique");
			}

			if (!IsRatingValid(movieCreateDto.Rating))
			{
				return (false, "Rating must be between 1 and 10");
			}

			return (true, string.Empty);
		}

		public async Task<(bool IsValid, string Message)> ValidateMovieForUpdateAsync(MovieForUpdateDTO movieUpdateDto, int id)
		{
			var movie = await db.Movie.FindAsync(id);
			if (movie == null)
			{
				return (false, "Movie not found");
			}

			if (movieUpdateDto.DirectorId > 0)
			{
				var director = await db.Director.FindAsync(movieUpdateDto.DirectorId);
				if (director == null)
				{
					return (false, "Director not found");
				}
			}

			if (movieUpdateDto.ActorIds != null)
			{
				if (HasDuplicateActors(movieUpdateDto.ActorIds))
				{
					return (false, "Duplicate actors are not allowed in the same movie");
				}

				var actors = await db.Actor.Where(a => movieUpdateDto.ActorIds.Contains(a.Id)).ToListAsync();
				if (actors.Count != movieUpdateDto.ActorIds.Count)
				{
					return (false, "Some actors not found");
				}
			}

			if (movieUpdateDto.GenreIds != null)
			{
				if (HasDuplicateGenres(movieUpdateDto.GenreIds))
				{
					return (false, "Duplicate genres are not allowed in the same movie");
				}

				var genres = await db.Genre.Where(g => movieUpdateDto.GenreIds.Contains(g.Id)).ToListAsync();
				if (genres.Count != movieUpdateDto.GenreIds.Count)
				{
					return (false, "Some genres not found");
				}
			}

			if (!IsReleaseDateValid(movieUpdateDto.ReleaseDate))
			{
				return (false, "Release date cannot be in the future");
			}

			if (!await IsTitleUniqueAsync(movieUpdateDto.Title, id))
			{
				return (false, "Movie title must be unique");
			}

			if (!IsRatingValid(movieUpdateDto.Rating))
			{
				return (false, "Rating must be between 1 and 10");
			}

			return (true, string.Empty);
		}
		public async Task<(bool IsValid, string Message, Actor? Actor)> GetOrCreateActorAsync(ActorForMovieDTO actorDto)
		{
			Actor? actor = null;

			// find the actor by ID if provided
			if (actorDto.Id.HasValue)
			{
				actor = await db.Actor.FindAsync(actorDto.Id.Value);
				if (actor == null)
				{
					return (false, $"Actor with ID {actorDto.Id} not found", null);
				}
			}
			else
			{
				// no id was provided - check for duplicates
				actor = await db.Actor.FirstOrDefaultAsync(a => a.Name == actorDto.Name && a.DateOfBirth == actorDto.DateOfBirth);

				// if actor still null, means actor doesnt exist, create 
				if (actor == null)
				{
					actor = new Actor
					{
						Name = actorDto.Name,
						DateOfBirth = actorDto.DateOfBirth
					};

					// add to db
					db.Actor.Add(actor);
				}
			}

			return (true, string.Empty, actor);
		}
	}
}
