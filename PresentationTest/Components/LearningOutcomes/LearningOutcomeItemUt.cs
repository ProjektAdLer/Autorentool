using Bunit;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.LearningOutcomes;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;
using TestHelpers;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.LearningOutcomes;

[TestFixture]
public class LearningOutcomeItemUt
{
    private TestContext _context;
    private IPresentationLogic _presentationLogic;
    private IDialogService _dialogService;

    [SetUp]
    public void Setup()
    {
        _context = new TestContext();
        _presentationLogic = Substitute.For<IPresentationLogic>();
        _dialogService = Substitute.For<IDialogService>();
        _context.Services.AddSingleton(_presentationLogic);
        _context.Services.AddSingleton(_dialogService);
        _context.AddLocalizerForTest<LearningOutcomeItem>();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public void Render_ShouldRenderLearningOutcomeItem()
    {
        // Arrange
        var collection = new LearningOutcomeCollectionViewModel();
        var outcome = ViewModelProvider.GetLearningOutcome();
        collection.LearningOutcomes.Add(outcome);
        var sut = GetRenderedComponent(collection, outcome);

        // Act
        var renderedComponentInstance = sut.Instance;

        // Assert
        Assert.That(renderedComponentInstance.LearningOutcome, Is.EqualTo(outcome));
        var li = sut.Find("li");
        li.MarkupMatches(
            $@"<li class=""marker:text-adlergrey cursor-default text-sm text-justify"">{outcome.GetOutcome()}</li>"
        );
    }

    [Test]
    // ANF-ID: [AHO04]
    public void ClickEdit_ManualOutcome_ShouldCallDialogProviderWithManualOutcomeDialog()
    {
        var collection = new LearningOutcomeCollectionViewModel();
        var outcome = ViewModelProvider.GetLearningOutcome(manual: true);
        collection.LearningOutcomes.Add(outcome);

        var sut = GetRenderedComponent(collection, outcome);

        sut.FindComponents<MudIconButton>()[0].Find("button").Click();

        _dialogService.Received().ShowAsync<CreateEditManualLearningOutcome>("", Arg.Is<DialogParameters>(d =>
            d[nameof(CreateEditManualLearningOutcome.LearningOutcomeCollection)] == collection &&
            d[nameof(CreateEditManualLearningOutcome.CurrentManualLearningOutcome)] == outcome
        ), Arg.Any<DialogOptions>());
    }

    [Test]
    // ANF-ID: [AHO03]
    public void ClickEdit_StructuredOutcome_ShouldCallDialogProviderWithManualOutcomeDialog()
    {
        var collection = new LearningOutcomeCollectionViewModel();
        var outcome = ViewModelProvider.GetLearningOutcome(manual: false);
        collection.LearningOutcomes.Add(outcome);

        var sut = GetRenderedComponent(collection, outcome);

        sut.FindComponents<MudIconButton>()[0].Find("button").Click();

        _dialogService.Received().ShowAsync<CreateEditStructuredLearningOutcome>("", Arg.Is<DialogParameters>(d =>
            d[nameof(CreateEditStructuredLearningOutcome.LearningOutcomes)] == collection &&
            d[nameof(CreateEditStructuredLearningOutcome.CurrentLearningOutcome)] == outcome
        ), Arg.Any<DialogOptions>());
    }

    [Test]
    // ANF-ID: [AHO05]
    public void ClickDelete_ShouldCallPresentationLogic()
    {
        var collection = new LearningOutcomeCollectionViewModel();
        var outcome = ViewModelProvider.GetLearningOutcome();
        collection.LearningOutcomes.Add(outcome);

        var sut = GetRenderedComponent(collection, outcome);

        sut.FindComponents<MudIconButton>()[1].Find("button").Click();

        _presentationLogic.Received().DeleteLearningOutcome(collection, outcome);
    }
    
    [Test]
    public void DisplayButtonsProperty_WorksAsExpected([Values] bool displayButtons)
    {
        var collection = new LearningOutcomeCollectionViewModel();
        var outcome = ViewModelProvider.GetLearningOutcome();
        collection.LearningOutcomes.Add(outcome);

        var sut = GetRenderedComponent(collection, outcome, displayButtons);

        var buttons = sut.FindComponents<MudIconButton>();
        var expected = displayButtons ? 2 : 0;
        Assert.That(buttons, Has.Count.EqualTo(expected));
    }


    private IRenderedComponent<LearningOutcomeItem> GetRenderedComponent(LearningOutcomeCollectionViewModel collection,
        ILearningOutcomeViewModel outcome, bool? displayButtons = null)
    {
        return _context.RenderComponent<LearningOutcomeItem>(pBuilder =>
        {
            pBuilder.Add(p => p.LearningOutcomeCollection, collection);
            pBuilder.Add(p => p.LearningOutcome, outcome);
            if(displayButtons is not null) pBuilder.Add(p => p.DisplayButtons, displayButtons.Value);
        });
    }
}