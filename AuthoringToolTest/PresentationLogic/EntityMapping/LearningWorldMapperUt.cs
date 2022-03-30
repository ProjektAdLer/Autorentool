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
public class LearningWorldMapperUt
{
    [Test]
    public void LearningWorldMapper_ToEntity_CallsSpaceMapperForSpaces()
    {
        var spaceMapper = Substitute.For<ILearningSpaceMapper>();
        var viewModel = new LearningWorldViewModel("a", "b", "c", "d", "e", "f");
        var space = new LearningSpaceViewModel("b", "b", "b", "b", "b");
        viewModel.LearningSpaces.Add(space);

        spaceMapper.ToEntity(space).Returns(new LearningSpace("b", "b", "b", "b", "b"));

        var systemUnderTest = CreateMapperForTesting(spaceMapper);

        systemUnderTest.ToEntity(viewModel);
        spaceMapper.Received().ToEntity(space);
    }

    [Test]
    public void LearningWorldMapper_ToEntity_CallsElementMapperForElements()
    {
        var elementMapper = Substitute.For<ILearningElementMapper>();
        var viewModel = new LearningWorldViewModel("a", "b", "c", "d", "e", "f");
        var element = new LearningElementViewModel("a", "a", "a", "a" ,"a", "a", "a");
        viewModel.LearningElements.Add(element);

        elementMapper.ToEntity(element).Returns(new LearningElement("a","b","c","d","e","f","g"));

        var systemUnderTest = CreateMapperForTesting(elementMapper: elementMapper);

        systemUnderTest.ToEntity(viewModel);
        elementMapper.Received().ToEntity(element);
    }

    [Test]
    public void LearningWorldMapper_ToEntity_MapsPropertiesCorrectly()
    {
        var subElementMapper = Substitute.For<ILearningElementMapper>();
        var spaceMapper = new LearningSpaceMapper(subElementMapper);
        var elementMapper = new LearningElementMapper();
        var viewModel = new LearningWorldViewModel("name", "shortname", "authors", "language",
            "description", "goals");

        var systemUnderTest = CreateMapperForTesting(spaceMapper, elementMapper);

        var entity = systemUnderTest.ToEntity(viewModel);
        Assert.IsNotNull(entity);
        Assert.AreEqual(viewModel.Name, entity.Name);
        Assert.AreEqual(viewModel.Shortname, entity.Shortname);
        Assert.AreEqual(viewModel.Authors, entity.Authors);
        Assert.AreEqual(viewModel.Language, entity.Language);
        Assert.AreEqual(viewModel.Description, entity.Description);
        Assert.AreEqual(viewModel.Goals, entity.Goals);
    }

    [Test]
    public void LearningWorldMapper_ToViewModel_CallsSpaceMapperForSpaces()
    {
        var spaceMapper = Substitute.For<ILearningSpaceMapper>();
        var entity = new AuthoringTool.Entities.LearningWorld("a", "b", "c", "d", "e", "f");
        var space = new LearningSpace("b", "b", "b", "b", "b");
        entity.LearningSpaces.Add(space);

        spaceMapper.ToViewModel(space).Returns(
            new LearningSpaceViewModel("a", "b", "c", "d", "e"));

        var systemUnderTest = CreateMapperForTesting(spaceMapper);

        systemUnderTest.ToViewModel(entity);
        spaceMapper.Received().ToViewModel(space);
    }

    [Test]
    public void LearningWorldMapper_ToViewModel_CallsElementMapperForElements()
    {
        var elementMapper = Substitute.For<ILearningElementMapper>();
        var entity = new AuthoringTool.Entities.LearningWorld("a", "b", "c", "d", "e", "f");
        var element = new LearningElement("a","b","c","d","e","f","g");
      
        entity.LearningElements.Add(element);

        elementMapper.ToViewModel(element).Returns(new LearningElementViewModel("a", "a", "a",
            "a", "a","a", "a"));

        var systemUnderTest = CreateMapperForTesting(elementMapper: elementMapper);

        systemUnderTest.ToViewModel(entity);
        elementMapper.Received().ToViewModel(element);
    }

    [Test]
    public void LearningWorldMapper_ToViewModel_MapsPropertiesCorrectly()
    {
        var subElementMapper = Substitute.For<ILearningElementMapper>();
        var spaceMapper = new LearningSpaceMapper(subElementMapper);
        var elementMapper = new LearningElementMapper();
        var entity = new AuthoringTool.Entities.LearningWorld("name", "shortname", "authors", "language",
            "description", "goals");

        var systemUnderTest = CreateMapperForTesting(spaceMapper, elementMapper);

        var viewModel = systemUnderTest.ToViewModel(entity);
        Assert.IsNotNull(entity);
        Assert.AreEqual(entity.Name, viewModel.Name);
        Assert.AreEqual(entity.Shortname, viewModel.Shortname);
        Assert.AreEqual(entity.Authors, viewModel.Authors);
        Assert.AreEqual(entity.Language, viewModel.Language);
        Assert.AreEqual(entity.Description, viewModel.Description);
        Assert.AreEqual(entity.Goals, viewModel.Goals);
    }

    private LearningWorldMapper CreateMapperForTesting(ILearningSpaceMapper? spaceMapper = null,
        ILearningElementMapper? elementMapper = null)
    {
        spaceMapper ??= Substitute.For<ILearningSpaceMapper>();
        elementMapper ??= Substitute.For<ILearningElementMapper>();
        return new LearningWorldMapper(spaceMapper, elementMapper);
    }
}