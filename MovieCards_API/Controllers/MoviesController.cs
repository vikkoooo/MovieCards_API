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
		public async Task<ActionResult<Movie>> GetMovie(int id)
		{
			var dto = await context.Movie
				.Where(m => m.Id == id)
				.Select(m => new MovieDTO
				{
					Title = m.Title,
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

		// PUT: api/Movies/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<IActionResult> PutMovie(int id, Movie movie)
		{
			if (id != movie.Id)
			{
				return BadRequest();
			}

			context.Entry(movie).State = EntityState.Modified;

			try
			{
				await context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!MovieExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/Movies
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<Movie>> PostMovie(Movie movie)
		{
			context.Movie.Add(movie);
			await context.SaveChangesAsync();

			return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
		}

		// DELETE: api/Movies/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteMovie(int id)
		{
			var movie = await context.Movie.FindAsync(id);
			if (movie == null)
			{
				return NotFound();
			}

			context.Movie.Remove(movie);
			await context.SaveChangesAsync();

			return NoContent();
		}

		private bool MovieExists(int id)
		{
			return context.Movie.Any(e => e.Id == id);
		}
	}
}
