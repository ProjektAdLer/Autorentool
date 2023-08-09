using Bunit;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.ContentFiles;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;

namespace IntegrationTest.Components.ContentFiles;

[TestFixture]
public class ContentFilesAddIt : MudBlazorTestFixture<ContentFilesAdd>
{
    [SetUp]
    public void Setup()
    {
        _dialogService = Substitute.For<IDialogService>();
        _presentationLogic = Substitute.For<IPresentationLogic>();
        _errorService = Substitute.For<IErrorService>();
        Context.Services.AddSingleton(_dialogService);
        Context.Services.AddSingleton(_presentationLogic);
        Context.Services.AddSingleton(_errorService);
    }

    private IDialogService _dialogService = null!;
    private IPresentationLogic _presentationLogic = null!;
    private IErrorService _errorService = null!;

    [Test]
    public void OnInitialized_DependenciesInjected()
    {
        var systemUnderTest = GetRenderedComponent();

        Assert.That(systemUnderTest.Instance.DialogService, Is.EqualTo(_dialogService));
        Assert.That(systemUnderTest.Instance.PresentationLogic, Is.EqualTo(_presentationLogic));
        Assert.That(systemUnderTest.Instance.ErrorService, Is.EqualTo(_errorService));
        Assert.That(systemUnderTest.Instance.Logger, Is.Not.Null);
        Assert.That(systemUnderTest.Instance.Localizer, Is.Not.Null);
    }

    private IRenderedComponent<ContentFilesAdd> GetRenderedComponent()
    {
        return Context.RenderComponent<ContentFilesAdd>();
    }
}