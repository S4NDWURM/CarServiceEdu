using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarService.DataAccess.Configurations
{
    public class PartBrandConfiguration : IEntityTypeConfiguration<PartBrandEntity>
    {
        public void Configure(EntityTypeBuilder<PartBrandEntity> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).IsRequired().HasMaxLength(100);
            b.HasIndex(x => x.Name).IsUnique();

            b.HasMany(pb => pb.Parts)           
                .WithOne(p => p.PartBrand)
                .HasForeignKey(p => p.PartBrandId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}