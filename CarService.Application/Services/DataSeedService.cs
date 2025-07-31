using CarService.DataAccess;
using CarService.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CarService.Application.Services;
using CarService.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using CarService.Infrastructure;

namespace CarService.Application.Services
{
    public class DataSeedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _seedFilePath = @"..\CarService.Application\seedData.json";

        public DataSeedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<CarServiceDbContext>();
            var employeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();
            var employeeStatusService = scope.ServiceProvider.GetRequiredService<IEmployeeStatusService>();
            var specializationService = scope.ServiceProvider.GetRequiredService<ISpecializationService>();
            var employeeSpecializationService = scope.ServiceProvider.GetRequiredService<IEmployeeSpecializationService>();
            var carBrandService = scope.ServiceProvider.GetRequiredService<ICarBrandService>();
            var carModelService = scope.ServiceProvider.GetRequiredService<ICarModelService>();
            var generationService = scope.ServiceProvider.GetRequiredService<IGenerationService>();
            var vehicleService = scope.ServiceProvider.GetRequiredService<IVehicleService>();
            var clientService = scope.ServiceProvider.GetRequiredService<IClientService>();
            var requestService = scope.ServiceProvider.GetRequiredService<IRequestService>();
            var statusService = scope.ServiceProvider.GetRequiredService<IStatusService>();
            var workService = scope.ServiceProvider.GetRequiredService<IWorkService>();
            var plannedWorkService = scope.ServiceProvider.GetRequiredService<IPlannedWorkService>();
            var plannedWorkEmployeeService = scope.ServiceProvider.GetRequiredService<IPlannedWorkEmployeeService>();
            var plannedWorkPartService = scope.ServiceProvider.GetRequiredService<IPlannedWorkPartService>();
            var partService = scope.ServiceProvider.GetRequiredService<IPartService>();
            var partBrandService = scope.ServiceProvider.GetRequiredService<IPartBrandService>();
            var workDayService = scope.ServiceProvider.GetRequiredService<IWorkDayService>();
            var typeOfDayService = scope.ServiceProvider.GetRequiredService<ITypeOfDayService>();
            var diagnosticsService = scope.ServiceProvider.GetRequiredService<IDiagnosticsService>();
            var roleRepository = scope.ServiceProvider.GetRequiredService<IRoleRepository>();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

            await SeedDatabase(
                dbContext,
                employeeService,
                employeeStatusService,
                specializationService,
                employeeSpecializationService,
                carBrandService,
                carModelService,
                generationService,
                vehicleService,
                clientService,
                requestService,
                statusService,
                workService,
                plannedWorkService,
                plannedWorkEmployeeService,
                plannedWorkPartService,
                partService,
                partBrandService,
                workDayService,
                typeOfDayService,
                diagnosticsService,
                roleRepository,
                userService,
                userRepository,
                passwordHasher,
                cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task SeedDatabase(
            CarServiceDbContext _dbContext,
            IEmployeeService _employeeService,
            IEmployeeStatusService _employeeStatusService,
            ISpecializationService _specializationService,
            IEmployeeSpecializationService _employeeSpecializationService,
            ICarBrandService _carBrandService,
            ICarModelService _carModelService,
            IGenerationService _generationService,
            IVehicleService _vehicleService,
            IClientService _clientService,
            IRequestService _requestService,
            IStatusService _statusService,
            IWorkService _workService,
            IPlannedWorkService _plannedWorkService,
            IPlannedWorkEmployeeService _plannedWorkEmployeeService,
            IPlannedWorkPartService _plannedWorkPartService,
            IPartService _partService,
            IPartBrandService _partBrandService,
            IWorkDayService _workDayService,
            ITypeOfDayService _typeOfDayService,
            IDiagnosticsService _diagnosticsService,
            IRoleRepository _roleRepository,
            IUserService _userService,
            IUserRepository _userRepository,
            IPasswordHasher _passwordHasher,
            CancellationToken cancellationToken)
        {
            if (!File.Exists(_seedFilePath))
            {
                throw new FileNotFoundException($"JSON-файл не найден: {_seedFilePath}");
            }

            var jsonData = await File.ReadAllTextAsync(_seedFilePath, cancellationToken);
            var seedData = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonData);
            try
            {
                var generatedIds = new Dictionary<string, List<Guid>>();

                var existingRoles = await _dbContext.Roles.OrderBy(r => r.Name).ToListAsync();
                generatedIds["Roles"] = new List<Guid>();
                if (existingRoles.Count == 0)
                {
                    var predefinedRoles = new Dictionary<Guid, string>
                    {
                        { new Guid("11111111-1111-1111-1111-111111111111"), "Admin" },
                        { new Guid("22222222-2222-2222-2222-222222222222"), "Specialist" },
                        { new Guid("33333333-3333-3333-3333-333333333333"), "Client" }
                    };
                    foreach (var role in predefinedRoles)
                    {
                        var existingRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == role.Value, cancellationToken);
                        if (existingRole == null)
                        {
                            await _roleRepository.Create(role.Key, role.Value);
                        }
                        generatedIds["Roles"].Add(role.Key);
                    }
                }
                else
                {
                    generatedIds["Roles"] = existingRoles.Select(r => r.Id).ToList();
                }

                var existingEmployeeStatuses = await _dbContext.EmployeeStatuses.OrderBy(es => es.Id).ToListAsync();
                if (existingEmployeeStatuses.Count == 0)
                {
                    if (seedData.TryGetValue("EmployeeStatuses", out var employeeStatusesData))
                    {
                        generatedIds["EmployeeStatuses"] = new List<Guid>();
                        var dtos = JsonSerializer.Deserialize<List<EmployeeStatusDto>>(employeeStatusesData.GetRawText());
                        foreach (var dto in dtos)
                        {
                            var id = Guid.NewGuid();
                            var (model, error) = EmployeeStatus.Create(id, dto.Name);
                            if (!string.IsNullOrEmpty(error))
                                throw new InvalidOperationException($"Ошибка при создании EmployeeStatus: {error}");
                            await _employeeStatusService.CreateEmployeeStatus(model);
                            generatedIds["EmployeeStatuses"].Add(id);
                        }
                    }
                }
                else
                {
                    generatedIds["EmployeeStatuses"] = existingEmployeeStatuses.Select(es => es.Id).ToList();
                }


                var existingSpecializations = await _dbContext.Specializations.OrderBy(s => s.Id).ToListAsync();
                if (existingSpecializations.Count == 0)
                {
                    if (seedData.TryGetValue("Specializations", out var specializationsData))
                    {
                        generatedIds["Specializations"] = new List<Guid>();
                        var dtos = JsonSerializer.Deserialize<List<SpecializationDto>>(specializationsData.GetRawText());
                        foreach (var dto in dtos)
                        {
                            var id = Guid.NewGuid();
                            var (model, error) = Specialization.Create(id, dto.Name);
                            if (!string.IsNullOrEmpty(error))
                                throw new InvalidOperationException($"Ошибка при создании Specialization: {error}");
                            await _specializationService.CreateSpecialization(model);
                            generatedIds["Specializations"].Add(id);
                        }
                    }
                }
                else
                {
                    generatedIds["Specializations"] = existingSpecializations.Select(s => s.Id).ToList();
                }


                var existingEmployees = await _dbContext.Employees.OrderBy(e => e.Id).ToListAsync();
                if (existingEmployees.Count == 0)
                {
                    if (seedData.TryGetValue("Employees", out var employeesData))
                    {
                        generatedIds["Employees"] = new List<Guid>();
                        var dtos = JsonSerializer.Deserialize<List<EmployeeDto>>(employeesData.GetRawText());
                        foreach (var dto in dtos)
                        {
                            var roleId = generatedIds["Roles"][dto.RoleIndex];
                            var employeeStatusId = generatedIds["EmployeeStatuses"][dto.EmployeeStatusIndex];
                            await _userService.RegisterEmployee(
                                dto.UserName,
                                dto.Email,
                                dto.Password,
                                roleId,
                                employeeStatusId,
                                dto.LastName,
                                dto.FirstName,
                                dto.MiddleName,
                                dto.WorkExperience,
                                dto.HireDate);
                            var user = await _dbContext.Users
                                .Where(u => u.UserName == dto.UserName && u.Email == dto.Email)
                                .FirstOrDefaultAsync(cancellationToken);
                            if (user == null || !user.EmployeeId.HasValue)
                                throw new InvalidOperationException($"Не удалось найти созданного сотрудника для {dto.UserName}");
                            generatedIds["Employees"].Add(user.EmployeeId.Value);
                        }
                    }
                }
                else
                {
                    generatedIds["Employees"] = existingEmployees.Select(e => e.Id).ToList();
                }

                var existingEmployeeSpecializations = await _dbContext.EmployeeSpecializations.OrderBy(es => es.EmployeeId).ToListAsync();
                if (existingEmployeeSpecializations.Count == 0)
                {
                    if (seedData.TryGetValue("EmployeeSpecializations", out var employeeSpecializationsData))
                    {
                        var dtos = JsonSerializer.Deserialize<List<EmployeeSpecializationDto>>(employeeSpecializationsData.GetRawText());
                        foreach (var dto in dtos)
                        {
                            var employeeId = generatedIds["Employees"][dto.EmployeeIndex];
                            var specializationId = generatedIds["Specializations"][dto.SpecializationIndex];
                            var (model, error) = EmployeeSpecialization.Create(employeeId, specializationId);
                            if (!string.IsNullOrEmpty(error))
                                throw new InvalidOperationException($"Ошибка при создании EmployeeSpecialization: {error}");
                            await _employeeSpecializationService.CreateEmployeeSpecialization(model);
                        }
                    }
                }

                var existingCarBrands = await _dbContext.CarBrands.OrderBy(cb => cb.Id).ToListAsync();
                if (existingCarBrands.Count == 0)
                {
                    if (seedData.TryGetValue("CarBrands", out var carBrandsData))
                    {
                        generatedIds["CarBrands"] = new List<Guid>();
                        var dtos = JsonSerializer.Deserialize<List<CarBrandDto>>(carBrandsData.GetRawText());
                        foreach (var dto in dtos)
                        {
                            var id = Guid.NewGuid();
                            var (model, error) = CarBrand.Create(id, dto.Name);
                            if (!string.IsNullOrEmpty(error))
                                throw new InvalidOperationException($"Ошибка при создании CarBrand: {error}");
                            await _carBrandService.CreateCarBrand(model);
                            generatedIds["CarBrands"].Add(id);
                        }
                    }
                }
                else
                {
                    generatedIds["CarBrands"] = existingCarBrands.Select(cb => cb.Id).ToList();
                }

                var existingCarModels = await _dbContext.CarModels.OrderBy(cm => cm.Id).ToListAsync();
                if (existingCarModels.Count == 0)
                {
                    if (seedData.TryGetValue("CarModels", out var carModelsData))
                    {
                        generatedIds["CarModels"] = new List<Guid>();
                        var dtos = JsonSerializer.Deserialize<List<CarModelDto>>(carModelsData.GetRawText());
                        foreach (var dto in dtos)
                        {
                            var id = Guid.NewGuid();
                            var carBrandId = generatedIds["CarBrands"][dto.CarBrandIndex];
                            var (model, error) = CarModel.Create(id, dto.Name, carBrandId);
                            if (!string.IsNullOrEmpty(error))
                                throw new InvalidOperationException($"Ошибка при создании CarModel: {error}");
                            await _carModelService.CreateCarModel(model);
                            generatedIds["CarModels"].Add(id);
                        }
                    }
                }
                else
                {
                    generatedIds["CarModels"] = existingCarModels.Select(cm => cm.Id).ToList();
                }

                var existingGenerations = await _dbContext.Generations.OrderBy(g => g.Id).ToListAsync();
                if (existingGenerations.Count == 0)
                {
                    if (seedData.TryGetValue("Generations", out var generationsData))
                    {
                        generatedIds["Generations"] = new List<Guid>();
                        var dtos = JsonSerializer.Deserialize<List<GenerationDto>>(generationsData.GetRawText());
                        foreach (var dto in dtos)
                        {
                            var id = Guid.NewGuid();
                            var carModelId = generatedIds["CarModels"][dto.CarModelIndex];
                            var (model, error) = Generation.Create(id, carModelId, dto.Name, dto.StartYear, dto.EndYear);
                            if (!string.IsNullOrEmpty(error))
                                throw new InvalidOperationException($"Ошибка при создании Generation: {error}");
                            await _generationService.CreateGeneration(model);
                            generatedIds["Generations"].Add(id);
                        }
                    }
                }
                else
                {
                    generatedIds["Generations"] = existingGenerations.Select(g => g.Id).ToList();
                }

                var existingVehicles = await _dbContext.Vehicles.OrderBy(v => v.Id).ToListAsync();
                if (existingVehicles.Count == 0)
                {
                    if (seedData.TryGetValue("Vehicles", out var vehiclesData))
                    {
                        generatedIds["Vehicles"] = new List<Guid>();
                        var dtos = JsonSerializer.Deserialize<List<VehicleDto>>(vehiclesData.GetRawText());
                        foreach (var dto in dtos)
                        {
                            var id = Guid.NewGuid();
                            var generationId = generatedIds["Generations"][dto.GenerationIndex];
                            var (model, error) = Vehicle.Create(id, dto.VIN, dto.Year, generationId);
                            if (!string.IsNullOrEmpty(error))
                                throw new InvalidOperationException($"Ошибка при создании Vehicle: {error}");
                            await _vehicleService.CreateVehicle(model);
                            generatedIds["Vehicles"].Add(id);
                        }
                    }
                }
                else
                {
                    generatedIds["Vehicles"] = existingVehicles.Select(v => v.Id).ToList();
                }

                var existingClients = await _dbContext.Clients.OrderBy(c => c.Id).ToListAsync();
                if (existingClients.Count == 0)
                {
                    if (seedData.TryGetValue("Clients", out var clientsData))
                    {
                        generatedIds["Clients"] = new List<Guid>();
                        var dtos = JsonSerializer.Deserialize<List<ClientDto>>(clientsData.GetRawText());
                        foreach (var dto in dtos)
                        {
                            var roleId = new Guid("33333333-3333-3333-3333-333333333333"); ;
                            await _userService.RegisterClient(
                                dto.UserName,
                                dto.Email,
                                dto.Password,
                                roleId,
                                dto.LastName,
                                dto.FirstName,
                                dto.MiddleName,
                                dto.DateOfBirth,
                                dto.RegistrationDate);
                            var user = await _dbContext.Users
                                .Where(u => u.UserName == dto.UserName && u.Email == dto.Email)
                                .FirstOrDefaultAsync(cancellationToken);
                            if (user == null || !user.ClientId.HasValue)
                                throw new InvalidOperationException($"Не удалось найти созданного клиента для {dto.UserName}");
                            generatedIds["Clients"].Add(user.ClientId.Value);
                        }
                    }
                }
                else
                {
                    generatedIds["Clients"] = existingClients.Select(c => c.Id).ToList();
                }

                var existingStatuses = await _dbContext.Statuses.OrderBy(s => s.Id).ToListAsync();
                if (existingStatuses.Count == 0)
                {
                    if (seedData.TryGetValue("Statuses", out var statusesData))
                    {
                        generatedIds["Statuses"] = new List<Guid>();
                        var dtos = JsonSerializer.Deserialize<List<StatusDto>>(statusesData.GetRawText());
                        foreach (var dto in dtos)
                        {
                            var (model, error) = Status.Create(dto.Id, dto.Name);
                            if (!string.IsNullOrEmpty(error))
                                throw new InvalidOperationException($"Ошибка при создании Status: {error}");
                            await _statusService.CreateStatus(model);
                            generatedIds["Statuses"].Add(dto.Id);
                        }
                    }
                }
                else
                {
                    generatedIds["Statuses"] = existingStatuses.Select(s => s.Id).ToList();
                }

                var existingRequests = await _dbContext.Requests.OrderBy(r => r.Id).ToListAsync();
                if (existingRequests.Count == 0)
                {
                    if (seedData.TryGetValue("Requests", out var requestsData))
                    {
                        generatedIds["Requests"] = new List<Guid>();
                        var dtos = JsonSerializer.Deserialize<List<UserRequestDto>>(requestsData.GetRawText());
                        foreach (var dto in dtos)
                        {
                            var id = Guid.NewGuid();
                            var clientId = generatedIds["Clients"][dto.ClientIndex];
                            var vehicleId = generatedIds["Vehicles"][dto.VehicleIndex];
                            var statusId = generatedIds["Statuses"][dto.StatusIndex];
                            var (model, error) = UserRequest.Create(id, dto.Reason, dto.OpenDate, dto.CloseDate, clientId, vehicleId, statusId);
                            if (!string.IsNullOrEmpty(error))
                                throw new InvalidOperationException($"Ошибка при создании UserRequest: {error}");
                            await _requestService.CreateRequest(model);
                            generatedIds["Requests"].Add(id);
                        }
                    }
                }
                else
                {
                    generatedIds["Requests"] = existingRequests.Select(r => r.Id).ToList();
                }

                var existingWorks = await _dbContext.Works.OrderBy(w => w.Id).ToListAsync();
                if (existingWorks.Count == 0)
                {
                    if (seedData.TryGetValue("Works", out var worksData))
                    {
                        generatedIds["Works"] = new List<Guid>();
                        var dtos = JsonSerializer.Deserialize<List<WorkDto>>(worksData.GetRawText());
                        foreach (var dto in dtos)
                        {
                            var id = Guid.NewGuid();
                            var (model, error) = Work.Create(id, dto.Name, dto.Description, dto.Cost);
                            if (!string.IsNullOrEmpty(error))
                                throw new InvalidOperationException($"Ошибка при создании Work: {error}");
                            await _workService.CreateWork(model);
                            generatedIds["Works"].Add(id);
                        }
                    }
                }
                else
                {
                    generatedIds["Works"] = existingWorks.Select(w => w.Id).ToList();
                }

                var existingPlannedWorks = await _dbContext.PlannedWorks.OrderBy(pw => pw.Id).ToListAsync();
                if (existingPlannedWorks.Count == 0)
                {
                    if (seedData.TryGetValue("PlannedWorks", out var plannedWorksData))
                    {
                        generatedIds["PlannedWorks"] = new List<Guid>();
                        var dtos = JsonSerializer.Deserialize<List<PlannedWorkDto>>(plannedWorksData.GetRawText());
                        foreach (var dto in dtos)
                        {
                            var id = Guid.NewGuid();
                            var workId = generatedIds["Works"][dto.WorkIndex];
                            var requestId = generatedIds["Requests"][dto.RequestIndex];
                            var statusId = generatedIds["Statuses"][dto.StatusIndex];
                            var (model, error) = PlannedWork.Create(id, dto.PlanDate, dto.ExpectedEndDate, dto.TotalCost, workId, requestId, statusId);
                            if (!string.IsNullOrEmpty(error))
                                throw new InvalidOperationException($"Ошибка при создании PlannedWork: {error}");
                            await _plannedWorkService.CreatePlannedWork(model);
                            generatedIds["PlannedWorks"].Add(id);
                        }
                    }
                }
                else
                {
                    generatedIds["PlannedWorks"] = existingPlannedWorks.Select(pw => pw.Id).ToList();
                }


                var existingPartBrands = await _dbContext.PartBrands.OrderBy(pb => pb.Id).ToListAsync();
                if (existingPartBrands.Count == 0)
                {
                    if (seedData.TryGetValue("PartBrands", out var partBrandsData))
                    {
                        generatedIds["PartBrands"] = new List<Guid>();
                        var dtos = JsonSerializer.Deserialize<List<PartBrandDto>>(partBrandsData.GetRawText());
                        foreach (var dto in dtos)
                        {
                            var id = Guid.NewGuid();
                            var (model, error) = PartBrand.Create(id, dto.Name);
                            if (!string.IsNullOrEmpty(error))
                                throw new InvalidOperationException($"Ошибка при создании PartBrand: {error}");
                            await _partBrandService.CreatePartBrand(model);
                            generatedIds["PartBrands"].Add(id);
                        }
                    }
                }
                else
                {
                    generatedIds["PartBrands"] = existingPartBrands.Select(pb => pb.Id).ToList();
                }


                var existingParts = await _dbContext.Parts.OrderBy(p => p.Id).ToListAsync();
                if (existingParts.Count == 0)
                {
                    if (seedData.TryGetValue("Parts", out var partsData))
                    {
                        generatedIds["Parts"] = new List<Guid>();
                        var dtos = JsonSerializer.Deserialize<List<PartDto>>(partsData.GetRawText());
                        foreach (var dto in dtos)
                        {
                            var id = Guid.NewGuid();
                            var partBrandId = generatedIds["PartBrands"][dto.PartBrandIndex];
                            var (model, error) = Part.Create(id, dto.Name, dto.Article, dto.Cost, partBrandId);
                            if (!string.IsNullOrEmpty(error))
                                throw new InvalidOperationException($"Ошибка при создании Part: {error}");
                            await _partService.CreatePart(model);
                            generatedIds["Parts"].Add(id);
                        }
                    }
                }
                else
                {
                    generatedIds["Parts"] = existingParts.Select(p => p.Id).ToList();
                }

                var existingPlannedWorkEmployees = await _dbContext.PlannedWorkEmployees.OrderBy(pwe => pwe.PlannedWorkId).ToListAsync();
                if (existingPlannedWorkEmployees.Count == 0)
                {
                    if (seedData.TryGetValue("PlannedWorkEmployees", out var plannedWorkEmployeesData))
                    {
                        var dtos = JsonSerializer.Deserialize<List<PlannedWorkEmployeeDto>>(plannedWorkEmployeesData.GetRawText());
                        foreach (var dto in dtos)
                        {
                            var plannedWorkId = generatedIds["PlannedWorks"][dto.PlannedWorkIndex];
                            var employeeId = generatedIds["Employees"][dto.EmployeeIndex];
                            var (model, error) = PlannedWorkEmployee.Create(plannedWorkId, employeeId);
                            if (!string.IsNullOrEmpty(error))
                                throw new InvalidOperationException($"Ошибка при создании PlannedWorkEmployee: {error}");
                            await _plannedWorkEmployeeService.CreatePlannedWorkEmployee(model);
                        }
                    }
                }

                var existingPlannedWorkParts = await _dbContext.PlannedWorkParts.OrderBy(pwp => pwp.PlannedWorkId).ToListAsync();
                if (existingPlannedWorkParts.Count == 0)
                {
                    if (seedData.TryGetValue("PlannedWorkParts", out var plannedWorkPartsData))
                    {
                        var dtos = JsonSerializer.Deserialize<List<PlannedWorkPartDto>>(plannedWorkPartsData.GetRawText());
                        foreach (var dto in dtos)
                        {
                            var plannedWorkId = generatedIds["PlannedWorks"][dto.PlannedWorkIndex];
                            var partId = generatedIds["Parts"][dto.PartIndex];
                            var (model, error) = PlannedWorkPart.Create(plannedWorkId, partId, dto.Quantity);
                            if (!string.IsNullOrEmpty(error))
                                throw new InvalidOperationException($"Ошибка при создании PlannedWorkPart: {error}");
                            await _plannedWorkPartService.CreatePlannedWorkPart(model);
                        }
                    }
                }


                var existingTypeOfDays = await _dbContext.TypesOfDay.OrderBy(tod => tod.Id).ToListAsync();
                if (existingTypeOfDays.Count == 0)
                {
                    if (seedData.TryGetValue("TypeOfDays", out var typeOfDaysData))
                    {
                        generatedIds["TypeOfDays"] = new List<Guid>();
                        var dtos = JsonSerializer.Deserialize<List<TypeOfDayDto>>(typeOfDaysData.GetRawText());
                        foreach (var dto in dtos)
                        {
                            var id = Guid.NewGuid();
                            var (model, error) = TypeOfDay.Create(id, dto.Name);
                            if (!string.IsNullOrEmpty(error))
                                throw new InvalidOperationException($"Ошибка при создании TypeOfDay: {error}");
                            await _typeOfDayService.CreateTypeOfDay(model);
                            generatedIds["TypeOfDays"].Add(id);
                        }
                    }
                }
                else
                {
                    generatedIds["TypeOfDays"] = existingTypeOfDays.Select(tod => tod.Id).ToList();
                }


                var existingWorkDays = await _dbContext.WorkDays.OrderBy(wd => wd.Id).ToListAsync();
                if (existingWorkDays.Count == 0)
                {
                    if (seedData.TryGetValue("WorkDays", out var workDaysData))
                    {
                        generatedIds["WorkDays"] = new List<Guid>();
                        var dtos = JsonSerializer.Deserialize<List<WorkDayDto>>(workDaysData.GetRawText());
                        foreach (var dto in dtos)
                        {
                            var id = Guid.NewGuid();
                            var employeeId = generatedIds["Employees"][dto.EmployeeIndex];
                            var typeOfDayId = generatedIds["TypeOfDays"][dto.TypeOfDayIndex];
                            var (model, error) = WorkDay.Create(id, employeeId, typeOfDayId, dto.StartTime, dto.EndTime);
                            if (!string.IsNullOrEmpty(error))
                                throw new InvalidOperationException($"Ошибка при создании WorkDay: {error}");
                            await _workDayService.CreateWorkDay(model);
                            generatedIds["WorkDays"].Add(id);
                        }
                    }
                }
                else
                {
                    generatedIds["WorkDays"] = existingWorkDays.Select(wd => wd.Id).ToList();
                }

      
                var existingDiagnostics = await _dbContext.Diagnostics.OrderBy(d => d.Id).ToListAsync();
                if (existingDiagnostics.Count == 0)
                {
                    if (seedData.TryGetValue("Diagnostics", out var diagnosticsData))
                    {
                        generatedIds["Diagnostics"] = new List<Guid>();
                        var dtos = JsonSerializer.Deserialize<List<DiagnosticsDto>>(diagnosticsData.GetRawText());
                        foreach (var dto in dtos)
                        {
                            var id = Guid.NewGuid();
                            var employeeId = generatedIds["Employees"][dto.EmployeeIndex];
                            var requestId = generatedIds["Requests"][dto.RequestIndex];
                            var (model, error) = Diagnostics.Create(id, dto.DiagnosticsDate, dto.ResultDescription, employeeId, requestId);
                            if (!string.IsNullOrEmpty(error))
                                throw new InvalidOperationException($"Ошибка при создании Diagnostics: {error}");
                            await _diagnosticsService.CreateDiagnostics(model);
                            generatedIds["Diagnostics"].Add(id);
                        }
                    }
                }
                else
                {
                    generatedIds["Diagnostics"] = existingDiagnostics.Select(d => d.Id).ToList();
                }

        
                if (!_dbContext.Users.Any(u => u.Email == "admin@mail.ru"))
                {
                    var adminRoleId = new Guid("11111111-1111-1111-1111-111111111111"); // Админка
                    var adminId = Guid.NewGuid();
                    var password = "qwerty"; 
                    var hashedPassword = _passwordHasher.Generate(password);
                    var (adminUser, error) = User.Create(
                        adminId,
                        "admin",
                        hashedPassword,
                        "admin@mail.ru",
                        adminRoleId,
                        null, // ClientId
                        null  // EmployeeId
                    );
                    if (!string.IsNullOrEmpty(error))
                        throw new InvalidOperationException($"Ошибка при создании администратора: {error}");
                    await _userRepository.Add(adminUser);
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка при заполнении БД", ex);
            }
        }
    }
}