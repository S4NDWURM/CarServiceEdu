using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarService.DataAccess.Configurations
{
    public class CarBrandConfiguration : IEntityTypeConfiguration<CarBrandEntity>
    {
        public void Configure(EntityTypeBuilder<CarBrandEntity> b)
        {
            b.HasKey(br => br.Id);
            b.Property(br => br.Name).IsRequired().HasMaxLength(100);
            b.HasIndex(br => br.Name).IsUnique();

            b.HasMany(br => br.CarModels)
                .WithOne(cm => cm.CarBrand)
                .HasForeignKey(cm => cm.CarBrandId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}