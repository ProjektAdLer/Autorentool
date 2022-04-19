using AuthoringTool.PresentationLogic.EntityMapping;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningWorld;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.EntityMapping;

[TestFixture]

public class LearningElementMapperUt
{
    [Test]
    public void LearningElementMapper_ToEntity_MapsPropertiesCorrectly()
    {
        var world = new LearningWorldViewModel("baba", "bubu", "", "", "", "");
        var viewModel = new LearningElementViewModel("name", "shortname", world, "type", "content", null,"authors",
            "description", "goals", 1, 2);
        
        var systemUnderTest = CreateTestableLearningElementMapper();
        
        var entity = systemUnderTest.ToEntity(viewModel);
        Assert.That(entity, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(entity.Name, Is.EqualTo(viewModel.Name));
            Assert.That(entity.Shortname, Is.EqualTo(viewModel.Shortname));
            Assert.That(entity.ParentName, Is.EqualTo(viewModel.Parent!.Name));
            Assert.That(entity.Type, Is.EqualTo(viewModel.Type));
            Assert.That(entity.Content, Is.EqualTo(viewModel.Content));
            Assert.That(entity.Authors, Is.EqualTo(viewModel.Authors));
            Assert.That(entity.Description, Is.EqualTo(viewModel.Description));
            Assert.That(entity.Goals, Is.EqualTo(viewModel.Goals));
            Assert.That(entity.PositionX, Is.EqualTo(viewModel.PositionX));
            Assert.That(entity.PositionY, Is.EqualTo(viewModel.PositionY));
        });
    }

    [Test]
    public void LearningElementMapper_ToViewModel_MapsPropertiesCorrectly()
    {
        var world = new LearningWorldViewModel("bubu", "", "", "", "", "");
        var entity = new AuthoringTool.Entities.LearningElement("name", "shortname", "type",world.Name, "content", "authors", "description",
            "goals", 1, 2);
        
        var systemUnderTest = CreateTestableLearningElementMapper();

        var viewModel = systemUnderTest.ToViewModel(entity, world);
        Assert.That(viewModel, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(viewModel.Name, Is.EqualTo(entity.Name));
            Assert.That(viewModel.Shortname, Is.EqualTo(entity.Shortname));
            Assert.That(viewModel.Parent, Is.EqualTo(world));
            Assert.That(viewModel.Type, Is.EqualTo(entity.Type));
            Assert.That(viewModel.Content, Is.EqualTo(entity.Content));
            Assert.That(viewModel.Authors, Is.EqualTo(entity.Authors));
            Assert.That(viewModel.Description, Is.EqualTo(entity.Description));
            Assert.That(viewModel.Goals, Is.EqualTo(entity.Goals));
            Assert.That(viewModel.PositionX, Is.EqualTo(entity.PositionX));
            Assert.That(viewModel.PositionY, Is.EqualTo(entity.PositionY));
        });
    }

    [Test]
    public void LearningElementMapper_ToViewModel_LogsSanityCheck()
    {
        var world = new LearningWorldViewModel("bubu", "", "", "", "", "");
        var entity = new AuthoringTool.Entities.LearningElement("name", "shortname", "type","blabla", "content", "authors", "description",
            "goals", 1, 2);
        var logger = Substitute.For<ILogger<LearningElementMapper>>();
        
        var systemUnderTest = CreateTestableLearningElementMapper(logger);

        var viewModel = systemUnderTest.ToViewModel(entity, world);
        Assert.That(viewModel, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(viewModel.Name, Is.EqualTo(entity.Name));
            Assert.That(viewModel.Shortname, Is.EqualTo(entity.Shortname));
            Assert.That(viewModel.Parent, Is.EqualTo(world));
            Assert.That(viewModel.Type, Is.EqualTo(entity.Type));
            Assert.That(viewModel.Content, Is.EqualTo(entity.Content));
            Assert.That(viewModel.Authors, Is.EqualTo(entity.Authors));
            Assert.That(viewModel.Description, Is.EqualTo(entity.Description));
            Assert.That(viewModel.Goals, Is.EqualTo(entity.Goals));
            Assert.That(viewModel.PositionX, Is.EqualTo(entity.PositionX));
            Assert.That(viewModel.PositionY, Is.EqualTo(entity.PositionY));
        });

        logger.Received().LogError("caller was not null but caller.Name != entity.ParentName: bubu!=blabla");
    }

    private LearningElementMapper CreateTestableLearningElementMapper(ILogger<LearningElementMapper>? logger = null)
    {
        logger ??= Substitute.For<ILogger<LearningElementMapper>>();
        return new LearningElementMapper(logger);
    }
}