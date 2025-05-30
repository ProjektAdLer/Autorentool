using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using BusinessLogic.Entities.LearningContent.FileContent;
using BusinessLogic.Validation.Validators;
using Microsoft.Extensions.Localization;
using NSubstitute;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Validators;

[TestFixture]
public class LearningWorldStructureValidatorUt
{
    private IStringLocalizer<LearningWorldStructureValidator> _localizer;
    private LearningWorldStructureValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _localizer = Substitute.For<IStringLocalizer<LearningWorldStructureValidator>>();

        _localizer[Arg.Any<string>(), Arg.Any<object[]>()]
            .Returns(call =>
            {
                var key = call.Arg<string>();
                var args = call.Arg<object[]>() ?? Array.Empty<object>();
                return new LocalizedString(key, $"{key}: {string.Join(", ", args)}");
            });

        _localizer[Arg.Any<string>()]
            .Returns(call =>
            {
                var key = call.Arg<string>();
                return new LocalizedString(key, $"{key}: a");
            });

        _validator = new LearningWorldStructureValidator(_localizer);
    }


    // ANF-ID: [ASN0001]
    [Test]
    public void ValidateForExport_NoErrors_ReturnsEmptyResult()
    {
        var fileContent = Substitute.For<IFileContent>();

        var world = EntityProvider.GetLearningWorld();

        var result = _validator.ValidateForExport(world, new List<ILearningContent> { fileContent });

        Assert.That(result.Errors, Is.Empty);
        Assert.That(result.IsValid, Is.True);
    }

    // ANF-ID: [ASN0001]
    [Test]
    public void ValidateForExport_InValid_ReturnsError()
    {
        var world = EntityProvider.GetLearningWorld();
        var space = Substitute.For<ILearningSpace>();
        world.LearningSpaces.Add(space);
        var ele = EntityProvider.GetLearningElement(false, new FileContent("TestFile", "pdf", "application/pdf"));
        var adaEle = EntityProvider.GetLearningElement();
        space.ContainedLearningElements.Returns(new[] { adaEle, ele });
        var adaptivityContent = EntityProvider.GetAdaptivityContent();
        adaptivityContent.Tasks = new List<IAdaptivityTask>
        {
            EntityProvider.GetAdaptivityTask(),
            EntityProvider.GetAdaptivityTask()
        };
        var eleRefAction = EntityProvider.GetElementReferenceAction();
        var contentRefAction = EntityProvider.GetContentReferenceAction();

        adaptivityContent.Tasks.First().Questions.First().Rules = new List<IAdaptivityRule>
        {
            EntityProvider.GetAdaptivityRule(null, eleRefAction),
            EntityProvider.GetAdaptivityRule(null, contentRefAction)
        };

        adaEle.LearningContent = adaptivityContent;

        var result = _validator.ValidateForExport(world, new List<ILearningContent>());

        Assert.That(result.Errors, Has.Count.EqualTo(3));
        Assert.That(result.Errors[0], Is.EqualTo("<li>ErrorString.Missing.LearningContent.Message: a</li>"));
        Assert.That(result.Errors[1], Is.EqualTo("<li>ErrorString.TaskReferencesNonexistantElement.Message: a</li>"));
        Assert.That(result.Errors[2], Is.EqualTo("<li>ErrorString.TaskReferencesNonexistantContent.Message: a</li>"));
        Assert.That(result.IsValid, Is.False);
    }

    // ANF-ID: [AHO22]
    [Test]
    public void ValidateForGeneration_NoErrors_ReturnsEmptyResult()
    {
        var fileContent = new FileContent("name", "txt", "text/plain");

        var world = EntityProvider.GetLearningWorld();
        var space = Substitute.For<ILearningSpace>();
        var element = EntityProvider.GetLearningElement(false, fileContent);
        space.ContainedLearningElements.Returns(new[] { element });
        space.Points.Returns(5);
        space.RequiredPoints.Returns(3);
        world.LearningSpaces.Add(space);

        var result = _validator.ValidateForGeneration(world, new List<ILearningContent> { fileContent });

        Assert.That(result.Errors, Is.Empty);
        Assert.That(result.IsValid, Is.True);
    }

    // ANF-ID: [AHO22]
    [Test]
    public void ValidateForGeneration_ReturnsMultipleErrors()
    {
        var world = EntityProvider.GetLearningWorld();

        var space1 = Substitute.For<ILearningSpace>();
        var element1 = EntityProvider.GetLearningElement(false, new FileContent("ele1", "pdf", "text/plain"));
        space1.ContainedLearningElements.Returns(new[] { element1 });
        space1.Points.Returns(1);
        space1.RequiredPoints.Returns(3);

        var space2 = Substitute.For<ILearningSpace>();
        var element2 = EntityProvider.GetLearningElement(false, new FileContent("missing", "txt", "text/plain"));
        space2.ContainedLearningElements.Returns(new[] { element2 });
        space2.Points.Returns(1);
        space2.RequiredPoints.Returns(3);

        var space3 = Substitute.For<ILearningSpace>();

        space1.OutBoundObjects.Returns(new List<IObjectInPathWay>() { space2 });
        space2.InBoundObjects.Returns(new List<IObjectInPathWay>() { space1 });
        space2.OutBoundObjects.Returns(new List<IObjectInPathWay>() { space3 });
        space3.InBoundObjects.Returns(new List<IObjectInPathWay>() { space2 });
        space3.OutBoundObjects.Returns(new List<IObjectInPathWay>());

        var adaptivityElement = EntityProvider.GetLearningElement();
        var adaptivityContent = EntityProvider.GetAdaptivityContent();
        var eleRefAction = EntityProvider.GetElementReferenceAction();
        var contentRefAction = EntityProvider.GetContentReferenceAction();

        adaptivityContent.Tasks = new List<IAdaptivityTask>
        {
            EntityProvider.GetAdaptivityTask()
        };
        adaptivityContent.Tasks.First().Questions.First().Rules = new List<IAdaptivityRule>
        {
            EntityProvider.GetAdaptivityRule(null, eleRefAction),
            EntityProvider.GetAdaptivityRule(null, contentRefAction)
        };
        adaptivityElement.LearningContent = adaptivityContent;

        space2.ContainedLearningElements.Returns(space2.ContainedLearningElements.Concat(new[] { adaptivityElement })
            .ToArray());

        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);

        var result = _validator.ValidateForGeneration(world, new List<ILearningContent>());

        Assert.That(result.Errors.Count, Is.EqualTo(6));
        Assert.That(result.Errors[0], Is.EqualTo("<li>ErrorString.Missing.LearningContent.Message: a</li>"));
        Assert.That(result.Errors[1], Is.EqualTo("<li>ErrorString.Missing.LearningContent.Message: a</li>"));
        Assert.That(result.Errors[2], Is.EqualTo("<li>ErrorString.TaskReferencesNonexistantElement.Message: a</li>"));
        Assert.That(result.Errors[3], Is.EqualTo("<li>ErrorString.TaskReferencesNonexistantContent.Message: a</li>"));
        Assert.That(result.Errors[4], Is.EqualTo("<li>ErrorString.Insufficient.Points.Message: </li>"));
        Assert.That(result.Errors[5], Is.EqualTo("<li>ErrorString.Insufficient.Points.Message: </li>"));
        Assert.That(result.IsValid, Is.False);
    }

    // ANF-ID: [AHO22]
    [Test]
    public void ValidateForGeneration_EmptyWorld_ReturnsError()
    {
        var world = EntityProvider.GetLearningWorld();
        world.LearningSpaces.Clear();

        var result = _validator.ValidateForGeneration(world, new List<ILearningContent>());

        Assert.That(result.Errors.Count, Is.EqualTo(1));
        Assert.That(result.Errors[0], Is.EqualTo("<li>ErrorString.Missing.LearningSpace.Message: a</li>"));
        Assert.That(result.IsValid, Is.False);
    }

    // ANF-ID: [AHO22]
    [Test]
    public void ValidateForGeneration_TooManySpaces_ReturnsError()
    {
        var world = EntityProvider.GetLearningWorld();
        for (int i = 0; i < 51; i++)
        {
            world.LearningSpaces.Add(EntityProvider.GetLearningSpace());
        }

        var result = _validator.ValidateForGeneration(world, new List<ILearningContent>());

        Assert.That(result.IsValid, Is.False);
    }

    // ANF-ID: [AHO22]
    [Test]
    public void ValidateForGeneration_AdaptivityReferencesElementInLaterSpace_ReturnsError()
    {
        var world = EntityProvider.GetLearningWorld();

        var space1 = EntityProvider.GetLearningSpace();
        var space2 = EntityProvider.GetLearningSpace();
        var space3 = EntityProvider.GetLearningSpace();

        space1.OutBoundObjects.Add(space2);
        space2.InBoundObjects.Add(space1);
        space2.OutBoundObjects.Add(space3);
        space3.InBoundObjects.Add(space2);

        var adaptivityElement = EntityProvider.GetLearningElement();
        var adaptivityContent = EntityProvider.GetAdaptivityContent();

        var targetElement = EntityProvider.GetLearningElement();
        var eleRefAction = new ElementReferenceAction(targetElement.Id, "Reference to later space element");

        adaptivityContent.Tasks = new List<IAdaptivityTask>
        {
            EntityProvider.GetAdaptivityTask()
        };
        adaptivityContent.Tasks.First().Questions.First().Rules = new List<IAdaptivityRule>
        {
            EntityProvider.GetAdaptivityRule(null, eleRefAction)
        };

        adaptivityElement.LearningContent = adaptivityContent;

        space1.LearningSpaceLayout.LearningElements.Add(0,adaptivityElement);
        space3.LearningSpaceLayout.LearningElements.Add(0,targetElement);

        world.LearningSpaces.Add(space1);
        world.LearningSpaces.Add(space2);
        world.LearningSpaces.Add(space3);

        var result = _validator.ValidateForGeneration(world, new List<ILearningContent>());

        Assert.That(result.Errors.Any(e => e.Contains("TaskReferencesElementInSpaceAfterOwnSpace")), Is.True);
        Assert.That(result.IsValid, Is.False);
    }
    
    // ANF-ID: [AHO22]
    [Test]
    public void ValidateForGeneration_AdaptivityReferencesUnplacedElement_ReturnsError()
    {
        var world = EntityProvider.GetLearningWorld();
    
        var space = EntityProvider.GetLearningSpace();
    
        var unplacedElement = EntityProvider.GetLearningElement();
        world.UnplacedLearningElements.Add(unplacedElement);
    
        var adaptivityElement = EntityProvider.GetLearningElement();
        var adaptivityContent = EntityProvider.GetAdaptivityContent();
    
        var eleRefAction = new ElementReferenceAction(unplacedElement.Id, "Reference to unplaced element");
    
        adaptivityContent.Tasks = new List<IAdaptivityTask>
        {
            EntityProvider.GetAdaptivityTask()
        };
        adaptivityContent.Tasks.First().Questions.First().Rules = new List<IAdaptivityRule>
        {
            EntityProvider.GetAdaptivityRule(null, eleRefAction)
        };
    
        adaptivityElement.LearningContent = adaptivityContent;
        space.LearningSpaceLayout.LearningElements.Add(0,adaptivityElement);
        world.LearningSpaces.Add(space);

        var result = _validator.ValidateForGeneration(world, new List<ILearningContent>());

        Assert.That(result.Errors.Any(e => e.Contains("TaskReferencesUnplacedElement")), Is.True);
        Assert.That(result.IsValid, Is.False);
    }
}