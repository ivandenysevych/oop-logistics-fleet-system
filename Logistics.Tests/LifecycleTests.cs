using Logistics.Domain.Lifecycle;
using Logistics.Domain.Models;

namespace Logistics.Tests;

public class LifecycleTests
{
    [Fact]
    public void Cargo_Constructors_ShouldSupportDefaultParameterizedAndCopy()
    {
        var source = new Cargo("Medical kits", 250m);
        var copy = new Cargo(source);
        var @default = new Cargo();

        Assert.Equal("Medical kits", source.Name);
        Assert.Equal(250m, source.Weight);
        Assert.Equal(source.Name, copy.Name);
        Assert.Equal(source.Weight, copy.Weight);
        Assert.NotSame(source, copy);
        Assert.Equal("Undefined cargo", @default.Name);
        Assert.Equal(1m, @default.Weight);
    }

    [Fact]
    public void Delivery_CopyConstructor_ShouldCreateDeepCopy()
    {
        var source = new Delivery(
            new Van("Urban Van", 900m),
            new Cargo("Home appliances", 450m),
            new Route("Kyiv", "Cherkasy", 190m));

        var copy = new Delivery(source);

        Assert.NotSame(source, copy);
        Assert.NotSame(source.Vehicle, copy.Vehicle);
        Assert.NotSame(source.Cargo, copy.Cargo);
        Assert.NotSame(source.Route, copy.Route);
        Assert.Equal(source.Vehicle.Name, copy.Vehicle.Name);
        Assert.Equal(source.Cargo.Weight, copy.Cargo.Weight);
        Assert.Equal(source.Route.Distance, copy.Route.Distance);
    }

    [Fact]
    public void CargoManifestBuffer_CopyConstructor_ShouldCloneBufferContent()
    {
        using var source = new CargoManifestBuffer(4);
        source.WriteByte(0, 11);
        source.WriteByte(1, 22);

        using var copy = new CargoManifestBuffer(source);

        Assert.Equal(11, copy.ReadByte(0));
        Assert.Equal(22, copy.ReadByte(1));
    }

    [Fact]
    public void CargoManifestBuffer_Dispose_ShouldBlockFurtherAccess()
    {
        var buffer = new CargoManifestBuffer(8);
        buffer.WriteByte(0, 77);

        buffer.Dispose();

        Assert.True(buffer.IsDisposed);
        Assert.Throws<ObjectDisposedException>(() => buffer.ReadByte(0));
    }
}
