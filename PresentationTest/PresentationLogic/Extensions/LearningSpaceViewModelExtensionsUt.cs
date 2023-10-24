using NUnit.Framework;
using NSubstitute;
using System.Collections.Generic;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.Extensions;
using Presentation.PresentationLogic.LearningSpace;

namespace PresentationTest.PresentationLogic.Extensions;
    

[TestFixture]
public class LearningSpaceViewModelExtensionsUt
{
    [Test]
    public void GetFollowingSpaces_ReturnsAllReachableSpaces()
    {
        // Arrange
        var initialSpace = Substitute.For<ILearningSpaceViewModel>();
        var objInPathway1 = Substitute.For<IObjectInPathWayViewModel>();
        var objInPathway2 = Substitute.For<IObjectInPathWayViewModel>();
        var followingSpace1 = Substitute.For<ILearningSpaceViewModel>();
        var followingSpace2 = Substitute.For<ILearningSpaceViewModel>();

        initialSpace.OutBoundObjects.Returns(new[] { objInPathway1, objInPathway2 });
        objInPathway1.OutBoundObjects.Returns(new[] { followingSpace1 });
        objInPathway2.OutBoundObjects.Returns(new[] { followingSpace2 });

        // Act
        var result = initialSpace.GetFollowingSpaces();

        // Assert
        Assert.That(result, Is.EquivalentTo(new List<ILearningSpaceViewModel> { followingSpace1, followingSpace2 }));
    }

    [Test]
    public void GetFollowingSpaces_NoSpacesFollowing_ReturnsEmptyList()
    {
        // Arrange
        var initialSpace = Substitute.For<ILearningSpaceViewModel>();
        initialSpace.OutBoundObjects.Returns(new List<IObjectInPathWayViewModel>());

        // Act
        var result = initialSpace.GetFollowingSpaces();

        // Assert
        Assert.That(result, Is.Empty);
    }
}