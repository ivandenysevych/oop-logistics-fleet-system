namespace Logistics.Domain.Models;

/// <summary>
/// Truck for heavy cargo deliveries.
/// </summary>
public sealed class Truck : Vehicle
{
    public Truck()
        : this("Standard Truck", 5000m)
    {
    }

    public Truck(string name, decimal capacity)
        : base(name, capacity)
    {
    }

    public Truck(Truck source)
        : base(source)
    {
    }
}
