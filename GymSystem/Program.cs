using GymSystem.BLL.Services.Classes;
using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.Utilities;
using GymSystem.DAL.Contexts;
using GymSystem.DAL.Models;
using GymSystem.DAL.Repository.Classes;
using GymSystem.DAL.Repository.Interfaces;
using GymSystem.PL;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;

namespace GymSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register the DbContext
            builder.Services.AddDbContext<GymDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //   Register our repositories
            //builder.Services.AddScoped<IPlanRepository, PlanRepository>();
            //builder.Services.AddScoped<IMemberRepository, MemberRepository>();

            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IMemberServices , MemberServices>();
            builder.Services.AddScoped<ISessionServices, SessionServices>();
            builder.Services.AddScoped<ITrainerServices, TrainerServices>();
            builder.Services.AddScoped<IPlanServices, PlanServices>();
            builder.Services.AddScoped<IAnalyticsServices, AnalyticsServices>();
            builder.Services.AddScoped<IAttachmentServices, AttachmentServices>();
            builder.Services.AddScoped<IMembershipService, MembershipService>();
            builder.Services.AddScoped<IBookingService, BookingService>();




            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile()));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;


            }).AddEntityFrameworkStores<GymDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";

                options.AccessDeniedPath = "/Account/AccessDenied";

            });

            var app = builder.Build();

            await app.MigratedAndSeedAsync();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            //app.MapStaticAssts();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
