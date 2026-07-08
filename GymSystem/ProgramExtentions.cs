using GymSystem.DAL.Contexts;
using GymSystem.DAL.DataSeeds;
using GymSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.PL
{
    public static class ProgramExtentions
    {
        public static async Task MigratedAndSeedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<GymDbContext>();
            var logger = services.GetRequiredService<ILogger<Program>>();
            var configuration = services.GetRequiredService<IConfiguration>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();


            // Pending Migrations ??
            var pending = await dbContext.Database.GetPendingMigrationsAsync();
            if(pending.Any())
            {
                logger.LogInformation($"Applying {pending.Count()} pending migrations...");
                await dbContext.Database.MigrateAsync(); // update the database to the latest version
                logger.LogInformation("Migrations applied successfully.");
            }

            var seedFilePath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");

            await GymDataSeed.seedAsync(dbContext, seedFilePath, logger);
            await IdentityDataSeed.SeedAsync(roleManager , userManager , logger);
        }
    }
}
