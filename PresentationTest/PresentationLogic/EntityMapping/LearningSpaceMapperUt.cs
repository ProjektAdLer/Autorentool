using NSubstitute;
using NUnit.Framework;

namespace PresentationTest.PresentationLogic.EntityMapping;

[TestFixture]
public class LearningSpaceMapperUt
{
    [Test]
    public void LearningSpaceMapper_ToEntity_CallsElementMapperForElements()
    {
        var elementMapper = Substitute.For<ILearningElementMapper>();
        var spaceViewModel = new LearningSpaceViewModel("a", "b", "c", "d", "e");
        var content = new LearningContent("bar", "foo", new byte[] {0x01, 0x02});
        var contentvm = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var elementViewModel = new LearningElementViewModel("a", "a", spaceViewModel, contentvm,"a", "a", "a", LearningElementDifficultyEnum.Easy);
        spaceViewModel.LearningElements.Add(elementViewModel);

        elementMapper.ToEntity(elementViewModel).Returns(new AuthoringToolLib.Entities.LearningElement( "a", "a","a", content, "a", "a", "a",LearningElementDifficultyEnum.Easy));

        var systemUnderTest = CreateMapperForTesting(elementMapper: elementMapper);

        systemUnderTest.ToEntity(spaceViewModel);
        elementMapper.Received().ToEntity(elementViewModel);
    }

    [Test]
    public void LearningSpaceMapper_ToEntity_MapsPropertiesCorrectly()
    {
        var viewModel = new LearningSpaceViewModel("name", "shortname", "authors", "description", "goals", null, 1, 2);

        var systemUnderTest = CreateMapperForTesting();

        var entity = systemUnderTest.ToEntity(viewModel);
        Assert.That(entity, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(entity.Name, Is.EqualTo(viewModel.Name));
            Assert.That(entity.Shortname, Is.EqualTo(viewModel.Shortname));
            Assert.That(entity.Authors, Is.EqualTo(viewModel.Authors));
            Assert.That(entity.Description, Is.EqualTo(viewModel.Description));
            Assert.That(entity.Goals, Is.EqualTo(viewModel.Goals));
            Assert.That(entity.PositionX, Is.EqualTo(viewModel.PositionX));
            Assert.That(entity.PositionY, Is.EqualTo(viewModel.PositionY));
        });
    }

    [Test]
    public void LearningSpaceMapper_ToViewModel_CallsElementMapperForElements()
    {
        var elementMapper = Substitute.For<ILearningElementMapper>();
        var entity = new AuthoringToolLib.Entities.LearningSpace("a", "b", "c", "d", "e");
        var element = new AuthoringToolLib.Entities.LearningElement("a", "a", "a", null, "a", "a", "a",LearningElementDifficultyEnum.Easy);
        entity.LearningElements.Add(element);

        elementMapper.ToViewModel(element).Returns(new LearningElementViewModel("a", "a", null, null,"a","a", "a",LearningElementDifficultyEnum.Easy));

        var systemUnderTest = CreateMapperForTesting(elementMapper: elementMapper);

        systemUnderTest.ToViewModel(entity);
        elementMapper.Received().ToViewModel(element, Arg.Any<ILearningElementViewModelParent?>());
    }

    [Test]
    public void LearningWorldMapper_ToViewModel_MapsPropertiesCorrectly()
    {
        var entity = new AuthoringToolLib.Entities.LearningSpace("name", "shortname", "authors", "description", "goals", null, 1, 2);

        var systemUnderTest = CreateMapperForTesting();

        var viewModel = systemUnderTest.ToViewModel(entity);
        Assert.That(viewModel, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(viewModel.Name, Is.EqualTo(entity.Name));
            Assert.That(viewModel.Shortname, Is.EqualTo(entity.Shortname));
            Assert.That(viewModel.Authors, Is.EqualTo(entity.Authors));
            Assert.That(viewModel.Description, Is.EqualTo(entity.Description));
            Assert.That(viewModel.Goals, Is.EqualTo(entity.Goals));
            Assert.That(viewModel.PositionX, Is.EqualTo(entity.PositionX));
            Assert.That(viewModel.PositionY, Is.EqualTo(entity.PositionY));
        });
    }

    private LearningSpaceMapper CreateMapperForTesting(ILearningElementMapper? elementMapper = null)
    {
        elementMapper ??= Substitute.For<ILearningElementMapper>();
        return new LearningSpaceMapper(elementMapper);
    }
}