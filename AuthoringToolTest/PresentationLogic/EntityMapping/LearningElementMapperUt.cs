using AuthoringTool.Entities;
using AuthoringTool.PresentationLogic.EntityMapping;
using AuthoringTool.PresentationLogic.LearningElement;
using NSubstitute;
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
        Assert.IsNotNull(entity);
        Assert.AreEqual(viewModel.Name, entity.Name);
        Assert.AreEqual(viewModel.Shortname, entity.Shortname);
        Assert.AreEqual(viewModel.Type, entity.Type);
        Assert.AreEqual(viewModel.Content, entity.Content);
        Assert.AreEqual(viewModel.Authors, entity.Authors);
        Assert.AreEqual(viewModel.Description, entity.Description);
        Assert.AreEqual(viewModel.Goals, entity.Goals);
        Assert.AreEqual(viewModel.PositionX, entity.PositionX);
        Assert.AreEqual(viewModel.PositionY, entity.PositionY);
    }
    
    [Test]
    public void LearningWorldMapper_ToViewModel_MapsPropertiesCorrectly()
    {
        var elementMapper = new LearningElementMapper();
        var entity = new LearningElement("name", "shortname", "type","content",
            "authors", "description", "goals", 1, 2);

        var viewModel = elementMapper.ToViewModel(entity);
        Assert.IsNotNull(entity);
        Assert.AreEqual(entity.Name, viewModel.Name);
        Assert.AreEqual(entity.Shortname, viewModel.Shortname);
        Assert.AreEqual(entity.Type, viewModel.Type);
        Assert.AreEqual(entity.Content, viewModel.Content);
        Assert.AreEqual(entity.Authors, viewModel.Authors);
        Assert.AreEqual(entity.Description, viewModel.Description);
        Assert.AreEqual(entity.Goals, viewModel.Goals);
        Assert.AreEqual(entity.PositionX, viewModel.PositionX);
        Assert.AreEqual(entity.PositionY, viewModel.PositionY);
    }
}