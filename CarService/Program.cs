using CarService.Application.Services;
using CarService.DataAccess;
using CarService.DataAccess.Repositories;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CarService.Core.Models;
using CarService.DataAccess.Entities;
using System.Data;
using CarService.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.CookiePolicy;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();                   
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy
            .AllowAnyOrigin() 
            .AllowAnyMethod()  
            .AllowAnyHeader();
    });

    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy
            .WithOrigins("http://localhost:8080")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });

});


builder.Services.AddDbContext<CarServiceDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString(nameof(CarServiceDbContext)),
        o => o.MigrationsAssembly("CarService.DataAccess"));
});



var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["jwt-token"];

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthentication();

builder.Services.AddAuthorization(
    options =>
{
    options.AddPolicy("AdminPolicy", policy =>
    {
        policy.RequireClaim("role", "Admin");
    });
    options.AddPolicy("SpecialistPolicy", policy =>
    {
        policy.RequireClaim("role", "Specialist");
    });
    options.AddPolicy("ClientPolicy", policy =>
    {
        policy.RequireClaim("role", "Client");
    });
}
);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddScoped<ICarBrandRepository, CarBrandRepository>();
builder.Services.AddScoped<ICarBrandService, CarBrandService>();

builder.Services.AddScoped<ICarModelRepository, CarModelRepository>();
builder.Services.AddScoped<ICarModelService, CarModelService>();


builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleService, VehicleService>();


builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientService, ClientService>();


builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IRequestService, RequestService>();

builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<IStatusService, StatusService>();


builder.Services.AddScoped<IWorkRepository, WorkRepository>();
builder.Services.AddScoped<IWorkService, WorkService>();
builder.Services.AddScoped<IDiagnosticsRepository, DiagnosticsRepository>();
builder.Services.AddScoped<IDiagnosticsService, DiagnosticsService>();
builder.Services.AddScoped<IPlannedWorkRepository, PlannedWorkRepository>();
builder.Services.AddScoped<IPlannedWorkService, PlannedWorkService>();


builder.Services.AddScoped<IPartBrandRepository, PartBrandRepository>();
builder.Services.AddScoped<IPartBrandService, PartBrandService>();
builder.Services.AddScoped<IPartRepository, PartRepository>();
builder.Services.AddScoped<IPartService, PartService>();


builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ISpecializationRepository, SpecializationRepository>();
builder.Services.AddScoped<ISpecializationService, SpecializationService>();
builder.Services.AddScoped<IEmployeeSpecializationRepository, EmployeeSpecializationRepository>();
builder.Services.AddScoped<IEmployeeSpecializationService, EmployeeSpecializationService>();
builder.Services.AddScoped<IGenerationRepository, GenerationRepository>();
builder.Services.AddScoped<IGenerationService, GenerationService>();
builder.Services.AddScoped<IWorkDayRepository, WorkDayRepository>();
builder.Services.AddScoped<IWorkDayService, WorkDayService>();
builder.Services.AddScoped<ITypeOfDayRepository, TypeOfDayRepository>();
builder.Services.AddScoped<ITypeOfDayService, TypeOfDayService>();

builder.Services.AddScoped<IEmployeeSpecializationRepository, EmployeeSpecializationRepository>();
builder.Services.AddScoped<IEmployeeSpecializationService, EmployeeSpecializationService>();

builder.Services.AddScoped<IPlannedWorkPartRepository, PlannedWorkPartRepository>();
builder.Services.AddScoped<IPlannedWorkPartService, PlannedWorkPartService>();

builder.Services.AddScoped<IPlannedWorkEmployeeRepository, PlannedWorkEmployeeRepository>();
builder.Services.AddScoped<IPlannedWorkEmployeeService, PlannedWorkEmployeeService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IEmployeeStatusRepository, EmployeeStatusRepository>();
builder.Services.AddScoped<IEmployeeStatusService, EmployeeStatusService>();
builder.Services.AddHostedService<DataSeedService>();
builder.Services.AddScoped<IExcelGenerationService, ExcelGenerationService>();



var app = builder.Build();
app.UseMiddleware<CarService.API.Middleware.ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger(c => c.RouteTemplate = "swagger/{documentname}/swagger.json");
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CarService API v1");
        c.RoutePrefix = "swagger";
    });
}
app.UseCors("AllowSpecificOrigin");


app.UseCookiePolicy(new CookiePolicyOptions {
    MinimumSameSitePolicy = SameSiteMode.Lax,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.None
});

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();