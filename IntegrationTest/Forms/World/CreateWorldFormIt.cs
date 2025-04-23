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
using Presentation.Components.Forms.Models;
using Presentation.Components.Forms.World;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Shared.Theme;
using TestHelpers;

namespace IntegrationTest.Forms.World;

[TestFixture]
public sealed class CreateWorldFormIt : MudFormTestFixture<CreateWorldForm, LearningWorldFormModel, LearningWorld>
{
    [SetUp]
    public new void Setup()
    {
        WorkspacePresenter = Substitute.For<IAuthoringToolWorkspacePresenter>();
        WorkspaceViewModel = Substitute.For<IAuthoringToolWorkspaceViewModel>();
        FormModel = FormModelProvider.GetLearningWorld();
        Entity = EntityProvider.GetLearningWorld();
        FormDataContainer = Substitute.For<IFormDataContainer<LearningWorldFormModel, LearningWorld>>();
        FormDataContainer.FormModel.Returns(FormModel);
        FormDataContainer.GetMappedEntity().Returns(Entity);
        Context.Services.AddSingleton(WorkspacePresenter);
        Context.Services.AddSingleton(WorkspaceViewModel);
        Context.Services.AddSingleton(FormDataContainer);
    }

    private IAuthoringToolWorkspacePresenter WorkspacePresenter { get; set; }
    private IAuthoringToolWorkspaceViewModel WorkspaceViewModel { get; set; }
    private IFormDataContainer<LearningWorldFormModel, LearningWorld> FormDataContainer { get; set; }
    private LearningWorldFormModel FormModel { get; set; }
    private LearningWorld Entity { get; set; }
    private const string Expected = "test";


    [Test]
    public void Render_InjectsDependencies()
    {
        var systemUnderTest = GetRenderedComponent();

        Assert.That(systemUnderTest.Instance.Localizer, Is.Not.Null);
        Assert.That(systemUnderTest.Instance.AuthoringToolWorkspacePresenter, Is.EqualTo(WorkspacePresenter));
        Assert.That(systemUnderTest.Instance.AuthoringToolWorkspaceViewModel, Is.EqualTo(WorkspaceViewModel));
        Assert.That(systemUnderTest.Instance.FormDataContainer, Is.EqualTo(FormDataContainer));
    }

    [Test]
    // ANF-ID: [ASE1]
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
        await systemUnderTest.InvokeAsync(() => systemUnderTest.Render());

        ConfigureValidatorAllMembersTest();

        Assert.Multiple(() =>
        {
            Assert.That(FormModel.Name, Is.EqualTo(""));
            Assert.That(FormModel.Shortname, Is.EqualTo(""));
            Assert.That(FormModel.Authors, Is.EqualTo(""));
            Assert.That(FormModel.Language, Is.EqualTo(""));
            Assert.That(FormModel.Description, Is.EqualTo(""));
            Assert.That(FormModel.Goals, Is.EqualTo(""));
            Assert.That(FormModel.EvaluationLink, Is.EqualTo(""));
            Assert.That(FormModel.EnrolmentKey, Is.EqualTo(""));
            Assert.That(FormModel.StoryStart, Is.EqualTo(""));
            Assert.That(FormModel.StoryEnd, Is.EqualTo(""));
        });
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.False);


        var mudInputs = systemUnderTest.FindComponents<MudTextField<string>>();
        foreach (var mudInput in mudInputs.Take(6))
        {
            var input = mudInput.Find("input");
            await input.ChangeAsync(new ChangeEventArgs
            {
                Value = Expected
            });
        }

        foreach (var mudInput in mudInputs.Skip(6))
        {
            var input = mudInput.Find("textarea");
            await input.ChangeAsync(new ChangeEventArgs
            {
                Value = Expected
            });
        }

        Assert.Multiple(() =>
        {
            Assert.That(() => FormModel.Name, Is.EqualTo(Expected).After(3).Seconds.PollEvery(250));
            Assert.That(() => FormModel.Shortname, Is.EqualTo(Expected).After(3).Seconds.PollEvery(250));
            Assert.That(() => FormModel.Authors, Is.EqualTo(Expected).After(3).Seconds.PollEvery(250));
            Assert.That(() => FormModel.Language, Is.EqualTo(Expected).After(3).Seconds.PollEvery(250));
            Assert.That(() => FormModel.Description, Is.EqualTo(Expected).After(3).Seconds.PollEvery(250));
            Assert.That(() => FormModel.Goals, Is.EqualTo(Expected).After(3).Seconds.PollEvery(250));
            Assert.That(() => FormModel.EvaluationLink, Is.EqualTo(Expected).After(3).Seconds.PollEvery(250));
            Assert.That(() => FormModel.EnrolmentKey, Is.EqualTo(Expected).After(3).Seconds.PollEvery(250));
            Assert.That(() => FormModel.StoryStart, Is.EqualTo(Expected).After(3).Seconds.PollEvery(250));
            Assert.That(() => FormModel.StoryEnd, Is.EqualTo(Expected).After(3).Seconds.PollEvery(250));
        });
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.True);
    }

    [Test]
    // ANF-ID: [ASE1]
    public void ResetButtonClicked_ResetsForm()
    {
        var systemUnderTest = GetRenderedComponent();

        Assert.That(FormDataContainer.FormModel.Name, Is.EqualTo(""));

        var mudInput = systemUnderTest.FindComponent<MudTextField<string>>();
        var input = mudInput.Find("input");
        input.Change(Expected);

        Assert.That(FormDataContainer.FormModel.Name, Is.EqualTo(Expected));

        var resetButton = systemUnderTest.FindComponent<DefaultResetButton>();
        resetButton.Find("button").Click();

        Assert.That(FormDataContainer.FormModel.Name, Is.EqualTo(""));
    }

    [Test]
    // ANF-ID: [ASE1]
    public async Task SubmitButtonClicked_SubmitsIfFormValid()
    {
        var callbackCalled = false;
        var callback = EventCallback.Factory.Create(this, () => callbackCalled = true);
        var systemUnderTest = GetRenderedComponent(callback);
        var mudForm = systemUnderTest.FindComponent<MudForm>();

        ConfigureValidatorNameIsTest();

        Assert.That(FormDataContainer.FormModel.Name, Is.EqualTo(""));
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.False);

        var submitButton = systemUnderTest.FindComponent<DefaultSubmitButton>();
        submitButton.Find("button").Click();
        Assert.That(callbackCalled, Is.False);
        WorkspacePresenter.DidNotReceive().CreateLearningWorld(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<WorldTheme>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());

        var mudInput = systemUnderTest.FindComponent<MudTextField<string>>();
        var input = mudInput.Find("input");
        input.Change(Expected);

        Assert.That(FormDataContainer.FormModel.Name, Is.EqualTo(Expected));
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.True);

        submitButton.Find("button").Click();
        Assert.That(callbackCalled, Is.True);
        WorkspacePresenter.Received().CreateLearningWorld(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<WorldTheme>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
    }

    [Test]
    // ANF-ID: [ASE1]
    public async Task EnterKeyPressed_SubmitsIfFormValid()
    {
        var callbackCalled = false;
        var callback = EventCallback.Factory.Create(this, () => callbackCalled = true);
        var systemUnderTest = GetRenderedComponent(callback);
        var mudForm = systemUnderTest.FindComponent<MudForm>();

        ConfigureValidatorNameIsTest();

        Assert.That(FormDataContainer.FormModel.Name, Is.EqualTo(""));
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.False);

        var mudInput = systemUnderTest.FindComponent<MudTextField<string>>();
        var input = mudInput.Find("input");
        input.KeyUp(Key.Enter);
        Assert.That(callbackCalled, Is.False);
        WorkspacePresenter.DidNotReceive().CreateLearningWorld(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<WorldTheme>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());

        input.Change(Expected);
        Assert.That(FormDataContainer.FormModel.Name, Is.EqualTo(Expected));
        input.KeyUp(Key.Enter);
        Assert.That(callbackCalled, Is.True);
        WorkspacePresenter.Received().CreateLearningWorld(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<WorldTheme>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
    }


    private void ConfigureValidatorNameIsTest()
    {
        Validator.ValidateAsync(Entity, Arg.Any<string>()).Returns(ci =>
            {
                if (ci.Arg<string>() != nameof(FormModel.Name)) return Enumerable.Empty<string>();
                return (string)FormModel.GetType().GetProperty(ci.Arg<string>())!.GetValue(FormModel)! == Expected
                    ? Enumerable.Empty<string>()
                    : new[] { "Must be test" };
            }
        );
    }

    private void ConfigureValidatorAllMembersTest()
    {
        Validator.ValidateAsync(Entity, Arg.Any<string>()).Returns(ci =>
            (string)FormModel.GetType().GetProperty(ci.Arg<string>())!.GetValue(FormModel)! == Expected
                ? Enumerable.Empty<string>()
                : new[] { "Must be test" }
        );
    }

    private IRenderedComponent<CreateWorldForm> GetRenderedComponent(EventCallback? onSubmitted = null)
    {
        onSubmitted ??= EventCallback.Empty;
        return Context.RenderComponent<CreateWorldForm>(parameters =>
        {
            parameters.Add(c => c.OnSubmitted, onSubmitted.Value);
            parameters.Add(c => c.DebounceInterval, 0);
        });
    }
}