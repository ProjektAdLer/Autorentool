using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using MudBlazor;
using NUnit.Framework;
using Presentation.Components.Forms;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.Forms;

[TestFixture]
public class CollapsableUt
{
    private TestContext _ctx;

    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _ctx.ComponentFactories.AddStub<MudIcon>();
    }

    [Test]
    public void Constructor_SetsParameters()
    {
        void ChildContent(RenderTreeBuilder builder) => builder.AddMarkupContent(0, "<p>Test</p>");
        var collapsedChanged = EventCallback.Factory.Create<bool>(this, _ => { });
        const string title = "Test";
        const bool collapsed = true;
        
        var systemUnderTest = RenderComponent(ChildContent, collapsed, collapsedChanged, title);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.Collapsed, Is.EqualTo(collapsed));
            Assert.That(systemUnderTest.Instance.CollapsedChanged, Is.EqualTo(collapsedChanged));
            Assert.That(systemUnderTest.Instance.Title, Is.EqualTo(title));
            Assert.That(systemUnderTest.Instance.ChildContent, Is.EqualTo((RenderFragment)ChildContent));
        });
    }

    [Test]
    public void Render_CollapsedTrue_RendersCorrectly()
    {
        const bool collapsed = true;
        
        var systemUnderTest = RenderComponent(collapsed: collapsed);

        var icon = systemUnderTest.FindComponent<Stub<MudIcon>>();
        Assert.Multiple(() =>
        {
            Assert.That(icon.Instance.Parameters["Icon"], Is.EqualTo(Icons.Material.Filled.ArrowRight));
            Assert.That(() => systemUnderTest.Find("div.inner"), Throws.TypeOf<ElementNotFoundException>());
        });
    }

    [Test]
    public void Render_CollapsedFalse_RendersCorrectly()
    {
        const bool collapsed = false;
        var childMarkup = "<p>Test</p>";
        void ChildContent(RenderTreeBuilder builder) => builder.AddMarkupContent(0, childMarkup);

        var systemUnderTest = RenderComponent(collapsed: collapsed, childContent: ChildContent);

        var icon = systemUnderTest.FindComponent<Stub<MudIcon>>();
        var inner = systemUnderTest.Find("div.inner p");
        Assert.Multiple(() =>
        {
            Assert.That(icon.Instance.Parameters["Icon"], Is.EqualTo(Icons.Material.Filled.ArrowDropDown));
            inner.MarkupMatches(childMarkup);
        });
    }

    [Test]
    public void ClickCollapsable_TogglesCollapsedAndCallsCollapsedChanges()
    {
        const bool collapsed = false;
        var collapsedChangedCallCounter = 0;
        var collapsedChanged = EventCallback.Factory.Create<bool>(this, _ => collapsedChangedCallCounter++);
        
        var systemUnderTest = RenderComponent(collapsed: collapsed, collapsedChanged: collapsedChanged);

        var togglerDiv = systemUnderTest.Find("div.toggler");
        
        Assert.Multiple(() =>
        {
            Assert.That(collapsedChangedCallCounter, Is.EqualTo(0));
            Assert.That(systemUnderTest.Instance.Collapsed, Is.EqualTo(collapsed));
        });
        
        togglerDiv.Click();
        
        Assert.Multiple(() =>
        {
            Assert.That(collapsedChangedCallCounter, Is.EqualTo(1));
            Assert.That(systemUnderTest.Instance.Collapsed, Is.EqualTo(!collapsed));
        });
        
        togglerDiv.Click();
        
        Assert.Multiple(() =>
        {
            Assert.That(collapsedChangedCallCounter, Is.EqualTo(2));
            Assert.That(systemUnderTest.Instance.Collapsed, Is.EqualTo(collapsed));
        });
    }

    private IRenderedComponent<Collapsable> RenderComponent(RenderFragment? childContent = null, bool collapsed = false,
        EventCallback<bool>? collapsedChanged = null, string? title = null)
    {
        childContent ??= builder => builder.AddMarkupContent(0, "<p>Test</p>");
        collapsedChanged ??= EventCallback.Factory.Create<bool>(this, _ => { });
        title ??= "Test";
        return _ctx.RenderComponent<Collapsable>(
            (nameof(Collapsable.Collapsed), collapsed),
            (nameof(Collapsable.CollapsedChanged), collapsedChanged),
            (nameof(Collapsable.Title), title),
            (nameof(Collapsable.ChildContent), childContent)
        );
    }
}