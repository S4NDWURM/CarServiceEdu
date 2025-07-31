using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CarService.DataAccess.Entities;

namespace CarService.DataAccess.Configurations
{
    public class EmployeeStatusConfiguration : IEntityTypeConfiguration<EmployeeStatusEntity>
    {
        public void Configure(EntityTypeBuilder<EmployeeStatusEntity> b)
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).HasMaxLength(100).IsRequired();

            b.HasMany(e => e.Employees)
                .WithOne(es => es.EmployeeStatus)
                .HasForeignKey(es => es.EmployeeStatusId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
