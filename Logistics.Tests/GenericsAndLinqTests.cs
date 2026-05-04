using Logistics.Application.Extensions;
using Logistics.Application.Generics;
using Logistics.Domain.Models;

namespace Logistics.Tests;

public class GenericsAndLinqTests
{
    [Fact]
    public void InMemoryRepository_ShouldStoreAndFindItems()
    {
        var repository = new InMemoryRepository<Cargo>();
        repository.Add(new Cargo("Boxes", 100m));
        repository.Add(new Cargo("Steel", 800m));

        var heavyItems = repository.Find(item => item.Weight > 500m);

        Assert.Single(heavyItems);
        Assert.Equal("Steel", heavyItems.Single().Name);
    }

    [Fact]
    public void Cache_ShouldReturnStoredValueByKey()
    {
        var cache = new Cache<string, decimal>();
        cache.Set("delivery-1", 2500m);

        var found = cache.TryGet("delivery-1", out var value);

        Assert.True(found);
        Assert.Equal(2500m, value);
    }

    [Fact]
    public void ExtensionMethods_MapAndReduce_ShouldProcessCollection()
    {
        var values = new List<int> { 2, 4, 6 };

        var mapped = values.Map(value => value * 10);
        var reduced = values.Reduce(0, (sum, value) => sum + value);

        Assert.Equal(new[] { 20, 40, 60 }, mapped);
        Assert.Equal(12, reduced);
    }

    [Fact]
    public void Linq_GroupBy_ShouldGroupDeliveriesByVehicleType()
    {
        var deliveries = new List<Delivery>
        {
            new(new Truck(), new Cargo("A", 300m), new Route("K", "L", 100m)),
            new(new Van(), new Cargo("B", 200m), new Route("K", "M", 80m)),
            new(new Van(), new Cargo("C", 250m), new Route("K", "N", 90m))
        };

        var grouped = deliveries
            .GroupBy(delivery => delivery.Vehicle.GetType().Name)
            .ToDictionary(group => group.Key, group => group.Count());

        Assert.Equal(1, grouped["Truck"]);
        Assert.Equal(2, grouped["Van"]);
    }
}
