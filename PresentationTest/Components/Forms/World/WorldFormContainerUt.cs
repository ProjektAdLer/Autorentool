using System.ComponentModel;
using System.Linq;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms.World;
using Presentation.PresentationLogic.LearningWorld;
using TestHelpers;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.Forms.World;

[TestFixture]
public class WorldFormContainerUt
{
    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        _testContext.ComponentFactories.AddStub<CreateWorldForm>();
        _testContext.ComponentFactories.AddStub<EditWorldForm>();
        Presenter = Substitute.For<ILearningWorldPresenter>();
        _testContext.Services.AddSingleton(Presenter);
    }

    [TearDown]
    public void Teardown()
    {
        _testContext.Dispose();
    }

    private TestContext _testContext = null!;

    private ILearningWorldPresenter Presenter { get; set; }

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
    // ANF-ID: [ASE1]
    public void Render_WorldNull_ShowsCreate()
    {
        Presenter.LearningWorldVm.Returns((ILearningWorldViewModel?)null);

        var sut = GetRenderedComponent();

        var createStubs = sut.FindComponents<Stub<CreateWorldForm>>().ToList();
        var editStubs = sut.FindComponents<Stub<EditWorldForm>>().ToList();

        Assert.Multiple(() =>
        {
            Assert.That(createStubs, Has.Count.EqualTo(1));
            Assert.That(editStubs, Has.Count.EqualTo(0));
        });
    }

    [Test]
    // ANF-ID: [ASE3]
    public void Render_WorldNotNull_ShowsEdit()
    {
        Presenter.LearningWorldVm.Returns(ViewModelProvider.GetLearningWorld());

        var sut = GetRenderedComponent();

        var createStubs = sut.FindComponents<Stub<CreateWorldForm>>().ToList();
        var editStubs = sut.FindComponents<Stub<EditWorldForm>>().ToList();

        Assert.Multiple(() =>
        {
            Assert.That(createStubs, Has.Count.EqualTo(0));
            Assert.That(editStubs, Has.Count.EqualTo(1));
        });
    }

    private IRenderedComponent<WorldFormContainer> GetRenderedComponent()
    {
        return _testContext.RenderComponent<WorldFormContainer>();
    }
}