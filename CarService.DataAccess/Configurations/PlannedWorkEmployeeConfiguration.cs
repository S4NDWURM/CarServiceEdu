using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class PlannedWorkEmployeeConfiguration : IEntityTypeConfiguration<PlannedWorkEmployeeEntity>
{
    public void Configure(EntityTypeBuilder<PlannedWorkEmployeeEntity> b)
    {
        b.ToTable("PlannedWorkEmployees");
        b.HasKey(e => new { e.PlannedWorkId, e.EmployeeId });

        b.HasOne(e => e.PlannedWork)
         .WithMany(pw => pw.PlannedWorkEmployees)
         .HasForeignKey(e => e.PlannedWorkId)
         .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(e => e.Employee)
         .WithMany(emp => emp.PlannedWorkEmployees)
         .HasForeignKey(e => e.EmployeeId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}
