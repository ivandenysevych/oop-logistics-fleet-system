namespace Logistics.Domain.Models;

/// <summary>
/// Cargo to be transported.
/// </summary>
public sealed class Cargo
{
    public Cargo()
        : this("Undefined cargo", 1m)
    {
    }

    public Cargo(string name, decimal weight)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Cargo name cannot be empty.", nameof(name));
        }

        if (weight <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(weight), "Cargo weight must be greater than zero.");
        }

        Name = name;
        Weight = weight;
    }

    public Cargo(Cargo source)
        : this(source?.Name ?? throw new ArgumentNullException(nameof(source)), source.Weight)
    {
    }

    public string Name { get; }

    public decimal Weight { get; }
}
