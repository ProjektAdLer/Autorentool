using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms.Element;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.Mediator;
using Presentation.PresentationLogic.SelectedViewModels;
using TestHelpers;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.Forms.Element;

[TestFixture]
public class ElementFormContainerUt
{
    private TestContext _testContext = null!;

    private ILearningWorldPresenter WorldPresenter { get; set; }
    private ISelectedViewModelsProvider SelectedVmProvider { get; set; }
    private IMediator Mediator { get; set; }


    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        _testContext.ComponentFactories.AddStub<CreateElementForm>();
        _testContext.ComponentFactories.AddStub<EditElementForm>();
        WorldPresenter = Substitute.For<ILearningWorldPresenter>();
        SelectedVmProvider = Substitute.For<ISelectedViewModelsProvider>();
        SelectedVmProvider.LearningWorld.Returns(ViewModelProvider.GetLearningWorld());
        Mediator = Substitute.For<IMediator>();
        _testContext.Services.AddSingleton(WorldPresenter);
        _testContext.Services.AddSingleton(SelectedVmProvider);
        _testContext.Services.AddSingleton(Mediator);
    }


    [TearDown]
    public void Teardown()
    {
        _testContext.Dispose();
    }

    [Test]
    public void OnParametersSet_RegistersToSelectedViewModelsProviderEvent()
    {
        var sut = GetRenderedComponent();
        
        SelectedVmProvider.Received().PropertyChanged += Arg.Any<PropertyChangedEventHandler>();
    }
    
    [Test]
    public void Dispose_UnregistersFromSelectedViewModelsProviderEvent()
    {
        var sut = GetRenderedComponent();
        sut.Instance.Dispose();
        
        SelectedVmProvider.Received().PropertyChanged -= Arg.Any<PropertyChangedEventHandler>();
    }
    
    [Test]
    public async Task DisposeAsync_UnregistersFromSelectedViewModelsProviderEvent()
    {
        var sut = GetRenderedComponent();
        await sut.Instance.DisposeAsync();
        
        SelectedVmProvider.Received().PropertyChanged -= Arg.Any<PropertyChangedEventHandler>();
    }
    
    [Test]
    public void Render_ElementNull_ShowsCreate([Values] ElementMode mode)
    {
        SelectedVmProvider.LearningWorld.Returns(ViewModelProvider.GetLearningWorld());
        SelectedVmProvider.LearningElement.Returns((ILearningElementViewModel?)null);

        var sut = GetRenderedComponent(mode);
        
        var createStubs = sut.FindComponents<Stub<CreateElementForm>>().ToList();
        var editStubs = sut.FindComponents<Stub<EditElementForm>>().ToList();

        Assert.Multiple(() =>
        {
            Assert.That(createStubs, Has.Count.EqualTo(1));
            Assert.That(editStubs, Has.Count.EqualTo(0));
        });
        Assert.That(createStubs.First().Instance.Parameters["ElementMode"], Is.EqualTo(mode));
    }
    
    [Test]
    public void Render_ElementNotNull_ShowsEdit([Values] ElementMode mode)
    {
        SelectedVmProvider.LearningWorld.Returns(ViewModelProvider.GetLearningWorld());
        SelectedVmProvider.LearningElement.Returns(ViewModelProvider.GetLearningElement());

        var sut = GetRenderedComponent(mode);
        
        var createStubs = sut.FindComponents<Stub<CreateElementForm>>().ToList();
        var editStubs = sut.FindComponents<Stub<EditElementForm>>().ToList();

        Assert.Multiple(() =>
        {
            Assert.That(createStubs, Has.Count.EqualTo(0));
            Assert.That(editStubs, Has.Count.EqualTo(1));
        });
        Assert.That(editStubs.First().Instance.Parameters["ElementMode"], Is.EqualTo(mode));
    }

    [Test]
    public void Render_ElementNotNull_MediatorOverwriteEditTrue_ShowsCreate()
    {
        SelectedVmProvider.LearningWorld.Returns(ViewModelProvider.GetLearningWorld());
        SelectedVmProvider.LearningElement.Returns(ViewModelProvider.GetLearningElement());
        Mediator.OverwriteElementEdit = true;
        
        var sut = GetRenderedComponent();
        
        var createStubs = sut.FindComponents<Stub<CreateElementForm>>().ToList();
        var editStubs = sut.FindComponents<Stub<EditElementForm>>().ToList();
        
        Assert.Multiple(() =>
        {
            Assert.That(createStubs, Has.Count.EqualTo(1));
            Assert.That(editStubs, Has.Count.EqualTo(0));
        });
    }
    
    [Test]
    public async Task OnForceNew_ForcesNew()
    {
        SelectedVmProvider.LearningWorld.Returns(ViewModelProvider.GetLearningWorld());
        SelectedVmProvider.LearningElement.Returns(ViewModelProvider.GetLearningElement());

        var sut = GetRenderedComponent();
        
        var createStubs = sut.FindComponents<Stub<CreateElementForm>>().ToList();
        var editStubs = sut.FindComponents<Stub<EditElementForm>>().ToList();
        
        Assert.Multiple(() =>
        {
            Assert.That(createStubs, Has.Count.EqualTo(0));
            Assert.That(editStubs, Has.Count.EqualTo(1));
        });
        
        var onNewCallback = (EventCallback)editStubs[0].Instance.Parameters["OnNewButtonClicked"];
        await sut.InvokeAsync(async () => await onNewCallback.InvokeAsync());
        
        createStubs = sut.FindComponents<Stub<CreateElementForm>>().ToList();
        editStubs = sut.FindComponents<Stub<EditElementForm>>().ToList();
        
        Assert.Multiple(() =>
        {
            Assert.That(createStubs, Has.Count.EqualTo(1));
            Assert.That(editStubs, Has.Count.EqualTo(0));
            Assert.That(Mediator.OverwriteElementEdit, Is.True);
        });
        SelectedVmProvider.Received().SetLearningElement(null, null);
    }

    [Test]
    public void SelectedViewModelsProviderOnPropertyChanged_ResetsMediatorOverwriteElementEdit()
    {
        SelectedVmProvider.LearningWorld.Returns(ViewModelProvider.GetLearningWorld());
        SelectedVmProvider.LearningElement.Returns(ViewModelProvider.GetLearningElement());

        var sut = GetRenderedComponent();
        
        Mediator.OverwriteElementEdit = true;
        
        SelectedVmProvider.PropertyChanged += Raise.Event<PropertyChangedEventHandler>(
            SelectedVmProvider, new PropertyChangedEventArgs(nameof(SelectedVmProvider.LearningElement)));
        
        Assert.That(Mediator.OverwriteElementEdit, Is.False);
    }
    
    private IRenderedComponent<ElementFormContainer> GetRenderedComponent(ElementMode? elementMode = null)
    {
        elementMode ??= ElementMode.Normal;
        return _testContext.RenderComponent<ElementFormContainer>(pBuilder =>
            pBuilder.Add(p => p.ElementMode, elementMode.Value));
    }
}