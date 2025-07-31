using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarService.DataAccess.Configurations
{
    public class CarModelConfiguration : IEntityTypeConfiguration<CarModelEntity>
    {
        public void Configure(EntityTypeBuilder<CarModelEntity> b)
        {
            b.HasKey(m => m.Id);

            b.Property(m => m.Name).IsRequired().HasMaxLength(100);
            b.HasIndex(m => new { m.CarBrandId, m.Name }).IsUnique();

            b.HasOne(m => m.CarBrand)
             .WithMany(bn => bn.CarModels)
             .HasForeignKey(m => m.CarBrandId)
             .OnDelete(DeleteBehavior.Cascade);
        }

    }
}