using AutoMapper;
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
		private readonly MovieCardsContext db;
		private readonly IMapper mapper;
		private readonly ValidationService validation;

		public MoviesController(MovieCardsContext db, IMapper mapper, ValidationService validation)
		{
			this.db = db;
			this.mapper = mapper;
			this.validation = validation;
		}

		// GET: api/movies
		[HttpGet]
		public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovie(

			[FromQuery] string? title = null,
			[FromQuery] string? genre = null,
			[FromQuery] string? actorName = null,
			[FromQuery] string? directorName = null,
			[FromQuery] DateTime? releaseDate = null,
			[FromQuery] bool includeActors = false,
			[FromQuery] bool includeDirector = false,
			[FromQuery] bool includeGenre = false)
		{
			// Create and build query
			var query = db.Movie.AsQueryable();

			if (!string.IsNullOrEmpty(title))
			{
				query = query.Where(m => m.Title.Contains(title));
			}

			if (!string.IsNullOrEmpty(genre))
			{
				query = query.Where(m => m.Genres.Any(g => g.Name.Contains(genre)));
			}

			if (!string.IsNullOrEmpty(actorName))
			{
				query = query.Where(m => m.Actors.Any(a => a.Name.Contains(actorName)));
			}

			if (!string.IsNullOrEmpty(directorName))
			{
				query = query.Where(m => m.Director.Name.Contains(directorName));
			}

			if (releaseDate.HasValue)
			{
				query = query.Where(m => m.ReleaseDate.Date == releaseDate.Value.Date);
			}

			if (includeActors)
			{
				query = query.Include(m => m.Actors);
			}

			if (includeDirector)
			{
				query = query.Include(m => m.Director);
			}

			if (includeGenre)
			{
				query = query.Include(m => m.Genres);
			}

			var movies = await query.ToListAsync();

			var movieDtoList = mapper.Map<IEnumerable<MovieDTO>>(movies);

			if (movieDtoList == null)
			{
				return NotFound();
			}

			return Ok(movieDtoList);
		}

		// GET: api/movies/5
		[HttpGet("{id}")]
		public async Task<ActionResult<MovieDTO>> GetMovie(int id)
		{
			var movie = await db.Movie
				.Include(m => m.Director)
				.Include(m => m.Actors)
				.Include(m => m.Genres)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (movie == null)
			{
				return NotFound();
			}

			var movieDto = mapper.Map<MovieDTO>(movie);

			return Ok(movieDto);
		}

		// POST: api/movies
		[HttpPost]
		public async Task<ActionResult<MovieDTO>> PostMovie(MovieForCreationDTO movieCreateDto)
		{
			var validationResult = await validation.ValidateMovieForCreationAsync(movieCreateDto);
			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.Message);
			}

			var movie = mapper.Map<Movie>(movieCreateDto);
			movie.Director = await db.Director.FindAsync(movieCreateDto.DirectorId);
			movie.Actors = await db.Actor.Where(a => movieCreateDto.ActorIds.Contains(a.Id)).ToListAsync();
			movie.Genres = await db.Genre.Where(g => movieCreateDto.GenreIds.Contains(g.Id)).ToListAsync();

			db.Movie.Add(movie);
			await db.SaveChangesAsync();

			var movieDto = mapper.Map<MovieDTO>(movie);

			return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movieDto);
		}

		// PUT: api/movies/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutMovie(int id, MovieForUpdateDTO movieUpdateDto)
		{
			// validate input param id
			if (id <= 0 || id != movieUpdateDto.Id)
			{
				return BadRequest("Invalid ID");
			}

			var validationResult = await validation.ValidateMovieForUpdateAsync(movieUpdateDto, id);
			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.Message);
			}

			// find existing movie by id or return notfound
			var movie = await db.Movie
				.Include(m => m.Director)
				.Include(m => m.Actors)
				.Include(m => m.Genres)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (movie == null)
			{
				return NotFound("ID not found");
			}

			mapper.Map(movieUpdateDto, movie);
			movie.DirectorId = movieUpdateDto.DirectorId;
			movie.Actors = await db.Actor.Where(a => movieUpdateDto.ActorIds.Contains(a.Id)).ToListAsync();
			movie.Genres = await db.Genre.Where(g => movieUpdateDto.GenreIds.Contains(g.Id)).ToListAsync();

			try
			{
				await db.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

			// return result feedback to api user
			var movieDto = mapper.Map<MovieDTO>(movie);

			return Ok(movieDto);
		}

		// DELETE: api/movies/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteMovie(int id)
		{
			if (id <= 0)
			{
				return BadRequest("Invalid ID");
			}

			var movie = await db.Movie.FindAsync(id);
			if (movie == null)
			{
				return NotFound("ID not found");
			}

			db.Movie.Remove(movie);
			await db.SaveChangesAsync();

			return NoContent();
		}

		// GET: api/movies/5/details
		[HttpGet("{id}/details")]
		public async Task<ActionResult<MovieDetailsDTO>> GetMovieDetails(int id)
		{
			// validate input param id
			if (id <= 0)
			{
				return BadRequest("Invalid ID");
			}

			// find existing movie by id or return notfound
			var movie = await db.Movie
				.Include(m => m.Director)
				.Include(m => m.Director.ContactInformation)
				.Include(m => m.Actors)
				.Include(m => m.Genres)
				.FirstOrDefaultAsync(m => m.Id == id);

			if (movie == null)
			{
				return NotFound("ID not found");
			}

			var movieDetailsDto = mapper.Map<MovieDetailsDTO>(movie);

			return Ok(movieDetailsDto);
		}
	}
}
