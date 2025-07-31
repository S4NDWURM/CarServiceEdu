using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarService.DataAccess.Configurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<VehicleEntity>
    {
        public void Configure(EntityTypeBuilder<VehicleEntity> b)
        {
            b.HasKey(v => v.Id);

            b.Property(v => v.VIN).IsRequired().HasMaxLength(100);
            b.HasIndex(v => v.VIN).IsUnique();
            b.Property(v => v.Year).IsRequired();

            b.HasOne(v => v.Generation)
             .WithMany(r => r.Vehicles)
             .HasForeignKey(v => v.GenerationId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(v => v.Requests)
                .WithOne(r => r.Vehicle)
                .HasForeignKey(r => r.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}