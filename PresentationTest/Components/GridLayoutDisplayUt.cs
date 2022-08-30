using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bunit;
using Microsoft.AspNetCore.Components;
using NUnit.Framework;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components;

[TestFixture]
public class GridLayoutDisplayUt
{
#pragma warning disable CS8618
    private TestContext _testContext;
#pragma warning restore CS8618

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
    }

    [TearDown]
    public void TearDown() => _testContext.Dispose();

    [Test]
    public void StandardConstructor_AllPropertiesInitialized()
    {
        var items = new List<int> { 1, 2, 3, 4, 5 };
        const string headerTitle = "This is a header";
        const uint itemsPerRow = 3u;

        var systemUnderTest = CreateRenderedGridLayoutComponent(items, Template, headerTitle, itemsPerRow);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.Items, Is.EqualTo(items));
            Assert.That(systemUnderTest.Instance.ItemRenderTemplate, Is.EqualTo((RenderFragment<int>) Template));
            Assert.That(systemUnderTest.Instance.HeaderTitle, Is.EqualTo(headerTitle));
            Assert.That(systemUnderTest.Instance.ItemsPerRow, Is.EqualTo(itemsPerRow));
        });
    }

    [Test]
    [TestCaseSource(typeof(GridLayoutDisplayTestCases))]
    public void StandardConstructor_CreatesLayoutCorrectly(IEnumerable<int> items, uint itemsPerRow,
        IEnumerable<int> expectedRowLengths)
    {
        var systemUnderTest = CreateRenderedGridLayoutComponent(items, Template, "foo", itemsPerRow);

        var rows = systemUnderTest.FindAll(".row").ToArray();
        var rowLengths = expectedRowLengths as int[] ?? expectedRowLengths.ToArray();

        Assert.That(rows, Has.Length.EqualTo(rowLengths.Length));

        foreach (var (rowLength, index) in rowLengths.Select((item, index) => (item, index)))
        {
            Assert.That(rows[index].QuerySelectorAll("text").Count, Is.EqualTo(rowLength));
        }
    }

    private RenderFragment Template(int val) => builder =>
        builder.AddMarkupContent(0, $"<text>{val}</text>");

    private IRenderedComponent<GridLayoutDisplay<T>> CreateRenderedGridLayoutComponent<T>(IEnumerable<T> items,
        RenderFragment<T> template, string headerTitle, uint itemsPerRow)
    {
        return _testContext.RenderComponent<GridLayoutDisplay<T>>(parameterBuilder => parameterBuilder
            .Add(p => p.Items, items)
            .Add(p => p.ItemRenderTemplate, template)
            .Add(p => p.HeaderTitle, headerTitle)
            .Add(p => p.ItemsPerRow, itemsPerRow)
        );
    }
}

internal class GridLayoutDisplayTestCases : IEnumerable
{
    public IEnumerator GetEnumerator()
    {
        yield return new object[] //fits in row
        {
            new[] { 1, 2, 3, 4, 5, 6 },
            3u,
            new[] {3, 3}
        };
        yield return new object[] //doesn't fit in row
        {
            new[] { 1, 2, 3, 4, 5 },
            3u,
            new[] { 3, 2 }
        };
        yield return new object[] //one item per row
        {
            new[] { 1, 2, 3, 4, 5 },
            1u,
            new[] { 1, 1, 1, 1, 1 }
        };
    }
}