using GymSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.DataSeeds
{
    public static class IdentityDataSeed
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager ,UserManager<ApplicationUser> userManager,ILogger logger , CancellationToken ct = default )
        {
            try
            {
                bool HasUsers = userManager.Users.Any();
                bool HasRoles = roleManager.Roles.Any();

                if (HasUsers && HasRoles) return;

                if(!HasRoles)
                {
                    var Roles = new List<IdentityRole>()
                    {
                        new IdentityRole() {Name = "SuperAdmin"},
                        new IdentityRole() {Name = "Admin"}

                    };
                    foreach (var roleName in Roles.Select(r => r.Name))
                    {
                        if (!await roleManager.RoleExistsAsync(roleName))
                        {
                            var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));

                            if (!roleResult.Succeeded)
                            { 
                                logger.LogError("Failed To Create Role..");
                            }
                        }
                    }
                }

                if(!HasUsers)
                {
                    var MainUser = new ApplicationUser()
                    {
                        FirstName = "Ahmed",
                        LastName = "Mostafa",
                        UserName = "Super",
                        Email = "super@gmail.com",
                        PhoneNumber = "01098423850"
                    };
                    await userManager.CreateAsync(MainUser , "P@ssw0rd");
                    await userManager.AddToRoleAsync(MainUser, "SuperAdmin");

                    var Admin01 = new ApplicationUser()
                    {
                        FirstName = "Hani",
                        LastName = "Mostafa",
                        UserName = "Admin01",
                        Email = "admin@gmail.com",
                        PhoneNumber = "01248454962"
                    };

                    var createResult = await userManager.CreateAsync(Admin01, "P@ssw0rd");
                    await userManager.AddToRoleAsync(Admin01, "Admin");


                    if (!createResult.Succeeded)
                    {
                        logger.LogError("Failed to Seed Users");
                        return;
                    }
                }

                return;

            }catch(Exception ex)
            {
                logger.LogError("Failed to Seed Identity Data");
                throw; 
            }
        }
    }
}
