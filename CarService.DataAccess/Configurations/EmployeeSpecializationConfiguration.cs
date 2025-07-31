using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class EmployeeSpecializationConfiguration : IEntityTypeConfiguration<EmployeeSpecializationEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeSpecializationEntity> b)
    {
        b.ToTable("EmployeeSpecializations");
        b.HasKey(e => new { e.EmployeeId, e.SpecializationId });

        b.HasOne(e => e.Employee)
         .WithMany(emp => emp.EmployeeSpecializations)
         .HasForeignKey(e => e.EmployeeId)
         .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(e => e.Specialization)
         .WithMany(s => s.EmployeeSpecializations)
         .HasForeignKey(e => e.SpecializationId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}
