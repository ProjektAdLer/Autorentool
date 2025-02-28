using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using NUnit.Framework;
using Presentation.Components.Forms;

namespace IntegrationTest.Forms;

[TestFixture(TypeArgs = new[] { typeof(string) })]
[TestFixture(TypeArgs = new[] { typeof(int) })]
[TestFixture(TypeArgs = new[] { typeof(GridSelectTestType) })]
public class GridSelectIt<T> : MudBlazorTestFixture<GridSelect<T>> where T : notnull
{
    [SetUp]
    public void Init()
    {
        Elements = GridSelectItHelper.GetElementsForType<T>();
        ElementTemplate = GridSelectItHelper.GetElementTemplateForType<T>();
        Component = GetRenderedComponent();
        Assert.That(Value, Is.EqualTo(default(T)));
    }

    [TearDown]
    public void Teardown()
    {
        Component.Dispose();
    }

    private IEnumerable<T> Elements { get; set; }
    private RenderFragment<T> ElementTemplate { get; set; }
    private IRenderedComponent<GridSelect<T>> Component { get; set; }
    private T? Value => Component.Instance.Value;

    [Test]
    public void Render_RendersAllElements_WithTemplate()
    {
        var elements = FindAllMudcardDivs();
        Assert.That(elements, Has.Count.EqualTo(Elements.Count()));
        for (var i = 0; i < elements.Count; i++)
        {
            var element = elements[i];
            element.Children.Single().MarkupMatches(ElementTemplate(Elements.ElementAt(i)));
        }
    }

    [Test]
    public void ClickElement_SelectsElementAsValue()
    {
        var elements = FindAllMudcardDivs();
        Assert.Multiple(() =>
        {
            Assert.That(elements, Has.Count.GreaterThan(0));
            Assert.That(() => Value, Is.EqualTo(default(T)).After(300, 10));
        });
        for (var i = 0; i < elements.Count; i++)
        {
            //force re-evaluation after every re-render (click changes rendertree)
            elements = FindAllMudcardDivs();
            var element = elements[i];
            element.Click();
            Assert.That(() => Value, Is.EqualTo(Elements.ElementAt(i)).After(300, 10));
        }
    }

    [Test]
    public async Task ClickElementAgain_UnselectsElement()
    {
        var elements = FindAllMudcardDivs();
        Assert.Multiple(() =>
        {
            Assert.That(elements, Has.Count.GreaterThan(0));
            Assert.That(() => Value, Is.EqualTo(default(T)).After(300, 10), "Initial value is not default(T).");
        });
        for (var i = 0; i < elements.Count; i++)
        {
            //force re-evaluation after every re-render (click changes rendertree)
            elements = FindAllMudcardDivs();
            var element = elements.Single(ele => ele.ClassList.Contains($"element-{i}"));
            await element.ClickAsync(new MouseEventArgs());
            Assert.That(() => Value, Is.EqualTo(Elements.ElementAt(i)).After(300, 10));
            //force re-evaluation after every re-render (click changes rendertree)
            elements = FindAllMudcardDivs();
            element = elements.Single(ele => ele.ClassList.Contains($"element-{i}"));
            await element.ClickAsync(new MouseEventArgs());
            Assert.That(() => Value, Is.EqualTo(default(T)).After(300, 10), $"Unselecting element {i} failed.");
        }
    }

    [Test]
    public void ErrorSet_DisplaysError()
    {
#pragma warning disable BL0005
        Component.Instance.Error = true;
        Component.Instance.ErrorText = "some error";
#pragma warning restore BL0005
        Component.Render();

        var errorText = Component.FindComponent<MudText>();
        errorText.MarkupMatches("<p diff:ignoreAttributes>some error</p>");
    }

    private IRefreshableElementCollection<IElement> FindAllMudcardDivs()
    {
        return Component.FindAll("div.mud-card");
    }

    private IRenderedComponent<GridSelect<T>> GetRenderedComponent()
    {
        return Context.RenderComponent<GridSelect<T>>(p =>
        {
            p.Add(x => x.Elements, Elements);
            p.Add(x => x.ElementTemplate, ElementTemplate);
        });
    }
}

internal static class GridSelectItHelper
{
    private static RenderFragment<string> StringRenderer => str => builder =>
    {
        builder.OpenElement(0, "p");
        builder.AddContent(1, str);
        builder.CloseElement();
    };

    private static RenderFragment<int> IntRenderer => i => builder =>
    {
        builder.OpenElement(0, "p");
        builder.AddContent(1, i);
        builder.CloseElement();
    };

    private static RenderFragment<GridSelectTestType> GridSelectTestTypeRenderer => t => builder =>
    {
        builder.OpenElement(0, "p");
        builder.AddContent(1, t.Value1);
        builder.AddContent(2, t.Value2);
        builder.CloseElement();
    };

    internal static IEnumerable<T> GetElementsForType<T>()
    {
        if (typeof(T) == typeof(string)) return (new[] { "Foo1", "Foo2", "bar" } as IEnumerable<T>)!;
        if (typeof(T) == typeof(int)) return (new[] { 1, int.MaxValue, int.MinValue, } as IEnumerable<T>)!;
        if (typeof(T) == typeof(GridSelectTestType))
            return (new[] { new GridSelectTestType("Foo", 1), new GridSelectTestType("Bar", 123) } as IEnumerable<T>)!;
        throw new ArgumentOutOfRangeException();
    }

    internal static RenderFragment<T> GetElementTemplateForType<T>()
    {
        if (typeof(T) == typeof(string)) return (StringRenderer as RenderFragment<T>)!;
        if (typeof(T) == typeof(int)) return (IntRenderer as RenderFragment<T>)!;
        if (typeof(T) == typeof(GridSelectTestType)) return (GridSelectTestTypeRenderer as RenderFragment<T>)!;
        throw new ArgumentOutOfRangeException();
    }
}

public record GridSelectTestType(string Value1, int Value2);