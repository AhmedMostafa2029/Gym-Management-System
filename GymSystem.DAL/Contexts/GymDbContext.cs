using FluentAssertions.Common;
using GymSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Contexts
{
    public class GymDbContext : IdentityDbContext<ApplicationUser>
    {
        protected GymDbContext()
        {
        }

        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {

        }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            //IdentityDbContext => Model Configrations Fluent APIs OnModelCreating
            base.OnModelCreating(modelBuilder); // Identity Configration
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());  // Application of all configurations in the assembly

            //modelBuilder.ApplyConfiguration<Plan>(new Configurations.PlanConfiguration());

        }


        public DbSet<Plan> Plans { get; set; }

        public DbSet<Member> Members { get; set; }
    
        public DbSet<Trainer> Trainers { get; set; }
    
        public DbSet<Session> Sessions { get; set; }
    
        public DbSet<Category> Categories { get; set; }
    
        public DbSet<Membership> Memberships { get; set; }
    
        public DbSet<Booking> Bookings { get; set; }

        public DbSet<HealthRecord> HealthRecords { get; set; }
    }
}
