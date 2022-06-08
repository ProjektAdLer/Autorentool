using AuthoringTool.PresentationLogic;
using AuthoringTool.PresentationLogic.EntityMapping;
using AuthoringTool.PresentationLogic.LearningContent;
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
        
        spaceMapper.ToEntity(space).Returns(new AuthoringTool.Entities.LearningSpace("b", "b", "b", "b", "b"));

        var systemUnderTest = CreateMapperForTesting(spaceMapper);

        systemUnderTest.ToEntity(viewModel);
        spaceMapper.Received().ToEntity(space);
    }

    [Test]
    public void LearningWorldMapper_ToEntity_CallsElementMapperForElements()
    {
        var elementMapper = Substitute.For<ILearningElementMapper>();
        var viewModel = new LearningWorldViewModel("a", "b", "c", "d", "e", "f");
        var content = new LearningContentViewModel("z", "e", new byte[]{0x05,0x01});
        var element = new LearningElementViewModel("a", "a", null, content, "a" , "a", "a");
        viewModel.LearningElements.Add(element);

        elementMapper.ToEntity(element).Returns(new AuthoringTool.Entities.LearningElement("a","b",null, null,"g","h","i"));

        var systemUnderTest = CreateMapperForTesting(elementMapper: elementMapper);

        systemUnderTest.ToEntity(viewModel);
        elementMapper.Received().ToEntity(element);
    }

    [Test]
    public void LearningWorldMapper_ToEntity_MapsPropertiesCorrectly()
    {
        var subElementMapper = Substitute.For<ILearningElementMapper>();
        var spaceMapper = new LearningSpaceMapper(subElementMapper);
        var viewModel = new LearningWorldViewModel("name", "shortname", "authors", "language",
            "description", "goals");

        var systemUnderTest = CreateMapperForTesting(spaceMapper);

        var entity = systemUnderTest.ToEntity(viewModel);
        Assert.That(entity, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(entity.Name, Is.EqualTo(viewModel.Name));
            Assert.That(entity.Shortname, Is.EqualTo(viewModel.Shortname));
            Assert.That(entity.Authors, Is.EqualTo(viewModel.Authors));
            Assert.That(entity.Language, Is.EqualTo(viewModel.Language));
            Assert.That(entity.Description, Is.EqualTo(viewModel.Description));
            Assert.That(entity.Goals, Is.EqualTo(viewModel.Goals));
        });
    }

    [Test]
    public void LearningWorldMapper_ToViewModel_CallsSpaceMapperForSpaces()
    {
        var spaceMapper = Substitute.For<ILearningSpaceMapper>();
        var entity = new AuthoringTool.Entities.LearningWorld("a", "b", "c", "d", "e", "f");
        var space = new AuthoringTool.Entities.LearningSpace("b", "b", "b", "b", "b");
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
        var element = new AuthoringTool.Entities.LearningElement("a","b","e",null, "f","nll","g");
      
        entity.LearningElements.Add(element);

        elementMapper.ToViewModel(element).Returns(new LearningElementViewModel("a", "a", null, null,
            "a", "a", "a"));

        var systemUnderTest = CreateMapperForTesting(elementMapper: elementMapper);

        systemUnderTest.ToViewModel(entity);
        elementMapper.Received().ToViewModel(element, Arg.Any<ILearningElementViewModelParent?>());
    }

    [Test]
    public void LearningWorldMapper_ToViewModel_MapsPropertiesCorrectly()
    {
        var subElementMapper = Substitute.For<ILearningElementMapper>();
        var spaceMapper = new LearningSpaceMapper(subElementMapper);
        var entity = new AuthoringTool.Entities.LearningWorld("name", "shortname", "authors", "language",
            "description", "goals");

        var systemUnderTest = CreateMapperForTesting(spaceMapper);

        var viewModel = systemUnderTest.ToViewModel(entity);
        Assert.That(viewModel, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(viewModel.Name, Is.EqualTo(entity.Name));
            Assert.That(viewModel.Shortname, Is.EqualTo(entity.Shortname));
            Assert.That(viewModel.Authors, Is.EqualTo(entity.Authors));
            Assert.That(viewModel.Language, Is.EqualTo(entity.Language));
            Assert.That(viewModel.Description, Is.EqualTo(entity.Description));
            Assert.That(viewModel.UnsavedChanges, Is.False);
            Assert.That(viewModel.Goals, Is.EqualTo(entity.Goals));
        });
    }

    private LearningWorldMapper CreateMapperForTesting(ILearningSpaceMapper? spaceMapper = null,
        ILearningElementMapper? elementMapper = null)
    {
        spaceMapper ??= Substitute.For<ILearningSpaceMapper>();
        elementMapper ??= Substitute.For<ILearningElementMapper>();
        return new LearningWorldMapper(spaceMapper, elementMapper);
    }
}