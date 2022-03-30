using System.Linq;
using AuthoringTool.Entities;
using AuthoringTool.PresentationLogic.EntityMapping;
using AuthoringTool.PresentationLogic.LearningElement;
using AuthoringTool.PresentationLogic.LearningSpace;
using AuthoringTool.PresentationLogic.LearningWorld;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.EntityMapping;

[TestFixture]
public class LearningSpaceMapperUt
{
    [Test]
    public void LearningSpaceMapper_ToEntity_CallsElementMapperForElements()
    {
        var elementMapper = Substitute.For<ILearningElementMapper>();
        var viewModel = new LearningSpaceViewModel("a", "b", "c", "d", "e");
        var element = new LearningElementViewModel("a", "a", "a", "a", "a");
        viewModel.LearningElements.Add(element);

        elementMapper.ToEntity(element).Returns(new LearningElement());

        var systemUnderTest = CreateMapperForTesting(elementMapper: elementMapper);

        systemUnderTest.ToEntity(viewModel);
        elementMapper.Received().ToEntity(element);
    }

    [Test]
    public void LearningSpaceMapper_ToEntity_MapsPropertiesCorrectly()
    {
        var elementMapper = new LearningElementMapper();
        var viewModel = new LearningSpaceViewModel("name", "shortname", "authors", "description", "goals", null, 1, 2);

        var systemUnderTest = CreateMapperForTesting(elementMapper);

        var entity = systemUnderTest.ToEntity(viewModel);
        Assert.IsNotNull(entity);
        Assert.AreEqual(viewModel.Name, entity.Name);
        Assert.AreEqual(viewModel.Shortname, entity.Shortname);
        Assert.AreEqual(viewModel.Authors, entity.Authors);
        Assert.AreEqual(viewModel.Description, entity.Description);
        Assert.AreEqual(viewModel.Goals, entity.Goals);
        Assert.AreEqual(viewModel.PositionX, entity.PositionX);
        Assert.AreEqual(viewModel.PositionY, entity.PositionY);
    }

    [Test]
    public void LearningSpaceMapper_ToViewModel_CallsElementMapperForElements()
    {
        var elementMapper = Substitute.For<ILearningElementMapper>();
        var entity = new LearningSpace("a", "b", "c", "d", "e");
        var element = new LearningElement();
        entity.LearningElements.Add(element);

        elementMapper.ToViewModel(element).Returns(new LearningElementViewModel("a", "a", "a",
            "a", "a"));

        var systemUnderTest = CreateMapperForTesting(elementMapper: elementMapper);

        systemUnderTest.ToViewModel(entity);
        elementMapper.Received().ToViewModel(element);
    }

    [Test]
    public void LearningWorldMapper_ToViewModel_MapsPropertiesCorrectly()
    {
        var elementMapper = new LearningElementMapper();
        var entity = new LearningSpace("name", "shortname", "authors", "description", "goals", null, 1, 2);

        var systemUnderTest = CreateMapperForTesting(elementMapper);

        var viewModel = systemUnderTest.ToViewModel(entity);
        Assert.IsNotNull(entity);
        Assert.AreEqual(entity.Name, viewModel.Name);
        Assert.AreEqual(entity.Shortname, viewModel.Shortname);
        Assert.AreEqual(entity.Authors, viewModel.Authors);
        Assert.AreEqual(entity.Description, viewModel.Description);
        Assert.AreEqual(entity.Goals, viewModel.Goals);
        Assert.AreEqual(entity.PositionX, viewModel.PositionX);
        Assert.AreEqual(entity.PositionY, viewModel.PositionY);
    }

    private LearningSpaceMapper CreateMapperForTesting(ILearningElementMapper? elementMapper = null)
    {
        elementMapper ??= Substitute.For<ILearningElementMapper>();
        return new LearningSpaceMapper(elementMapper);
    }
}