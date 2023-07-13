using System;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms;
using Presentation.Components.Forms.Buttons;
using Presentation.Components.Forms.Element;
using Presentation.Components.Forms.Models;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.SelectedViewModels;
using Shared;
using TestHelpers;

namespace IntegrationTest.Forms.Element;

[TestFixture]
public class CreateElementFormIt : MudFormTestFixture<CreateElementForm, LearningElementFormModel, LearningElement>
{
    public ILearningWorldPresenter WorldPresenter { get; set; }
    public ILearningSpacePresenter SpacePresenter { get; set; }
    public ISelectedViewModelsProvider SelectedViewModelsProvider { get; set; }
    public IElementModelHandler ElementModelHandler { get; set; }
    public IPresentationLogic PresentationLogic { get; set; }
    public ILearningContentViewModel[] LearningContentViewModels { get; set; }

    private const string Expected = "test";

    [SetUp]
    public void Setup()
    {
        WorldPresenter = Substitute.For<ILearningWorldPresenter>();
        LearningContentViewModels = new ILearningContentViewModel[]
            { ViewModelProvider.GetFileContent(), ViewModelProvider.GetLinkContent() };
        WorldPresenter.GetAllContent().Returns(LearningContentViewModels);
        SpacePresenter = Substitute.For<ILearningSpacePresenter>();
        SelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        SelectedViewModelsProvider.LearningContent.Returns((ILearningContentViewModel?)null);
        ElementModelHandler = Substitute.For<IElementModelHandler>();
        PresentationLogic = Substitute.For<IPresentationLogic>();
        Context.Services.AddSingleton(WorldPresenter);
        Context.Services.AddSingleton(SpacePresenter);
        Context.Services.AddSingleton(SelectedViewModelsProvider);
        Context.Services.AddSingleton(ElementModelHandler);
        Context.Services.AddSingleton(PresentationLogic);
    }


    [Test]
    public void Render_InjectsDependenciesAndParameters()
    {
        var onSubmitted = EventCallback.Factory.Create(this, () => { });

        var systemUnderTest = GetRenderedComponent(onSubmitted);

        Assert.That(systemUnderTest.Instance.WorldPresenter, Is.EqualTo(WorldPresenter));
        Assert.That(systemUnderTest.Instance.SpacePresenter, Is.EqualTo(SpacePresenter));
        Assert.That(systemUnderTest.Instance.SelectedViewModelsProvider, Is.EqualTo(SelectedViewModelsProvider));
        Assert.That(systemUnderTest.Instance.ElementModelHandler, Is.EqualTo(ElementModelHandler));
        Assert.That(systemUnderTest.Instance.PresentationLogic, Is.EqualTo(PresentationLogic));
        Assert.That(systemUnderTest.Instance.OnSubmitted, Is.EqualTo(onSubmitted));
        Assert.That(systemUnderTest.Instance.DebounceInterval, Is.EqualTo(0));
    }

    [Test]
    public void Initialize_SelectedViewModelsProviderContentSet_SetsInFormModelAndResetsProvider()
    {
        SelectedViewModelsProvider.LearningContent.Returns(LearningContentViewModels.First());

        var systemUnderTest = GetRenderedComponent();

        Assert.That(FormModel.LearningContent, Is.EqualTo(LearningContentViewModels.First()));
        SelectedViewModelsProvider.Received(1).SetLearningContent(null, null);
    }

    [Test]
    public async Task ChangeFieldValues_ChangesContainerValuesAndCallsValidation()
    {
        var systemUnderTest = GetFormWithPopoverProvider();
        var mudForm = systemUnderTest.FindComponent<MudForm>();
        var popover = systemUnderTest.FindComponent<MudPopoverProvider>();

        var collapsables = systemUnderTest.FindComponents<Collapsable>();
        collapsables[2].Find("div.toggler").Click();
        collapsables[3].Find("div.toggler").Click();
        //await systemUnderTest.InvokeAsync(() => systemUnderTest);

        ConfigureValidatorAllMembers();

        Assert.That(FormModel.Name, Is.EqualTo(""));
        Assert.That(FormModel.Description, Is.EqualTo(""));
        Assert.That(FormModel.Goals, Is.EqualTo(""));
        Assert.That(FormModel.Difficulty, Is.EqualTo(LearningElementDifficultyEnum.None));
        Assert.That(FormModel.ElementModel, Is.EqualTo(ElementModel.l_random));
        Assert.That(FormModel.Workload, Is.EqualTo(0));
        Assert.That(FormModel.Points, Is.EqualTo(1));
        Assert.That(FormModel.LearningContent, Is.EqualTo(null));
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.False);

        ChangeFields(systemUnderTest, popover);

        AssertFieldsSet(systemUnderTest);
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.True);
    }

    [Test]
    public void ResetButtonClicked_ResetsForm()
    {
        var systemUnderTest = GetRenderedComponent();

        Assert.That(FormDataContainer.FormModel.Name, Is.EqualTo(""));

        var mudTextField = systemUnderTest.FindComponent<MudTextField<string>>();
        mudTextField.Find("input").Change(Expected);

        Assert.That(FormDataContainer.FormModel.Name, Is.EqualTo(Expected));

        var resetButton = systemUnderTest.FindComponent<DefaultResetButton>();
        resetButton.Find("button").Click();

        Assert.That(FormDataContainer.FormModel.Name, Is.EqualTo(""));
    }

    [Test]
    public async Task SubmitButtonClicked_SubmitsIfFormValid()
    {
        var callbackCalledCount = 0;
        var callback = EventCallback.Factory.Create(this, () => callbackCalledCount++);
        var systemUnderTest = GetFormWithPopoverProvider(callback);
        var mudForm = systemUnderTest.FindComponent<MudForm>();
        var popover = systemUnderTest.FindComponent<MudPopoverProvider>();

        var collapsables = systemUnderTest.FindComponents<Collapsable>();
        collapsables[2].Find("div.toggler").Click();
        collapsables[3].Find("div.toggler").Click();
        //await systemUnderTest.InvokeAsync(() => systemUnderTest);

        ConfigureValidatorAllMembers();

        Assert.That(FormModel.Name, Is.EqualTo(""));
        Assert.That(FormModel.Description, Is.EqualTo(""));
        Assert.That(FormModel.Goals, Is.EqualTo(""));
        Assert.That(FormModel.Difficulty, Is.EqualTo(LearningElementDifficultyEnum.None));
        Assert.That(FormModel.ElementModel, Is.EqualTo(ElementModel.l_random));
        Assert.That(FormModel.Workload, Is.EqualTo(0));
        Assert.That(FormModel.Points, Is.EqualTo(1));
        Assert.That(FormModel.LearningContent, Is.EqualTo(null));
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.False);

        var submitButton = systemUnderTest.FindComponent<DefaultSubmitButton>();
        submitButton.Find("button").Click();
        WorldPresenter.DidNotReceiveWithAnyArgs().CreateUnplacedLearningElement(Arg.Any<string>(),
            Arg.Any<ILearningContentViewModel>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<ElementModel>(), Arg.Any<int>(), Arg.Any<int>());
        SpacePresenter.DidNotReceive().CreateLearningElementInSlot(Arg.Any<string>(),
            Arg.Any<ILearningContentViewModel>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<ElementModel>(), Arg.Any<int>(), Arg.Any<int>());
        Assert.That(callbackCalledCount, Is.EqualTo(0));

        ChangeFields(systemUnderTest, popover);
        AssertFieldsSet(systemUnderTest);
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.True);

        WorldPresenter.ClearReceivedCalls();
        SpacePresenter.ClearReceivedCalls();

        SelectedViewModelsProvider.ActiveSlotInSpace.Returns(-1);
        submitButton.Find("button").Click();
        WorldPresenter.Received().CreateUnplacedLearningElement(Expected, LearningContentViewModels[0], Expected,
            Expected, LearningElementDifficultyEnum.Hard, ElementModel.l_random, 123, 123);
        SpacePresenter.DidNotReceive().CreateLearningElementInSlot(Expected, LearningContentViewModels[0], Expected,
            Expected, LearningElementDifficultyEnum.Hard, ElementModel.l_random, 123, 123);
        Assert.That(callbackCalledCount, Is.EqualTo(1));

        ChangeFields(systemUnderTest, popover);
        AssertFieldsSet(systemUnderTest);
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.True);

        WorldPresenter.ClearReceivedCalls();
        SpacePresenter.ClearReceivedCalls();

        SelectedViewModelsProvider.ActiveSlotInSpace.Returns(0);
        submitButton.Find("button").Click();
        submitButton.WaitForAssertion(() =>
            WorldPresenter.DidNotReceive().CreateUnplacedLearningElement(Expected, LearningContentViewModels[0],
                Expected,
                Expected, LearningElementDifficultyEnum.Hard, ElementModel.l_random, 123, 123),
            TimeSpan.FromSeconds(2));
        submitButton.WaitForAssertion(() =>
            SpacePresenter.Received().CreateLearningElementInSlot(Expected, LearningContentViewModels[0], Expected,
                Expected, LearningElementDifficultyEnum.Hard, ElementModel.l_random, 123, 123),
            TimeSpan.FromSeconds(2));
        Assert.That(callbackCalledCount, Is.EqualTo(2));
    }

    private void AssertFieldsSet(IRenderedFragment systemUnderTest)
    {
        Assert.That(FormModel.Name, Is.EqualTo(Expected));
        Assert.That(FormModel.Description, Is.EqualTo(Expected));
        Assert.That(FormModel.Goals, Is.EqualTo(Expected));
        systemUnderTest.WaitForAssertion(
            () => Assert.That(FormModel.Difficulty, Is.EqualTo(LearningElementDifficultyEnum.Hard)),
            TimeSpan.FromSeconds(2));
        Assert.That(FormModel.ElementModel, Is.EqualTo(ElementModel.l_random));
        Assert.That(FormModel.Workload, Is.EqualTo(123));
        Assert.That(FormModel.Points, Is.EqualTo(123));
        systemUnderTest.WaitForAssertion(
            () => Assert.That(FormModel.LearningContent, Is.EqualTo(LearningContentViewModels[0])),
            TimeSpan.FromSeconds(2));
    }

    private static void ChangeFields(IRenderedFragment systemUnderTest, IRenderedComponent<MudPopoverProvider> popover)
    {
        var mudTextFields = systemUnderTest.FindComponents<MudTextField<string>>();
        var mudNumericFields = systemUnderTest.FindComponents<MudNumericField<int>>();
        var mudSelect = systemUnderTest.FindComponent<MudSelect<LearningElementDifficultyEnum>>();
        var tableSelect = systemUnderTest.FindComponent<TableSelect<ILearningContentViewModel>>();
        mudTextFields[0].Find("input").Change(Expected);
        mudTextFields[2].Find("textarea").Change(Expected);
        mudTextFields[3].Find("textarea").Change(Expected);
        mudNumericFields[0].Find("input").Change(123);
        mudNumericFields[1].Find("input").Change(123);
        mudSelect.Find("div.mud-input-control").Click();
        popover.Render();
        popover.WaitForElements("div.mud-list-item", TimeSpan.FromSeconds(2))[2].Click();
        tableSelect.WaitForElements("tbody tr", TimeSpan.FromSeconds(2))[0].Click();
    }


    private void ConfigureValidatorAllMembers()
    {
        Validator.ValidateAsync(Entity, Arg.Any<string>()).Returns(ci =>
            {
                var value = FormModel.GetType().GetProperty(ci.Arg<string>())?.GetValue(FormModel);
                var valid = value switch
                {
                    null => false,
                    string str => str == Expected,
                    int i => i == 123,
                    LearningElementDifficultyEnum difficulty => difficulty == LearningElementDifficultyEnum.Hard,
                    ElementModel model => model == ElementModel.l_random,
                    ILearningContentViewModel => true,
                    _ => throw new ArgumentOutOfRangeException()
                };
                return valid ? Enumerable.Empty<string>() : new[] { "Must be test or 123" };
            }
        );
    }

    private IRenderedComponent<CreateElementForm> GetRenderedComponent(EventCallback? onSubmitted = null)
    {
        onSubmitted ??= EventCallback.Empty;
        return Context.RenderComponent<CreateElementForm>(p =>
        {
            p.Add(c => c.OnSubmitted, onSubmitted.Value);
            p.Add(c => c.DebounceInterval, 0);
        });
    }

    private IRenderedFragment GetFormWithPopoverProvider(EventCallback? onSubmitted = null)
    {
        onSubmitted ??= EventCallback.Empty;
        return Context.Render(builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();
            builder.OpenComponent<CreateElementForm>(1);
            builder.AddAttribute(2, nameof(CreateElementForm.OnSubmitted), onSubmitted);
            builder.AddAttribute(3, nameof(CreateElementForm.DebounceInterval), 0);
            builder.CloseComponent();
        });
    }
}