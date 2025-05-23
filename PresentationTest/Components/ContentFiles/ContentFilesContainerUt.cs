using System;
using System.Threading.Tasks;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.ContentFiles;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.Mediator;
using Presentation.PresentationLogic.SelectedViewModels;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.ContentFiles;

[TestFixture]
public class ContentFilesContainerUt
{
    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        _presentationLogic = Substitute.For<IPresentationLogic>();
        _dialogService = Substitute.For<IDialogService>();
        _mediator = Substitute.For<IMediator>();
        _authoringToolWorkspaceViewModel = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        _localizer = Substitute.For<IStringLocalizer<ContentFilesView>>();
        _errorService = Substitute.For<IErrorService>();
        _snackbar = Substitute.For<ISnackbar>();
        _selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();

        _testContext.Services.AddSingleton(_presentationLogic);
        _testContext.Services.AddSingleton(_dialogService);
        _testContext.Services.AddSingleton(_mediator);
        _testContext.Services.AddSingleton(_authoringToolWorkspaceViewModel);
        _testContext.Services.AddSingleton(_localizer);
        _testContext.Services.AddSingleton(_errorService);
        _testContext.Services.AddSingleton(_snackbar);
        _testContext.Services.AddSingleton(_selectedViewModelsProvider);

        _testContext.ComponentFactories.AddStub<ContentFilesAdd>();
        _contentFilesViewSubstitute = Substitute.For<ContentFilesView>();
        _testContext.ComponentFactories.Add(_contentFilesViewSubstitute);
    }

    [TearDown]
    public void TearDown()
    {
        _testContext.Dispose();
        _snackbar.Dispose();
    }

    private TestContext _testContext;
    private IPresentationLogic _presentationLogic;
    private IDialogService _dialogService;
    private ContentFilesView _contentFilesViewSubstitute;
    private IMediator _mediator;
    private IAuthoringToolWorkspaceViewModel _authoringToolWorkspaceViewModel;
    private IStringLocalizer<ContentFilesView> _localizer;
    private IErrorService _errorService;
    private ISnackbar _snackbar;
    private ISelectedViewModelsProvider _selectedViewModelsProvider;

    [Test]
    public void Constructor_SetsAllPropertiesAndRendersCorrectly()
    {
        var component = GetRenderedComponent();

        var contentFilesViewComponent = component.FindComponent<ContentFilesView>();
        var contentFilesContainerCascadingValue = component.FindComponent<CascadingValue<Func<Task>>>().Instance;

        Assert.Multiple(() =>
        {
            Assert.That(component.HasComponent<Stub<ContentFilesAdd>>());
            Assert.That(contentFilesViewComponent.Instance, Is.Not.Null);
            Assert.That(contentFilesContainerCascadingValue.Value, Is.Not.Null);
        });
    }

    [Test]
    public async Task RerenderAsync_CallsRerenderAsyncOnContentFilesView()
    {
        var component = GetRenderedComponent();

        await _contentFilesViewSubstitute.DidNotReceive().RerenderAsync();

        await component.Instance.RerenderAsync();

        await _contentFilesViewSubstitute.Received().RerenderAsync();
    }

    private IRenderedComponent<ContentFilesContainer> GetRenderedComponent()
    {
        return _testContext.RenderComponent<ContentFilesContainer>();
    }
}