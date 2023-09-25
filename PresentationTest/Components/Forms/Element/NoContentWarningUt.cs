using Bunit;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms.Element;
using Presentation.PresentationLogic.Mediator;
using TestHelpers;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.Forms.Element;

[TestFixture]
public class NoContentWarningUt
{
    private TestContext _testContext = null!;
    private IMediator Mediator { get; set; }

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        Mediator = Substitute.For<IMediator>();
        _testContext.Services.AddSingleton(Mediator);
        _testContext.AddLocalizerForTest<NoContentWarning>();
    }
    
    [TearDown]
    public void TearDown()
    {
        _testContext.Dispose();
    }

    [Test]
    public void ButtonClick_CallsMediator()
    {
        var systemUnderTest = GetRenderedComponent();
        
        systemUnderTest.Find("button").Click();
        
        Mediator.Received(1).RequestOpenContentDialog();
    }

    private IRenderedComponent<NoContentWarning> GetRenderedComponent()
    {
        return _testContext.RenderComponent<NoContentWarning>();
    }
}