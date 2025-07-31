using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarService.DataAccess.Configurations
{
    public class TypeOfDayConfiguration : IEntityTypeConfiguration<TypeOfDayEntity>
    {
        public void Configure(EntityTypeBuilder<TypeOfDayEntity> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).IsRequired().HasMaxLength(100);
            b.HasIndex(x => x.Name).IsUnique();

            b.HasMany(t => t.WorkDays)            
             .WithOne(wd => wd.TypeOfDay)
             .HasForeignKey(wd => wd.TypeOfDayId)
             .OnDelete(DeleteBehavior.Restrict);
        }

    }
}