using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Validation.Validators;
using FluentValidation.Results;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Validators;

[TestFixture]
public class LearningContentValidatorUt
{
    private LearningContentValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new LearningContentValidator();
    }

    [Test]
    public void Validate_EmptyName_Error()
    {
        var learningContent = Substitute.For<ILearningContent>();
        learningContent.Name.Returns(string.Empty);

        ValidationResult result = _validator.Validate(learningContent);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(x => x.PropertyName == "Name"));
    }

    [Test]
    public void Validate_ValideName_Positive()
    {
        var learningContent = Substitute.For<ILearningContent>();
        learningContent.Name.Returns("Valid Name");

        ValidationResult result = _validator.Validate(learningContent);

        Assert.That(result.IsValid, Is.True);
    }
}