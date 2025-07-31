using System;
using System.Text.RegularExpressions;

namespace CarService.Core.Models
{
    public class Vehicle
    {
        private Vehicle(Guid id, string vIN, int year, Guid generationId)
        {
            Id = id;
            VIN = vIN;
            Year = year;
            GenerationId = generationId;
        }

        public Guid Id { get; }
        public string VIN { get; }
        public int Year { get; }
        public Guid GenerationId { get; }

        public static (Vehicle? Item, string Error) Create(Guid id, string vIN, int year, Guid generationId)
        {
            string error = string.Empty;

            if (id == Guid.Empty)
            {
                return (null, "Id cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(vIN) || !IsValidVIN(vIN))
            {
                return (null, "Invalid VIN. It must be exactly 17 characters long and contain only uppercase letters and digits.");
            }

            if (year < 1800)
            {
                return (null, "Year must be between 1800 and the current year.");
            }

            if (generationId == Guid.Empty)
            {
                return (null, "GenerationId cannot be empty.");
            }

            var item = new Vehicle(id, vIN, year, generationId);
            return (item, error);
        }

        private static bool IsValidVIN(string vin)
        {
            var vinRegex = @"(?=.*\d|=.*[A-Z])(?=.*[A-Z])[A-Z0-9]{17}";
            var regex = new Regex(vinRegex);
            return regex.IsMatch(vin);
        }
    }
}
