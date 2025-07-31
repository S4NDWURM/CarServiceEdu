namespace CarService.Application.Services
{
    internal class EmployeeDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleIndex { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public int WorkExperience { get; set; }
        public DateTime HireDate { get; set; }
        public int EmployeeStatusIndex { get; set; }
    }

    internal class ClientDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }
    }

    internal class EmployeeStatusDto { public string Name { get; set; } }
    internal class SpecializationDto { public string Name { get; set; } }
    internal class EmployeeSpecializationDto
    {
        public int EmployeeIndex { get; set; }
        public int SpecializationIndex { get; set; }
    }
    internal class CarBrandDto { public string Name { get; set; } }
    internal class CarModelDto
    {
        public string Name { get; set; }
        public int CarBrandIndex { get; set; }
    }
    internal class GenerationDto
    {
        public int CarModelIndex { get; set; }
        public string Name { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
    }
    internal class VehicleDto
    {
        public string VIN { get; set; }
        public int Year { get; set; }
        public int GenerationIndex { get; set; }
    }
    internal class StatusDto { 
        public Guid Id { get; set; }
        public string Name { get; set; } 
    }
    internal class UserRequestDto
    {
        public string Reason { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public int ClientIndex { get; set; }
        public int VehicleIndex { get; set; }
        public int StatusIndex { get; set; }
    }
    internal class WorkDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
    }
    internal class PlannedWorkDto
    {
        public DateTime PlanDate { get; set; }
        public DateTime ExpectedEndDate { get; set; }
        public decimal TotalCost { get; set; }
        public int WorkIndex { get; set; }
        public int RequestIndex { get; set; }
        public int StatusIndex { get; set; }
    }
    internal class PartBrandDto { public string Name { get; set; } }
    internal class PartDto
    {
        public string Name { get; set; }
        public string Article { get; set; }
        public decimal Cost { get; set; }
        public int PartBrandIndex { get; set; }
    }
    internal class PlannedWorkEmployeeDto
    {
        public int PlannedWorkIndex { get; set; }
        public int EmployeeIndex { get; set; }
    }
    internal class PlannedWorkPartDto
    {
        public int PlannedWorkIndex { get; set; }
        public int PartIndex { get; set; }
        public int Quantity { get; set; }
    }
    internal class TypeOfDayDto { public string Name { get; set; } }
    internal class WorkDayDto
    {
        public int EmployeeIndex { get; set; }
        public int TypeOfDayIndex { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
    internal class DiagnosticsDto
    {
        public DateTime DiagnosticsDate { get; set; }
        public string ResultDescription { get; set; }
        public int EmployeeIndex { get; set; }
        public int RequestIndex { get; set; }
    }
}