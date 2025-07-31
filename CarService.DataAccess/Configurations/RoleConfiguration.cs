using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarService.DataAccess.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> b)
        {
            b.HasKey(r => r.Id);

            b.Property(r => r.Name)
             .HasMaxLength(50)
             .IsRequired();
            b.HasIndex(r => r.Name).IsUnique();

            b.HasMany(r => r.Users)
             .WithOne(u => u.Role)
             .HasForeignKey(u => u.RoleId)
             .OnDelete(DeleteBehavior.Restrict);

            var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var specialistId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var clientId = Guid.Parse("33333333-3333-3333-3333-333333333333");

            b.HasData(
                new RoleEntity { Id = adminId, Name = "Admin" },
                new RoleEntity { Id = specialistId, Name = "Specialist" },
                new RoleEntity { Id = clientId, Name = "Client" }
            );
        }
    }
}
