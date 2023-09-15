using AuthoringTool.Mapping;
using AutoMapper;
using BusinessLogic.Entities.LearningContent.AdaptivityContent;
using NUnit.Framework;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent;
using TestHelpers;

namespace BusinessLogicTest.Entities.LearningContent.AdaptivityContent;

[TestFixture]
public class AdaptivityContentIt
{
    [Test]
    public void TwoAdaptivityContents_SameStructure_DifferentObjects_EqualsTrue()
    {
        var mapper = new MapperConfiguration(cfg =>
        {
            ViewModelEntityMappingProfile.Configure(cfg);
            cfg.AddCollectionMappersOnce();
        }).CreateMapper();
        
        var content = EntityProvider.GetAdaptivityContent();
        //use automapper to make a perfect copy of the object
        var contentvm = mapper.Map<IAdaptivityContentViewModel>(content);
        var content2 = mapper.Map<IAdaptivityContent>(contentvm);

        Assert.Multiple(() =>
        {
            Assert.That(content, Is.EqualTo(content2));
            Assert.That(ReferenceEquals(content, content2), Is.False);
        });
    }
}