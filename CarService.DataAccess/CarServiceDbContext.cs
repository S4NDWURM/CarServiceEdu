using CarService.Core.Models;
using CarService.DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace CarService.DataAccess
{
    public class CarServiceDbContext : DbContext
    {
        public CarServiceDbContext(DbContextOptions<CarServiceDbContext> options)
            : base(options)
        {
        }

        public DbSet<CarBrandEntity> CarBrands { get; set; } = default!;
        public DbSet<CarModelEntity> CarModels { get; set; } = default!;
        public DbSet<VehicleEntity> Vehicles { get; set; } = default!;
        public DbSet<ClientEntity> Clients { get; set; } = default!;
        public DbSet<StatusEntity> Statuses { get; set; } = default!;
        public DbSet<RequestEntity> Requests { get; set; } = default!;
        public DbSet<WorkEntity> Works { get; set; } = default!;
        public DbSet<DiagnosticsEntity> Diagnostics { get; set; } = default!;
        public DbSet<PlannedWorkEntity> PlannedWorks { get; set; } = default!;
        public DbSet<PartBrandEntity> PartBrands { get; set; } = default!;
        public DbSet<PartEntity> Parts { get; set; } = default!;
        public DbSet<EmployeeEntity> Employees { get; set; } = default!;
        public DbSet<SpecializationEntity> Specializations { get; set; } = default!;
        public DbSet<EmployeeSpecializationEntity> EmployeeSpecializations { get; set; } = default!;
        public DbSet<WorkDayEntity> WorkDays { get; set; } = default!;
        public DbSet<TypeOfDayEntity> TypesOfDay { get; set; } = default!;
        public DbSet<PlannedWorkPartEntity> PlannedWorkParts { get; set; } = default!;
        public DbSet<PlannedWorkEmployeeEntity> PlannedWorkEmployees { get; set; } = default!;
        public DbSet<GenerationEntity> Generations { get; set; } = default!;
        public DbSet<EmployeeStatusEntity> EmployeeStatuses { get; set; } = default!;

        public DbSet<UserEntity> Users { get; set; } = default!;
        public DbSet<RoleEntity> Roles { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CarServiceDbContext).Assembly);
        }
    }
}
