using System;
using System.Linq;
using System.Threading.Tasks;
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
using Presentation.Components.Forms.Space;
using Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;
using Presentation.PresentationLogic.LearningWorld;
using Shared;

namespace IntegrationTest.Forms.Space;

[TestFixture]
public class CreateSpaceFormIt : MudFormTestFixture<CreateSpaceForm, LearningSpaceFormModel, LearningSpace>
{
    [SetUp]
    public void Setup()
    {
        WorldPresenter = Substitute.For<ILearningWorldPresenter>();
        var themeLocalizer = Substitute.For<IStringLocalizer<Theme>>();
        themeLocalizer[Arg.Any<string>()].Returns(ci => new LocalizedString(ci.Arg<string>(), ci.Arg<string>()));
        ThemeHelper.Initialize(themeLocalizer);
        Context.Services.AddSingleton(WorldPresenter);
    }

    private ILearningWorldPresenter WorldPresenter { get; set; }
    private const string Expected = "test";

    [Test]
    public void Render_InjectsDependencies()
    {
        var systemUnderTest = GetRenderedComponent();

        Assert.That(systemUnderTest.Instance.LearningWorldPresenter, Is.EqualTo(WorldPresenter));
        Assert.That(systemUnderTest.Instance.FormDataContainer, Is.EqualTo(FormDataContainer));
        Assert.That(systemUnderTest.Instance.Localizer, Is.Not.Null);
    }

    [Test]
    // ANF-ID: [AWA0001]
    public async Task ChangeFieldValues_ChangesContainerValuesAndCallsValidation()
    {
        var systemUnderTest = GetRenderedComponent();

        var mudForm = systemUnderTest.FindComponent<MudForm>();
        Context.RenderComponent<MudPopoverProvider>();
        var collapsables = systemUnderTest.FindComponents<Collapsable>();
        foreach (var collapsable in collapsables.Skip(1))
        {
            collapsable.Find("div.toggler").Click();
        }

        ConfigureValidatorAllMembersTestOr123OrCampus();

        Assert.That(FormModel.Name, Is.EqualTo(""));
        Assert.That(FormModel.Description, Is.EqualTo(""));
        Assert.That(FormModel.RequiredPoints, Is.EqualTo(0));
        Assert.That(FormModel.Theme, Is.EqualTo(default(Theme)));
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.False);

        var mudStringInputs = systemUnderTest.FindComponents<MudTextField<string>>();
        var mudIntInput = systemUnderTest.FindComponent<MudNumericField<int>>();
        var mudSelect = systemUnderTest.FindComponent<MudSelect<Theme>>();

        mudStringInputs[0].Find("input").Change(Expected);
        mudStringInputs[1].Find("textarea").Change(Expected);
        mudIntInput.Find("input").Change(123);
        //TODO: once we have more themes, change to a different theme and test that
        mudSelect.Find("input").Change(Theme.CampusAschaffenburg);

        Assert.That(FormModel.Name, Is.EqualTo(Expected));
        Assert.That(FormModel.Description, Is.EqualTo(Expected));
        Assert.That(FormModel.RequiredPoints, Is.EqualTo(123));
        Assert.That(FormModel.Theme, Is.EqualTo(Theme.CampusAschaffenburg));
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.True);
    }

    [Test]
    // ANF-ID: [AWA0001]
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
    // ANF-ID: [AWA0001]
    public async Task SubmitButtonClicked_SubmitsIfFormValid()
    {
        var systemUnderTest = GetRenderedComponent();
        var mudForm = systemUnderTest.FindComponent<MudForm>();

        ConfigureValidatorNameIsTest();

        Assert.That(FormDataContainer.FormModel.Name, Is.EqualTo(""));
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.False);

        var submitButton = systemUnderTest.FindComponent<DefaultSubmitButton>();
        submitButton.Find("button").Click();
        WorldPresenter.DidNotReceive().CreateLearningSpace(Expected, Arg.Any<string>(),
            Arg.Any<LearningOutcomeCollectionViewModel>(),
            Arg.Any<int>(), Arg.Any<Theme>());

        var mudInput = systemUnderTest.FindComponent<MudTextField<string>>();
        var input = mudInput.Find("input");
        input.Change(Expected);

        Assert.That(FormDataContainer.FormModel.Name, Is.EqualTo(Expected));
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.True);

        submitButton.Find("button").Click();
        WorldPresenter.Received().CreateLearningSpace(Expected, Arg.Any<string>(),
            Arg.Any<LearningOutcomeCollectionViewModel>(),
            Arg.Any<int>(), Arg.Any<Theme>());
    }

    [Test]
    // ANF-ID: [AWA0001]
    public async Task EnterKeyPressed_SubmitsIfFormValid()
    {
        var systemUnderTest = GetRenderedComponent();
        var mudForm = systemUnderTest.FindComponent<MudForm>();

        ConfigureValidatorNameIsTest();

        Assert.That(FormDataContainer.FormModel.Name, Is.EqualTo(""));
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.False);

        var mudInput = systemUnderTest.FindComponent<MudTextField<string>>();
        var input = mudInput.Find("input");
        input.KeyUp(Key.Enter);
        WorldPresenter.DidNotReceive().CreateLearningSpace(Arg.Any<string>(), Arg.Any<string>(),
            Arg.Any<LearningOutcomeCollectionViewModel>(),
            Arg.Any<int>(), Arg.Any<Theme>());

        input.Change(Expected);
        Assert.That(FormDataContainer.FormModel.Name, Is.EqualTo(Expected));
        input.KeyUp(Key.Enter);
        WorldPresenter.Received().CreateLearningSpace(Expected, Arg.Any<string>(),
            Arg.Any<LearningOutcomeCollectionViewModel>(),
            Arg.Any<int>(), Arg.Any<Theme>());
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
                    Theme t => t == Theme.CampusAschaffenburg,
                    _ => throw new ArgumentOutOfRangeException()
                };
                return valid ? Enumerable.Empty<string>() : new[] { "Must be test or 123" };
            }
        );
    }

    private IRenderedComponent<CreateSpaceForm> GetRenderedComponent()
    {
        return Context.RenderComponent<CreateSpaceForm>(p =>
            p.Add(c => c.DebounceInterval, 0));
    }
}