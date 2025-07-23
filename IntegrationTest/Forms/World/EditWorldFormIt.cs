using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bunit;
using BusinessLogic.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms;
using Presentation.Components.Forms.Buttons;
using Presentation.Components.Forms.Models;
using Presentation.Components.Forms.World;
using Presentation.PresentationLogic.LearningWorld;
using PresentationTest;
using Shared.Theme;
using TestHelpers;

namespace IntegrationTest.Forms.World;

[TestFixture]
public class EditWorldFormIt : MudFormTestFixture<EditWorldForm, LearningWorldFormModel, LearningWorld>
{
    [SetUp]
    public new void Setup()
    {
        WorldPresenter = Substitute.For<ILearningWorldPresenter>();
        var themeLocalizer = Substitute.For<IStringLocalizer<WorldTheme>>();
        themeLocalizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
        ThemeHelper<WorldTheme>.Initialize(themeLocalizer);
        Mapper = Substitute.For<IMapper>();
        Context.Services.AddSingleton(WorldPresenter);
        Context.Services.AddSingleton(Mapper);
    }

    private ILearningWorldPresenter WorldPresenter { get; set; }
    private IMapper Mapper { get; set; }
    private const string Expected = "test";


    [Test]
    public void Render_SetsParameters()
    {
        var vm = ViewModelProvider.GetLearningWorld();

        var systemUnderTest = GetRenderedComponent(vm);

        Assert.That(systemUnderTest.Instance.WorldToEdit, Is.EqualTo(vm));
        Assert.That(systemUnderTest.Instance.DebounceInterval, Is.EqualTo(0));
    }

    [Test]
    public void OnParametersSet_CallsMapper()
    {
        var vm = ViewModelProvider.GetLearningWorld();

        _ = GetRenderedComponent(vm);

        Mapper.Received(1).Map(vm, FormDataContainer.FormModel);
    }

    [Test]
    // ANF-ID: [ASE3]
    public void WorldPresenterLearningWorld_PropertyChanged_RemapsIntoContainer()
    {
        var vm = ViewModelProvider.GetLearningWorld();
        WorldPresenter.LearningWorldVm.Returns(vm);

        _ = GetRenderedComponent(vm);

        Mapper.Received(1).Map(vm, FormDataContainer.FormModel);

        Assert.That(vm.Name, Is.Not.EqualTo("foobar"));
        vm.Name = "foobar";

        Mapper.Received(2).Map(vm, FormDataContainer.FormModel);
    }

    [Test]
    // ANF-ID: [ASE3]
    public void ResetButton_Clicked_RemapsIntoContainer()
    {
        var vm = ViewModelProvider.GetLearningWorld();
        WorldPresenter.LearningWorldVm.Returns(vm);

        var systemUnderTest = GetRenderedComponent(vm);

        Mapper.Received(1).Map(vm, FormDataContainer.FormModel);
        Mapper.ClearReceivedCalls();

        systemUnderTest.FindComponentWithMarkup<MudIconButton>("reset-form").Find("button").Click();

        Mapper.Received(1).Map(vm, FormDataContainer.FormModel);
    }

    [Test]
    // ANF-ID: [ASE3]
    public async Task ChangeFieldValues_ChangesContainerValuesAndCallsValidation()
    {
        var systemUnderTest = GetRenderedComponent();
        var mudForm = systemUnderTest.FindComponent<MudForm>();
        Context.RenderComponent<MudPopoverProvider>();
        var collapsables = systemUnderTest.FindComponents<Collapsable>();
        collapsables[1].Find("div.toggler").Click();
        collapsables[2].Find("div.toggler").Click();
        collapsables[3].Find("div.toggler").Click();
        collapsables[4].Find("div.toggler").Click();
        collapsables[5].Find("div.toggler").Click();
        collapsables[6].Find("div.toggler").Click();
        await systemUnderTest.InvokeAsync(() => systemUnderTest.Render());

        ConfigureValidatorAllMembersTest();

        WorldFormStaticTestMethods.AssertAllFieldsAreSetToValue(FormModel, "");
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.False);


        var mudInputs = systemUnderTest.FindComponents<MudTextField<string>>();
        await WorldFormStaticTestMethods.SetAllInputAndTextareaValueToExpected(mudInputs, Expected);

        WorldFormStaticTestMethods.AssertAllFieldsAreSetToValue(FormModel, Expected);
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.True);
    }


    [Test]
    // ANF-ID: [ASE3]
    public async Task SubmitThenRemapButton_CallsPresenterWithNewValues_ThenRemapsEntityIntoForm()
    {
        var worldToMap = ViewModelProvider.GetLearningWorld();
        var systemUnderTest = GetRenderedComponent(worldToMap);
        Context.RenderComponent<MudPopoverProvider>();
        Mapper.Received(1).Map(worldToMap, FormDataContainer.FormModel);
        Mapper.ClearReceivedCalls();

        var collapsables = systemUnderTest.FindComponents<Collapsable>();
        collapsables[1].Find("div.toggler").Click();
        collapsables[2].Find("div.toggler").Click();
        collapsables[3].Find("div.toggler").Click();
        collapsables[4].Find("div.toggler").Click();
        collapsables[5].Find("div.toggler").Click();
        collapsables[6].Find("div.toggler").Click();
        await systemUnderTest.InvokeAsync(() => systemUnderTest.Render());
        var mudInputs = systemUnderTest.FindComponents<MudTextField<string>>();
        await WorldFormStaticTestMethods.SetAllInputAndTextareaValueToExpected(mudInputs, Expected);

        WorldFormStaticTestMethods.AssertAllFieldsAreSetToValue(FormModel, Expected);

        Mapper.ClearReceivedCalls();

        systemUnderTest.FindComponent<SubmitThenRemapButton>().Find("button").Click();

        WorldPresenter.Received(2).EditLearningWorld(Expected, Expected, Expected, Expected,
            Expected, Expected, WorldTheme.CampusAschaffenburg, Expected, Expected, Expected, Expected, Expected,
            Expected);
        Mapper.Received(1).Map(worldToMap, FormDataContainer.FormModel);
    }

    private void ConfigureValidatorAllMembersTest()
    {
        Validator.ValidateAsync(Entity, Arg.Any<string>()).Returns(ci =>
            {
                var value = FormModel.GetType().GetProperty(ci.Arg<string>())?.GetValue(FormModel);
                var valid = value switch
                {
                    string str => str == Expected,
                    WorldTheme t => t == WorldTheme.CampusAschaffenburg,
                    _ => throw new ArgumentOutOfRangeException()
                };
                return valid ? Enumerable.Empty<string>() : new[] { "Must be test" };
            }
        );
    }

    private IRenderedComponent<EditWorldForm> GetRenderedComponent(ILearningWorldViewModel? worldToEdit = null)
    {
        worldToEdit ??= ViewModelProvider.GetLearningWorld();
        return Context.RenderComponent<EditWorldForm>(parameters =>
        {
            parameters.Add(c => c.WorldToEdit, worldToEdit);
            parameters.Add(c => c.DebounceInterval, 0);
        });
    }
}