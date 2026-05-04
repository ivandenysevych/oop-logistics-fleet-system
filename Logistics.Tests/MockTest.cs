using Moq;
using Xunit;

public class MockTest
{
    public interface IService
    {
        int GetValue();
    }

    [Fact]
    public void Mock_ReturnsValue()
    {
        var mock = new Mock<IService>();

        mock.Setup(x => x.GetValue()).Returns(10);

        var result = mock.Object.GetValue();

        Assert.Equal(10, result);
    }
}   