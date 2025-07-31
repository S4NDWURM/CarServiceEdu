using CarService.DataAccess.Entities;

public class VehicleEntity
{
    public Guid Id { get; set; }
    public string VIN { get; set; }
    public int Year { get; set; }

    public Guid GenerationId { get; set; }

    public GenerationEntity Generation { get; set; }

    public ICollection<RequestEntity> Requests { get; set; } = [];
}