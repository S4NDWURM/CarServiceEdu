using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarService.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> b)
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.UserName).IsRequired().HasMaxLength(50);
            b.Property(x => x.PasswordHash).IsRequired().HasMaxLength(512);
            b.Property(x => x.Email).IsRequired().HasMaxLength(100);
            b.HasIndex(x => x.UserName).IsUnique();                  
            b.HasIndex(x => x.Email).IsUnique();
            b.HasIndex(x => x.ClientId).IsUnique();
            b.HasIndex(x => x.EmployeeId).IsUnique();

            b.HasOne(u => u.Role)
             .WithMany(r => r.Users)
             .HasForeignKey(u => u.RoleId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}