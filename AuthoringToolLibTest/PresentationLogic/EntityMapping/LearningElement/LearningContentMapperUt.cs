using AuthoringToolLib.Entities;
using AuthoringToolLib.PresentationLogic.EntityMapping.LearningElementMapper;
using AuthoringToolLib.PresentationLogic.LearningContent;
using NUnit.Framework;

namespace AuthoringToolLibTest.PresentationLogic.EntityMapping.LearningElement;

[TestFixture]

public class LearningContentMapperUt
{
    [Test]
    public void LearningContentMapper_ToEntity_MapsPropertiesCorrectly()
    {
        var viewModel = new LearningContentViewModel("name", "type", new byte[] {0x01, 0x02, 0x03});
        var systemUnderTest = new LearningContentMapper();

        var entity = systemUnderTest.ToEntity(viewModel);
        Assert.That(entity, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(entity.Name, Is.EqualTo(viewModel.Name));
            Assert.That(entity.Type, Is.EqualTo(viewModel.Type));
            Assert.That(entity.Content, Is.EqualTo(viewModel.Content));
        });
    }

    [Test]
    public void LearningContentMapper_ToViewModel_MapsPropertiesCorrectly()
    {
        var entity = new LearningContent("name", "type", new byte[] {0x03, 0x02, 0x01});
        var systemUnderTest = new LearningContentMapper();

        var viewModel = systemUnderTest.ToViewModel(entity);
        Assert.That(viewModel, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(viewModel.Name, Is.EqualTo(entity.Name));
            Assert.That(viewModel.Type, Is.EqualTo(entity.Type));
            Assert.That(viewModel.Content, Is.EqualTo(entity.Content));
        });
    }
}