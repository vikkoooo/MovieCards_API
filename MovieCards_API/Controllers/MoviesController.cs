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

		public MoviesController(MovieCardsContext db, IMapper mapper)
		{
			this.db = db;
			this.mapper = mapper;
		}

		// GET: api/movies
		[HttpGet]
		public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovie()
		{
			var movies = await db.Movie
				.Include(d => d.Director)
				.Include(a => a.Actors)
				.Include(g => g.Genres)
				.ToListAsync();

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
			// make sure all user inputs are parsable
			if (!DateTime.TryParse(movieCreateDto.ReleaseDate, out DateTime releaseDate))
			{
				return BadRequest("Invalid date format. Please use YYYY-MM-DD");
			}

			var director = await db.Director.FindAsync(movieCreateDto.DirectorId);
			if (director == null)
			{
				return BadRequest("Director not found");
			}

			var actors = await db.Actor.Where(a => movieCreateDto.ActorIds.Contains(a.Id)).ToListAsync();
			if (actors.Count != movieCreateDto.ActorIds.Count)
			{
				return BadRequest("Actors not found");
			}

			var genres = await db.Genre.Where(g => movieCreateDto.GenreIds.Contains(g.Id)).ToListAsync();
			if (genres.Count != movieCreateDto.GenreIds.Count)
			{
				return BadRequest("Genres not found");
			}

			var movie = mapper.Map<Movie>(movieCreateDto);
			movie.ReleaseDate = releaseDate;
			movie.Director = director;
			movie.Actors = actors;
			movie.Genres = genres;

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

			Director director = new Director();
			List<Actor> actors = new List<Actor>();
			List<Genre> genres = new List<Genre>();

			if (movieUpdateDto.DirectorId > 0)
			{
				director = await db.Director.FindAsync(movieUpdateDto.DirectorId);
				if (director == null)
				{
					return BadRequest("Director not found");
				}

			}

			if (movieUpdateDto.ActorIds != null)
			{
				actors = await db.Actor.Where(a => movieUpdateDto.ActorIds.Contains(a.Id)).ToListAsync();
				if (actors.Count != movieUpdateDto.ActorIds.Count)
				{
					return BadRequest("Actors not found");
				}

			}

			if (movieUpdateDto.GenreIds != null)
			{
				genres = await db.Genre.Where(g => movieUpdateDto.GenreIds.Contains(g.Id)).ToListAsync();
				if (genres.Count != movieUpdateDto.GenreIds.Count)
				{
					return BadRequest("Genres not found");
				}
			}

			mapper.Map(movieUpdateDto, movie);
			movie.DirectorId = movieUpdateDto.DirectorId;
			movie.Actors = actors;
			movie.Genres = genres;

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
