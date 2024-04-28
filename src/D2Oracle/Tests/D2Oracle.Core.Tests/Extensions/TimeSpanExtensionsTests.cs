using D2Oracle.Core.Extensions;

namespace D2Oracle.Core.Tests.Extensions;

public class TimeSpanExtensionsTests
{
    [Test]
    [TestCase(1, 2, 0, false)]
    [TestCase(2, 1, 0, false)]
    [TestCase(1, 1, 0, true)]
    [TestCase(1, 2, 1, true)]
    [TestCase(1, 3, 3, true)]
    [TestCase(1, 3, 1, false)]
    public void EqualsWithPrecision_Always_ReturnExpected(double valueInSeconds, double otherValueInSeconds,
        double precisionInSeconds, bool expected)
    {
        var value = TimeSpan.FromSeconds(valueInSeconds);
        var otherValue = TimeSpan.FromSeconds(otherValueInSeconds);
        var precision = TimeSpan.FromSeconds(precisionInSeconds);

        // ReSharper disable once InvokeAsExtensionMethod
        var actual = TimeSpanExtensions.EqualsWithPrecision(value, otherValue, precision);

        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void EqualsWithPrecisionNullable_WhenValueIsNull_ReturnFalse()
    {
        TimeSpan? value = null;
        var otherValue = TimeSpan.FromSeconds(1);
        var precision = TimeSpan.FromSeconds(0);
        
        // ReSharper disable once InvokeAsExtensionMethod
        Assert.That(TimeSpanExtensions.EqualsWithPrecision(value, otherValue, precision), Is.False);
    }
    
    [Test]
    public void EqualsWithPrecisionNullable_WhenOtherValueIsNull_ReturnFalse()
    {
        TimeSpan? value = TimeSpan.FromSeconds(1);
        TimeSpan? otherValue = null;
        var precision = TimeSpan.FromSeconds(0);
        
        // ReSharper disable once InvokeAsExtensionMethod
        Assert.That(TimeSpanExtensions.EqualsWithPrecision(value, otherValue, precision), Is.False);
    }
    
    [Test]
    [TestCase(1, 2, 0, false)]
    [TestCase(2, 1, 0, false)]
    [TestCase(1, 1, 0, true)]
    [TestCase(1, 2, 1, true)]
    [TestCase(1, 3, 3, true)]
    [TestCase(1, 3, 1, false)]
    public void EqualsWithPrecisionNullable_Always_ReturnExpected(double valueInSeconds, double otherValueInSeconds,
        double precisionInSeconds, bool expected)
    {
        TimeSpan? value = TimeSpan.FromSeconds(valueInSeconds);
        TimeSpan? otherValue = TimeSpan.FromSeconds(otherValueInSeconds);
        var precision = TimeSpan.FromSeconds(precisionInSeconds);

        // ReSharper disable once InvokeAsExtensionMethod
        var actual = TimeSpanExtensions.EqualsWithPrecision(value, otherValue, precision);

        Assert.That(actual, Is.EqualTo(expected));
    }
}