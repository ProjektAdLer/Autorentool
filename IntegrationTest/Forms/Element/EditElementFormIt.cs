using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bunit;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NSubstitute.ClearExtensions;
using NUnit.Framework;
using Presentation.Components.Forms;
using Presentation.Components.Forms.Buttons;
using Presentation.Components.Forms.Element;
using Presentation.Components.Forms.Models;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using PresentationTest;
using Shared;
using TestHelpers;

namespace IntegrationTest.Forms.Element;

[TestFixture]
public class EditElementFormIt : MudFormTestFixture<EditElementForm, LearningElementFormModel, LearningElement>
{
    [SetUp]
    public void Setup()
    {
        WorldPresenter = Substitute.For<ILearningWorldPresenter>();
        LearningContentViewModels = new ILearningContentViewModel[]
            { ViewModelProvider.GetFileContent(), ViewModelProvider.GetLinkContent() };
        WorldPresenter.GetAllContent().Returns(LearningContentViewModels);
        SpacePresenter = Substitute.For<ILearningSpacePresenter>();
        ElementModelHandler = Substitute.For<IElementModelHandler>();
        PresentationLogic = Substitute.For<IPresentationLogic>();
        Context.Services.AddSingleton(WorldPresenter);
        Context.Services.AddSingleton(SpacePresenter);
        Context.Services.AddSingleton(ElementModelHandler);
        Context.Services.AddSingleton(PresentationLogic);
    }

    public ILearningWorldPresenter WorldPresenter { get; set; }
    public ILearningSpacePresenter SpacePresenter { get; set; }
    public IElementModelHandler ElementModelHandler { get; set; }
    public IPresentationLogic PresentationLogic { get; set; }
    public ILearningContentViewModel[] LearningContentViewModels { get; set; }

    private const string Expected = "test";

    [Test]
    public void Render_InjectsDependenciesAndParameters()
    {
        var vm = ViewModelProvider.GetLearningElement();
        var onNewClicked = EventCallback.Empty;
        var masterLayoutStateHasChanged = () => { };

        var systemUnderTest = GetRenderedComponent(vm, onNewClicked, masterLayoutStateHasChanged);

        Assert.That(systemUnderTest.Instance.WorldPresenter, Is.EqualTo(WorldPresenter));
        Assert.That(systemUnderTest.Instance.FormDataContainer, Is.EqualTo(FormDataContainer));
        Assert.That(systemUnderTest.Instance.ElementModelHandler, Is.EqualTo(ElementModelHandler));
        Assert.That(systemUnderTest.Instance.PresentationLogic, Is.EqualTo(PresentationLogic));
        Assert.That(systemUnderTest.Instance.OnNewButtonClicked, Is.EqualTo(onNewClicked));
        Assert.That(systemUnderTest.Instance.DebounceInterval, Is.EqualTo(0));
        Assert.That(systemUnderTest.Instance.TriggerMasterLayoutStateHasChanged,
            Is.EqualTo(masterLayoutStateHasChanged));
    }

    [Test]
    public void OnParametersSet_CallsMapper()
    {
        var vm = ViewModelProvider.GetLearningElement();

        var systemUnderTest = GetRenderedComponent(vm);

        Mapper.Received(1).Map(vm, FormDataContainer.FormModel);
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
    [Retry(3)]
    public void SubmitThenRemapButton_CallsPresenterWithNewValues_ThenRemapsEntityIntoForm()
    {
        var vm = ViewModelProvider.GetLearningElement();
        var systemUnderTest = GetFormWithPopoverProvider(vm);
        var mudForm = systemUnderTest.FindComponent<MudForm>();
        var popover = systemUnderTest.FindComponent<MudPopoverProvider>();

        var collapsables = systemUnderTest.FindComponents<Collapsable>();
        collapsables[2].Find("div.toggler").Click();
        collapsables[3].Find("div.toggler").Click();

        ChangeFields(systemUnderTest, popover);

        AssertFieldsSet(systemUnderTest);

        Mapper.ClearReceivedCalls();

        systemUnderTest.FindComponent<SubmitThenRemapButton>().Find("button").Click();

        Assert.That(() => WorldPresenter.Received(2).EditLearningElement(vm.Parent, vm, Expected, Expected, Expected,
                LearningElementDifficultyEnum.Hard, ElementModel.l_random, 123, 123, LearningContentViewModels[0]),
            Throws.Nothing);
        Mapper.Received(1).Map(vm, FormDataContainer.FormModel);
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
        tableSelect.FindAll("tbody tr")[0].Click();
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

    [Test]
    public void ShowElementContentButton_Clicked_CallsShowSelectedElementContentAsync()
    {
        var vm = ViewModelProvider.GetLearningElement();

        var systemUnderTest = GetRenderedComponent(vm);

        systemUnderTest.FindComponentWithMarkup<MudIconButton>("btn-standard rounded").Find("button").Click();

        WorldPresenter.Received(1).ShowSelectedElementContentAsync(vm);
    }

    private IRenderedComponent<EditElementForm> GetRenderedComponent(ILearningElementViewModel? vm = null,
        EventCallback? onNewClicked = null, Action? masterLayoutStateHasChanged = null)
    {
        vm ??= ViewModelProvider.GetLearningElement();
        onNewClicked ??= EventCallback.Empty;
        masterLayoutStateHasChanged ??= () => { };
        return Context.RenderComponent<EditElementForm>(p =>
        {
            p.Add(c => c.ElementToEdit, vm);
            p.Add(c => c.OnNewButtonClicked, onNewClicked.Value);
            p.Add(c => c.DebounceInterval, 0);
            p.AddCascadingValue("TriggerMasterLayoutStateHasChanged", masterLayoutStateHasChanged);
        });
    }

    private IRenderedFragment GetFormWithPopoverProvider(ILearningElementViewModel? vm = null,
        EventCallback? onNewClicked = null, Action? masterLayoutStateHasChanged = null, int debounceInterval = 0)
    {
        vm ??= ViewModelProvider.GetLearningElement();
        onNewClicked ??= EventCallback.Empty;
        masterLayoutStateHasChanged ??= () => { };
        RenderFragment innerFrag = builder =>
        {
            builder.OpenComponent<MudPopoverProvider>(0);
            builder.CloseComponent();
            builder.OpenComponent<EditElementForm>(1);
            builder.AddAttribute(2, nameof(EditElementForm.ElementToEdit), vm);
            builder.AddAttribute(3, nameof(EditElementForm.DebounceInterval), debounceInterval);
            builder.AddAttribute(4, nameof(EditElementForm.OnNewButtonClicked), onNewClicked.Value);
            builder.CloseComponent();
        };
        return Context.Render(builder =>
        {
            builder.OpenComponent<CascadingValue<Action>>(0);
            builder.AddAttribute(1, "Value", masterLayoutStateHasChanged);
            builder.AddAttribute(2, "ChildContent", innerFrag);
            builder.AddAttribute(3, "Name", "TriggerMasterLayoutStateHasChanged");
            builder.CloseComponent();
        });
    }
}