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

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    private Bunit.TestContext _context;
    private IShutdownManager _shutdownManager;

    [Test]
    // ANF-ID: [ASN0025]
    public void OnClick_CallsShutdownManager()
    {
        using (_context)
        {
            var systemUnderTest = CreateTestableCloseAppButtonComponent();

            var button = systemUnderTest.Find("button");
            button.Click();

            _shutdownManager.Received().RequestShutdownAsync();
        }
    }

    private IRenderedComponent<CloseAppButton> CreateTestableCloseAppButtonComponent()
    {
        return _context.RenderComponent<CloseAppButton>();
    }
}