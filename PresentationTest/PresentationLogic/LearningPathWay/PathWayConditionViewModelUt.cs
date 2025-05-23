using System.Collections.Generic;
using NUnit.Framework;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Shared;
using Shared.Theme;

namespace PresentationTest.PresentationLogic.LearningPathWay;

[TestFixture]
public class PathWayConditionViewModelUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var positionX = 20;
        var positionY = 30;
        var condition = ConditionEnum.And;
        var inBoundCondition = new PathWayConditionViewModel(ConditionEnum.And, false, 2, 3);
        var outBoundSpace = new LearningSpaceViewModel("a", "b", SpaceTheme.LearningArea, 3);
        var inBoundObjects = new List<IObjectInPathWayViewModel> { inBoundCondition };
        var outBoundObjects = new List<IObjectInPathWayViewModel> { outBoundSpace };

        var systemUnderTest =
            new PathWayConditionViewModel(condition, false, positionX, positionY, inBoundObjects, outBoundObjects);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Condition, Is.EqualTo(condition));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
            Assert.That(systemUnderTest.InBoundObjects, Is.EqualTo(inBoundObjects));
            Assert.That(systemUnderTest.OutBoundObjects, Is.EqualTo(outBoundObjects));
            Assert.That(systemUnderTest.InputConnectionX,
                Is.EqualTo(positionX + PathWayConditionViewModel.InputConnectionXOffset));
            Assert.That(systemUnderTest.InputConnectionY,
                Is.EqualTo(positionY + PathWayConditionViewModel.InputConnectionYOffset));
            Assert.That(systemUnderTest.OutputConnectionX,
                Is.EqualTo(positionX + PathWayConditionViewModel.OutputConnectionXOffset));
            Assert.That(systemUnderTest.OutputConnectionY,
                Is.EqualTo(positionY + PathWayConditionViewModel.OutputConnectionYOffset));
        });
    }
}