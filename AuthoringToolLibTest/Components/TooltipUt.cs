using AngleSharp.Dom;
using AuthoringToolLib.Components;
using Bunit;
using Microsoft.AspNetCore.Components;
using NUnit.Framework;
using TestContext = Bunit.TestContext;

namespace AuthoringToolLibTest.Components;

[TestFixture]
public class TooltipUt
{
#pragma warning disable CS8618
    private TestContext _testContext;
#pragma warning restore CS8618

    private const string TooltipText = "This is the text.";
    
    private readonly RenderFragment _childContent = builder =>
    {
        builder.OpenElement(1, "button");
        builder.AddContent(2, "foo foo bar bar");
        builder.CloseElement();
    };
    
    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
    }

    [TearDown]
    public void TearDown() => _testContext.Dispose();
    
    [Test]
    public void Constructor_PropertiesSet()
    {
        var systemUnderTest = CreateRenderedTooltipComponent(TooltipText, _childContent);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.Text, Is.EqualTo(TooltipText));
            Assert.That(systemUnderTest.Instance.ChildContent, Is.EqualTo(_childContent));
        });
    }

    [Test]
    public void ContainsExpectedElements()
    {
        var systemUnderTest = CreateRenderedTooltipComponent(TooltipText, _childContent);
        
        //check for div wrapper
        IElement? wrapper = null;
        Assert.That(() => wrapper = systemUnderTest.Find("div.tooltip-wrapper"), Throws.Nothing);
        if (wrapper == null)
            Assert.Fail("Could not find tooltip wrapper");
        Assert.That(wrapper!.ChildElementCount, Is.EqualTo(2));

        //check span in div
        IElement? span = null;
        Assert.That(() => span = systemUnderTest.Find("div.tooltip-wrapper span"), Throws.Nothing);
        if (span == null)
            Assert.Fail("Could not find tooltip span");
        Assert.That(span!.ChildElementCount, Is.EqualTo(1));

        //check p in span
        IElement? p = null;
        Assert.That(() => p = systemUnderTest.Find("div.tooltip-wrapper span p"), Throws.Nothing);
        if (p == null)
            Assert.Fail("Could not find tooltip p");
        Assert.That(p!.InnerHtml, Is.EqualTo(TooltipText));
        
        //check childContent in wrapper
        IElement? button = null;
        Assert.That(() => button = systemUnderTest.Find("div.tooltip-wrapper button"), Throws.Nothing);
        if (button == null)
            Assert.Fail("Could not find tooltip childContent");
    }

    [Test]
    public void TooltipTextHiddenByDefault()
    {
        var systemUnderTest = CreateRenderedTooltipComponent(TooltipText, _childContent);
        
        var span = systemUnderTest.Find("div.tooltip-wrapper span");
        span.MarkupMatches(@$"<span style=""visibility: hidden""><p>{TooltipText}</p></span>");
        Assert.That(systemUnderTest.Instance.TooltipVisible, Is.False);
    }

    [Test]
    public void TooltipTextAppearsOnMouseOver()
    {
        var systemUnderTest = CreateRenderedTooltipComponent(TooltipText, _childContent);
        
        var div = systemUnderTest.Find("div.tooltip-wrapper");
        div.MouseOver();
        var span = systemUnderTest.Find("div.tooltip-wrapper span");
        span.MarkupMatches(@$"<span style=""visibility: visible""><p>{TooltipText}</p></span>");
        Assert.That(systemUnderTest.Instance.TooltipVisible, Is.True);
    }

    [Test]
    public void TooltipTextDisappearsOnMouseOut()
    {
        var systemUnderTest = CreateRenderedTooltipComponent(TooltipText, _childContent);
        
        var div = systemUnderTest.Find("div.tooltip-wrapper");
        div.MouseOver();
        var span = systemUnderTest.Find("div.tooltip-wrapper span");
        span.MarkupMatches(@$"<span style=""visibility: visible""><p>{TooltipText}</p></span>");
        Assert.That(systemUnderTest.Instance.TooltipVisible, Is.True);
        
        div.MouseOut();
        span.MarkupMatches(@$"<span style=""visibility: hidden""><p>{TooltipText}</p></span>");
        Assert.That(systemUnderTest.Instance.TooltipVisible, Is.False);
    }

    [Test]
    public void ChildContentCanBeNull()
    {
        Assert.That(() => CreateRenderedTooltipComponent(TooltipText, null), Throws.Nothing);
    }

    private IRenderedComponent<Tooltip> CreateRenderedTooltipComponent(string tooltipText, RenderFragment? childContent)
    {
        return _testContext.RenderComponent<Tooltip>(parameters => parameters
            .Add(p => p.Text, tooltipText)
            .Add(p => p.ChildContent, childContent));
    }
}

