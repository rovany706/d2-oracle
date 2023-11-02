using D2Oracle.Core.Extensions;

namespace D2Oracle.Core.Tests.Extensions;

public class DotaExtensionsTests
{
    [Test]
    public void FormatAsDotaTime_WhenTimeSpanIsNull_ReturnEmpty()
    {
        TimeSpan? time = null;

        var actual = time.FormatAsDotaTime();
        
        Assert.That(actual, Is.Empty);
    }
    
    [Test]
    [TestCase(0, "0:00")]
    [TestCase(543, "9:03")]
    [TestCase(-125, "-2:05")]
    [TestCase(5000, "83:20")]
    public void FormatAsDotaTime_ReturnExpected(int seconds, string expected)
    {
        var time = TimeSpan.FromSeconds(seconds);

        var actual = time.FormatAsDotaTime();
        
        Assert.That(actual, Is.EqualTo(expected));
    }
}