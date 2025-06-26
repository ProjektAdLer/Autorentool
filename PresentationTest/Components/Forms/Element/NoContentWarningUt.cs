using Bunit;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;
using Presentation.Components.ContentFiles;
using Presentation.Components.Forms.Element;
using Presentation.PresentationLogic.Mediator;
using TestHelpers;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.Forms.Element;

[TestFixture]
public class NoContentWarningUt
{
    private TestContext _testContext = null!;

    private IDialogService _dialogService = null!;
    
    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        _dialogService = Substitute.For<IDialogService>();
        _testContext.Services.AddSingleton(_dialogService);
        _testContext.AddLocalizerForTest<NoContentWarning>();
        _testContext.AddMudBlazorTestServices();
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

        _dialogService.Received(1).ShowAsync<ContentFilesContainer>(Arg.Any<string>(), Arg.Any<DialogOptions>());
    }

    private IRenderedComponent<NoContentWarning> GetRenderedComponent()
    {
        return _testContext.RenderComponent<NoContentWarning>();
    }
}