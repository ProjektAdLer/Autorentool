using System.Linq;
using System.Threading.Tasks;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.ContentFiles;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.Mediator;
using TestContext = Bunit.TestContext;
#pragma warning disable CS8618

namespace PresentationTest.Components.ContentFiles;

[TestFixture]
public class ContentFilesContainerUt
{
    private TestContext _testContext;
    private IPresentationLogic _presentationLogic;
    private IDialogService _dialogService;
    private ContentFilesView _contentFilesViewSubstitute;
    private IMediator _mediator;
    private IAuthoringToolWorkspaceViewModel _authoringToolWorkspaceViewModel;

    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        _presentationLogic = Substitute.For<IPresentationLogic>();
        _dialogService = Substitute.For<IDialogService>();
        _mediator = Substitute.For<IMediator>();
        _authoringToolWorkspaceViewModel = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        
        _testContext.Services.AddSingleton(_presentationLogic);
        _testContext.Services.AddSingleton(_dialogService);
        _testContext.Services.AddSingleton(_mediator);
        _testContext.Services.AddSingleton(_authoringToolWorkspaceViewModel);
        
        _testContext.ComponentFactories.AddStub<ContentFilesAdd>();
        _contentFilesViewSubstitute = Substitute.For<ContentFilesView>();
        _testContext.ComponentFactories.Add(_contentFilesViewSubstitute);
    }

    [Test]
    public void Constructor_SetsAllPropertiesAndRendersCorrectly()
    {
        var component = GetRenderedComponent();
        
        var contentFilesViewComponent = component.FindComponent<ContentFilesView>();
        var contentFilesContainerCascadingValue = component.FindComponent<CascadingValue<ContentFilesContainer>>().Instance;
        
        Assert.Multiple(() =>
        {
            Assert.That(component.HasComponent<Stub<ContentFilesAdd>>());
            Assert.That(contentFilesViewComponent.Instance, Is.EqualTo(_contentFilesViewSubstitute));
            Assert.That(contentFilesContainerCascadingValue.Value, Is.EqualTo(component.Instance));
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