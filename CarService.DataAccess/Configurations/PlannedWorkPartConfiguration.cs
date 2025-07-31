using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class PlannedWorkPartConfiguration : IEntityTypeConfiguration<PlannedWorkPartEntity>
{
    public void Configure(EntityTypeBuilder<PlannedWorkPartEntity> b)
    {
        b.ToTable("PlannedWorkParts");
        b.HasKey(e => new { e.PlannedWorkId, e.PartId });

        b.Property(e => e.Quantity).IsRequired();

        b.HasOne(e => e.PlannedWork)
         .WithMany(pw => pw.PlannedWorkParts)
         .HasForeignKey(e => e.PlannedWorkId)
         .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(e => e.Part)
         .WithMany(p => p.PlannedWorkParts)
         .HasForeignKey(e => e.PartId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}
