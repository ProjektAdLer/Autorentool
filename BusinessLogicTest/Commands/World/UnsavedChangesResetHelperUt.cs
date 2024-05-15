using BusinessLogic.Commands.World;
using NUnit.Framework;
using TestHelpers;

namespace BusinessLogicTest.Commands.World;

public class UnsavedChangesResetHelperUt
{
    private UnsavedChangesResetHelper _unsavedChangesResetHelper;

    [SetUp]
    public void Setup()
    {
        _unsavedChangesResetHelper = new UnsavedChangesResetHelper();
    }

    [Test]
    public void ResetWorldUnsavedChangesState_WithLearningWorld_SetsUnsavedChangesToFalseInWholeStructure()
    {
        // Arrange
        var learningWorld = EntityProvider.GetLearningWorld();
        var learningSpace = EntityProvider.GetLearningSpace();
        var learningElement = EntityProvider.GetLearningElement();
        var adaptivityElement = EntityProvider.GetLearningElement(EntityProvider.GetAdaptivityContent());
        var storyElement = EntityProvider.GetLearningElement(content: EntityProvider.GetStoryContent());
        var unplacedElement = EntityProvider.GetLearningElement();
        var unplacedAdaptivityElement = EntityProvider.GetLearningElement(EntityProvider.GetAdaptivityContent());
        var unplacedStoryElement = EntityProvider.GetLearningElement(content: EntityProvider.GetStoryContent());
        learningSpace.LearningSpaceLayout.LearningElements[0] = learningElement;
        learningSpace.LearningSpaceLayout.LearningElements[1] = adaptivityElement;
        learningSpace.LearningSpaceLayout.StoryElements[0] = storyElement;
        learningWorld.LearningSpaces.Add(learningSpace);
        learningWorld.UnplacedLearningElements.Add(unplacedElement);
        learningWorld.UnplacedLearningElements.Add(unplacedAdaptivityElement);
        learningWorld.UnplacedLearningElements.Add(unplacedStoryElement);

        learningWorld.UnsavedChanges = true;
        learningSpace.UnsavedChanges = true;
        learningElement.UnsavedChanges = true;
        adaptivityElement.UnsavedChanges = true;
        storyElement.UnsavedChanges = true;
        unplacedElement.UnsavedChanges = true;
        unplacedAdaptivityElement.UnsavedChanges = true;
        unplacedStoryElement.UnsavedChanges = true;

        // Act
        _unsavedChangesResetHelper.ResetWorldUnsavedChangesState(learningWorld);


        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(learningWorld.UnsavedChanges, Is.False);
            Assert.That(learningSpace.UnsavedChanges, Is.False);
            Assert.That(learningElement.UnsavedChanges, Is.False);
            Assert.That(adaptivityElement.UnsavedChanges, Is.False);
            Assert.That(storyElement.UnsavedChanges, Is.False);
            Assert.That(unplacedElement.UnsavedChanges, Is.False);
            Assert.That(unplacedAdaptivityElement.UnsavedChanges, Is.False);
            Assert.That(unplacedStoryElement.UnsavedChanges, Is.False);
        });
    }
}