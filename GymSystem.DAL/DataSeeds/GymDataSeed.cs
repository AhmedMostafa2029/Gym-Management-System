using GymSystem.DAL.Contexts;
using GymSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GymSystem.DAL.DataSeeds
{
    public static class GymDataSeed
    {
        public static async Task seedAsync(GymDbContext dbContext , string seedFilePath,ILogger logger, CancellationToken ct = default)
        {
            try
            {
                if (!await dbContext.Plans.AnyAsync(ct))
                {
                    var plans = LoadDataFromJsonFile<Plan>("plans.json", seedFilePath);
                    if(plans.Count > 0)
                    {
                        dbContext.Plans.AddRange(plans);
                        logger.LogInformation($"Seeded {plans.Count} plans completed successfully.");

                    }
                }
                if(dbContext.ChangeTracker.HasChanges())
                {
                    await dbContext.SaveChangesAsync(ct);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding.");
                throw;
            }
        }

        private static List<T> LoadDataFromJsonFile<T>(string fileName,string FolderPath)
        {
            var filePath = Path.Combine(FolderPath, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified seed file was not found.", filePath);
            }

            var jsonData = File.ReadAllText(filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            options.Converters.Add(new JsonStringEnumConverter());


            return JsonSerializer.Deserialize<List<T>>(jsonData , options) ?? [];
        }
    }
}
