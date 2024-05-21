using AgileObjects.ReadableExpressions;
using AuthoringTool.Mapping;
using AutoMapper;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using NUnit.Framework;
using Presentation.Components.Forms.Models;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Question;
using TestHelpers;

namespace AuthoringToolTest.Mapping;

[TestFixture]
public class FormModelEntityMappingProfileUt
{
    private const string QuestionText = "questionText";
    private const int ExpectedCompletionTime = 10;

    // [Test]
    // public void Constructor_TestConfigurationisValid()
    // {
    //     var mapper = new MapperConfiguration(cfg =>
    //     {
    //         FormModelEntityMappingProfile.Configure(cfg);
    //         cfg.AddCollectionMappersOnce();
    //     });
    //     
    //     Assert.That(() => mapper.AssertConfigurationIsValid(), Throws.Nothing);
    // }

    [Test]
    public void Debug()
    {
        var mapper = new MapperConfiguration(cfg =>
        {
            FormModelEntityMappingProfile.Configure(cfg);
            ViewModelFormModelMappingProfile.Configure(cfg);
            cfg.AddCollectionMappersOnce();
        });

        var executionPlan = mapper.BuildExecutionPlan(typeof(LearningElementFormModel), typeof(LearningElement));
        var plan = executionPlan.ToReadableString();
    }

    [Test]
    public void MultipleChoiceQuestion_SingleResponse_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var questionFm = FormModelProvider.GetMultipleChoiceQuestion();
        var choices = new List<ChoiceViewModel>
            {ViewModelProvider.GetChoice(), ViewModelProvider.GetChoice(), ViewModelProvider.GetChoice()};
        questionFm.IsSingleResponse = true;
        questionFm.Choices = choices;
        questionFm.CorrectChoices.Clear();
        questionFm.CorrectChoices.Add(choices[1]);
        questionFm.Text = QuestionText;
        questionFm.ExpectedCompletionTime = ExpectedCompletionTime;

        var questionEntity = systemUnderTest.Map<IMultipleChoiceQuestion>(questionFm);

        Assert.That(questionEntity, Is.TypeOf<MultipleChoiceSingleResponseQuestion>());
        Assert.Multiple(() =>
        {
            Assert.That(questionEntity.Text, Is.EqualTo(QuestionText));
            Assert.That(questionEntity.ExpectedCompletionTime, Is.EqualTo(ExpectedCompletionTime));
            Assert.That(questionEntity.Choices.Select(x => x.Id), Is.EquivalentTo(choices.Select(x => x.Id)));
            Assert.That(questionEntity.CorrectChoices.Select(x => x.Id),
                Is.EquivalentTo(questionFm.CorrectChoices.Select(x => x.Id)));
            Assert.That(((MultipleChoiceSingleResponseQuestion) questionEntity).CorrectChoice.Id,
                Is.EqualTo(questionFm.CorrectChoices.First().Id));
        });
    }

    [Test]
    public void MultipleChoiceQuestion_MultipleResponse_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var questionFm = FormModelProvider.GetMultipleChoiceQuestion();
        var choices = new List<ChoiceViewModel>
            {ViewModelProvider.GetChoice(), ViewModelProvider.GetChoice(), ViewModelProvider.GetChoice()};
        questionFm.IsSingleResponse = false;
        questionFm.Choices = choices;
        questionFm.CorrectChoices.Clear();
        questionFm.CorrectChoices.Add(choices[0]);
        questionFm.CorrectChoices.Add(choices[2]);
        questionFm.Text = QuestionText;
        questionFm.ExpectedCompletionTime = ExpectedCompletionTime;

        var questionEntity = systemUnderTest.Map<IMultipleChoiceQuestion>(questionFm);

        Assert.That(questionEntity, Is.TypeOf<MultipleChoiceMultipleResponseQuestion>());
        Assert.Multiple(() =>
        {
            Assert.That(questionEntity.Text, Is.EqualTo(QuestionText));
            Assert.That(questionEntity.ExpectedCompletionTime, Is.EqualTo(ExpectedCompletionTime));
            Assert.That(questionEntity.Choices.Select(x => x.Id), Is.EquivalentTo(choices.Select(x => x.Id)));
            Assert.That(questionEntity.CorrectChoices.Select(x => x.Id),
                Is.EquivalentTo(questionFm.CorrectChoices.Select(x => x.Id)));
        });
    }

    private static IMapper CreateTestableMapper()
    {
        var mapper = new MapperConfiguration(cfg =>
        {
            FormModelEntityMappingProfile.Configure(cfg);
            // ViewModelEntityMappingProfile is needed for the mapping of the choices
            ViewModelEntityMappingProfile.Configure(cfg);
            cfg.AddCollectionMappersOnce();
        });
        var systemUnderTest = mapper.CreateMapper();
        return systemUnderTest;
    }
}