using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCards.Domain.Entities;
using MovieCards.Infrastructure.Repositories;
using MovieCards_API.Data;
using MovieCards_API.Model.DTO;
using NuGet.Packaging;

namespace MovieCards_API.Controllers
{
	[Route("api/movies")]
	[ApiController]
	public class MoviesController : ControllerBase
	{
		private readonly IMovieRepository movieRepository;
		private readonly IActorRepository actorRepository;
		private readonly IDirectorRepository directorRepository;
		private readonly IGenreRepository genreRepository;
		private readonly IMapper mapper;
		private readonly ValidationService validation;

		public MoviesController(IMovieRepository movieRepository, IActorRepository actorRepository, IDirectorRepository directorRepository, IGenreRepository genreRepository, IMapper mapper, ValidationService validation)
		{
			this.movieRepository = movieRepository;
			this.actorRepository = actorRepository;
			this.directorRepository = directorRepository;
			this.genreRepository = genreRepository;
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
			[FromQuery] bool includeGenre = false,
			[FromQuery] string? sortBy = null,
			[FromQuery] string? sortOrder = null)
		{
			// Create and build query
			//var query = db.Movie.AsQueryable();
			var query = movieRepository.GetAllMovies();

			// filter params
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

			// sorting
			if (!string.IsNullOrEmpty(sortBy))
			{
				if (string.IsNullOrEmpty(sortOrder))
				{
					return BadRequest("sortOrder is required when using sortBy.");
				}

				sortOrder = sortOrder.ToLower();

				if (sortOrder != "asc" && sortOrder != "desc")
				{
					return BadRequest("Invalid sortOrder parameter. Valid options are: 'asc', 'desc'.");
				}

				if (sortBy.Equals("title", StringComparison.OrdinalIgnoreCase))
				{
					if (sortOrder == "desc")
					{
						query = query.OrderByDescending(m => m.Title);
					}
					else if (sortOrder == "asc")
					{
						query = query.OrderBy(m => m.Title);
					}
				}
				else if (sortBy.Equals("releasedate", StringComparison.OrdinalIgnoreCase))
				{
					if (sortOrder == "desc")
					{
						query = query.OrderByDescending(m => m.ReleaseDate);
					}
					else if (sortOrder == "asc")
					{
						query = query.OrderBy(m => m.ReleaseDate);
					}
				}
				else if (sortBy.Equals("rating", StringComparison.OrdinalIgnoreCase))
				{
					if (sortOrder == "desc")
					{
						query = query.OrderByDescending(m => m.Rating);
					}
					else if (sortOrder == "asc")
					{
						query = query.OrderBy(m => m.Rating);
					}
				}
				else
				{
					return BadRequest("Invalid sortBy parameter. Valid options are: 'title', 'releaseDate', 'rating'.");
				}
			}

			// run query 
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
			//var movie = await db.Movie
			//	.Include(m => m.Director)
			//	.Include(m => m.Actors)
			//	.Include(m => m.Genres)
			//	.FirstOrDefaultAsync(m => m.Id == id);
			var movie = await movieRepository.GetByIdAsync(id);

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
			// duplicate check
			// var existingMovie = await db.Movie.FirstOrDefaultAsync(m => m.Title == movieCreateDto.Title);
			var existingMovie = await movieRepository.GetByTitleAsync(movieCreateDto.Title);
			if (existingMovie != null)
			{
				return Conflict($"A movie with the title '{movieCreateDto.Title}' already exists");
			}

			var validationResult = await validation.ValidateMovieForCreationAsync(movieCreateDto);
			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.Message);
			}

			var movie = mapper.Map<Movie>(movieCreateDto);
			//movie.Director = await db.Director.FindAsync(movieCreateDto.DirectorId);
			//movie.Actors = await db.Actor.Where(a => movieCreateDto.ActorIds.Contains(a.Id)).ToListAsync();
			//movie.Genres = await db.Genre.Where(g => movieCreateDto.GenreIds.Contains(g.Id)).ToListAsync();
			movie.Director = await directorRepository.GetByIdAsync(movieCreateDto.DirectorId);
			movie.Actors = await actorRepository.GetActorsByIdsAsync(movieCreateDto.ActorIds);
			movie.Genres = await genreRepository.GetGenresByIdsAsync(movieCreateDto.GenreIds);

			//db.Movie.Add(movie);
			//await db.SaveChangesAsync();
			await movieRepository.AddAsync(movie);
			await movieRepository.SaveChangesAsync();

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

			// duplicate check
			//var existingMovie = await db.Movie.FirstOrDefaultAsync(m => m.Title == movieUpdateDto.Title && m.Id != id);
			var existingMovie = await movieRepository.GetByTitleAsync(movieUpdateDto.Title);
			if (existingMovie != null)
			{
				return Conflict($"A movie with the title '{movieUpdateDto.Title}' already exists");
			}

			var validationResult = await validation.ValidateMovieForUpdateAsync(movieUpdateDto, id);
			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.Message);
			}

			// find existing movie by id or return notfound
			//var movie = await db.Movie
			//	.Include(m => m.Director)
			//	.Include(m => m.Actors)
			//	.Include(m => m.Genres)
			//	.FirstOrDefaultAsync(m => m.Id == id);
			var movie = await movieRepository.GetByIdAsync(id);
			if (movie == null)
			{
				return NotFound("ID not found");
			}

			mapper.Map(movieUpdateDto, movie);
			//movie.DirectorId = movieUpdateDto.DirectorId;
			//movie.Actors = await db.Actor.Where(a => movieUpdateDto.ActorIds.Contains(a.Id)).ToListAsync();
			//movie.Genres = await db.Genre.Where(g => movieUpdateDto.GenreIds.Contains(g.Id)).ToListAsync();
			movie.Director = await directorRepository.GetByIdAsync(movieUpdateDto.DirectorId);
			movie.Actors = await actorRepository.GetActorsByIdsAsync(movieUpdateDto.ActorIds);
			movie.Genres = await genreRepository.GetGenresByIdsAsync(movieUpdateDto.GenreIds);

			try
			{
				//await db.SaveChangesAsync();
				await movieRepository.SaveChangesAsync();
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

			//var movie = await db.Movie.FindAsync(id);
			var movie = await movieRepository.GetByIdAsync(id);
			if (movie == null)
			{
				return NotFound("ID not found");
			}

			//db.Movie.Remove(movie);
			//await db.SaveChangesAsync();
			movieRepository.DeleteMovie(movie);
			await movieRepository.SaveChangesAsync();

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
			//var movie = await db.Movie
			//	.Include(m => m.Director)
			//	.Include(m => m.Director.ContactInformation)
			//	.Include(m => m.Actors)
			//	.Include(m => m.Genres)
			//	.FirstOrDefaultAsync(m => m.Id == id);
			var movie = await movieRepository.GetByIdAsync(id);
			if (movie == null)
			{
				return NotFound("ID not found");
			}

			var movieDetailsDto = mapper.Map<MovieDetailsDTO>(movie);

			return Ok(movieDetailsDto);
		}

		// POST: api/movies/5/actors
		[HttpPost("{id}/actors")]
		public async Task<ActionResult<MovieDTO>> AddActorsToMovie(int id, List<ActorForMovieDTO> actorDtos)
		{
			// find movie by input id
			//var movie = await db.Movie
			//	.Include(m => m.Actors)
			//	.FirstOrDefaultAsync(m => m.Id == id);
			var movie = await movieRepository.GetByIdAsync(id);
			if (movie == null)
			{
				return NotFound("Movie not found");
			}

			var actorsToAdd = new List<Actor>();
			foreach (var actorDto in actorDtos)
			{
				// find or create new actor, if success we continue, else send error message
				var newActor = await validation.GetOrCreateActorAsync(actorDto);

				// send error message if not valid response 
				if (!newActor.IsValid)
				{
					return BadRequest(newActor.Message);
				}

				var actor = newActor.Actor; // set actor

				// check for duplicated actors on same movie
				if (movie.Actors.Any(a => a.Id == actor.Id))
				{
					return BadRequest($"Actor {actor.Name} is already associated with this movie");
				}

				actorsToAdd.Add(actor);
			}

			// update movie with actors list
			movie.Actors.AddRange(actorsToAdd);

			//await db.SaveChangesAsync();
			await movieRepository.SaveChangesAsync();

			var movieDto = mapper.Map<MovieDTO>(movie);
			return Ok(movieDto);
		}

		// PATCH: api/movies/5
		[HttpPatch("{id}")]
		public async Task<ActionResult> PatchMovie(int id, JsonPatchDocument<MovieForPatchDTO> patchDocument)
		{
			// make sure we get input
			if (patchDocument == null)
			{
				return BadRequest("Patch document is null");
			}

			// find movie in db
			//var movie = await db.Movie.FindAsync(id);
			var movie = await movieRepository.GetByIdAsync(id);
			if (movie == null)
			{
				return NotFound("Movie not found");
			}

			// make a MovieForPatchDTO to "start with"
			var moviePatchDto = mapper.Map<MovieForPatchDTO>(movie);

			// change state in the DTO
			patchDocument.ApplyTo(moviePatchDto, ModelState);

			if (!ModelState.IsValid)
			{
				return UnprocessableEntity(ModelState);
			}

			// map updated dto back to movie and save
			mapper.Map(moviePatchDto, movie);
			//await db.SaveChangesAsync();
			await movieRepository.SaveChangesAsync();

			// return user feedback
			var updatedMovieDto = mapper.Map<MovieDetailsDTO>(movie);

			return Ok(updatedMovieDto);
		}
	}
}
