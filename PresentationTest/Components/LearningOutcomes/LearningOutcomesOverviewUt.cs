using System;
using System.Collections.ObjectModel;
using Bunit;
using Bunit.TestDoubles;
using BusinessLogic.Entities;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.LearningOutcomes;
using Presentation.PresentationLogic.LearningOutcome;
using TestHelpers;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.LearningOutcomes;

[TestFixture]
public class LearningOutcomesOverviewUt
{
    private TestContext _context;
    private IDialogService _dialogService;

    [SetUp]
    public void Setup()
    {
        _context = new TestContext();
        _dialogService = Substitute.For<IDialogService>();
        _context.AddLocalizerForTest<LearningOutcomesOverview>();
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
            LearningOutcomes = new ObservableCollection<ILearningOutcomeViewModel>()
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

    private IRenderedComponent<LearningOutcomesOverview> GetRenderedComponent(
        LearningOutcomeCollectionViewModel learningOutcomeCollection, Type? entityType = null)
    {
        entityType ??= typeof(LearningSpace);
        return _context.RenderComponent<LearningOutcomesOverview>(pBuilder =>
        {
            pBuilder.Add(p => p.LearningOutcomeCollection, learningOutcomeCollection);
            pBuilder.Add(p => p.EntityType, entityType);
        });
    }
}