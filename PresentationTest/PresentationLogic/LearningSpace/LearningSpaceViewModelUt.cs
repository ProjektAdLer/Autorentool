using System.Collections.Generic;
using NUnit.Framework;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningSpace.SpaceLayout;
using Shared;
using TestHelpers;

namespace PresentationTest.PresentationLogic.LearningSpace;

[TestFixture]
public class LearningSpaceViewModelUt
{
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var name = "asdf";
        var description = "very cool element";
        var goals = "learn very many things";
        var requiredPoints = 10;
        var positionX = 20;
        var positionY = 30;
        var topic = new Presentation.PresentationLogic.Topic.TopicViewModel("topic1");
        var ele1 = ViewModelProvider.GetLearningElement();
        var ele2 = ViewModelProvider.GetLearningElement("2");
        var inBoundCondition = new PathWayConditionViewModel(ConditionEnum.And, false, 2, 3);
        var outBoundSpace = new LearningSpaceViewModel("a", "b", "t", Theme.CampusAschaffenburg, 3);
        var inBoundObjects = new List<IObjectInPathWayViewModel> { inBoundCondition };
        var outBoundObjects = new List<IObjectInPathWayViewModel> { outBoundSpace };
        var learningElements = new Dictionary<int, ILearningElementViewModel>
        {
            {
                0, ele1
            },
            {
                1, ele2
            }
        };
        var learningSpaceLayoutVm = new LearningSpaceLayoutViewModel(FloorPlanEnum.R_20X20_6L)
        {
            LearningElements = learningElements
        };

        var systemUnderTest = new LearningSpaceViewModel(name, description, goals, Theme.CampusAschaffenburg,
            requiredPoints,
            learningSpaceLayoutVm, positionX: positionX, positionY: positionY, inBoundObjects: inBoundObjects,
            outBoundObjects: outBoundObjects, assignedTopic: topic);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.ContainedLearningElements, Is.EqualTo(learningElements.Values));
            Assert.That(systemUnderTest.PositionX, Is.EqualTo(positionX));
            Assert.That(systemUnderTest.PositionY, Is.EqualTo(positionY));
            Assert.That(systemUnderTest.InBoundObjects, Is.EqualTo(inBoundObjects));
            Assert.That(systemUnderTest.OutBoundObjects, Is.EqualTo(outBoundObjects));
            Assert.That(systemUnderTest.AssignedTopic, Is.EqualTo(topic));
            Assert.That(systemUnderTest.InputConnectionX, Is.EqualTo(positionX + 40));
            Assert.That(systemUnderTest.InputConnectionY, Is.EqualTo(positionY - 7));
            Assert.That(systemUnderTest.OutputConnectionX, Is.EqualTo(positionX + 40));
            Assert.That(systemUnderTest.OutputConnectionY, Is.EqualTo(positionY + 78));
        });
    }

    [Test]
    public void FileEnding_ReturnsCorrectEnding()
    {
        const string expectedFileEnding = "asf";
        var systemUnderTest = new LearningSpaceViewModel("foo", "foo", "foo", Theme.CampusAschaffenburg);
        Assert.That(systemUnderTest.FileEnding, Is.EqualTo(expectedFileEnding));
    }

    [Test]
    public void Workload_ReturnsCorrectWorkload()
    {
        var systemUnderTest = new LearningSpaceViewModel("a", "d", "e", Theme.CampusAschaffenburg,
            layoutViewModel: new LearningSpaceLayoutViewModel(FloorPlanEnum.R_20X30_8L));
        var element1 = ViewModelProvider.GetLearningElement(workload: 6);
        var element2 = ViewModelProvider.GetLearningElement("2", workload: 14);

        Assert.That(systemUnderTest.Workload, Is.EqualTo(0));

        systemUnderTest.LearningSpaceLayout.PutElement(0, element1);
        Assert.That(systemUnderTest.Workload, Is.EqualTo(6));

        systemUnderTest.LearningSpaceLayout.PutElement(1, element2);
        Assert.That(systemUnderTest.Workload, Is.EqualTo(20));

        systemUnderTest.LearningSpaceLayout.RemoveElement(0);
        Assert.That(systemUnderTest.Workload, Is.EqualTo(14));
    }

    [Test]
    public void Points_ReturnsCorrectSum()
    {
        var systemUnderTest = new LearningSpaceViewModel("a", "d", "e", Theme.CampusAschaffenburg,
            layoutViewModel: new LearningSpaceLayoutViewModel(FloorPlanEnum.R_20X30_8L));
        var element1 = ViewModelProvider.GetLearningElement(points: 7);
        var element2 = ViewModelProvider.GetLearningElement("2", points: 15);

        Assert.That(systemUnderTest.Points, Is.EqualTo(0));

        systemUnderTest.LearningSpaceLayout.PutElement(0, element1);
        Assert.That(systemUnderTest.Points, Is.EqualTo(7));

        systemUnderTest.LearningSpaceLayout.PutElement(1, element2);
        Assert.That(systemUnderTest.Points, Is.EqualTo(22));

        systemUnderTest.LearningSpaceLayout.RemoveElement(0);
        Assert.That(systemUnderTest.Points, Is.EqualTo(15));
    }
}