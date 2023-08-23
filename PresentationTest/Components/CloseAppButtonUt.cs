using Bunit;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components;
using Presentation.PresentationLogic;
using TestHelpers;

namespace PresentationTest.Components;

[TestFixture]
public class CloseAppButtonUt
{
    [SetUp]
    public void Setup()
    {
        _context = new Bunit.TestContext();
        _shutdownManager = Substitute.For<IShutdownManager>();
        _context.Services.AddSingleton(_shutdownManager);
        _context.AddLocalizerForTest<CloseAppButton>();
    }

    private Bunit.TestContext _context;
    private IShutdownManager _shutdownManager;

    [Test]
    public void OnClick_CallsShutdownManager()
    {
        using (_context)
        {
            var systemUnderTest = CreateTestableCloseAppButtonComponent();

            var button = systemUnderTest.Find(".btn-standard");
            button.Click();

            _shutdownManager.Received().RequestShutdownAsync();
        }
    }

    private IRenderedComponent<CloseAppButton> CreateTestableCloseAppButtonComponent()
    {
        return _context.RenderComponent<CloseAppButton>();
    }
}