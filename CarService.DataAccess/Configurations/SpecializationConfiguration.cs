using CarService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class SpecializationConfiguration : IEntityTypeConfiguration<SpecializationEntity>
{
    public void Configure(EntityTypeBuilder<SpecializationEntity> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).IsRequired().HasMaxLength(100);
        b.HasIndex(x => x.Name).IsUnique();

        b.HasMany(s => s.EmployeeSpecializations)   
         .WithOne(es => es.Specialization)
         .HasForeignKey(es => es.SpecializationId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}
