using BusinessLogic.API;
using BusinessLogic.Commands.World;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.World;

[TestFixture]
public class WorldCommandFactoryUt
{
    [SetUp]
    public void SetUp()
    {
        _factory = new WorldCommandFactory(new NullLoggerFactory());
    }

    private IWorldCommandFactory _factory = null!;

    [Test]
    public void GetCreateCommand_WithAuthoringToolWorkspaceAndParameters_ReturnsCreateLearningWorldCommand()
    {
        // Arrange
        var authoringToolWorkspace = EntityProvider.GetAuthoringToolWorkspace();
        var name = "WorldName";
        var shortname = "WorldShortName";
        var authors = "WorldAuthors";
        var language = "WorldLanguage";
        var description = "WorldDescription";
        var goals = "WorldGoals";
        Action<AuthoringToolWorkspace> mappingAction = _ => { };

        // Act
        var result = _factory.GetCreateCommand(authoringToolWorkspace, name, shortname, authors, language,
            description, goals, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<CreateLearningWorld>());
        var resultCasted = result as CreateLearningWorld;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.AuthoringToolWorkspace, Is.EqualTo(authoringToolWorkspace));
            Assert.That(resultCasted.LearningWorld.Name, Is.EqualTo(name));
            Assert.That(resultCasted.LearningWorld.Shortname, Is.EqualTo(shortname));
            Assert.That(resultCasted.LearningWorld.Authors, Is.EqualTo(authors));
            Assert.That(resultCasted.LearningWorld.Language, Is.EqualTo(language));
            Assert.That(resultCasted.LearningWorld.Description, Is.EqualTo(description));
            Assert.That(resultCasted.LearningWorld.Goals, Is.EqualTo(goals));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetCreateCommand_WithAuthoringToolWorkspaceAndLearningWorld_ReturnsCreateLearningWorldCommand()
    {
        // Arrange
        var authoringToolWorkspace = EntityProvider.GetAuthoringToolWorkspace();
        var learningWorld = EntityProvider.GetLearningWorld();
        Action<AuthoringToolWorkspace> mappingAction = _ => { };

        // Act
        var result = _factory.GetCreateCommand(authoringToolWorkspace, learningWorld, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<CreateLearningWorld>());
        var resultCasted = result as CreateLearningWorld;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.AuthoringToolWorkspace, Is.EqualTo(authoringToolWorkspace));
            Assert.That(resultCasted.LearningWorld, Is.EqualTo(learningWorld));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetDeleteCommand_WithAuthoringToolWorkspaceAndLearningWorld_ReturnsDeleteLearningWorldCommand()
    {
        // Arrange
        var authoringToolWorkspace = EntityProvider.GetAuthoringToolWorkspace();
        var learningWorld = EntityProvider.GetLearningWorld();
        Action<AuthoringToolWorkspace> mappingAction = _ => { };

        // Act
        var result = _factory.GetDeleteCommand(authoringToolWorkspace, learningWorld, mappingAction);
        // Assert
        Assert.That(result, Is.InstanceOf<DeleteLearningWorld>());
        var resultCasted = result as DeleteLearningWorld;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.AuthoringToolWorkspace, Is.EqualTo(authoringToolWorkspace));
            Assert.That(resultCasted.LearningWorld, Is.EqualTo(learningWorld));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetEditCommand_WithLearningWorldAndParameters_ReturnsEditLearningWorldCommand()
    {
        // Arrange
        var learningWorld = EntityProvider.GetLearningWorld();
        var name = "NewName";
        var shortname = "NewShortName";
        var authors = "NewAuthors";
        var language = "NewLanguage";
        var description = "NewDescription";
        var goals = "NewGoals";
        Action<LearningWorld> mappingAction = _ => { };

        // Act
        var result = _factory.GetEditCommand(learningWorld, name, shortname, authors, language,
            description, goals, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<EditLearningWorld>());
        var resultCasted = result as EditLearningWorld;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.LearningWorld, Is.EqualTo(learningWorld));
            Assert.That(resultCasted.WorldName, Is.EqualTo(name));
            Assert.That(resultCasted.Shortname, Is.EqualTo(shortname));
            Assert.That(resultCasted.Authors, Is.EqualTo(authors));
            Assert.That(resultCasted.Language, Is.EqualTo(language));
            Assert.That(resultCasted.Description, Is.EqualTo(description));
            Assert.That(resultCasted.Goals, Is.EqualTo(goals));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetLoadCommand_WithAuthoringToolWorkspaceAndFilePath_ReturnsLoadLearningWorldCommand()
    {
        // Arrange
        var authoringToolWorkspace = EntityProvider.GetAuthoringToolWorkspace();
        var filepath = "FilePath";
        var businessLogic = Substitute.For<IBusinessLogic>();
        Action<AuthoringToolWorkspace> mappingAction = _ => { };

        // Act
        var result = _factory.GetLoadCommand(authoringToolWorkspace, filepath, businessLogic, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<LoadLearningWorld>());
        var resultCasted = result as LoadLearningWorld;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.Workspace, Is.EqualTo(authoringToolWorkspace));
            Assert.That(resultCasted.Filepath, Is.EqualTo(filepath));
            Assert.That(resultCasted.BusinessLogic, Is.EqualTo(businessLogic));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetLoadCommand_WithAuthoringToolWorkspaceAndStream_ReturnsLoadLearningWorldCommand()
    {
        // Arrange
        var authoringToolWorkspace = EntityProvider.GetAuthoringToolWorkspace();
        var stream = Substitute.For<Stream>();
        var businessLogic = Substitute.For<IBusinessLogic>();
        var learningWorld = EntityProvider.GetLearningWorld();
        businessLogic.LoadLearningWorld(Arg.Any<Stream>()).Returns(learningWorld);
        Action<AuthoringToolWorkspace> mappingAction = _ => { };

        // Act
        var result = _factory.GetLoadCommand(authoringToolWorkspace, stream, businessLogic, mappingAction);

        // Assert
        Assert.That(result, Is.InstanceOf<LoadLearningWorld>());
        var resultCasted = result as LoadLearningWorld;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.Workspace, Is.EqualTo(authoringToolWorkspace));
            Assert.That(resultCasted.LearningWorld, Is.EqualTo(learningWorld));
            Assert.That(resultCasted.BusinessLogic, Is.EqualTo(businessLogic));
            Assert.That(resultCasted.MappingAction, Is.EqualTo(mappingAction));
        });
    }

    [Test]
    public void GetSaveCommand_WithBusinessLogicLearningWorldAndFilePath_ReturnsSaveLearningWorldCommand()
    {
        // Arrange
        var businessLogic = Substitute.For<IBusinessLogic>();
        var learningWorld = EntityProvider.GetLearningWorld();
        var filepath = "FilePath";
        // Act
        var result = _factory.GetSaveCommand(businessLogic, learningWorld, filepath);

        // Assert
        Assert.That(result, Is.InstanceOf<SaveLearningWorld>());
        var resultCasted = result as SaveLearningWorld;
        Assert.Multiple(() =>
        {
            Assert.That(resultCasted!.BusinessLogic, Is.EqualTo(businessLogic));
            Assert.That(resultCasted.LearningWorld, Is.EqualTo(learningWorld));
            Assert.That(resultCasted.Filepath, Is.EqualTo(filepath));
        });
    }
}