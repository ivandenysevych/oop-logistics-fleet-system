using Logistics.Domain.Models;

namespace Logistics.Application.Strategies;

/// <summary>
/// Strategy contract for delivery cost calculation.
/// </summary>
public interface ICostStrategy
{
    decimal CalculateCost(Delivery delivery);
}
