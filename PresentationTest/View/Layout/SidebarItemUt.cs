using Bunit;
using Microsoft.AspNetCore.Components;
using NSubstitute;
using NUnit.Framework;
using Presentation.View.Layout;
using TestContext = Bunit.TestContext;

namespace PresentationTest.View.Layout;

[TestFixture]
public class SidebarItemUt
{
    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        SidebarSubstitute = Substitute.For<ISidebar>();
    }

    public ISidebar SidebarSubstitute { get; set; }
    private TestContext _testContext;

    [Test]
    public void Render_RendersSidebarContent()
    {
        RenderFragment sidebarContent = builder =>
        {
            builder.OpenElement(0, "div");
            builder.CloseElement();
        };

        var systemUnderTest = GetRenderedComponent(sidebarContent, _ => { }, false);

        var sidebar = systemUnderTest.Find("button");
        Assert.That(sidebar.Children, Has.Length.EqualTo(1));
        Assert.That(sidebar.Children[0].TagName.ToLowerInvariant(), Is.EqualTo("div"));
    }

    [Test]
    public void Click_CallsCallback()
    {
        var callbackCalled = false;
        var callback = EventCallback.Factory.Create<bool>(this, _ => callbackCalled = true);

        var systemUnderTest = GetRenderedComponent(_ => { }, _ => { }, false, callback);

        systemUnderTest.Find("button").Click();

        Assert.That(callbackCalled, Is.True);
    }

    [Test]
    public void IsActive_Setter_NotifiesSidebar()
    {
        var systemUnderTest = GetRenderedComponent(_ => { }, _ => { }, false);
#pragma warning disable BL0005
        systemUnderTest.Instance.IsActive = true;

        SidebarSubstitute.Received(1).SetSidebarItem(systemUnderTest.Instance);
        SidebarSubstitute.CurrentItem.Returns(systemUnderTest.Instance);

        systemUnderTest.Instance.IsActive = false;
#pragma warning restore BL0005

        SidebarSubstitute.Received(1).ClearSidebarItem();
    }

    private IRenderedComponent<SidebarItem> GetRenderedComponent(RenderFragment sidebarContent,
        RenderFragment mainContent,
        bool isActive,
        EventCallback<bool>? requestIsActiveToggle = null)
    {
        requestIsActiveToggle ??= EventCallback<bool>.Empty;
        return _testContext.RenderComponent<SidebarItem>(
            parameters => parameters
                .Add(p => p.Sidebar, SidebarSubstitute)
                .Add(p => p.SidebarContent, sidebarContent)
                .Add(p => p.MainContent, mainContent)
                .Add(p => p.IsActive, isActive)
                .Add(p => p.RequestIsActiveToggle, requestIsActiveToggle.Value)
        );
    }
}