using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarService.DataAccess.Configurations
{
    public class GenerationConfiguration : IEntityTypeConfiguration<GenerationEntity>
    {
        public void Configure(EntityTypeBuilder<GenerationEntity> b)
        {
            b.HasKey(g => g.Id);

            b.Property(g => g.Name).IsRequired().HasMaxLength(100);
            b.Property(g => g.StartYear).IsRequired();
            b.Property(g => g.EndYear).IsRequired();
            b.HasIndex(g => new { g.CarModelId, g.Name }).IsUnique();

            b.HasOne(g => g.CarModel)                 
                .WithMany(m => m.Generations)
                .HasForeignKey(g => g.CarModelId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
