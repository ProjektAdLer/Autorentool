using System;
using System.Linq;
using System.Threading.Tasks;
using Bunit;
using Bunit.TestDoubles;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Adaptivity.Dialogues;
using Presentation.Components.Forms;
using Presentation.Components.Forms.Buttons;
using Presentation.Components.Forms.Content;
using Presentation.Components.Forms.Element;
using Presentation.Components.Forms.Models;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.SelectedViewModels;
using PresentationTest;
using Shared;
using TestHelpers;

namespace IntegrationTest.Forms.Element;

[TestFixture]
public class CreateElementFormIt : MudFormTestFixture<CreateElementForm, LearningElementFormModel, LearningElement>
{
    [SetUp]
#pragma warning disable CS0108, CS0114
    public void Setup()
#pragma warning restore CS0108, CS0114
    {
        WorldPresenter = Substitute.For<ILearningWorldPresenter>();
        LearningContentViewModels = new ILearningContentViewModel[]
            { ViewModelProvider.GetFileContent(), ViewModelProvider.GetLinkContent() };
        WorldPresenter.GetAllContent().Returns(LearningContentViewModels);
        SpacePresenter = Substitute.For<ILearningSpacePresenter>();
        SelectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        SelectedViewModelsProvider.LearningContent.Returns((ILearningContentViewModel?)null);
        ElementModelHandler = Substitute.For<IElementModelHandler>();
        ElementModelHandler.GetElementModels(Arg.Any<ElementModelContentType>(), Arg.Any<string>(), Arg.Any<Theme?>())
            .Returns(new[]
            {
                ElementModel.l_random
            });
        PresentationLogic = Substitute.For<IPresentationLogic>();
        var localizer = Substitute.For<IStringLocalizer<ElementModelGridSelect>>();
        Context.Services.AddSingleton(WorldPresenter);
        Context.Services.AddSingleton(SpacePresenter);
        Context.Services.AddSingleton(SelectedViewModelsProvider);
        Context.Services.AddSingleton(ElementModelHandler);
        Context.Services.AddSingleton(PresentationLogic);
        Context.Services.AddSingleton(localizer);
        Context.ComponentFactories.AddStub<NoContentWarning>();

        LearningContentFormModels = new[]
        {
            FormModelProvider.GetFileContent(),
            FormModelProvider.GetLinkContent()
        };
        Mapper.Map<ILearningContentFormModel>(LearningContentViewModels[0]).Returns(LearningContentFormModels[0]);
        Mapper.Map<ILearningContentFormModel>(LearningContentViewModels[1]).Returns(LearningContentFormModels[1]);

        var learningContent = LearningContentFormModels[0];
        var dialogReference = Substitute.For<IDialogReference>();
        dialogReference.Result.Returns(DialogResult.Ok(learningContent));

        _dialogServiceMock = Substitute.For<IDialogService>();
        _dialogServiceMock
            .ShowAsync<LearningContentDialog>(Arg.Any<string>(), Arg.Any<DialogParameters>(), Arg.Any<DialogOptions>())
            .Returns(dialogReference);

        Context.Services.AddSingleton(_dialogServiceMock);
    }


    private const string Expected = "test";
    private IDialogService _dialogServiceMock = Substitute.For<IDialogService>();


    [Test]
    public void Render_InjectsDependenciesAndParameters()
    {
        var onSubmitted = EventCallback.Factory.Create(this, () => { });

        var systemUnderTest = GetRenderedComponent();

        Assert.That(systemUnderTest.Instance.WorldPresenter, Is.EqualTo(WorldPresenter));
        Assert.That(systemUnderTest.Instance.SpacePresenter, Is.EqualTo(SpacePresenter));
        Assert.That(systemUnderTest.Instance.SelectedViewModelsProvider, Is.EqualTo(SelectedViewModelsProvider));
        Assert.That(systemUnderTest.Instance.ElementModelHandler, Is.EqualTo(ElementModelHandler));
        Assert.That(systemUnderTest.Instance.PresentationLogic, Is.EqualTo(PresentationLogic));
        Assert.That(systemUnderTest.Instance.DebounceInterval, Is.EqualTo(0));
    }

    [Test]
    public void Initialize_SelectedViewModelsProviderContentSet_SetsInFormModelAndResetsProvider()
    {
        SelectedViewModelsProvider.LearningContent.Returns(LearningContentViewModels.First());

        GetRenderedComponent();

        Assert.That(FormModel.LearningContent, Is.EqualTo(LearningContentFormModels.First()));
        SelectedViewModelsProvider.Received(1).SetLearningContent(null, null);
    }

    [Test]
    public void Initialize_AdaptivityElementModeTrue_SetsContentInFormModel()
    {
        Assert.That(FormModel.LearningContent, Is.Null);
        GetFormWithPopoverProvider(elementMode: ElementMode.Adaptivity);

        Assert.That(FormModel.LearningContent, Is.TypeOf<AdaptivityContentFormModel>());
    }

    [Test]
    public void Initialize_AdaptivityElementModeTrue_RendersTaskCollapsibleInstead()
    {
        var systemUnderTest = GetFormWithPopoverProvider(elementMode: ElementMode.Adaptivity);

        var collapsables = systemUnderTest.FindComponents<Collapsable>();
        Assert.That(() => collapsables.Single(collapsable =>
                collapsable.Instance.Title == "CreateAdaptivityElementForm.Fields.Collapsable.Tasks.Title"),
            Throws.Nothing);
    }

    [Test]
    // ANF-ID: [AWA0002]
    public async Task ChangeFieldValues_ChangesContainerValuesAndCallsValidation()
    {
        var systemUnderTest = GetFormWithPopoverProvider();
        var mudForm = systemUnderTest.FindComponent<MudForm>();
        var popover = systemUnderTest.FindComponent<MudPopoverProvider>();

        var collapsables = systemUnderTest.FindComponents<Collapsable>();
        collapsables[2].Find("div.toggler").Click();
        collapsables[3].Find("div.toggler").Click();
        collapsables[4].Find("div.toggler").Click();
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
    // ANF-ID: [AWA0002]
    public async Task SubmitButtonClicked_SubmitsIfFormValid()
    {
        var systemUnderTest = GetFormWithPopoverProvider();
        var mudForm = systemUnderTest.FindComponent<MudForm>();
        var popover = systemUnderTest.FindComponent<MudPopoverProvider>();

        var collapsables = systemUnderTest.FindComponents<Collapsable>();
        collapsables[2].Find("div.toggler").Click();
        collapsables[3].Find("div.toggler").Click();
        collapsables[4].Find("div.toggler").Click();
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
        systemUnderTest.WaitForAssertion(() =>
            WorldPresenter.DidNotReceiveWithAnyArgs().CreateUnplacedLearningElement(Arg.Any<string>(),
                Arg.Any<ILearningContentViewModel>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<ElementModel>(), Arg.Any<int>(), Arg.Any<int>()));
        systemUnderTest.WaitForAssertion(() =>
            SpacePresenter.DidNotReceive().CreateLearningElementInSlot(Arg.Any<string>(),
                Arg.Any<ILearningContentViewModel>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<ElementModel>(), Arg.Any<int>(), Arg.Any<int>()));

        ChangeFields(systemUnderTest, popover);
        AssertFieldsSet(systemUnderTest);
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.True);

        WorldPresenter.ClearReceivedCalls();
        SpacePresenter.ClearReceivedCalls();

        SelectedViewModelsProvider.ActiveElementSlotInSpace.Returns(-1);
        submitButton.Find("button").Click();
        systemUnderTest.WaitForAssertion(() =>
            WorldPresenter.Received().CreateUnplacedLearningElementFromFormModel(FormModel));
        systemUnderTest.WaitForAssertion(() =>
            SpacePresenter.DidNotReceive().CreateLearningElementInSlotFromFormModel(FormModel));

        ChangeFields(systemUnderTest, popover);
        AssertFieldsSet(systemUnderTest);
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.True);

        WorldPresenter.ClearReceivedCalls();
        SpacePresenter.ClearReceivedCalls();
        //form resets formmodel after submit so we need to catch the newest reference
        FormModel = FormDataContainer.FormModel;

        SelectedViewModelsProvider.ActiveElementSlotInSpace.Returns(0);
        submitButton.Find("button").Click();
        submitButton.WaitForAssertion(() =>
                WorldPresenter.DidNotReceive().CreateUnplacedLearningElement(Expected, LearningContentViewModels[0],
                    Expected,
                    Expected, LearningElementDifficultyEnum.Hard, ElementModel.l_random, 123, 123),
            TimeSpan.FromSeconds(2));
        submitButton.WaitForAssertion(() =>
                SpacePresenter.Received().CreateLearningElementInSlotFromFormModel(FormModel),
            TimeSpan.FromSeconds(2));
    }

    [Test]
    // ANF-ID: [AWA0002]
    public async Task EnterKeyPressed_SubmitsIfFormValid()
    {
        var systemUnderTest = GetFormWithPopoverProvider();
        var mudForm = systemUnderTest.FindComponent<MudForm>();
        var popover = systemUnderTest.FindComponent<MudPopoverProvider>();

        var collapsables = systemUnderTest.FindComponents<Collapsable>();
        collapsables[2].Find("div.toggler").Click();
        collapsables[3].Find("div.toggler").Click();
        collapsables[4].Find("div.toggler").Click();

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

        var mudInput = systemUnderTest.FindComponent<MudTextField<string>>();
        var input = mudInput.Find("input");
        input.KeyUp(Key.Enter);
        SpacePresenter.DidNotReceive().CreateLearningElementInSlot(Arg.Any<string>(),
            Arg.Any<ILearningContentViewModel>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<LearningElementDifficultyEnum>(), Arg.Any<ElementModel>(), Arg.Any<int>(), Arg.Any<int>());

        ChangeFields(systemUnderTest, popover);

        Assert.That(FormDataContainer.FormModel.Name, Is.EqualTo(Expected));
        input.KeyUp(Key.Enter);
        systemUnderTest.WaitForAssertion(
            () => SpacePresenter.Received().CreateLearningElementInSlotFromFormModel(FormModel),
            TimeSpan.FromSeconds(2));
    }


    [Test]
    // ANF-ID: [AWA0002]
    public void NoContentAvailable_ShowsNoContentWarningInsteadOfTableSelect()
    {
        WorldPresenter.GetAllContent().Returns(Enumerable.Empty<ILearningContentViewModel>());
        var systemUnderTest = GetFormWithPopoverProvider();
        var popover = systemUnderTest.FindComponent<MudPopoverProvider>();

        Assert.That(systemUnderTest.HasComponent<Stub<NoContentWarning>>(), Is.True);
    }

    [Test]
    // ANF-ID: [AWA0003]
    public async Task AddTasksButtonClicked_OpensAdaptivityContentDialog()
    {
        var dialogServiceMock = Substitute.For<IDialogService>();
        Context.Services.AddSingleton(dialogServiceMock);

        var systemUnderTest = GetFormWithPopoverProvider(elementMode: ElementMode.Adaptivity);

        var button = systemUnderTest.FindComponentWithMarkup<MudButton>("add-tasks");
        button.Find("button").Click();

        await dialogServiceMock.Received(1).ShowAsync<AdaptivityContentDialog>(Arg.Any<string>(),
            Arg.Any<DialogParameters>(), Arg.Any<DialogOptions>());
    }

    [Test]
    public void StoryMode_ShowsStoryContentCollapsable_SubmitsOnClick()
    {
        var sut = GetRenderedComponent(ElementMode.Story);

        var collapsables = sut.FindComponents<Collapsable>();

        var storyCollapsable = collapsables.First(collapsable =>
            collapsable.Instance.Title == "CreateStoryElementForm.Fields.Collapsable.Story.Title");

        var addButton = storyCollapsable.FindComponentWithMarkup<MudIconButton>("add-story-block-button");
        addButton.Find("button").Click();

        var tfs = storyCollapsable.FindComponentsOrFail<MudTextField<string>>().ToList();
        tfs.ElementAt(0).Find("textarea").Change("a sob story");
        tfs.ElementAt(1).Find("textarea").Change("a happy story");

        var submitButton = sut.FindComponent<DefaultSubmitButton>();
        submitButton.Find("button").Click();

        SpacePresenter.Received().CreateStoryElementInSlotFromFormModel(FormModel);
        var storyContent = (StoryContentFormModel)FormModel.LearningContent!;
        Assert.That(storyContent.StoryText, Has.Count.EqualTo(2));
        Assert.That(storyContent.StoryText.ElementAt(0), Is.EqualTo("a sob story"));
        Assert.That(storyContent.StoryText.ElementAt(1), Is.EqualTo("a happy story"));
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
            () => Assert.That(FormModel.LearningContent, Is.EqualTo(LearningContentFormModels[0])),
            TimeSpan.FromSeconds(2));
    }

    private static void ChangeFields(IRenderedFragment systemUnderTest, IRenderedComponent<MudPopoverProvider> popover)
    {
        var mudTextFields = systemUnderTest.FindComponents<MudTextField<string>>();
        var mudNumericFields = systemUnderTest.FindComponents<MudNumericField<int>>();
        var mudSelect = systemUnderTest.FindComponent<MudSelect<LearningElementDifficultyEnum>>();

        var editContentButton = systemUnderTest.FindComponents<MudIconButton>();
        editContentButton[1].Find("button").Click();

        mudTextFields[0].Find("input").Change(Expected);
        mudTextFields[1].Find("textarea").Change(Expected);
        mudTextFields[2].Find("textarea").Change(Expected);
        mudNumericFields[0].Find("input").Change(123);
        mudNumericFields[1].Find("input").Change(123);
        mudSelect.Find("div.mud-input-control").Click();
        popover.Render();
        popover.WaitForElements("div.mud-list-item", TimeSpan.FromSeconds(2))[2].Click();
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
                    ILearningContentFormModel => true,
                    _ => throw new ArgumentOutOfRangeException()
                };
                return valid ? Enumerable.Empty<string>() : new[] { "Must be test or 123" };
            }
        );
    }

    private IRenderedComponent<CreateElementForm> GetRenderedComponent(ElementMode elementMode = ElementMode.Normal)
    {
        return Context.RenderComponent<CreateElementForm>(p =>
        {
            p.Add(c => c.DebounceInterval, 0);
            p.Add(c => c.ElementMode, elementMode);
        });
    }

    private IRenderedFragment GetFormWithPopoverProvider(ElementMode elementMode = ElementMode.Normal)
    {
        return Context.Render(builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();
            builder.OpenComponent<CreateElementForm>(1);
            builder.AddAttribute(3, nameof(CreateElementForm.DebounceInterval), 0);
            builder.AddAttribute(4, nameof(CreateElementForm.ElementMode), elementMode);
            builder.CloseComponent();
        });
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private ILearningWorldPresenter WorldPresenter { get; set; }
    private ILearningSpacePresenter SpacePresenter { get; set; }
    private ISelectedViewModelsProvider SelectedViewModelsProvider { get; set; }
    private IElementModelHandler ElementModelHandler { get; set; }
    private IPresentationLogic PresentationLogic { get; set; }
    private ILearningContentViewModel[] LearningContentViewModels { get; set; }
    private ILearningContentFormModel[] LearningContentFormModels { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}