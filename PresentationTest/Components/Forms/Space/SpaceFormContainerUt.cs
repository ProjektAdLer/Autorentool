using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms.Space;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.SelectedViewModels;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.Forms.Space;

[TestFixture]
public class SpaceFormContainerUt
{
    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        _testContext.ComponentFactories.AddStub<CreateSpaceForm>();
        _testContext.ComponentFactories.AddStub<EditSpaceForm>();
        Presenter = Substitute.For<ILearningSpacePresenter>();
        SelectedVmProvider = Substitute.For<ISelectedViewModelsProvider>();
        _testContext.Services.AddSingleton(Presenter);
        _testContext.Services.AddSingleton(SelectedVmProvider);
    }

    [TearDown]
    public void Teardown()
    {
        _testContext.Dispose();
    }

    private TestContext _testContext = null!;

    private ILearningSpacePresenter Presenter { get; set; }

    private ISelectedViewModelsProvider SelectedVmProvider { get; set; }

    [Test]
    public void OnParametersSet_RegistersToPresenterEvent()
    {
        var sut = GetRenderedComponent();

        Presenter.Received().PropertyChanged += Arg.Any<PropertyChangedEventHandler>();
    }

    [Test]
    public void Dispose_UnregistersFromPresenterEvent()
    {
        var sut = GetRenderedComponent();
        sut.Instance.Dispose();

        Presenter.Received().PropertyChanged -= Arg.Any<PropertyChangedEventHandler>();
    }

    [Test]
    // ANF-ID: [AWA0001]
    public void Render_SpaceNull_ShowsCreate()
    {
        Presenter.LearningSpaceVm.Returns((ILearningSpaceViewModel?)null);

        var sut = GetRenderedComponent();

        var createStubs = sut.FindComponents<Stub<CreateSpaceForm>>().ToList();
        var editStubs = sut.FindComponents<Stub<EditSpaceForm>>().ToList();

        Assert.Multiple(() =>
        {
            Assert.That(createStubs, Has.Count.EqualTo(1));
            Assert.That(editStubs, Has.Count.EqualTo(0));
        });
    }

    [Test]
    // ANF-ID: [AWA0023]
    public void Render_SpaceNotNull_ShowsEdit()
    {
        Presenter.LearningSpaceVm.Returns(Substitute.For<ILearningSpaceViewModel>());

        var sut = GetRenderedComponent();

        var createStubs = sut.FindComponents<Stub<CreateSpaceForm>>().ToList();
        var editStubs = sut.FindComponents<Stub<EditSpaceForm>>().ToList();

        Assert.Multiple(() =>
        {
            Assert.That(createStubs, Has.Count.EqualTo(0));
            Assert.That(editStubs, Has.Count.EqualTo(1));
        });
    }

    [Test]
    // ANF-ID: [AWA0001]
    public async Task OnForceNew_ForcesNew()
    {
        Presenter.LearningSpaceVm.Returns(Substitute.For<ILearningSpaceViewModel>());

        var sut = GetRenderedComponent();

        var createStubs = sut.FindComponents<Stub<CreateSpaceForm>>().ToList();
        var editStubs = sut.FindComponents<Stub<EditSpaceForm>>().ToList();

        Assert.Multiple(() =>
        {
            Assert.That(createStubs, Has.Count.EqualTo(0));
            Assert.That(editStubs, Has.Count.EqualTo(1));
        });

        var editStub = editStubs[0];

        var onNewCallback = (EventCallback)editStub.Instance.Parameters["OnNewButtonClicked"];
        await sut.InvokeAsync(async () => await onNewCallback.InvokeAsync());


        createStubs = sut.FindComponents<Stub<CreateSpaceForm>>().ToList();
        editStubs = sut.FindComponents<Stub<EditSpaceForm>>().ToList();

        Assert.Multiple(() =>
        {
            Assert.That(createStubs, Has.Count.EqualTo(1));
            Assert.That(editStubs, Has.Count.EqualTo(0));
        });

        SelectedVmProvider.Received().SetLearningObjectInPathWay(null, null);
    }

    private IRenderedComponent<SpaceFormContainer> GetRenderedComponent()
    {
        return _testContext.RenderComponent<SpaceFormContainer>();
    }
}