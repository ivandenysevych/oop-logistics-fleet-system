using Logistics.Application.Factories;
using Logistics.Application.Services;
using Logistics.Application.Strategies;
using Logistics.Domain.Models;

namespace Logistics.Tests;

public class LogisticsPatternsTests
{
    [Fact]
    public void DistanceCostStrategy_ShouldMultiplyDistanceByTen()
    {
        var delivery = new Delivery(
            new Truck("Heavy Truck", 6000m),
            new Cargo("Steel", 800m),
            new Route("A", "B", 120m));

        var strategy = new DistanceCostStrategy();
        var cost = strategy.CalculateCost(delivery);

        Assert.Equal(1200m, cost);
    }

    [Fact]
    public void WeightCostStrategy_ShouldMultiplyWeightByFive()
    {
        var delivery = new Delivery(
            new Van("City Van", 1000m),
            new Cargo("Boxes", 150m),
            new Route("A", "B", 90m));

        var strategy = new WeightCostStrategy();
        var cost = strategy.CalculateCost(delivery);

        Assert.Equal(750m, cost);
    }

    [Fact]
    public void VehicleFactory_CreateTruck_ShouldReturnTruck()
    {
        var factory = new VehicleFactory();

        var vehicle = factory.Create("truck");

        Assert.IsType<Truck>(vehicle);
    }

    [Fact]
    public void DeliveryService_ShouldThrow_WhenCargoExceedsVehicleCapacity()
    {
        var service = new DeliveryService();
        var strategy = new DistanceCostStrategy();
        var delivery = new Delivery(
            new Van("Small Van", 200m),
            new Cargo("Engine", 350m),
            new Route("A", "B", 40m));

        Assert.Throws<InvalidOperationException>(() => service.CalculateCost(delivery, strategy));
    }
}
