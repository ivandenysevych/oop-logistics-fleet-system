namespace Logistics.Domain.Models;

/// <summary>
/// Base transport entity for deliveries.
/// </summary>
public abstract class Vehicle
{
    protected Vehicle()
        : this("Unnamed vehicle", 1m)
    {
    }

    protected Vehicle(string name, decimal capacity)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Vehicle name cannot be empty.", nameof(name));
        }

        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Vehicle capacity must be greater than zero.");
        }

        Name = name;
        Capacity = capacity;
    }

    protected Vehicle(Vehicle source)
        : this(source?.Name ?? throw new ArgumentNullException(nameof(source)), source.Capacity)
    {
    }

    public string Name { get; }

    public decimal Capacity { get; }
}
