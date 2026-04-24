namespace Logistics.Application.Decorators;

/// <summary>
/// Base decorator for delivery cost components.
/// </summary>
public abstract class DeliveryDecorator : IDeliveryCostComponent
{
    protected DeliveryDecorator(IDeliveryCostComponent innerComponent)
    {
        InnerComponent = innerComponent ?? throw new ArgumentNullException(nameof(innerComponent));
    }

    protected IDeliveryCostComponent InnerComponent { get; }

    public virtual decimal CalculateCost()
    {
        return InnerComponent.CalculateCost();
    }
}
