using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Bunit;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Adaptivity.Forms.AdaptivityQuestion;
using Presentation.Components.Adaptivity.Forms.Models;
using Presentation.Components.Forms;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using PresentationTest;
using Shared.Adaptivity;
using TestHelpers;

namespace IntegrationTest.Components.Adaptivity.Forms.AdaptivityQuestion;

[TestFixture]
public class MultipleChoiceQuestionFormIt : MudFormTestFixture<MultipleChoiceQuestionForm,
    MultipleChoiceQuestionFormModel, IMultipleChoiceQuestion>
{
    [SetUp]
#pragma warning disable CS0108
    public void Setup()
#pragma warning restore CS0108
    {
        ChoiceValidator = Substitute.For<IValidationWrapper<Choice>>();
        Context.Services.AddSingleton(ChoiceValidator);
        PresentationLogic = Substitute.For<IPresentationLogic>();
        Context.Services.AddSingleton(PresentationLogic);
        MultipleResponseQuestionValidator =
            Substitute.For<IValidationWrapper<MultipleChoiceMultipleResponseQuestion>>();
        Context.Services.AddSingleton(MultipleResponseQuestionValidator);
        SingleResponseQuestionValidator = Substitute.For<IValidationWrapper<MultipleChoiceSingleResponseQuestion>>();
        Context.Services.AddSingleton(SingleResponseQuestionValidator);
    }

    private IValidationWrapper<Choice> ChoiceValidator { get; set; } = null!;
    private IPresentationLogic PresentationLogic { get; set; } = null!;

    private IValidationWrapper<MultipleChoiceMultipleResponseQuestion> MultipleResponseQuestionValidator { get; set; } =
        null!;

    private IValidationWrapper<MultipleChoiceSingleResponseQuestion> SingleResponseQuestionValidator { get; set; } =
        null!;

    private const string ExpectedString = "expectedString";

    [Test]
    public void Render_InjectsDependenciesAndParameters()
    {
        var task = ViewModelProvider.GetAdaptivityTask();
        const QuestionDifficulty difficulty = QuestionDifficulty.Medium;
        var questionToEdit = ViewModelProvider.GetMultipleChoiceMultipleResponseQuestion();
        var onSubmitted = EventCallback.Empty;

        var systemUnderTest = GetRenderedComponent(task, difficulty, questionToEdit, onSubmitted);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.ChoiceValidator, Is.EqualTo(ChoiceValidator));
            Assert.That(systemUnderTest.Instance.FormDataContainer, Is.EqualTo(FormDataContainer));
            Assert.That(systemUnderTest.Instance.PresentationLogic, Is.EqualTo(PresentationLogic));
            Assert.That(systemUnderTest.Instance.MultipleResponseQuestionValidator,
                Is.EqualTo(MultipleResponseQuestionValidator));
            Assert.That(systemUnderTest.Instance.SingleResponseQuestionValidator,
                Is.EqualTo(SingleResponseQuestionValidator));
            Assert.That(systemUnderTest.Instance.DebounceInterval, Is.EqualTo(0));
            Assert.That(systemUnderTest.Instance.OnSubmitted, Is.EqualTo(onSubmitted));
            Assert.That(systemUnderTest.Instance.Task, Is.EqualTo(task));
            Assert.That(systemUnderTest.Instance.Difficulty, Is.EqualTo(difficulty));
            Assert.That(systemUnderTest.Instance.QuestionToEdit, Is.EqualTo(questionToEdit));
        });
    }

    [Test]
    public void OnParametersSetWithQuestionToEdit_CallsMapper()
    {
        var questionToEdit = ViewModelProvider.GetMultipleChoiceMultipleResponseQuestion();

        GetRenderedComponent(questionToEditVm: questionToEdit);

        Mapper.Received(1).Map(questionToEdit, FormDataContainer.FormModel);
    }

    [Test]
    // ANF-ID: [AWA0004, AWA0008]
    public async Task OnValidate_CallsCorrectValidator([Values] bool isSingleResponse)
    {
        var multipleResponseQuestion = EntityProvider.GetMultipleChoiceMultipleResponseQuestion();
        var singleResponseQuestion = EntityProvider.GetMultipleChoiceSingleResponseQuestion();
        if (isSingleResponse)
        {
            Mapper.Map<MultipleChoiceQuestionFormModel, IMultipleChoiceQuestion>(FormModel)
                .Returns(singleResponseQuestion);
        }
        else
        {
            Mapper.Map<MultipleChoiceQuestionFormModel, IMultipleChoiceQuestion>(FormModel)
                .Returns(multipleResponseQuestion);
        }

        var systemUnderTest = GetRenderedComponent();
        var mudForm = systemUnderTest.FindComponent<MudForm>();
        var mudCheckBox = systemUnderTest.FindComponent<MudCheckBox<bool>>();

        mudCheckBox.Find("input").Change(isSingleResponse);

        SingleResponseQuestionValidator.ClearReceivedCalls();
        MultipleResponseQuestionValidator.ClearReceivedCalls();

        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());

        if (isSingleResponse)
        {
            Assert.Multiple(async () =>
            {
                await SingleResponseQuestionValidator.Received(3)
                    .ValidateAsync(singleResponseQuestion, Arg.Any<string>());
                await MultipleResponseQuestionValidator.DidNotReceive()
                    .ValidateAsync(multipleResponseQuestion, Arg.Any<string>());
            });
        }
        else
        {
            Assert.Multiple(async () =>
            {
                await MultipleResponseQuestionValidator.Received(3)
                    .ValidateAsync(multipleResponseQuestion, Arg.Any<string>());
                await SingleResponseQuestionValidator.DidNotReceive()
                    .ValidateAsync(singleResponseQuestion, Arg.Any<string>());
            });
        }
    }

    [Test]
    // ANF-ID: [AWA0004, AWA0008]
    public async Task OnValidate_UnknownEntity_ThrowsException()
    {
        var systemUnderTest = GetRenderedComponent();
        var mudForm = systemUnderTest.FindComponent<MudForm>();

        Mapper.Map<MultipleChoiceQuestionFormModel, IMultipleChoiceQuestion>(FormModel)
            .Returns((IMultipleChoiceQuestion?)null);

        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        await MultipleResponseQuestionValidator.DidNotReceive()
            .ValidateAsync(Arg.Any<MultipleChoiceMultipleResponseQuestion>(), Arg.Any<string>());
        await SingleResponseQuestionValidator.DidNotReceive()
            .ValidateAsync(Arg.Any<MultipleChoiceSingleResponseQuestion>(), Arg.Any<string>());
        // Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
        //     await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate()));
    }

    [Test]
    // ANF-ID: [AWA0004, AWA0008]
    public async Task ChangeFieldValues_ChangesContainerValuesToAValidStateAndCallsValidation()
    {
        var systemUnderTest = GetRenderedComponent();
        var mudForm = systemUnderTest.FindComponent<MudForm>();

        SetValidatorAllMembers();

        Assert.That(FormModel.Text, Is.EqualTo(""));
        Assert.That(FormModel.Choices, Has.Count.EqualTo(2));
        Assert.That(FormModel.CorrectChoices, Has.Count.EqualTo(0));


        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.False);

        var mudTextFields = systemUnderTest.FindComponents<MudTextField<string>>();
        var mudCheckBoxes = systemUnderTest.FindComponents<MudCheckBox<bool>>();
        mudTextFields[0].Find("textarea").Change(ExpectedString);
        mudCheckBoxes.Last().Find("input").Change(true);

        Assert.Multiple(() =>
        {
            Assert.That(FormModel.Text, Is.EqualTo(ExpectedString));
            Assert.That(FormModel.Choices, Has.Count.EqualTo(2));
            Assert.That(FormModel.CorrectChoices, Has.Count.EqualTo(1));
            Assert.That(FormModel.CorrectChoices.First(), Is.EqualTo(FormModel.Choices.Last()));
        });
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.True);
    }

    [Test]
    // ANF-ID: [AWA0004, AWA0008]
    public void AddAndRemoveChoiceButtonsClicked_AddsAndRemovesChoice()
    {
        var systemUnderTest = GetRenderedComponent();
        var buttons = systemUnderTest.FindComponents<MudButton>();
        var addButton = buttons[0].Find("button");
        var choices = FormModel.Choices.ToList();
        Assert.That(choices, Has.Count.EqualTo(2));
        addButton.Click();
        choices = FormModel.Choices.ToList();
        Assert.That(choices, Has.Count.EqualTo(3));
        var choiceToRemove = choices[1]; // Choice 2
        var iconButtons = systemUnderTest.FindComponentsWithMarkup<MudIconButton>("button-delete-choice");
        var removeButton = iconButtons.ElementAt(1).Find("button");
        Assert.That(choices.Any(x => x.Id == choiceToRemove.Id), Is.True);
        removeButton.Click();
        choices = FormModel.Choices.ToList();
        Assert.That(choices, Has.Count.EqualTo(2));
        Assert.That(choices.Any(x => x.Id == choiceToRemove.Id), Is.False);
    }

    [Test]
    // ANF-ID: [AWA0004, AWA0008]
    public void ChangeCorrectness_MultipleResponse_ChangesCorrectnessOfResponse()
    {
        var systemUnderTest = GetRenderedComponent();
        var mudCheckBoxes = systemUnderTest.FindComponents<MudCheckBox<bool>>();

        Assert.That(mudCheckBoxes, Has.Count.EqualTo(2));
        var checkBoxChoice1 = mudCheckBoxes[0];
        var checkBoxChoice2 = mudCheckBoxes[1];

        var choice1 = FormModel.Choices.First();
        var choice2 = FormModel.Choices.Last();
        var choices = FormModel.Choices.ToList();

        CheckFormModelContainingChoicesAndCorrectChoices(choices, new List<ChoiceFormModel>());

        checkBoxChoice1.Find("input").Change(true);

        CheckFormModelContainingChoicesAndCorrectChoices(choices, new List<ChoiceFormModel> { choice1 });

        checkBoxChoice2.Find("input").Change(true);

        CheckFormModelContainingChoicesAndCorrectChoices(choices, new List<ChoiceFormModel> { choice1, choice2 });

        checkBoxChoice1.Find("input").Change(false);

        CheckFormModelContainingChoicesAndCorrectChoices(choices, new List<ChoiceFormModel> { choice2 });

        checkBoxChoice2.Find("input").Change(false);

        CheckFormModelContainingChoicesAndCorrectChoices(choices, new List<ChoiceFormModel>());
    }

    [Test]
    // ANF-ID: [AWA0004, AWA0008]
    public void ChangeCorrectChoice_SingleResponse_ChangesTheCorrectChoice()
    {
        var systemUnderTest = GetRenderedComponent();
        var mudCheckBox = systemUnderTest.FindComponent<MudSwitch<bool>>();
        var checkbox = mudCheckBox.Find("input");
        checkbox.Change(true);
        var mudRadioButtons = systemUnderTest.FindComponents<MudRadio<ChoiceFormModel>>();

        Assert.That(mudRadioButtons, Has.Count.EqualTo(2));
        var radioButtonChoice1 = mudRadioButtons[0];
        var radioButtonChoice2 = mudRadioButtons[1];

        var choice1 = FormModel.Choices.First();
        var choice2 = FormModel.Choices.Last();
        var choices = FormModel.Choices.ToList();

        CheckFormModelContainingChoicesAndCorrectChoices(choices, new List<ChoiceFormModel> { choice1 });

        radioButtonChoice1.Find("input").Click();

        CheckFormModelContainingChoicesAndCorrectChoices(choices, new List<ChoiceFormModel> { choice1 });

        radioButtonChoice2.Find("input").Click();

        CheckFormModelContainingChoicesAndCorrectChoices(choices, new List<ChoiceFormModel> { choice2 });

        radioButtonChoice1.Find("input").Click();

        CheckFormModelContainingChoicesAndCorrectChoices(choices, new List<ChoiceFormModel> { choice1 });
    }

    [Test]
    // ANF-ID: [AWA0004, AWA0008]
    public void ChangeIsSingleResponse_KeepsTheCorrectCorrectChoices()
    {
        var systemUnderTest = GetRenderedComponent();
        _ = systemUnderTest.FindComponent<MudSwitch<bool>>().Find("input");
        var choice1 = FormModel.Choices.First();
        var choice2 = FormModel.Choices.Last();
        var choices = FormModel.Choices.ToList();

        // Multiple response - no correct choice
        CheckFormModelContainingChoicesAndCorrectChoices(choices, new List<ChoiceFormModel>());

        systemUnderTest.InvokeAsync(() =>
        {
            var mudSwitchInput = systemUnderTest.FindComponent<MudSwitch<bool>>().Find("input");
            mudSwitchInput.Change(true);
        });

        // Single response - automatically sets the first choice as correct
        CheckFormModelContainingChoicesAndCorrectChoices(choices, new List<ChoiceFormModel> { choice1 });

        systemUnderTest.InvokeAsync(() =>
        {
            var mudRadioButtonOfChoice2 =
                systemUnderTest.FindComponents<MudRadio<ChoiceFormModel>>().Last().Find("input");
            mudRadioButtonOfChoice2.Click();
        });

        // Single response - choice 2 is now correct
        CheckFormModelContainingChoicesAndCorrectChoices(choices, new List<ChoiceFormModel> { choice2 });

        systemUnderTest.InvokeAsync(() =>
        {
            var mudSwitchInput = systemUnderTest.FindComponent<MudSwitch<bool>>().Find("input");
            mudSwitchInput.Change(false);
        });

        // Multiple response - choice 2 is still correct
        CheckFormModelContainingChoicesAndCorrectChoices(choices, new List<ChoiceFormModel> { choice2 });

        systemUnderTest.InvokeAsync(() =>
        {
            var mudCheckBoxOfChoice1 = systemUnderTest.FindComponents<MudCheckBox<bool>>()[0].Find("input");
            mudCheckBoxOfChoice1.Change(true);
        });

        // Multiple response - choice 1 is now also correct
        CheckFormModelContainingChoicesAndCorrectChoices(choices, new List<ChoiceFormModel> { choice1, choice2 });

        systemUnderTest.InvokeAsync(() =>
        {
            var mudSwitchInput = systemUnderTest.FindComponent<MudSwitch<bool>>().Find("input");
            mudSwitchInput.Change(true);
        });


        // Single response - choice 1 is now correct because it is the first choice in the choices list that is correct
        CheckFormModelContainingChoicesAndCorrectChoices(choices, new List<ChoiceFormModel> { choice1 });
    }

    [Test]
    // ANF-ID: [AWA0004]
    public async Task CreatQuestionOnValidSubmit_CallsPresentationLogic([Values] bool isSingleResponse)
    {
        var task = Substitute.For<IAdaptivityTaskViewModel>();
        const QuestionDifficulty difficulty = QuestionDifficulty.Medium;
        var onSubmittedCalled = false;
        var onSubmitted = EventCallback.Factory.Create(this, () => onSubmittedCalled = true);
        var systemUnderTest = GetRenderedComponent(taskVm: task, difficulty: difficulty, onSubmitted: onSubmitted);
        var mudForm = systemUnderTest.FindComponent<MudForm>();

        // Check that validation everytime returns true
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.True);

        // Set IsSingleResponse to testcase value
        var mudCheckBoxIsSingleResponse = systemUnderTest.FindComponent<MudSwitch<bool>>();
        mudCheckBoxIsSingleResponse.Find("input").Change(isSingleResponse);

        // Add choice
        var mudButtonAddChoice = systemUnderTest.FindComponents<MudButton>()[0].Find("button");
        mudButtonAddChoice.Click();

        // Get all components
        var mudSubmitButton = systemUnderTest.FindComponents<MudButton>().Last();
        var mudTextFields = systemUnderTest.FindComponents<MudTextField<string>>();
        Assert.That(mudTextFields, Has.Count.EqualTo(4));
        var textFieldQuestionText = mudTextFields[0];
        var textFieldChoice1 = mudTextFields[1];
        var textFieldChoice2 = mudTextFields[2];
        var textFieldChoice3 = mudTextFields[3];
        var radioButtons = systemUnderTest.FindComponents<MudRadio<ChoiceFormModel>>();
        var checkBoxes = systemUnderTest.FindComponents<MudCheckBox<bool>>();
        IElement checkChoice1;
        IElement checkChoice3;
        if (isSingleResponse)
        {
            Assert.That(radioButtons, Has.Count.EqualTo(3));
            Assert.That(checkBoxes, Has.Count.EqualTo(0));
            checkChoice1 = radioButtons[0].Find("input");
            checkChoice3 = radioButtons[2].Find("input");
        }
        else
        {
            Assert.That(radioButtons, Has.Count.EqualTo(0));
            Assert.That(checkBoxes, Has.Count.EqualTo(3));
            checkChoice1 = checkBoxes[0].Find("input");
            checkChoice3 = checkBoxes[2].Find("input");
        }

        Assert.Multiple(() => { Assert.That(textFieldQuestionText.Instance.Text, Is.Empty); });

        // Set values
        textFieldQuestionText.Find("textarea").Change(ExpectedString);
        textFieldChoice1.Find("input").Change(ExpectedString + 1);
        textFieldChoice2.Find("input").Change(ExpectedString + 2);
        textFieldChoice3.Find("input").Change(ExpectedString + 3);
        if (isSingleResponse)
        {
            checkChoice3.Click();
        }
        else
        {
            checkChoice1.Change(true);
            checkChoice3.Change(true);
        }

        // Submit
        mudSubmitButton.Find("button").Click();
        Assert.That(onSubmittedCalled, Is.True);
        if (isSingleResponse)
        {
            PresentationLogic.Received(1).CreateMultipleChoiceSingleResponseQuestion(task, difficulty,
                ExpectedString, Arg.Any<List<ChoiceFormModel>>(), Arg.Any<ChoiceFormModel>(), Arg.Any<int>());
            var arguments = PresentationLogic.ReceivedCalls().First().GetArguments();
            var choices = arguments[3] as List<ChoiceFormModel>;
            Assert.That(choices, Has.Count.EqualTo(3));
            Assert.Multiple(() =>
            {
                Assert.That(choices![0].Text, Is.EqualTo(ExpectedString + 1));
                Assert.That(choices[1].Text, Is.EqualTo(ExpectedString + 2));
                Assert.That(choices[2].Text, Is.EqualTo(ExpectedString + 3));
                Assert.That(arguments[4], Is.EqualTo(choices[2]));
            });
        }
        else
        {
            PresentationLogic.Received(1).CreateMultipleChoiceMultipleResponseQuestion(task, difficulty, ExpectedString,
                Arg.Any<List<ChoiceFormModel>>(), Arg.Any<List<ChoiceFormModel>>(), Arg.Any<int>());
            var arguments = PresentationLogic.ReceivedCalls().First().GetArguments();
            var choices = arguments[3] as List<ChoiceFormModel>;
            var correctChoices = arguments[4] as List<ChoiceFormModel>;
            Assert.That(choices, Has.Count.EqualTo(3));
            Assert.That(correctChoices, Has.Count.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(choices![0].Text, Is.EqualTo(ExpectedString + 1));
                Assert.That(choices[1].Text, Is.EqualTo(ExpectedString + 2));
                Assert.That(choices[2].Text, Is.EqualTo(ExpectedString + 3));
                Assert.That(correctChoices![0], Is.EqualTo(choices[0]));
                Assert.That(correctChoices[1], Is.EqualTo(choices[2]));
            });
        }
    }

    [Test]
    // ANF-ID: [AWA0008]
    // ReSharper disable once CognitiveComplexity
    public async Task EditQuestionOnValidSubmit_CallsPresentationLogic([Values] bool wasSingleResponse,
        [Values] bool isSingleResponse)
    {
        var task = Substitute.For<IAdaptivityTaskViewModel>();
        const QuestionDifficulty difficulty = QuestionDifficulty.Medium;
        var onSubmittedCalled = false;
        var onSubmitted = EventCallback.Factory.Create(this, () => onSubmittedCalled = true);
        var singleResponseQuestion = ViewModelProvider.GetMultipleChoiceSingleResponseQuestion();
        var multipleResponseQuestion = ViewModelProvider.GetMultipleChoiceMultipleResponseQuestion();
        IMultipleChoiceQuestionViewModel questionToEdit = wasSingleResponse
            ? singleResponseQuestion
            : multipleResponseQuestion;
        Mapper.When(x => x.Map(questionToEdit, FormDataContainer.FormModel)).Do(_ =>
        {
            FormDataContainer.FormModel.Text = "old";
            FormDataContainer.FormModel.IsSingleResponse =
                questionToEdit is MultipleChoiceSingleResponseQuestionViewModel;
            FormDataContainer.FormModel.Choices = new List<ChoiceFormModel> { new("old1"), new("old2") };
            FormDataContainer.FormModel.CorrectChoices = new List<ChoiceFormModel>
                { FormDataContainer.FormModel.Choices.First() };
        });

        var systemUnderTest = GetRenderedComponent(taskVm: task, difficulty: difficulty, onSubmitted: onSubmitted,
            questionToEditVm: questionToEdit);
        var mudForm = systemUnderTest.FindComponent<MudForm>();

        // Check that validation everytime returns true
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.True);

        // Set IsSingleResponse to testcase value
        var mudCheckBoxIsSingleResponse = systemUnderTest.FindComponent<MudSwitch<bool>>();
        mudCheckBoxIsSingleResponse.Find("input").Change(isSingleResponse);

        // Add choice
        var mudButtonAddChoice = systemUnderTest.FindComponents<MudButton>()[0].Find("button");
        mudButtonAddChoice.Click();

        // Get all components
        var mudSubmitButton = systemUnderTest.FindComponents<MudButton>().Last();
        var mudTextFields = systemUnderTest.FindComponents<MudTextField<string>>();
        Assert.That(mudTextFields, Has.Count.EqualTo(4));
        var textFieldQuestionText = mudTextFields[0];
        var textFieldChoice1 = mudTextFields[1];
        var textFieldChoice2 = mudTextFields[2];
        var textFieldChoice3 = mudTextFields[3];
        var radioButtons = systemUnderTest.FindComponents<MudRadio<ChoiceFormModel>>();
        var checkBoxes = systemUnderTest.FindComponents<MudCheckBox<bool>>();
        IElement checkChoice1;
        IElement checkChoice2;
        IElement checkChoice3;
        if (isSingleResponse)
        {
            Assert.That(radioButtons, Has.Count.EqualTo(3));
            Assert.That(checkBoxes, Has.Count.EqualTo(0));
            checkChoice1 = radioButtons[0].Find("input");
            checkChoice2 = radioButtons[1].Find("input");
            checkChoice3 = radioButtons[2].Find("input");
        }
        else
        {
            Assert.That(radioButtons, Has.Count.EqualTo(0));
            Assert.That(checkBoxes, Has.Count.EqualTo(3));
            checkChoice1 = checkBoxes[0].Find("input");
            checkChoice2 = checkBoxes[1].Find("input");
            checkChoice3 = checkBoxes[2].Find("input");
        }

        Assert.Multiple(() =>
        {
            Assert.That(textFieldQuestionText.Instance.Text, Is.EqualTo("old"));
            Assert.That(textFieldChoice1.Instance.Text, Is.EqualTo("old1"));
            Assert.That(textFieldChoice2.Instance.Text, Is.EqualTo("old2"));
        });

        // Set values
        textFieldQuestionText.Find("textarea").Change(ExpectedString);
        textFieldChoice1.Find("input").Change(ExpectedString + 1);
        textFieldChoice2.Find("input").Change(ExpectedString + 2);
        textFieldChoice3.Find("input").Change(ExpectedString + 3);
        if (isSingleResponse)
        {
            checkChoice3.Click();
        }
        else
        {
            checkChoice1.Change(false);
            checkChoice2.Change(true);
            checkChoice3.Change(true);
        }

        // Submit
        mudSubmitButton.Find("button").Click();
        Assert.That(onSubmittedCalled, Is.True);

        switch (wasSingleResponse, isSingleResponse)
        {
            case (false, true):
            case (true, false):
                PresentationLogic.Received(1).EditMultipleChoiceQuestionWithTypeChange(task, questionToEdit,
                    isSingleResponse, ExpectedString, Arg.Any<List<ChoiceFormModel>>(),
                    Arg.Any<List<ChoiceFormModel>>(), Arg.Any<int>());
                var arguments = PresentationLogic.ReceivedCalls().First().GetArguments();
                var choices = arguments[4] as List<ChoiceFormModel>;
                var correctChoices = arguments[5] as List<ChoiceFormModel>;
                Assert.That(choices, Has.Count.EqualTo(3));
                Assert.Multiple(() =>
                {
                    Assert.That(choices![0].Text, Is.EqualTo(ExpectedString + 1));
                    Assert.That(choices[1].Text, Is.EqualTo(ExpectedString + 2));
                    Assert.That(choices[2].Text, Is.EqualTo(ExpectedString + 3));
                });
                if (isSingleResponse)
                {
                    Assert.That(correctChoices, Has.Count.EqualTo(1));
                    Assert.That(correctChoices![0], Is.EqualTo(choices![2]));
                }
                else
                {
                    Assert.That(correctChoices, Has.Count.EqualTo(2));
                    Assert.That(correctChoices![0], Is.EqualTo(choices![1]));
                    Assert.That(correctChoices[1], Is.EqualTo(choices[2]));
                }

                break;
            case (false, false):
                PresentationLogic.Received(1).EditMultipleChoiceMultipleResponseQuestion(multipleResponseQuestion,
                    ExpectedString, Arg.Any<List<ChoiceFormModel>>(), Arg.Any<List<ChoiceFormModel>>(), Arg.Any<int>());
                arguments = PresentationLogic.ReceivedCalls().First().GetArguments();
                choices = arguments[2] as List<ChoiceFormModel>;
                correctChoices = arguments[3] as List<ChoiceFormModel>;
                Assert.That(choices, Has.Count.EqualTo(3));
                Assert.That(correctChoices, Has.Count.EqualTo(2));
                Assert.Multiple(() =>
                {
                    Assert.That(choices![0].Text, Is.EqualTo(ExpectedString + 1));
                    Assert.That(choices[1].Text, Is.EqualTo(ExpectedString + 2));
                    Assert.That(choices[2].Text, Is.EqualTo(ExpectedString + 3));
                });
                Assert.That(correctChoices![0], Is.EqualTo(choices![1]));
                Assert.That(correctChoices[1], Is.EqualTo(choices[2]));
                break;
            case (true, true):
                PresentationLogic.Received(1).EditMultipleChoiceSingleResponseQuestion(singleResponseQuestion,
                    ExpectedString, Arg.Any<List<ChoiceFormModel>>(), Arg.Any<ChoiceFormModel>(), Arg.Any<int>());
                arguments = PresentationLogic.ReceivedCalls().First().GetArguments();
                choices = arguments[2] as List<ChoiceFormModel>;
                Assert.That(choices, Has.Count.EqualTo(3));
                Assert.Multiple(() =>
                {
                    Assert.That(choices![0].Text, Is.EqualTo(ExpectedString + 1));
                    Assert.That(choices[1].Text, Is.EqualTo(ExpectedString + 2));
                    Assert.That(choices[2].Text, Is.EqualTo(ExpectedString + 3));
                    Assert.That(arguments[3], Is.EqualTo(choices[2]));
                });
                break;
        }
    }

    [Test]
    public async Task EditQuestion_OnCloseWithoutSubmit_DoesNotChangeQuestion([Values] bool wasSingleResponse,
        [Values] bool isSingleResponse)
    {
        const string changedString = "ChangedString";
        var task = Substitute.For<IAdaptivityTaskViewModel>();
        const QuestionDifficulty difficulty = QuestionDifficulty.Medium;
        var onSubmittedCalled = false;
        var onSubmitted = EventCallback.Factory.Create(this, () => onSubmittedCalled = true);
        var choicesVm = new List<ChoiceViewModel>
        {
            new(ExpectedString + 1),
            new(ExpectedString + 2)
        };
        var singleResponseQuestion = new MultipleChoiceSingleResponseQuestionViewModel(0, ExpectedString,
            choicesVm.First(), difficulty, choicesVm);
        var multipleResponseQuestion = new MultipleChoiceMultipleResponseQuestionViewModel(0, ExpectedString,
            difficulty, new Collection<ChoiceViewModel> { choicesVm.First(), choicesVm.Last() },
            choicesVm);
        IMultipleChoiceQuestionViewModel questionToEdit = wasSingleResponse
            ? singleResponseQuestion
            : multipleResponseQuestion;
        Mapper.When(x => x.Map(questionToEdit, FormDataContainer.FormModel)).Do(_ =>
        {
            FormDataContainer.FormModel.Text = questionToEdit.Text;
            FormDataContainer.FormModel.IsSingleResponse =
                questionToEdit is MultipleChoiceSingleResponseQuestionViewModel;
            FormDataContainer.FormModel.Choices =
                questionToEdit.Choices.Select(x => new ChoiceFormModel(x.Text)).ToList();
            FormDataContainer.FormModel.CorrectChoices = questionToEdit is MultipleChoiceSingleResponseQuestionViewModel
                ? FormDataContainer.FormModel.Choices
                    .Where(x =>
                        ((MultipleChoiceSingleResponseQuestionViewModel)questionToEdit).CorrectChoice.Text ==
                        x.Text).ToList()
                : FormDataContainer.FormModel.Choices
                    .Where(x => questionToEdit.CorrectChoices.Any(y => x.Text.Contains(y.Text))).ToList();
        });

        var systemUnderTest = GetRenderedComponent(taskVm: task, difficulty: difficulty, onSubmitted: onSubmitted,
            questionToEditVm: questionToEdit);

        var mudForm = systemUnderTest.FindComponent<MudForm>();

        // Check that validation everytime returns true
        await mudForm.InvokeAsync(async () => await mudForm.Instance.Validate());
        Assert.That(mudForm.Instance.IsValid, Is.True);

        // Set IsSingleResponse to testcase value
        var mudCheckBoxIsSingleResponse = systemUnderTest.FindComponent<MudSwitch<bool>>();
        mudCheckBoxIsSingleResponse.Find("input").Change(isSingleResponse);

        // Add choice
        var mudButtonAddChoice = systemUnderTest.FindComponents<MudButton>()[0].Find("button");
        mudButtonAddChoice.Click();

        // Get all components
        _ = systemUnderTest.FindComponents<MudButton>().Last();
        var mudTextFields = systemUnderTest.FindComponents<MudTextField<string>>();
        Assert.That(mudTextFields, Has.Count.EqualTo(4));
        var textFieldQuestionText = mudTextFields[0];
        var textFieldChoice1 = mudTextFields[1];
        var textFieldChoice2 = mudTextFields[2];
        var textFieldChoice3 = mudTextFields[3];
        var radioButtons = systemUnderTest.FindComponents<MudRadio<ChoiceFormModel>>();
        var checkBoxes = systemUnderTest.FindComponents<MudCheckBox<bool>>();
        IElement checkChoice1;
        IElement checkChoice2;
        IElement checkChoice3;
        if (isSingleResponse)
        {
            Assert.That(radioButtons, Has.Count.EqualTo(3));
            Assert.That(checkBoxes, Has.Count.EqualTo(0));
            checkChoice1 = radioButtons[0].Find("input");
            checkChoice2 = radioButtons[1].Find("input");
            checkChoice3 = radioButtons[2].Find("input");
        }

        else
        {
            Assert.That(radioButtons, Has.Count.EqualTo(0));
            Assert.That(checkBoxes, Has.Count.EqualTo(3));
            checkChoice1 = checkBoxes[0].Find("input");
            checkChoice2 = checkBoxes[1].Find("input");
            checkChoice3 = checkBoxes[2].Find("input");
        }

        Assert.Multiple(() =>
        {
            Assert.That(textFieldQuestionText.Instance.Text, Is.EqualTo(ExpectedString));
            Assert.That(textFieldChoice1.Instance.Text, Is.EqualTo(ExpectedString + 1));
            Assert.That(textFieldChoice2.Instance.Text, Is.EqualTo(ExpectedString + 2));
        });

        // Set values
        textFieldQuestionText.Find("textarea").Change(changedString);
        textFieldChoice1.Find("input").Change(changedString + 1);
        textFieldChoice2.Find("input").Change(changedString + 2);
        textFieldChoice3.Find("input").Change(changedString + 3);
        if (isSingleResponse)
        {
            checkChoice3.Click();
        }
        else
        {
            checkChoice1.Change(false);
            checkChoice2.Change(true);
            checkChoice3.Change(true);
        }

        // Close without submit
        systemUnderTest.Dispose();
        Assert.That(onSubmittedCalled, Is.False);

        switch (wasSingleResponse, isSingleResponse)
        {
            case (false, true):
            case (true, false):
                PresentationLogic.DidNotReceive().EditMultipleChoiceQuestionWithTypeChange(task, questionToEdit,
                    isSingleResponse, Arg.Any<string>(), Arg.Any<List<ChoiceFormModel>>(),
                    Arg.Any<List<ChoiceFormModel>>(), Arg.Any<int>());
                break;
            case (false, false):
                PresentationLogic.DidNotReceive().EditMultipleChoiceMultipleResponseQuestion(multipleResponseQuestion,
                    Arg.Any<string>(), Arg.Any<List<ChoiceFormModel>>(), Arg.Any<List<ChoiceFormModel>>(),
                    Arg.Any<int>());
                break;
            case (true, true):
                PresentationLogic.DidNotReceive().EditMultipleChoiceSingleResponseQuestion(singleResponseQuestion,
                    Arg.Any<string>(), Arg.Any<List<ChoiceFormModel>>(), Arg.Any<ChoiceFormModel>(), Arg.Any<int>());
                break;
        }

        Assert.Multiple(() =>
        {
            Assert.That(questionToEdit.Text, Is.EqualTo(ExpectedString));
            Assert.That(questionToEdit.Choices, Has.Count.EqualTo(2));
            Assert.That(questionToEdit.CorrectChoices,
                questionToEdit is MultipleChoiceSingleResponseQuestionViewModel
                    ? Has.Count.EqualTo(1)
                    : Has.Count.EqualTo(2));
        });
    }

    private void CheckFormModelContainingChoicesAndCorrectChoices(ICollection<ChoiceFormModel> choices,
        ICollection<ChoiceFormModel> correctChoices)
    {
        Assert.That(FormModel.Choices, Has.Count.EqualTo(choices.Count));
        Assert.That(FormModel.CorrectChoices, Has.Count.EqualTo(correctChoices.Count));
        foreach (var choice in choices)
        {
            Assert.That(FormModel.Choices, Contains.Item(choice));
        }

        foreach (var choice in correctChoices)
        {
            Assert.That(FormModel.CorrectChoices, Contains.Item(choice));
        }
    }


    private void SetValidatorAllMembers()
    {
        MultipleResponseQuestionValidator
            .ValidateAsync((Entity as MultipleChoiceMultipleResponseQuestion)!, Arg.Any<string>())
            .Returns(ci => Validator.ValidateAsync(Entity, ci.Arg<string>()));
        SingleResponseQuestionValidator
            .ValidateAsync((Entity as MultipleChoiceSingleResponseQuestion)!, Arg.Any<string>())
            .Returns(ci => Validator.ValidateAsync(Entity, ci.Arg<string>()));
        Validator.ValidateAsync(Entity, Arg.Any<string>()).Returns(ci =>
            {
                var value = FormModel.GetType().GetProperty(ci.Arg<string>())?.GetValue(FormModel);
                var valid = value switch
                {
                    null => false,
                    string str => str != "" && str == ExpectedString,
                    bool => true,
                    List<ChoiceFormModel> choices => choices.Any(),
                    _ => throw new ArgumentOutOfRangeException()
                };
                return valid ? Enumerable.Empty<string>() : new[] { $"Must be {ExpectedString}" };
            }
        );
    }

    private IRenderedComponent<MultipleChoiceQuestionForm> GetRenderedComponent(IAdaptivityTaskViewModel? taskVm = null,
        QuestionDifficulty? difficulty = null, IMultipleChoiceQuestionViewModel? questionToEditVm = null,
        EventCallback? onSubmitted = null)
    {
        taskVm ??= ViewModelProvider.GetAdaptivityTask();
        difficulty ??= QuestionDifficulty.Easy;
        onSubmitted ??= EventCallback.Empty;
        return Context.RenderComponent<MultipleChoiceQuestionForm>(p =>
        {
            p.Add(c => c.Task, taskVm);
            p.Add(c => c.Difficulty, difficulty.Value);
            p.Add(c => c.DebounceInterval, 0);
            p.Add(c => c.QuestionToEdit, questionToEditVm);
            p.Add(c => c.OnSubmitted, onSubmitted.Value);
        });
    }
}