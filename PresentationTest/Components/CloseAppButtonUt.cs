using Bunit;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components;
using Presentation.PresentationLogic;

namespace PresentationTest.Components;

[TestFixture]
public class CloseAppButtonUt
{
#pragma warning disable CS8618
    private Bunit.TestContext _context;
    private IShutdownManager _shutdownManager;
#pragma warning restore CS8618
    [SetUp]
    public void Setup()
    {
        _context = new Bunit.TestContext();
        _shutdownManager = Substitute.For<IShutdownManager>();
        _context.Services.AddSingleton(_shutdownManager);
    }

    [Test]
    public void OnClick_CallsShutdownManager()
    {
        using (_context)
        {
            var systemUnderTest = CreateTestableCloseAppButtonComponent();

            var button = systemUnderTest.Find(".btn");
            button.Click();

            _shutdownManager.Received().BeginShutdown();
        }
    }

    private IRenderedComponent<CloseAppButton> CreateTestableCloseAppButtonComponent()
    {
        return _context.RenderComponent<CloseAppButton>();
    }
}