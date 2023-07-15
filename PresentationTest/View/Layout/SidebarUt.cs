using Bunit;
using Microsoft.AspNetCore.Components;
using NUnit.Framework;
using Presentation.View.Layout;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.Layout;

[TestFixture]
public class SidebarUt
{
    private TestContext _testContext;

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
    }

    [Test]
    public void Render_ParametersSet()
    {
        RenderFragment childContent = _ => { };
        var side = Side.Left;
        
        var systemUnderTest = GetRenderedSidebar(childContent, side);
        
        Assert.That(systemUnderTest.Instance.Side, Is.EqualTo(side));
        Assert.That(systemUnderTest.Instance.ChildContent, Is.EqualTo(childContent));
    }

    [Test]
    public void SetSidebarItem_SetsSidebarItem_AndRendersIt()
    {
        RenderFragment sidebarMainContent = builder =>
        {
            builder.OpenElement(0, "button");
            builder.CloseElement();
        };
        RenderFragment childContent = builder =>
        {
            builder.OpenComponent<SidebarItem>(0);
            builder.AddAttribute(1, nameof(SidebarItem.MainContent), sidebarMainContent);
            builder.CloseComponent();
        };

        var systemUnderTest = GetRenderedSidebar(childContent, Side.Right);
        
        Assert.That(systemUnderTest.Instance.CurrentItem, Is.Null);
        Assert.That(() => systemUnderTest.Find("div.main-content"), Throws.TypeOf<ElementNotFoundException>());
        
        var sidebarItem = systemUnderTest.FindComponent<SidebarItem>();
        systemUnderTest.Instance.SetSidebarItem(sidebarItem.Instance);
        
        Assert.That(systemUnderTest.Instance.CurrentItem, Is.EqualTo(sidebarItem.Instance));

        var main = systemUnderTest.Find("div.main-content");
        Assert.That(main.Children, Has.Length.EqualTo(1));
        Assert.That(main.Children[0].TagName.ToLowerInvariant(), Is.EqualTo("button"));
    }

    private IRenderedComponent<Sidebar> GetRenderedSidebar(RenderFragment childContent, Side side)
    {
        return _testContext.RenderComponent<Sidebar>(
            (nameof(Sidebar.ChildContent), childContent),
            (nameof(Sidebar.Side), side)
        );
    }
}