namespace Logistics.Domain.Models;

/// <summary>
/// Full delivery details.
/// </summary>
public sealed class Delivery
{
    public Delivery()
        : this(new Truck(), new Cargo(), new Route())
    {
    }

    public Delivery(Vehicle vehicle, Cargo cargo, Route route)
    {
        Vehicle = vehicle ?? throw new ArgumentNullException(nameof(vehicle));
        Cargo = cargo ?? throw new ArgumentNullException(nameof(cargo));
        Route = route ?? throw new ArgumentNullException(nameof(route));
    }

    public Delivery(Delivery source)
        : this(
            CloneVehicle(source?.Vehicle ?? throw new ArgumentNullException(nameof(source))),
            new Cargo(source.Cargo),
            new Route(source.Route))
    {
    }

    public Vehicle Vehicle { get; }

    public Cargo Cargo { get; }

    public Route Route { get; }

    private static Vehicle CloneVehicle(Vehicle source)
    {
        return source switch
        {
            Truck truck => new Truck(truck),
            Van van => new Van(van),
            _ => throw new NotSupportedException($"Unsupported vehicle type: {source.GetType().Name}")
        };
    }
}
