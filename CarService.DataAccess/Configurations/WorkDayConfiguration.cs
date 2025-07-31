using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarService.DataAccess.Configurations
{
    public class WorkDayConfiguration : IEntityTypeConfiguration<WorkDayEntity>
    {
        public void Configure(EntityTypeBuilder<WorkDayEntity> b)
        {
            b.HasKey(x => x.Id);

            b.Property(x => x.StartTime).IsRequired();
            b.Property(x => x.EndTime).IsRequired();

            b.HasOne(wd => wd.Employee)
             .WithMany(e => e.WorkDays)
             .HasForeignKey(wd => wd.EmployeeId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(wd => wd.TypeOfDay)
             .WithMany(t => t.WorkDays)
             .HasForeignKey(wd => wd.TypeOfDayId)
             .OnDelete(DeleteBehavior.Restrict);
        }

    }
}