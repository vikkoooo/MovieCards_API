using Microsoft.EntityFrameworkCore;
using MovieCards_API.Data;
using MovieCards_API.Extensions;

namespace MovieCards_API
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddDbContext<MovieCardsContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("MovieCardsContext") ?? throw new InvalidOperationException("Connection string 'MovieCardsContext' not found.")));

			builder.Services.AddControllers();
			var app = builder.Build();

			// seed data
			await app.SeedDataAsync();

			app.UseHttpsRedirection();
			app.UseAuthorization();
			app.MapControllers();

			app.Run();
		}
	}
}
