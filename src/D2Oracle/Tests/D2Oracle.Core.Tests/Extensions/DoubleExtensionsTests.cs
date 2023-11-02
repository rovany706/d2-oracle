using D2Oracle.Core.Extensions;

namespace D2Oracle.Core.Tests.Extensions;

public class DoubleExtensionsTests
{
    [Test]
    [TestCase(5, 7, 7)]
    [TestCase(8, 5, 10)]
    [TestCase(45, 0, 0)]
    [TestCase(4, -5, 0)]
    [TestCase(-7, -5, -5)]
    public void ClosestMultipleCeil_ReturnExpected(double value, double multiple, double expected)
    {
        var actual = value.ClosestMultipleCeil(multiple);
        
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [Test]
    [TestCase(5, 7, 0)]
    [TestCase(8, 5, 5)]
    [TestCase(45, 0, 0)]
    [TestCase(4, -5, 5)]
    [TestCase(-7, -5, -5)]
    public void ClosestMultipleFloor_ReturnExpected(double value, double multiple, double expected)
    {
        var actual = value.ClosestMultipleFloor(multiple);
        
        Assert.That(actual, Is.EqualTo(expected));
    }
}