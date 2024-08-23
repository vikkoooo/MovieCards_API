using Microsoft.EntityFrameworkCore;
using MovieCards_API.Data;

namespace MovieCards_API.Extensions
{
	public static class WebApplicationExtensions
	{
		public static async Task SeedDataAsync(this IApplicationBuilder app)
		{
			using (var scope = app.ApplicationServices.CreateScope())
			{
				var serviceProvider = scope.ServiceProvider;
				var context = serviceProvider.GetRequiredService<MovieCardsContext>();

				await context.Database.EnsureDeletedAsync();
				await context.Database.MigrateAsync();

				try
				{
					await SeedData.InitAsync(context);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					throw;
				}
			}
		}
	}
}
