using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CarService.DataAccess.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<EmployeeEntity>
    {
        public void Configure(EntityTypeBuilder<EmployeeEntity> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            b.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            b.Property(x => x.MiddleName).HasMaxLength(100).IsRequired();
            b.Property(x => x.WorkExperience).IsRequired();  
            b.Property(x => x.HireDate).IsRequired();         
            b.Property(x => x.EmployeeStatusId).IsRequired();   

            b.HasMany(e => e.PlannedWorkEmployees)
                .WithOne(pwe => pwe.Employee)
                .HasForeignKey(pwe => pwe.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(e => e.EmployeeSpecializations)
                .WithOne(es => es.Employee)
                .HasForeignKey(es => es.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(e => e.Diagnostics)
                .WithOne(d => d.Employee)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(e => e.WorkDays)
                .WithOne(wd => wd.Employee)
                .HasForeignKey(wd => wd.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(e => e.EmployeeStatus)
                .WithMany(es => es.Employees)
                .HasForeignKey(e => e.EmployeeStatusId)
                .OnDelete(DeleteBehavior.Restrict);

             b.HasOne(r => r.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<UserEntity>(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
