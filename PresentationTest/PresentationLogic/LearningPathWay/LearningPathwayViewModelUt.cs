using NUnit.Framework;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Shared;

namespace PresentationTest.PresentationLogic.LearningPathWay;

[TestFixture]
public class LearningPathwayViewModelUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var sourceCondition = new PathWayConditionViewModel(ConditionEnum.And, false, 2, 3);
        var targetSpace = new LearningSpaceViewModel("a", "b", "t", Theme.CampusAschaffenburg, 3);

        var systemUnderTest = new LearningPathwayViewModel(sourceCondition, targetSpace);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.SourceObject, Is.EqualTo(sourceCondition));
            Assert.That(systemUnderTest.TargetObject, Is.EqualTo(targetSpace));
        });
    }
}