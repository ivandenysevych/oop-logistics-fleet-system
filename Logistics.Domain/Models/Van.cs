namespace Logistics.Domain.Models;

/// <summary>
/// Van for city and medium deliveries.
/// </summary>
public sealed class Van : Vehicle
{
    public Van()
        : this("Standard Van", 1200m)
    {
    }

    public Van(string name, decimal capacity)
        : base(name, capacity)
    {
    }

    public Van(Van source)
        : base(source)
    {
    }
}
