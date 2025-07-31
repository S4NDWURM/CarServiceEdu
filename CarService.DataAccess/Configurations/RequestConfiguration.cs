using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarService.DataAccess.Configurations
{
    public class RequestConfiguration : IEntityTypeConfiguration<RequestEntity>
    {
        public void Configure(EntityTypeBuilder<RequestEntity> b)
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.Reason).IsRequired().HasMaxLength(100);
            b.Property(x => x.OpenDate).IsRequired();
            b.Property(x => x.CloseDate).IsRequired(false);     

            b.HasOne(r => r.Client)
             .WithMany(c => c.Requests)
             .HasForeignKey(r => r.ClientId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(r => r.Vehicle)
             .WithMany(v => v.Requests)
             .HasForeignKey(r => r.VehicleId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(r => r.Status)
             .WithMany(s => s.Requests)
             .HasForeignKey(r => r.StatusId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(r => r.Diagnostics)
                .WithOne(d => d.Request)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasMany(r => r.PlannedWorks)
                .WithOne(pw => pw.Request)
                .HasForeignKey(pw => pw.RequestId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}