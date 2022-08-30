using Bunit;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;

namespace PresentationTest.Components;

[TestFixture]
public class CloseAppButtonUt
{
#pragma warning disable CS8618
    private Bunit.TestContext context;
    private IShutdownManager shutdownManager;
#pragma warning restore CS8618
    [SetUp]
    public void Setup()
    {
        context = new Bunit.TestContext();
        shutdownManager = Substitute.For<IShutdownManager>();
        context.Services.AddSingleton(shutdownManager);
    }

    [Test]
    public void OnClick_CallsShutdownManager()
    {
        using (context)
        {
            var systemUnderTest = CreateTestableCloseAppButtonComponent();

            var button = systemUnderTest.Find(".btn");
            button.Click();

            shutdownManager.Received().BeginShutdown();
        }
    }

    private IRenderedComponent<CloseAppButton> CreateTestableCloseAppButtonComponent()
    {
        return context.RenderComponent<CloseAppButton>();
    }
}