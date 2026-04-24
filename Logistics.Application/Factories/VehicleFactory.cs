using Logistics.Domain.Models;

namespace Logistics.Application.Factories;

/// <summary>
/// Factory Method for creating vehicles by type.
/// </summary>
public sealed class VehicleFactory
{
    public Vehicle Create(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
        {
            throw new ArgumentException("Vehicle type cannot be empty.", nameof(type));
        }

        return type.Trim().ToLowerInvariant() switch
        {
            "truck" => new Truck(),
            "van" => new Van(),
            _ => throw new ArgumentException($"Unsupported vehicle type: '{type}'.", nameof(type))
        };
    }
}
