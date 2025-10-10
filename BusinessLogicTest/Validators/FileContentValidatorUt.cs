using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.FileContent;
using BusinessLogic.Validation.Validators;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NUnit.Framework;

namespace BusinessLogicTest.Validators;

[TestFixture]
public class FileContentValidatorUt
{
    private FileContentValidator _validator;

    [SetUp]
    public void SetUp()
    {
        var baseValidator = Substitute.For<IValidator<ILearningContent>>();
        baseValidator.Validate(Arg.Any<ILearningContent>()).Returns(new ValidationResult());
        _validator = new FileContentValidator(baseValidator);
    }

    [TestCase("file.h5p", ".h5p", true)]
    [TestCase("file.pdf", ".pdf", true)]
    [TestCase("file.py", ".py", true)]
    [TestCase("file.cs", ".cs", true)]
    [TestCase("file.jpg", ".jpg", true)]
    [TestCase("file.jpeg", ".jpeg", true)]
    [TestCase("file.png", ".png", true)]
    [TestCase("file.webp", ".webp", true)]
    [TestCase("file.txt", ".txt", true)]
    [TestCase("file.html", ".html", true)]
    [TestCase("file.css", ".css", true)]
    [TestCase("file.exe", ".exe", false)]
    [TestCase("file.zip", ".zip", false)]
    [TestCase("file.rar", ".rar", false)]
    [TestCase("file.bat", ".bat", false)]
    [TestCase("file.sh", ".sh", false)]
    [TestCase("file.docx", ".docx", false)]
    [TestCase("file", "", false)]
    [TestCase("file", null, false)]
    public void Validate_FileTypeAccordingToBusinessRules(string filepath, string? type, bool expectedValid)
    {
        var fileContent = Substitute.For<IFileContent>();
        fileContent.Filepath.Returns(filepath);
        fileContent.Type.Returns(type);

        var result = _validator.Validate(fileContent);

        Assert.That(result.IsValid, Is.EqualTo(expectedValid), $"Expected validity for type '{type}' to be {expectedValid}, but was {result.IsValid}.");
    }

    [Test]
    public void Validate_EmptyFilepath_ShouldBeInvalid()
    {
        var fileContent = Substitute.For<IFileContent>();
        fileContent.Filepath.Returns("");
        fileContent.Type.Returns(".pdf");

        var result = _validator.Validate(fileContent);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(e => e.PropertyName == "Filepath"));
    }

    [Test]
    public void Validate_NullFilepath_ShouldBeInvalid()
    {
        var fileContent = Substitute.For<IFileContent>();
        fileContent.Filepath.Returns((string?)null);
        fileContent.Type.Returns(".pdf");

        var result = _validator.Validate(fileContent);

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Errors.Any(e => e.PropertyName == "Filepath"));
    }

    [Test]
    public void Validate_AllFieldsValid_ShouldBeValid()
    {
        var fileContent = Substitute.For<IFileContent>();
        fileContent.Filepath.Returns("uploads/safe.pdf");
        fileContent.Type.Returns(".pdf");

        var result = _validator.Validate(fileContent);

        Assert.That(result.IsValid, Is.True);
    }
}
