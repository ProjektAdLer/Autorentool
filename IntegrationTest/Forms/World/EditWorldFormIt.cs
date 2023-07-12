using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bunit;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Forms;
using Presentation.Components.Forms.Buttons;
using Presentation.Components.Forms.Models;
using Presentation.Components.Forms.World;
using Presentation.PresentationLogic.LearningWorld;
using PresentationTest;
using TestHelpers;

namespace IntegrationTest.Forms.World;

[TestFixture]
public class EditWorldFormIt : MudFormTestFixture<EditWorldForm, LearningWorldFormModel, LearningWorld>
{
    private ILearningWorldPresenter WorldPresenter { get; set; }
    private IMapper Mapper { get; set; }
    private const string Expected = "test";

    [SetUp]
    public void Setup()
    {
        WorldPresenter = Substitute.For<ILearningWorldPresenter>();
        Mapper = Substitute.For<IMapper>();
        Context.Services.AddSingleton(WorldPresenter);
        Context.Services.AddSingleton(Mapper);
    }


    [Test]
    public void Render_SetsParameters()
    {
        var vm = ViewModelProvider.GetLearningWorld();
        var onNewClicked = EventCallback.Factory.Create(this, () => { });

        var systemUnderTest = GetRenderedComponent(vm);

        Assert.That(systemUnderTest.Instance.WorldToEdit, Is.EqualTo(vm));
        Assert.That(systemUnderTest.Instance.DebounceInterval, Is.EqualTo(0));
    }

    [Test]
    public void OnParametersSet_CallsMapper()
    {
        var vm = ViewModelProvider.GetLearningWorld();

        var systemUnderTest = GetRenderedComponent(vm);

        Mapper.Received(1).Map(vm, FormDataContainer.FormModel);
    }

    [Test]
    public void WorldPresenterLearningWorld_PropertyChanged_RemapsIntoContainer()
    {
        var vm = ViewModelProvider.GetLearningWorld();
        WorldPresenter.LearningWorldVm.Returns(vm);

        var systemUnderTest = GetRenderedComponent(vm);

        Mapper.Received(1).Map(vm, FormDataContainer.FormModel);

        Assert.That(vm.Name, Is.Not.EqualTo("foobar"));
        vm.Name = "foobar";

        Mapper.Received(2).Map(vm, FormDataContainer.FormModel);
    }

    [Test]
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
    public async Task ChangeFieldValues_ChangesContainerValuesAndCallsValidation()
    {
        var systemUnderTest = GetRenderedComponent();
        var mudForm = systemUnderTest.FindComponent<MudForm>();

        systemUnderTest.FindComponents<Collapsable>()[1].Find("div.toggler").Click();
        await systemUnderTest.InvokeAsync(() => systemUnderTest.Render());

        ConfigureValidatorAllMembersTest();

        Assert.That(FormModel.Name, Is.EqualTo(""));
        Assert.That(FormModel.Shortname, Is.EqualTo(""));
        Assert.That(FormModel.Authors, Is.EqualTo(""));
        Assert.That(FormModel.Language, Is.EqualTo(""));
        Assert.That(FormModel.Description, Is.EqualTo(""));
        Assert.That(FormModel.Goals, Is.EqualTo(""));
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.False);


        var mudInputs = systemUnderTest.FindComponents<MudTextField<string>>();
        foreach (var mudInput in mudInputs.Take(4))
        {
            var input = mudInput.Find("input");
            input.Change(Expected);
        }

        foreach (var mudInput in mudInputs.Skip(4))
        {
            var input = mudInput.Find("textarea");
            input.Change(Expected);
        }

        Assert.That(FormModel.Name, Is.EqualTo(Expected));
        Assert.That(FormModel.Shortname, Is.EqualTo(Expected));
        Assert.That(FormModel.Authors, Is.EqualTo(Expected));
        Assert.That(FormModel.Language, Is.EqualTo(Expected));
        Assert.That(FormModel.Description, Is.EqualTo(Expected));
        Assert.That(FormModel.Goals, Is.EqualTo(Expected));
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.True);
    }

    [Test]
    public async Task SubmitThenRemapButton_CallsPresenterWithNewValues_ThenRemapsEntityIntoForm()
    {
        var worldToMap = ViewModelProvider.GetLearningWorld();
        var systemUnderTest = GetRenderedComponent(worldToMap);

        Mapper.Received(1).Map(worldToMap, FormDataContainer.FormModel);
        Mapper.ClearReceivedCalls();
        
        systemUnderTest.FindComponents<Collapsable>()[1].Find("div.toggler").Click();
        await systemUnderTest.InvokeAsync(() => systemUnderTest.Render());
        var mudInputs = systemUnderTest.FindComponents<MudTextField<string>>();
        foreach (var mudInput in mudInputs.Take(4))
        {
            var input = mudInput.Find("input");
            input.Change(Expected);
        }

        foreach (var mudInput in mudInputs.Skip(4))
        {
            var input = mudInput.Find("textarea");
            input.Change(Expected);
        }

        Assert.That(FormModel.Name, Is.EqualTo(Expected));
        Assert.That(FormModel.Shortname, Is.EqualTo(Expected));
        Assert.That(FormModel.Authors, Is.EqualTo(Expected));
        Assert.That(FormModel.Language, Is.EqualTo(Expected));
        Assert.That(FormModel.Description, Is.EqualTo(Expected));
        Assert.That(FormModel.Goals, Is.EqualTo(Expected));

        Mapper.ClearReceivedCalls();
        
        systemUnderTest.FindComponent<SubmitThenRemapButton>().Find("button").Click();

        WorldPresenter.Received(1).EditLearningWorld(Expected, Expected, Expected, Expected,
            Expected, Expected);
        Mapper.Received(1).Map(worldToMap, FormDataContainer.FormModel);
    }

    private void ConfigureValidatorAllMembersTest()
    {
        Validator.ValidateAsync(Entity, Arg.Any<string>()).Returns(ci =>
            (string)FormModel.GetType().GetProperty(ci.Arg<string>()).GetValue(FormModel) == Expected
                ? Enumerable.Empty<string>()
                : new[] { "Must be test" }
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