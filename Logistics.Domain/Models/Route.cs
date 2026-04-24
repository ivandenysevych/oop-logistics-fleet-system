namespace Logistics.Domain.Models;

/// <summary>
/// Delivery route between two points.
/// </summary>
public sealed class Route
{
    public Route()
        : this("Unknown origin", "Unknown destination", 1m)
    {
    }

    public Route(string from, string to, decimal distance)
    {
        if (string.IsNullOrWhiteSpace(from))
        {
            throw new ArgumentException("Route start point cannot be empty.", nameof(from));
        }

        if (string.IsNullOrWhiteSpace(to))
        {
            throw new ArgumentException("Route destination cannot be empty.", nameof(to));
        }

        if (distance <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(distance), "Route distance must be greater than zero.");
        }

        From = from;
        To = to;
        Distance = distance;
    }

    public Route(Route source)
        : this(
            source?.From ?? throw new ArgumentNullException(nameof(source)),
            source.To,
            source.Distance)
    {
    }

    public string From { get; }

    public string To { get; }

    public decimal Distance { get; }
}
