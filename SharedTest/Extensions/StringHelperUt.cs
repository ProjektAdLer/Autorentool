using NUnit.Framework;
using Shared.Extensions;

namespace SharedTest.Extensions;

[TestFixture]
public class StringHelperUt
{
    [Test]
    [TestCase("Foo", "Foo(1)")]
    [TestCase("Foo(0)", "Foo(1)")]
    [TestCase("Foo(1)", "Foo(2)")]
    [TestCase("Foo(10)", "Foo(11)")]
    [TestCase("Foo(-1)", "Foo(-1)(1)")]
    [TestCase("Foo1)", "Foo1)(1)")]
    [TestCase("Foo1", "Foo1(1)")]
    [TestCase("Foo(1", "Foo(1(1)")]
    public void IncrementName_IncrementsCorrectly(string input, string expected)
    {
        var actual = StringHelper.IncrementName(input);
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(new[]{"bla"}, "Foo", "Foo")]
    [TestCase(new string[]{}, "Foo", "Foo")]
    [TestCase(new[]{""}, "Foo", "Foo")]
    [TestCase(new[]{"Foo"}, "Foo", "Foo(1)")]
    [TestCase(new[]{"Foo", "Foo(1)"}, "Foo", "Foo(2)")]
    [TestCase(new[]{"Foo", "Foo(1)","Foo(3)"}, "Foo", "Foo(2)")]
    [TestCase(new[]{"Foo", "Foo1"}, "Foo", "Foo(1)")]
    public void GetUniqueName_AlwaysGivesUniqueNames(IEnumerable<string> taken, string input, string expected)
    {
        var actual = StringHelper.GetUniqueName(taken, input);
        Assert.That(actual, Is.EqualTo(expected));
    }
}