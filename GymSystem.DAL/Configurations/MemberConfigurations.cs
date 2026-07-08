using GymSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymSystem.DAL.Configurations
{
    internal class MemberConfigurations : GymUserConfigurations<Member>, IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(X => X.JoinDate)
                   .HasColumnName("JoinDate")
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(M => M.HealthRecord)
                   .WithOne(HR => HR.Member)
                   .HasForeignKey<HealthRecord>(M => M.MemberId);

            base.Configure(builder);

        }
    }
}
