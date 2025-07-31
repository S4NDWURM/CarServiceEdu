using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class PartConfiguration : IEntityTypeConfiguration<PartEntity>
{
    public void Configure(EntityTypeBuilder<PartEntity> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(100).IsRequired();
        b.Property(x => x.Article).HasMaxLength(100).IsRequired();
        b.Property(x => x.Cost).IsRequired().HasColumnType("numeric(18,2)");
        b.HasIndex(x => x.Article).IsUnique();

        b.HasMany(p => p.PlannedWorkParts)
         .WithOne(pwp => pwp.Part)
         .HasForeignKey(pwp => pwp.PartId)
         .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(p => p.PartBrand)          
            .WithMany(pb => pb.Parts)
            .HasForeignKey(p => p.PartBrandId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
