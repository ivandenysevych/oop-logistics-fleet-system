namespace Logistics.Application.Decorators;

/// <summary>
/// Component that returns final delivery cost.
/// </summary>
public interface IDeliveryCostComponent
{
    decimal CalculateCost();
}
