namespace Logistics.Application.Decorators;

/// <summary>
/// Adds insurance surcharge to delivery cost (+100).
/// </summary>
public sealed class InsuranceDecorator : DeliveryDecorator
{
    private const decimal InsuranceFee = 100m;

    public InsuranceDecorator(IDeliveryCostComponent innerComponent)
        : base(innerComponent)
    {
    }

    public override decimal CalculateCost()
    {
        return base.CalculateCost() + InsuranceFee;
    }
}
