using System;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.EntityMapping.LearningElementMapper;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement.ActivationElement;
using Presentation.PresentationLogic.LearningWorld;
using Shared;

namespace PresentationTest.PresentationLogic.EntityMapping.LearningElement;

[TestFixture]

public class VideoActivationElementMapperUt
{
    [Test]
    public void VideoActivationElementMapper_ToEntity_MapsPropertiesCorrectly()
    {
        var contentMapper = Substitute.For<ILearningContentMapper>();
        var contentViewModel = new LearningContentViewModel("a", "b", Array.Empty<byte>());
        var world = new LearningWorldViewModel("baba", "bubu", "", "", "", "");
        var viewModel = new  VideoActivationElementViewModel("name", "shortname", world, contentViewModel,"authors",
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
    public void VideoActivationElementMapper_ToViewModel_MapsPropertiesCorrectly()
    {
        var contentMapper = Substitute.For<ILearningContentMapper>();
        var world = new LearningWorldViewModel("bubu", "", "", "", "", "");
        var content = new LearningContent("content", "pdf", Array.Empty<byte>());
        var entity = new  VideoActivationElement("name", "shortname",world.Name, content,"authors", "description",
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
    
    private VideoActivationElementMapper CreateTestableLearningElementMapper(ILearningContentMapper? contentMapper = null)
    {
        contentMapper ??= Substitute.For<ILearningContentMapper>();
        return new VideoActivationElementMapper(contentMapper);
    }
}