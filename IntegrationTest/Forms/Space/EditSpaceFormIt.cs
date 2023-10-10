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
using PresentationTest;
using Shared;
using TestHelpers;

namespace IntegrationTest.Forms.Space;

[TestFixture]
public class EditSpaceFormIt : MudFormTestFixture<EditSpaceForm, LearningSpaceFormModel, LearningSpace>
{
    [SetUp]
    public void Setup()
    {
        SpacePresenter = Substitute.For<ILearningSpacePresenter>();
        Mapper = Substitute.For<IMapper>();
        var themeLocalizer = Substitute.For<IStringLocalizer<Theme>>();
        themeLocalizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
        ThemeHelper.Initialize(themeLocalizer);
        Context.Services.AddSingleton(SpacePresenter);
        Context.Services.AddSingleton(Mapper);
        Context.AddLocalizerForTest<SpaceLayoutSelection>();
        Context.AddLocalizerForTest<FloorPlanEnum>();
        Context.ComponentFactories.AddStub<SpaceLayoutSelection>();
    }

    private ILearningSpacePresenter SpacePresenter { get; set; }
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

        var systemUnderTest = GetRenderedComponent(vm);

        Mapper.Received(1).Map(vm, FormDataContainer.FormModel);
    }

    [Test]
    public void SpacePresenterLearningSpace_PropertyChanged_RemapsIntoContainer()
    {
        var vm = ViewModelProvider.GetLearningSpace();
        SpacePresenter.LearningSpaceVm.Returns(vm);

        var systemUnderTest = GetRenderedComponent(vm);

        Mapper.Received(1).Map(vm, FormDataContainer.FormModel);

        Assert.That(vm.Name, Is.Not.EqualTo("foobar"));
        vm.Name = "foobar";

        Mapper.Received(2).Map(vm, FormDataContainer.FormModel);
    }

    [Test]
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

        ConfigureValidatorAllMembersTestOr123OrCampus();

        Assert.That(FormModel.Name, Is.EqualTo(""));
        Assert.That(FormModel.Description, Is.EqualTo(""));
        Assert.That(FormModel.Goals, Is.EqualTo(""));
        Assert.That(FormModel.RequiredPoints, Is.Null);
        Assert.That(FormModel.Theme, Is.EqualTo(default(Theme)));
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.False);

        var mudStringInputs = systemUnderTest.FindComponents<MudTextField<string>>();
        var mudIntInput = systemUnderTest.FindComponent<MudNumericField<int?>>();
        var mudSelect = systemUnderTest.FindComponent<MudSelect<Theme>>();

        mudStringInputs[0].Find("input").Change(Expected);
        mudStringInputs[1].Find("textarea").Change(Expected);
        mudStringInputs[2].Find("textarea").Change(Expected);
        mudIntInput.Find("input").Change(123);
        //TODO: once we have more themes, change to a different theme and test that
        mudSelect.Find("input").Change(Theme.Campus);

        systemUnderTest.WaitForAssertion(() => Assert.That(FormModel.Name, Is.EqualTo(Expected)),
            TimeSpan.FromSeconds(2));
        Assert.That(FormModel.Description, Is.EqualTo(Expected));
        Assert.That(FormModel.Goals, Is.EqualTo(Expected));
        Assert.That(FormModel.RequiredPoints, Is.EqualTo(123));
        Assert.That(FormModel.Theme, Is.EqualTo(Theme.Campus));
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.True);
    }

    [Test]
    public async Task SubmitThenRemapButton_CallsPresenterWithNewValues_ThenRemapsEntityIntoForm()
    {
        var vm = ViewModelProvider.GetLearningSpace();
        SpacePresenter.LearningSpaceVm.Returns(vm);

        var systemUnderTest = GetRenderedComponent(vm);
        Mapper.Received(1).Map(vm, FormDataContainer.FormModel);
        Mapper.ClearReceivedCalls();

        var collapsables = systemUnderTest.FindComponents<Collapsable>();
        collapsables[1].Find("div.toggler").Click();
        collapsables[3].Find("div.toggler").Click();
        collapsables[4].Find("div.toggler").Click();

        var mudStringInputs = systemUnderTest.FindComponents<MudTextField<string>>();
        var mudIntInput = systemUnderTest.FindComponent<MudNumericField<int?>>();
        var mudSelect = systemUnderTest.FindComponent<MudSelect<Theme>>();

        mudStringInputs[0].Find("input").Change(Expected);
        mudStringInputs[1].Find("textarea").Change(Expected);
        mudStringInputs[2].Find("textarea").Change(Expected);
        mudIntInput.Find("input").Change(123);
        //TODO: once we have more themes, change to a different theme and test that
        mudSelect.Find("input").Change(Theme.Campus);

        Assert.That(FormModel.Name, Is.EqualTo(Expected));
        Assert.That(FormModel.Description, Is.EqualTo(Expected));
        Assert.That(FormModel.Goals, Is.EqualTo(Expected));
        Assert.That(FormModel.RequiredPoints, Is.EqualTo(123));
        Assert.That(FormModel.Theme, Is.EqualTo(Theme.Campus));

        Mapper.ClearReceivedCalls();

        systemUnderTest.FindComponent<SubmitThenRemapButton>().Find("button").Click();

        SpacePresenter.Received(2).EditLearningSpace(Expected, Expected, Expected, 123, Theme.Campus);
        Mapper.Received(1).Map(vm, FormDataContainer.FormModel);
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
                    Theme t => t == Theme.Campus,
                    _ => throw new ArgumentOutOfRangeException()
                };
                return valid ? Enumerable.Empty<string>() : new[] {"Must be test or 123"};
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