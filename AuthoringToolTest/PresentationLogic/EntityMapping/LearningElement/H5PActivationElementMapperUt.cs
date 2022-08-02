using System;
using AuthoringTool.Entities;
using AuthoringTool.PresentationLogic.EntityMapping.LearningElementMapper;
using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningElement.ActivationElement;
using AuthoringTool.PresentationLogic.LearningWorld;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.EntityMapping.LearningElement;

[TestFixture]

public class H5PActivationElementMapperUt
{
    [Test]
    public void H5PActivationElementMapper_ToEntity_MapsPropertiesCorrectly()
    {
        var contentMapper = Substitute.For<ILearningContentMapper>();
        var contentViewModel = new LearningContentViewModel("a", "b", Array.Empty<byte>());
        var world = new LearningWorldViewModel("baba", "bubu", "", "", "", "");
        var viewModel = new H5PActivationElementViewModel("name", "shortname", world, contentViewModel,"authors",
            "description", "goals",LearningElementDifficultyEnum.Easy, 1, 2,3);

        contentMapper.ToEntity(contentViewModel).Returns(new LearningContent("a", "b", Array.Empty<byte>()));
        
        var systemUnderTest = CreateTestableLearningElementMapper(contentMapper);
        
        var entity = systemUnderTest.ToEntity(viewModel);
        Assert.That(entity, Is.Not.Null);
        contentMapper.Received().ToEntity(contentViewModel);
        Assert.Multiple(() =>
        {
            Assert.That(entity.Name, Is.EqualTo(viewModel.Name));
            Assert.That(entity.Shortname, Is.EqualTo(viewModel.Shortname));
            Assert.That(entity.ParentName, Is.EqualTo(viewModel.Parent!.Name));
            Assert.That(entity.Authors, Is.EqualTo(viewModel.Authors));
            Assert.That(entity.Description, Is.EqualTo(viewModel.Description));
            Assert.That(entity.Goals, Is.EqualTo(viewModel.Goals));
            Assert.That(entity.Difficulty, Is.EqualTo(viewModel.Difficulty));
            Assert.That(entity.Workload, Is.EqualTo(viewModel.Workload));
            Assert.That(entity.PositionX, Is.EqualTo(viewModel.PositionX));
            Assert.That(entity.PositionY, Is.EqualTo(viewModel.PositionY));
        });
    }

    [Test]
    public void H5PActivationElementMapper_ToViewModel_MapsPropertiesCorrectly()
    {
        var contentMapper = Substitute.For<ILearningContentMapper>();
        var world = new LearningWorldViewModel("bubu", "", "", "", "", "");
        var content = new LearningContent("content", "pdf", Array.Empty<byte>());
        var entity = new H5PActivationElement("name", "shortname",world.Name, content,"authors", "description",
            "goals",LearningElementDifficultyEnum.Easy, 1, 3, 2);
        
        var systemUnderTest = CreateTestableLearningElementMapper(contentMapper);

        contentMapper.ToViewModel(content).Returns(new LearningContentViewModel("content", "pdf", Array.Empty<byte>()));

        var viewModel = systemUnderTest.ToViewModel(entity, world);
        Assert.That(viewModel, Is.Not.Null);
        contentMapper.Received().ToViewModel(content);
        Assert.Multiple(() =>
        {
            Assert.That(viewModel.Name, Is.EqualTo(entity.Name));
            Assert.That(viewModel.Shortname, Is.EqualTo(entity.Shortname));
            Assert.That(viewModel.Parent, Is.EqualTo(world));
            Assert.That(viewModel.LearningContent, Is.Not.Null);
            Assert.That(viewModel.LearningContent?.Name, Is.EqualTo(entity.Content.Name));
            Assert.That(viewModel.LearningContent?.Type, Is.EqualTo(entity.Content.Type));
            Assert.That(viewModel.LearningContent?.Content, Is.EqualTo(entity.Content.Content));
            Assert.That(viewModel.Authors, Is.EqualTo(entity.Authors));
            Assert.That(viewModel.Description, Is.EqualTo(entity.Description));
            Assert.That(viewModel.Goals, Is.EqualTo(entity.Goals));
            Assert.That(viewModel.Difficulty, Is.EqualTo(entity.Difficulty));
            Assert.That(viewModel.Workload, Is.EqualTo(entity.Workload));
            Assert.That(viewModel.PositionX, Is.EqualTo(entity.PositionX));
            Assert.That(viewModel.PositionY, Is.EqualTo(entity.PositionY));
        });
    }
    
    private H5PActivationElementMapper CreateTestableLearningElementMapper(ILearningContentMapper? contentMapper = null)
    {
        contentMapper ??= Substitute.For<ILearningContentMapper>();
        return new H5PActivationElementMapper(contentMapper);
    }
}