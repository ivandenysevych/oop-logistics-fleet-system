using Logistics.Application.Decorators;
using Logistics.Application.Factories;
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

        // LINQ: convert scenarios into output-ready reports.
        var reports = scenarios
            .Select((scenario, index) => BuildReport(index + 1, scenario, deliveryService))
            .ToList();

        foreach (var report in reports)
        {
            PrintReport(report);
        }

        var successfulCosts = reports
            .Where(report => report.IsSuccess)
            .Select(report => report.Cost)
            .ToList();

        if (successfulCosts.Count == 0)
        {
            Console.WriteLine("No successful deliveries.");
            return;
        }

        Console.WriteLine("----- Summary -----");
        Console.WriteLine($"Successful deliveries: {successfulCosts.Count}");
        Console.WriteLine($"Total cost: {successfulCosts.Sum():0.##}");
        Console.WriteLine($"Average cost: {successfulCosts.Average():0.##}");
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
