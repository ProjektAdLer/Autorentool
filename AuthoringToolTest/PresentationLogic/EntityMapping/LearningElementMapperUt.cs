using AuthoringTool.PresentationLogic.EntityMapping;
using AuthoringTool.PresentationLogic.LearningElement;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.EntityMapping;

[TestFixture]

public class LearningElementMapperUt
{
    [Test]
    public void LearningSpaceMapper_ToEntity_MapsPropertiesCorrectly()
    {
        var elementMapper = new LearningElementMapper();
        var viewModel = new LearningElementViewModel("name", "shortname", "type", "content",
            "authors", "description", "goals", 1, 2);
        
        var entity = elementMapper.ToEntity(viewModel);
        Assert.That(entity, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(entity.Name, Is.EqualTo(viewModel.Name));
            Assert.That(entity.Shortname, Is.EqualTo(viewModel.Shortname));
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
    public void LearningWorldMapper_ToViewModel_MapsPropertiesCorrectly()
    {
        var elementMapper = new LearningElementMapper();
        var entity = new AuthoringTool.Entities.LearningElement("name", "shortname", "type","content",
            "authors", "description", "goals", 1, 2);

        var viewModel = elementMapper.ToViewModel(entity);
        Assert.That(viewModel, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(viewModel.Name, Is.EqualTo(entity.Name));
            Assert.That(viewModel.Shortname, Is.EqualTo(entity.Shortname));
            Assert.That(viewModel.Type, Is.EqualTo(entity.Type));
            Assert.That(viewModel.Content, Is.EqualTo(entity.Content));
            Assert.That(viewModel.Authors, Is.EqualTo(entity.Authors));
            Assert.That(viewModel.Description, Is.EqualTo(entity.Description));
            Assert.That(viewModel.Goals, Is.EqualTo(entity.Goals));
            Assert.That(viewModel.PositionX, Is.EqualTo(entity.PositionX));
            Assert.That(viewModel.PositionY, Is.EqualTo(entity.PositionY));
        });
    }
}