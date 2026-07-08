using GymSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Configurations
{
    public class PlanConfigurations:IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(p => p.Name).HasColumnType("varchar").HasMaxLength(50);
            builder.Property(p => p.Description).HasMaxLength(200);

            builder.Property(p => p.Price).HasPrecision(10, 2);//decimal 10,2

            builder.Property(p => p.CreatedAt).HasDefaultValueSql("GetDate()");

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("DurationCheckValue", "Duration Between 1 and 365");
            });

            //builder.HasData(
            //    new Plan
            //    {
            //        Id = 1,
            //        Name = "Basic",
            //        Description = "Access during staffed hours",
            //        Duration = 30,
            //        Price = 300,
            //        IsActive = true,
            //        CreatedAt = new DateTime(2026, 1, 1)
            //    },
            //    new Plan
            //    {
            //        Id = 2,
            //        Name = "Standard",
            //        Description = "Includes group classes",
            //        Duration = 60,
            //        Price = 500,
            //        IsActive = true,
            //        CreatedAt = new DateTime(2026, 1, 1)
            //    },
            //    new Plan
            //    {
            //        Id = 3,
            //        Name = "Premium",
            //        Description = "Unlimited access to gym equipment and classes",
            //        Duration = 90,
            //        Price = 900,
            //        IsActive = true,
            //        CreatedAt = new DateTime(2026, 1, 1)
            //    },
            //    new Plan
            //    {
            //        Id = 4,
            //        Name = "Annual",
            //        Description = "Full year access with personal trainer sessions",
            //        Duration = 365,
            //        Price = 3000,
            //        IsActive = true,
            //        CreatedAt = new DateTime(2026, 1, 1)
            //    }
            //);

        }
    }
}
