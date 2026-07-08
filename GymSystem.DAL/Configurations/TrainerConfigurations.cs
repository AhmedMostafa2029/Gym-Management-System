using GymSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystem.DAL.Configurations
{
    internal class TrainerConfigurations : GymUserConfigurations<Trainer>, IEntityTypeConfiguration<Trainer>
    {
        public new void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.Property(X => X.HireDate)
                   .HasColumnName("HireDate")
                   .HasDefaultValueSql("GETDATE()");

            base.Configure(builder);
        }
    }
}
