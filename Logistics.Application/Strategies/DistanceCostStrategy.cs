using Logistics.Domain.Models;

namespace Logistics.Application.Strategies;

/// <summary>
/// Cost depends on route distance: distance * 10.
/// </summary>
public sealed class DistanceCostStrategy : ICostStrategy
{
    private const decimal RatePerKilometer = 10m;

    public decimal CalculateCost(Delivery delivery)
    {
        ArgumentNullException.ThrowIfNull(delivery);
        return delivery.Route.Distance * RatePerKilometer;
    }
}
