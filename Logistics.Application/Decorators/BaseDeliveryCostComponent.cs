using Logistics.Application.Services;
using Logistics.Application.Strategies;
using Logistics.Domain.Models;

namespace Logistics.Application.Decorators;

/// <summary>
/// Base delivery cost without add-ons.
/// </summary>
public sealed class BaseDeliveryCostComponent : IDeliveryCostComponent
{
    private readonly DeliveryService _deliveryService;
    private readonly Delivery _delivery;
    private readonly ICostStrategy _strategy;

    public BaseDeliveryCostComponent(DeliveryService deliveryService, Delivery delivery, ICostStrategy strategy)
    {
        _deliveryService = deliveryService ?? throw new ArgumentNullException(nameof(deliveryService));
        _delivery = delivery ?? throw new ArgumentNullException(nameof(delivery));
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
    }

    public decimal CalculateCost()
    {
        return _deliveryService.CalculateCost(_delivery, _strategy);
    }
}
