using Microsoft.EntityFrameworkCore;

namespace WebAPI.Data
{
    public static class Extensions
    {
        public static void ApplyMigrationsToDb(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<BookContext>())
                    context.Database.Migrate();
            }
        }
    }
}
