using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCards_API.Data;
using MovieCards_API.Model.DTO;
using MovieCards_API.Model.Entities;

namespace MovieCards_API.Controllers
{
	[Route("api/movies")]
	[ApiController]
	public class MoviesController : ControllerBase
	{
		private readonly MovieCardsContext context;

		public MoviesController(MovieCardsContext context)
		{
			this.context = context;
		}

		// GET: api/movies
		[HttpGet]
		public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovie()
		{
			var dtoList = await context.Movie.Select(m => new MovieDTO
			{
				Title = m.Title,
				Rating = m.Rating,
				ReleaseDate = m.ReleaseDate,
				Description = m.Description,
				DirectorName = m.Director.Name,
				ActorNames = m.Actors.Select(a => a.Name).ToList(),
				GenreNames = m.Genres.Select(g => g.Name).ToList()
			}).ToListAsync();

			if (dtoList == null)
			{
				return NotFound();
			}

			return Ok(dtoList);
		}

		// GET: api/movies/5
		[HttpGet("{id}")]
		public async Task<ActionResult<MovieDTO>> GetMovie(int id)
		{
			var dto = await context.Movie
				.Where(m => m.Id == id)
				.Select(m => new MovieDTO
				{
					Title = m.Title,
					Rating = m.Rating,
					ReleaseDate = m.ReleaseDate,
					Description = m.Description,
					DirectorName = m.Director.Name,
					ActorNames = m.Actors.Select(a => a.Name).ToList(),
					GenreNames = m.Genres.Select(g => g.Name).ToList()
				}).FirstOrDefaultAsync();

			if (dto == null)
			{
				return NotFound();
			}

			return Ok(dto);
		}

		// POST: api/movies
		[HttpPost]
		public async Task<ActionResult<MovieDTO>> PostMovie(MovieForCreationDTO movieCreateDto)
		{
			// make sure all user inputs are parsable
			if (!DateTime.TryParse(movieCreateDto.ReleaseDate, out DateTime releaseDate))
			{
				return BadRequest("Invalid date format. Please use YYYY-MM-DD");
			}

			var director = await context.Director.FindAsync(movieCreateDto.DirectorId);
			if (director == null)
			{
				return BadRequest("Director not found");
			}

			var actors = await context.Actor.Where(a => movieCreateDto.ActorIds.Contains(a.Id)).ToListAsync();
			if (actors.Count != movieCreateDto.ActorIds.Count)
			{
				return BadRequest("Actors not found");
			}

			var genres = await context.Genre.Where(g => movieCreateDto.GenreIds.Contains(g.Id)).ToListAsync();
			if (genres.Count != movieCreateDto.GenreIds.Count)
			{
				return BadRequest("Genres not found");
			}

			var movie = new Movie
			{
				Title = movieCreateDto.Title,
				Rating = movieCreateDto.Rating,
				ReleaseDate = releaseDate,
				Description = movieCreateDto.Description,
				Director = director,
				Actors = actors,
				Genres = genres
			};

			context.Movie.Add(movie);
			await context.SaveChangesAsync();

			// return result feedback to api user
			var resultDto = new MovieDTO
			{
				Title = movie.Title,
				Rating = movie.Rating,
				ReleaseDate = movie.ReleaseDate,
				Description = movie.Description,
				DirectorName = movie.Director.Name,
				ActorNames = movie.Actors.Select(a => a.Name).ToList(),
				GenreNames = movie.Genres.Select(g => g.Name).ToList()
			};

			return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, resultDto);
		}

		// PUT: api/movies/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutMovie(int id, MovieForUpdateDTO movieUpdateDto)
		{
			// validate input param id
			if (id <= 0)
			{
				return BadRequest("Invalid ID");
			}

			// find existing movie by id or return notfound
			var movie = await context.Movie
				.Include(m => m.Director)
				.Include(m => m.Actors)
				.Include(m => m.Genres)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (movie == null)
			{
				return NotFound("ID not found");
			}

			// check for changes in the input fields, only update if changed
			if (!string.IsNullOrEmpty(movieUpdateDto.Title))
			{
				movie.Title = movieUpdateDto.Title;
			}

			if (movieUpdateDto.Rating > 0 && movieUpdateDto.Rating <= 5)
			{
				movie.Rating = movieUpdateDto.Rating;
			}

			if (!string.IsNullOrEmpty(movieUpdateDto.ReleaseDate))
			{
				if (!DateTime.TryParse(movieUpdateDto.ReleaseDate, out DateTime releaseDate))
				{
					return BadRequest("Invalid date format. Please use YYYY-MM-DD");
				}
				movie.ReleaseDate = releaseDate;
			}

			if (!string.IsNullOrEmpty(movieUpdateDto.Description))
			{
				movie.Description = movieUpdateDto.Description;
			}

			if (movieUpdateDto.DirectorId > 0)
			{
				var director = await context.Director.FindAsync(movieUpdateDto.DirectorId);
				if (director == null)
				{
					return BadRequest("Director not found");
				}
				movie.DirectorId = movieUpdateDto.DirectorId;
			}

			if (movieUpdateDto.ActorIds != null)
			{
				var actors = await context.Actor.Where(a => movieUpdateDto.ActorIds.Contains(a.Id)).ToListAsync();
				if (actors.Count != movieUpdateDto.ActorIds.Count)
				{
					return BadRequest("Actors not found");
				}
				movie.Actors = actors;
			}

			if (movieUpdateDto.GenreIds != null)
			{
				var genres = await context.Genre.Where(g => movieUpdateDto.GenreIds.Contains(g.Id)).ToListAsync();
				if (genres.Count != movieUpdateDto.GenreIds.Count)
				{
					return BadRequest("Genres not found");
				}
				movie.Genres = genres;
			}

			context.Entry(movie).State = EntityState.Modified;

			try
			{
				await context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

			// return result feedback to api user
			var resultDto = new MovieDTO
			{
				Title = movie.Title,
				Rating = movie.Rating,
				ReleaseDate = movie.ReleaseDate,
				Description = movie.Description,
				DirectorName = movie.Director.Name,
				ActorNames = movie.Actors.Select(a => a.Name).ToList(),
				GenreNames = movie.Genres.Select(g => g.Name).ToList()
			};

			return Ok(resultDto);
		}

		// DELETE: api/movies/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteMovie(int id)
		{
			if (id <= 0)
			{
				return BadRequest("Invalid ID");
			}

			var movie = await context.Movie.FindAsync(id);
			if (movie == null)
			{
				return NotFound("ID not found");
			}

			context.Movie.Remove(movie);
			await context.SaveChangesAsync();

			return NoContent();
		}
	}
}
