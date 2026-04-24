using Logistics.Application.Strategies;
using Logistics.Domain.Models;

namespace Logistics.Application.Services;

/// <summary>
/// Delivery service with validation and cost calculation.
/// </summary>
public sealed class DeliveryService
{
    public decimal CalculateCost(Delivery delivery, ICostStrategy strategy)
    {
        ArgumentNullException.ThrowIfNull(delivery);
        ArgumentNullException.ThrowIfNull(strategy);

        ValidateVehicleCapacity(delivery);
        return strategy.CalculateCost(delivery);
    }

    private static void ValidateVehicleCapacity(Delivery delivery)
    {
        if (delivery.Cargo.Weight > delivery.Vehicle.Capacity)
        {
            throw new InvalidOperationException(
                $"Cargo weight ({delivery.Cargo.Weight}) exceeds vehicle capacity ({delivery.Vehicle.Capacity}).");
        }
    }
}
