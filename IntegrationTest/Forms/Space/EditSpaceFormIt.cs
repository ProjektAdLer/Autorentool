using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bunit;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms;
using Presentation.Components.Forms.Buttons;
using Presentation.Components.Forms.Models;
using Presentation.Components.Forms.Space;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using PresentationTest;
using Shared;
using Shared.Theme;
using TestHelpers;

namespace IntegrationTest.Forms.Space;

[TestFixture]
public class EditSpaceFormIt : MudFormTestFixture<EditSpaceForm, LearningSpaceFormModel, LearningSpace>
{
    [SetUp]
    public new void Setup()
    {
        SpacePresenter = Substitute.For<ILearningSpacePresenter>();
        WorldPresenter = Substitute.For<ILearningWorldPresenter>();
        Mapper = Substitute.For<IMapper>();
        var themeLocalizer = Substitute.For<IStringLocalizer<SpaceTheme>>();
        themeLocalizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
        ThemeHelper<SpaceTheme>.Initialize(themeLocalizer);
        Context.Services.AddSingleton(SpacePresenter);
        Context.Services.AddSingleton(WorldPresenter);
        Context.Services.AddSingleton(Mapper);
        Context.AddLocalizerForTest<SpaceLayoutSelection>();
        Context.AddLocalizerForTest<FloorPlanEnum>();
        Context.ComponentFactories.AddStub<SpaceLayoutSelection>();
        Context.RenderComponent<MudPopoverProvider>();
    }

    private ILearningSpacePresenter SpacePresenter { get; set; }
    private ILearningWorldPresenter WorldPresenter { get; set; }
    private IMapper Mapper { get; set; }
    private const string Expected = "test";

    [Test]
    public void Render_SetsParameters()
    {
        var vm = ViewModelProvider.GetLearningSpace();
        var onNewClicked = EventCallback.Factory.Create(this, () => { });
        SpacePresenter.LearningSpaceVm.Returns(vm);

        var systemUnderTest = GetRenderedComponent(vm, onNewClicked);

        Assert.That(systemUnderTest.Instance.SpaceToEdit, Is.EqualTo(vm));
        Assert.That(systemUnderTest.Instance.OnNewButtonClicked, Is.EqualTo(onNewClicked));
        Assert.That(systemUnderTest.Instance.DebounceInterval, Is.EqualTo(0));
    }

    [Test]
    public void OnParametersSet_CallsMapper()
    {
        var vm = ViewModelProvider.GetLearningSpace();
        SpacePresenter.LearningSpaceVm.Returns(vm);

        _ = GetRenderedComponent(vm);

        Mapper.Received(1).Map(vm, FormDataContainer.FormModel);
    }

    [Test]
    // ANF-ID: [AWA0023]
    public void SpacePresenterLearningSpace_PropertyChanged_RemapsIntoContainer()
    {
        var vm = ViewModelProvider.GetLearningSpace();
        SpacePresenter.LearningSpaceVm.Returns(vm);

        _ = GetRenderedComponent(vm);

        Mapper.Received(1).Map(vm, FormDataContainer.FormModel);

        Assert.That(vm.Name, Is.Not.EqualTo("foobar"));
        vm.Name = "foobar";

        Mapper.Received(2).Map(vm, FormDataContainer.FormModel);
    }

    [Test]
    // ANF-ID: [AWA0023]
    public void ResetButton_Clicked_RemapsIntoContainer()
    {
        var vm = ViewModelProvider.GetLearningSpace();
        SpacePresenter.LearningSpaceVm.Returns(vm);

        var systemUnderTest = GetRenderedComponent(vm);

        Mapper.Received(1).Map(vm, FormDataContainer.FormModel);
        Mapper.ClearReceivedCalls();

        systemUnderTest.FindComponentWithMarkup<MudIconButton>("reset-form").Find("button").Click();

        Mapper.Received(1).Map(vm, FormDataContainer.FormModel);
    }

    [Test]
    // ANF-ID: [AWA0023]
    public async Task ChangeFieldValues_ChangesContainerValuesAndCallsValidation()
    {
        var vm = ViewModelProvider.GetLearningSpace();
        SpacePresenter.LearningSpaceVm.Returns(vm);

        var systemUnderTest = GetRenderedComponent(vm);
        var mudForm = systemUnderTest.FindComponent<MudForm>();

        var collapsables = systemUnderTest.FindComponents<Collapsable>();
        collapsables[1].Find("div.toggler").Click();
        collapsables[3].Find("div.toggler").Click();
        collapsables[4].Find("div.toggler").Click();
        collapsables[5].Find("div.toggler").Click();

        ConfigureValidatorAllMembersTestOr123OrCampus();

        Assert.That(FormModel.Name, Is.EqualTo(""));
        Assert.That(FormModel.Description, Is.EqualTo(""));
        Assert.That(FormModel.RequiredPoints, Is.EqualTo(0));
        Assert.That(FormModel.SpaceTheme, Is.EqualTo(default(SpaceTheme)));
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.False);

        var mudStringInputs = systemUnderTest.FindComponents<MudTextField<string>>();
        var mudSelect = systemUnderTest.FindComponent<MudSelect<SpaceTheme>>();

        mudStringInputs[0].Find("input").Change(Expected);
        mudStringInputs[1].Find("textarea").Change(Expected);
        //TODO: once we have more themes, change to a different theme and test that
        mudSelect.Find("input").Change(SpaceTheme.LearningArea);

        systemUnderTest.WaitForAssertion(() => Assert.That(FormModel.Name, Is.EqualTo(Expected)),
            TimeSpan.FromSeconds(2));
        Assert.That(FormModel.Description, Is.EqualTo(Expected));
        Assert.That(FormModel.SpaceTheme, Is.EqualTo(SpaceTheme.LearningArea));
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.True);
    }

    [Test]
    // ANF-ID: [AWA0023]
    public void SubmitThenRemapButton_CallsPresenterWithNewValues_ThenRemapsEntityIntoForm()
    {
        var vm = ViewModelProvider.GetLearningSpace();
        SpacePresenter.LearningSpaceVm.Returns(vm);
        var assertionAttempts = 0;

        var systemUnderTest = GetRenderedComponent(vm);
        Mapper.Received(1).Map(vm, FormDataContainer.FormModel);
        Mapper.ClearReceivedCalls();

        var collapsables = systemUnderTest.FindComponents<Collapsable>();
        collapsables[1].Find("div.toggler").Click();
        collapsables[3].Find("div.toggler").Click();
        collapsables[4].Find("div.toggler").Click();
        collapsables[5].Find("div.toggler").Click();

        var mudStringInputs = systemUnderTest.FindComponents<MudTextField<string>>();
        var mudSelect = systemUnderTest.FindComponent<MudSelect<SpaceTheme>>();

        mudStringInputs[0].Find("input").Change(Expected);
        mudStringInputs[1].Find("textarea").Change(Expected);
        //TODO: once we have more themes, change to a different theme and test that
        mudSelect.Find("input").Change(SpaceTheme.LearningArea);

        Assert.Multiple(() =>
        {
            Assert.That(() => FormModel.Name, Is.EqualTo(Expected).After(300, 10));
            Assert.That(() => FormModel.Description, Is.EqualTo(Expected).After(300, 10));
            Assert.That(() => FormModel.SpaceTheme, Is.EqualTo(SpaceTheme.LearningArea).After(300, 10));
        });

        Mapper.ClearReceivedCalls();

        systemUnderTest.FindComponent<SubmitThenRemapButton>().Find("button").Click();

        SpacePresenter.Received(2).EditLearningSpace(Expected, Expected, 0, SpaceTheme.LearningArea);
        systemUnderTest.WaitForAssertion(() =>
            {
                Mapper.Received(1).Map(vm, FormDataContainer.FormModel);
                assertionAttempts++;
            },
            TimeSpan.FromSeconds(3));
        Console.WriteLine($@"{nameof(SubmitThenRemapButton_CallsPresenterWithNewValues_ThenRemapsEntityIntoForm)}: Assertion attempts: {assertionAttempts}");
    }

    private void ConfigureValidatorAllMembersTestOr123OrCampus()
    {
        Validator.ValidateAsync(Entity, Arg.Any<string>()).Returns(ci =>
            {
                var value = FormModel.GetType().GetProperty(ci.Arg<string>())?.GetValue(FormModel);
                var valid = value switch
                {
                    string str => str == Expected,
                    int i => i == 123,
                    //TODO: once we have more themes, change to a different theme and test that
                    SpaceTheme t => t == SpaceTheme.LearningArea,
                    _ => throw new ArgumentOutOfRangeException()
                };
                return valid ? Enumerable.Empty<string>() : new[] { "Must be test or 123" };
            }
        );
    }

    private IRenderedComponent<EditSpaceForm> GetRenderedComponent(ILearningSpaceViewModel? vm = null,
        EventCallback? onNewButtonClicked = null)
    {
        vm ??= ViewModelProvider.GetLearningSpace();
        onNewButtonClicked ??= EventCallback.Empty;
        return Context.RenderComponent<EditSpaceForm>(p =>
        {
            p.Add(c => c.SpaceToEdit, vm);
            p.Add(c => c.OnNewButtonClicked, onNewButtonClicked.Value);
            p.Add(c => c.DebounceInterval, 0);
        });
    }
}