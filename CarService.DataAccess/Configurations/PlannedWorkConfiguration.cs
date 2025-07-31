using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class PlannedWorkConfiguration : IEntityTypeConfiguration<PlannedWorkEntity>
{
    public void Configure(EntityTypeBuilder<PlannedWorkEntity> b)
    {
        b.ToTable("PlannedWorks");
        b.HasKey(x => x.Id);

        b.Property(x => x.PlanDate).IsRequired();
        b.Property(x => x.ExpectedEndDate).IsRequired();
        b.Property(x => x.TotalCost).HasColumnType("numeric(18,2)").IsRequired();

        b.HasMany(pw => pw.PlannedWorkParts)
         .WithOne(pwp => pwp.PlannedWork)
         .HasForeignKey(pwp => pwp.PlannedWorkId)
         .OnDelete(DeleteBehavior.Cascade);

        b.HasMany(pw => pw.PlannedWorkEmployees)
         .WithOne(pwe => pwe.PlannedWork)
         .HasForeignKey(pwe => pwe.PlannedWorkId)
         .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(pw => pw.Work)                
            .WithMany(w => w.PlannedWorks)
            .HasForeignKey(pw => pw.WorkId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(pw => pw.Request)
            .WithMany(r => r.PlannedWorks)        
            .HasForeignKey(pw => pw.RequestId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(pw => pw.Status)
         .WithMany(s => s.PlannedWorks)        
         .HasForeignKey(pw => pw.StatusId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}
