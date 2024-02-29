using Bunit;
using BusinessLogic.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms.Element;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.SelectedViewModels;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.Forms.Element;

[TestFixture]
public class UnplacedElementsDropZoneUt
{
    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();

        _worldPresenter = Substitute.For<ILearningWorldPresenter>();
        _localizer = Substitute.For<IStringLocalizer<UnplacedElementsDropZone>>();
        _localizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
        _localizer[Arg.Any<string>(), Arg.Any<object[]>()].Returns(ci =>
            new LocalizedString(ci.Arg<string>() + string.Concat(ci.Arg<object[]>()),
                ci.Arg<string>() + string.Concat(ci.Arg<object[]>())));
        _selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        _undoRedoSource = Substitute.For<IOnUndoRedo>();

        _testContext.Services.AddSingleton(_worldPresenter);
        _testContext.Services.AddSingleton(_localizer);
        _testContext.Services.AddSingleton(_selectedViewModelsProvider);
        _testContext.Services.AddSingleton(_undoRedoSource);

        _testContext.AddMudBlazorTestServices();
    }

    [TearDown]
    public void TearDown()
    {
        _testContext.Dispose();
    }

    private TestContext _testContext;
    private ILearningWorldPresenter _worldPresenter;
    private IStringLocalizer<UnplacedElementsDropZone> _localizer;
    private ISelectedViewModelsProvider _selectedViewModelsProvider;
    private IOnUndoRedo _undoRedoSource;

    [Test]
    public void Constructor_InjectsDependencies()
    {
        var systemUnderTest = GetRenderedComponent();

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.WorldPresenter, Is.EqualTo(_worldPresenter));
            Assert.That(systemUnderTest.Instance.Localizer, Is.EqualTo(_localizer));
            Assert.That(systemUnderTest.Instance.SelectedViewModelsProvider, Is.EqualTo(_selectedViewModelsProvider));
            Assert.That(systemUnderTest.Instance.UndoRedoSource, Is.EqualTo(_undoRedoSource));
        });
    }


    private IRenderedComponent<UnplacedElementsDropZone> GetRenderedComponent()
    {
        return _testContext.RenderComponent<UnplacedElementsDropZone>();
    }
}