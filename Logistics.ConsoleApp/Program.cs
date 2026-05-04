using Logistics.Application.Decorators;
using Logistics.Application.Extensions;
using Logistics.Application.Factories;
using Logistics.Application.Generics;
using Logistics.Application.Services;
using Logistics.Application.Strategies;
using Logistics.Domain.Models;

namespace Logistics.ConsoleApp;

internal static class Program
{
    private static void Main()
    {
        var factory = new VehicleFactory();
        var deliveryService = new DeliveryService();
        var repository = new InMemoryRepository<Delivery>();
        var costCache = new Cache<string, decimal>();

        var scenarios = new List<DeliveryScenario>
        {
            new(
                new Delivery(
                    factory.Create("truck"),
                    new Cargo("Metal constructions", 1800m),
                    new Route("Kyiv", "Lviv", 540m)),
                new DistanceCostStrategy(),
                WithInsurance: false),
            new(
                new Delivery(
                    factory.Create("van"),
                    new Cargo("Office supplies", 300m),
                    new Route("Odesa", "Mykolaiv", 130m)),
                new WeightCostStrategy(),
                WithInsurance: true),
            new(
                new Delivery(
                    factory.Create("van"),
                    new Cargo("Industrial machine", 1500m),
                    new Route("Kharkiv", "Dnipro", 220m)),
                new DistanceCostStrategy(),
                WithInsurance: false)
        };

        scenarios.ForEachItem(scenario => repository.Add(scenario.Delivery));

        // Delegate + lambda: reusable predicate for selection.
        Func<Delivery, bool> heavyCargoFilter = delivery => delivery.Cargo.Weight >= 1000m;
        var heavyDeliveries = repository.Find(heavyCargoFilter);
        Console.WriteLine($"Heavy cargo deliveries (>= 1000): {heavyDeliveries.Count}");
        Console.WriteLine();

        // LINQ: convert scenarios into output-ready reports.
        var reports = scenarios
            .Select((scenario, index) => BuildReport(index + 1, scenario, deliveryService))
            .ToList();

        // Delegate + extension method: batch output.
        Action<DeliveryReport> reportPrinter = PrintReport;
        reports.ForEachItem(reportPrinter);

        var successfulCosts = reports
            .Where(report => report.IsSuccess)
            .Select(report => report.Cost)
            .ToList();

        if (successfulCosts.Count == 0)
        {
            Console.WriteLine("No successful deliveries.");
            return;
        }

        var indexedCosts = reports
            .Where(report => report.IsSuccess)
            .ToDictionary(report => report.Number, report => report.Cost);
        foreach (var pair in indexedCosts)
        {
            costCache.Set($"delivery-{pair.Key}", pair.Value);
        }

        // Query syntax example.
        var expensiveReports =
            from report in reports
            where report.IsSuccess && report.Cost > 3000m
            select report;

        // GroupBy + projection.
        var groupedByVehicle = reports
            .Where(report => report.IsSuccess)
            .GroupBy(report => report.Delivery.Vehicle.GetType().Name)
            .Select(group => new VehicleGroupReport(
                group.Key,
                group.Count(),
                group.Sum(report => report.Cost)))
            .ToList();

        // Map + Reduce extensions over a typed collection.
        var costLabels = successfulCosts.Map(cost => $"Cost item: {cost:0.##}");
        var totalByReduce = successfulCosts.Reduce(0m, (sum, current) => sum + current);

        Console.WriteLine("----- Summary -----");
        Console.WriteLine($"Successful deliveries: {successfulCosts.Count}");
        Console.WriteLine($"Total cost: {successfulCosts.Sum():0.##}");
        Console.WriteLine($"Total by Reduce extension: {totalByReduce:0.##}");
        Console.WriteLine($"Average cost: {successfulCosts.Average():0.##}");
        Console.WriteLine($"Expensive deliveries (> 3000): {expensiveReports.Count()}");
        Console.WriteLine();

        Console.WriteLine("----- Grouped By Vehicle -----");
        groupedByVehicle.ForEachItem(group =>
            Console.WriteLine($"{group.VehicleType}: Count={group.Count}, Total={group.TotalCost:0.##}"));
        Console.WriteLine();

        Console.WriteLine("----- Labels (Map extension) -----");
        costLabels.ForEachItem(Console.WriteLine);
    }

    private static DeliveryReport BuildReport(int number, DeliveryScenario scenario, DeliveryService service)
    {
        try
        {
            IDeliveryCostComponent component = new BaseDeliveryCostComponent(service, scenario.Delivery, scenario.Strategy);
            if (scenario.WithInsurance)
            {
                component = new InsuranceDecorator(component);
            }

            var cost = component.CalculateCost();
            return new DeliveryReport(number, scenario.Delivery, cost, true, null);
        }
        catch (Exception ex)
        {
            return new DeliveryReport(number, scenario.Delivery, 0m, false, ex.Message);
        }
    }

    private static void PrintReport(DeliveryReport report)
    {
        Console.WriteLine($"----- Delivery #{report.Number} -----");
        Console.WriteLine($"Vehicle: {report.Delivery.Vehicle.Name} ({report.Delivery.Vehicle.GetType().Name})");
        Console.WriteLine($"Capacity: {report.Delivery.Vehicle.Capacity}");
        Console.WriteLine($"Cargo: {report.Delivery.Cargo.Name}, Weight: {report.Delivery.Cargo.Weight}");
        Console.WriteLine(
            $"Route: {report.Delivery.Route.From} -> {report.Delivery.Route.To}, Distance: {report.Delivery.Route.Distance}");

        if (report.IsSuccess)
        {
            Console.WriteLine($"Calculated cost: {report.Cost:0.##}");
        }
        else
        {
            Console.WriteLine($"Error: {report.Error}");
        }

        Console.WriteLine();
    }
}

internal sealed record DeliveryScenario(Delivery Delivery, ICostStrategy Strategy, bool WithInsurance);

internal sealed record DeliveryReport(int Number, Delivery Delivery, decimal Cost, bool IsSuccess, string? Error);

internal sealed record VehicleGroupReport(string VehicleType, int Count, decimal TotalCost);
