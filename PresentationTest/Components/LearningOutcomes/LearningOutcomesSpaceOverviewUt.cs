using System.Collections.Generic;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.LearningOutcomes;
using Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;
using TestHelpers;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.LearningOutcomes;

[TestFixture]
public class LearningOutcomesSpaceOverviewUt
{
    private TestContext _context;
    private IDialogService _dialogService;

    [SetUp]
    public void Setup()
    {
        _context = new TestContext();
        _dialogService = Substitute.For<IDialogService>();
        _context.AddLocalizerForTest<LearningOutcomesSpaceOverview>();
        _context.Services.AddSingleton(_dialogService);
        _context.ComponentFactories.AddStub<LearningOutcomeItem>();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public void Render_NoOutcomesInCollection_DoesNotRenderOutcomesDiv()
    {
        var learningOutcomeCollection = new LearningOutcomeCollectionViewModel();
        var sut = GetRenderedComponent(learningOutcomeCollection);

        Assert.That(() => sut.Find("div.outcomes"), Throws.TypeOf<ElementNotFoundException>());
    }

    [Test]
    public void Render_OutcomesInCollection_RendersLearningOutcomeItemForEachItemInCollection()
    {
        var learningOutcomeCollection = new LearningOutcomeCollectionViewModel
        {
            LearningOutcomes = new List<ILearningOutcomeViewModel>()
                { ViewModelProvider.GetLearningOutcome(true), ViewModelProvider.GetLearningOutcome() }
        };

        var sut = GetRenderedComponent(learningOutcomeCollection);

        Assert.That(sut.FindComponents<Stub<LearningOutcomeItem>>(), Has.Count.EqualTo(2));
    }

    [Test]
    // ANF-ID: [AHO01]
    public void ClickCreateStructuredButton_CallsDialogService_WithStructuredLearningOutcomeDialog()
    {
        var collection = new LearningOutcomeCollectionViewModel();
        var sut = GetRenderedComponent(collection);

        sut.FindComponents<MudButton>()[0].Find("button").Click();

        _dialogService.Received().ShowAsync<CreateEditStructuredLearningOutcome>("",
            Arg.Is<DialogParameters>(dp =>
                dp[nameof(CreateEditStructuredLearningOutcome.LearningOutcomes)] == collection),
            Arg.Any<DialogOptions>());
    }

    [Test]
    // ANF-ID: [AHO02]
    public void ClickCreateManualButton_CallsDialogService_WithStructuredLearningOutcomeDialog()
    {
        var collection = new LearningOutcomeCollectionViewModel();
        var sut = GetRenderedComponent(collection);

        sut.FindComponents<MudButton>()[1].Find("button").Click();

        _dialogService.Received().ShowAsync<CreateEditManualLearningOutcome>("",
            Arg.Is<DialogParameters>(dp =>
                dp[nameof(CreateEditManualLearningOutcome.LearningOutcomeCollection)] == collection),
            Arg.Any<DialogOptions>());
    }

    private IRenderedComponent<LearningOutcomesSpaceOverview> GetRenderedComponent(
        LearningOutcomeCollectionViewModel learningOutcomeCollection)
    {
        return _context.RenderComponent<LearningOutcomesSpaceOverview>(pBuilder =>
        {
            pBuilder.Add(p => p.LearningOutcomeCollection, learningOutcomeCollection);
        });
    }
}