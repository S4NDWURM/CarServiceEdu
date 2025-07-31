using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarService.DataAccess.Configurations
{
    public class DiagnosticsConfiguration : IEntityTypeConfiguration<DiagnosticsEntity>
    {
        public void Configure(EntityTypeBuilder<DiagnosticsEntity> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.DiagnosticsDate).IsRequired();
            b.Property(x => x.ResultDescription).IsRequired().HasMaxLength(100);
            b.Property(x => x.EmployeeId).IsRequired();
            b.Property(x => x.RequestId).IsRequired();
            b.HasIndex(d => d.DiagnosticsDate);

            b.HasOne(d => d.Employee)               
                .WithMany(e => e.Diagnostics)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(d => d.Request)                 
             .WithMany(r => r.Diagnostics)
             .HasForeignKey(d => d.RequestId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}