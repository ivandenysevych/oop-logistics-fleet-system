using Logistics.Domain.Models;

namespace Logistics.Application.Strategies;

/// <summary>
/// Cost depends on cargo weight: weight * 5.
/// </summary>
public sealed class WeightCostStrategy : ICostStrategy
{
    private const decimal RatePerKilogram = 5m;

    public decimal CalculateCost(Delivery delivery)
    {
        ArgumentNullException.ThrowIfNull(delivery);
        return delivery.Cargo.Weight * RatePerKilogram;
    }
}
