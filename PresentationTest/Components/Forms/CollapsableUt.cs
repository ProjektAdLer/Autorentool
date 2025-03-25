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

    [TearDown]
    public void TearDown()
    {
        _ctx.Dispose();
    }

    [Test]
    public void Constructor_SetsParameters()
    {
        void ChildContent(RenderTreeBuilder builder) => builder.AddMarkupContent(0, "<p>Test</p>");
        const string title = "Test";
        const bool collapsed = true;

        var systemUnderTest = RenderComponent(ChildContent, collapsed, title);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.InitiallyCollapsed, Is.EqualTo(collapsed));
            Assert.That(systemUnderTest.Instance.Title, Is.EqualTo(title));
            Assert.That(systemUnderTest.Instance.ChildContent, Is.EqualTo((RenderFragment)ChildContent));
        });
    }

    [Test]
    public void Render_CollapsedTrue_RendersCorrectly()
    {
        const bool collapsed = true;

        var systemUnderTest = RenderComponent(initiallyCollapsed: collapsed);

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

        var systemUnderTest = RenderComponent(initiallyCollapsed: collapsed, childContent: ChildContent);

        var icon = systemUnderTest.FindComponent<Stub<MudIcon>>();
        var inner = systemUnderTest.Find("div.inner p");
        Assert.Multiple(() =>
        {
            Assert.That(icon.Instance.Parameters["Icon"], Is.EqualTo(Icons.Material.Filled.ArrowDropDown));
            inner.MarkupMatches(childMarkup);
        });
    }

    [Test]
    public void Render_TitleContentSet_ShowsTitleContentInsteadOfTitle()
    {
        const bool collapsed = true;
        var titleContent = GetChildContent();
        
        var systemUnderTest = RenderComponent(initiallyCollapsed: collapsed, titleContent: titleContent);
        
        Assert.Multiple(() =>
        {
            Assert.That(() => systemUnderTest.Find("p.select-none"), Throws.TypeOf<ElementNotFoundException>());
            Assert.That(() => systemUnderTest.Find("div.toggler p.child-content"), Throws.Nothing);
        });
    }
    
    [Test]
    public void Render_TitleContentAfterTogglerSet_ShowsTitleContentAfterToggler()
    {
        const bool collapsed = true;
        const string title = "Title";
        var titleContentAfterToggler = GetChildContent();

        var systemUnderTest = RenderComponent(initiallyCollapsed: collapsed, title: title,
            titleContentAfterToggler: titleContentAfterToggler);
        
        Assert.Multiple(() =>
        {
            Assert.That(() => systemUnderTest.Find("div.flex.flex-row div p.child-content"), Throws.Nothing);
        });
    }

    [Test]
    public void Render_NoTogglerContentSet_ShowsDefaultToggler()
    {
        const bool collapsed = true;
        const string title = "Title";
        
        var systemUnderTest = RenderComponent(initiallyCollapsed: collapsed, title: title);

        var togglerIcon = systemUnderTest.FindComponent<Stub<MudIcon>>();
        Assert.That(togglerIcon.Instance.Parameters["Icon"], Is.EqualTo(Icons.Material.Filled.ArrowRight));
        
        systemUnderTest.Find("div.toggler").Click();
        togglerIcon = systemUnderTest.FindComponent<Stub<MudIcon>>();
        
        Assert.That(togglerIcon.Instance.Parameters["Icon"], Is.EqualTo(Icons.Material.Filled.ArrowDropDown));
    }

    [Test]
    public void Render_TogglerContentSet_ShowsTogglerContent()
    {
        //generate a render fragment that takes a bool as input
        RenderFragment<bool> togglerContent = ctx => builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", "toggler-content");
            builder.AddContent(2, ctx.ToString());
            builder.CloseElement();

        };
        
        const bool collapsed = true;
        const string title = "Title";
        
        var systemUnderTest = RenderComponent(initiallyCollapsed: collapsed, title: title, togglerContent: togglerContent);
        
        Assert.Multiple(() =>
        {
            Assert.That(() => systemUnderTest.Find("div.toggler div.toggler-content"), Throws.Nothing);
            Assert.That(systemUnderTest.Find("div.toggler-content").TextContent, Is.EqualTo(collapsed.ToString()));
        });
        
        systemUnderTest.Find("div.toggler").Click();
        
        Assert.That(systemUnderTest.Find("div.toggler-content").TextContent, Is.EqualTo((!collapsed).ToString()));
    }

    [Test]
    public void Render_NoVerticalMarginSet_DefaultsTo_my4()
    {
        const bool collapsed = true;
        const string title = "Title";
        
        var systemUnderTest = RenderComponent(initiallyCollapsed: collapsed, title: title);
        
        Assert.That(() => systemUnderTest.Find("div.gap-2.my-3"), Throws.Nothing);
    }
    
    [Test]
    public void Render_VerticalMarginSet_SetsMargin()
    {
        const bool collapsed = true;
        const string title = "Title";
        const string verticalMargin = "my-2";
        
        var systemUnderTest = RenderComponent(initiallyCollapsed: collapsed, title: title, verticalMargin: verticalMargin);
        
        Assert.That(() => systemUnderTest.Find("div.gap-2.my-2"), Throws.Nothing);
    }

    [Test]
    public void ClickCollapsable_TogglesCollapsed()
    {
        const bool collapsed = true;
        var childContent = GetChildContent();

        var systemUnderTest = RenderComponent(initiallyCollapsed: collapsed, childContent: childContent);

        var togglerDiv = systemUnderTest.Find("div.toggler");

        Assert.Multiple(() =>
        {
            Assert.That(() => systemUnderTest.Find("p.child-content"), Throws.TypeOf<ElementNotFoundException>());
        });

        togglerDiv.Click();

        Assert.Multiple(() => { Assert.That(() => systemUnderTest.Find("p.child-content"), Throws.Nothing); });

        togglerDiv.Click();

        Assert.Multiple(() =>
        {
            Assert.That(() => systemUnderTest.Find("p.child-content"), Throws.TypeOf<ElementNotFoundException>());
        });
    }

    private static RenderFragment GetChildContent()
    {
        return builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddAttribute(1, "class", "child-content");
            builder.AddContent(2, "ChildContent");
            builder.CloseElement();
        };
    }

    private IRenderedComponent<Collapsable> RenderComponent(RenderFragment? childContent = null,
        bool initiallyCollapsed = false, string? title = null, RenderFragment? titleContent = null,
        RenderFragment? titleContentAfterToggler = null, RenderFragment<bool>? togglerContent = null,
        string? verticalMargin = null)
    {
        childContent ??= builder => builder.AddMarkupContent(0, "<p>Test</p>");
        // title ??= "Test";
        return _ctx.RenderComponent<Collapsable>(builder =>
                {
                    builder.Add(p => p.InitiallyCollapsed, initiallyCollapsed);
                    builder.Add(p => p.Title, title);
                    builder.Add(p => p.ChildContent, childContent);
                    builder.Add(p => p.TitleContent, titleContent);
                    builder.Add(p => p.TitleContentAfterToggler, titleContentAfterToggler);
                    builder.Add(p => p.TogglerContent, togglerContent);
                    if(verticalMargin != null)
                        builder.Add(p => p.VerticalMargin, verticalMargin);
                }
        );
    }
}