using NUnit.Framework;
using Shared;

namespace SharedTest;

[TestFixture]
public class ValidationResultUt
{
    [Test]
    public void IsValid_WhenNoErrors_ReturnsTrue()
    {
        var result = new ValidationResult();

        Assert.That(result.IsValid, Is.True);
    }

    [Test]
    public void IsValid_WhenHasErrors_ReturnsFalse()
    {
        var result = new ValidationResult();
        result.Errors.Add("Error 1");

        Assert.That(result.IsValid, Is.False);
    }

    [Test]
    public void ToHtmlList_WhenNoErrors_ReturnsEmptyList()
    {
        var result = new ValidationResult();

        var html = result.ToHtmlList();

        Assert.That(html, Is.EqualTo("<ul></ul>"));
    }

    [Test]
    public void ToHtmlList_WithMultipleErrors_ReturnsListWithErrors()
    {
        var result = new ValidationResult();
        result.Errors.Add("<li>Error 1</li>");
        result.Errors.Add("<li>Error 2</li>");

        var html = result.ToHtmlList();

        var expected = $"<ul><li>Error 1</li>{Environment.NewLine}<li>Error 2</li></ul>";
        Assert.That(html, Is.EqualTo(expected));
    }
}