using NUnit.Framework;
using Presentation.PresentationLogic.PathWay;
using Presentation.PresentationLogic.Space;
using Shared;

namespace PresentationTest.PresentationLogic.PathWay;

[TestFixture]
public class PathwayViewModelUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        
        var sourceCondition = new PathWayConditionViewModel(ConditionEnum.And, 2, 3);
        var targetSpace = new SpaceViewModel("a", "z", "d", "b", "t", 3);

        var systemUnderTest = new PathwayViewModel(sourceCondition, targetSpace);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.SourceObject, Is.EqualTo(sourceCondition));
            Assert.That(systemUnderTest.TargetObject, Is.EqualTo(targetSpace));
        });
    }
}