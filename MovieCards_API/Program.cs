using Microsoft.EntityFrameworkCore;
using MovieCards_API.Data;

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

			builder.Services.AddControllers().AddNewtonsoftJson();
			builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
			builder.Services.AddScoped<ValidationService>();
			var app = builder.Build();

			// reset db and reseed data on startup
			using (var scope = app.Services.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<MovieCardsContext>();

				// delete db
				await context.Database.EnsureDeletedAsync();
				await context.Database.MigrateAsync();

				// seed data
				await SeedData.InitAsync(context);
			}

			app.UseHttpsRedirection();
			app.UseAuthorization();
			app.MapControllers();

			app.Run();
		}
	}
}
