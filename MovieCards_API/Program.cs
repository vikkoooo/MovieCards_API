using Microsoft.EntityFrameworkCore;
using MovieCards_API.Data;

namespace MovieCards_API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddDbContext<MovieCardsContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("MovieCardsContext") ?? throw new InvalidOperationException("Connection string 'MovieCardsContext' not found.")));

			builder.Services.AddControllers();
			var app = builder.Build();

			app.UseHttpsRedirection();
			app.UseAuthorization();
			app.MapControllers();

			app.Run();
		}
	}
}
