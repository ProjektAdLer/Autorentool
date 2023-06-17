using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;
using TestHelpers;

namespace BusinessLogicTest.Commands.Layout;

[TestFixture]
public class ChangeLearningSpaceLayoutUt
{
    [Test]
    public void Execute_ChangesLayout_PutsExtraLearningElementsIntoWorld()
    {
        var world = EntityProvider.GetLearningWorld(unsavedChanges: false);
        var element1 = EntityProvider.GetLearningElement(unsavedChanges: false);
        var element2 = EntityProvider.GetLearningElement(unsavedChanges: false);
        var element3 = EntityProvider.GetLearningElement(unsavedChanges: false);
        var element4 = EntityProvider.GetLearningElement(unsavedChanges: false);
        var element5 = EntityProvider.GetLearningElement(unsavedChanges: false);
        var element6 = EntityProvider.GetLearningElement(unsavedChanges: false);
        var element7 = EntityProvider.GetLearningElement(unsavedChanges: false);
        var element8 = EntityProvider.GetLearningElement(unsavedChanges: false);
        var layout = new LearningSpaceLayout(new Dictionary<int, ILearningElement>
        {
            {0, element1},
            {1, element2},
            {2, element3},
            {3, element4},
            {4, element5},
            {5, element6},
            {6, element7},
            {7, element8},
        }, FloorPlanEnum.R_20X30_8L);
        var space = new LearningSpace("", "", "", 0, Theme.Campus, false,layout)
        {
            UnsavedChanges = false
        };
        world.LearningSpaces.Add(space);

        var systemUnderTest = new ChangeLearningSpaceLayout(space, world, FloorPlanEnum.R_20X20_6L, _ => { });

        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(8));
            Assert.That(world.UnplacedLearningElements, Is.Empty);
            Assert.That(world.UnsavedChanges, Is.False);
            Assert.That(space.UnsavedChanges, Is.False);
        });

        systemUnderTest.Execute();

        Assert.Multiple(() =>
        {
            var expectedDict = new Dictionary<int, ILearningElement>
            {
                {0, element1},
                {1, element2},
                {2, element3},
                {3, element4},
                {4, element5},
                {5, element6},
            };
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(6));
            Assert.That(space.LearningSpaceLayout.LearningElements, Is.EquivalentTo(expectedDict));
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(2));
            Assert.That(world.UnplacedLearningElements, Has.Exactly(1).EqualTo(element7));
            Assert.That(world.UnplacedLearningElements, Has.Exactly(1).EqualTo(element8));
            Assert.That(world.UnsavedChanges, Is.True);
            Assert.That(space.UnsavedChanges, Is.True);
        });
    }

    [Test]
    public void UndoRedo_UndoesAndRedoesChanges()
    {
        var world = EntityProvider.GetLearningWorld(unsavedChanges: false);
        var element1 = EntityProvider.GetLearningElement(unsavedChanges: false);
        var element2 = EntityProvider.GetLearningElement(unsavedChanges: false);
        var element3 = EntityProvider.GetLearningElement(unsavedChanges: false);
        var element4 = EntityProvider.GetLearningElement(unsavedChanges: false);
        var element5 = EntityProvider.GetLearningElement(unsavedChanges: false);
        var element6 = EntityProvider.GetLearningElement(unsavedChanges: false);
        var element7 = EntityProvider.GetLearningElement(unsavedChanges: false);
        var element8 = EntityProvider.GetLearningElement(unsavedChanges: false);
        var layout = new LearningSpaceLayout(new Dictionary<int, ILearningElement>
        {
            {0, element1},
            {1, element2},
            {2, element3},
            {3, element4},
            {4, element5},
            {5, element6},
            {6, element7},
            {7, element8},
        }, FloorPlanEnum.R_20X30_8L);
        var space = new LearningSpace("", "", "", 0, Theme.Campus, false, layout)
        {
            UnsavedChanges = false
        };
        world.LearningSpaces.Add(space);

        var systemUnderTest = new ChangeLearningSpaceLayout(space, world, FloorPlanEnum.R_20X20_6L, _ => { });

        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(8));
            Assert.That(world.UnplacedLearningElements, Is.Empty);
            Assert.That(world.UnsavedChanges, Is.False);
            Assert.That(space.UnsavedChanges, Is.False);
        });

        systemUnderTest.Execute();

        Assert.Multiple(() =>
        {
            var expectedDict = new Dictionary<int, ILearningElement>
            {
                {0, element1},
                {1, element2},
                {2, element3},
                {3, element4},
                {4, element5},
                {5, element6},
            };
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(6));
            Assert.That(space.LearningSpaceLayout.LearningElements, Is.EquivalentTo(expectedDict));
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(2));
            Assert.That(world.UnplacedLearningElements, Has.Exactly(1).EqualTo(element7));
            Assert.That(world.UnplacedLearningElements, Has.Exactly(1).EqualTo(element8));
            Assert.That(world.UnsavedChanges, Is.True);
            Assert.That(space.UnsavedChanges, Is.True);
        });

        systemUnderTest.Undo();

        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(8));
            Assert.That(world.UnplacedLearningElements, Is.Empty);
            Assert.That(world.UnsavedChanges, Is.False);
            Assert.That(space.UnsavedChanges, Is.False);
        });

        systemUnderTest.Redo();

        Assert.Multiple(() =>
        {
            var expectedDict = new Dictionary<int, ILearningElement>
            {
                {0, element1},
                {1, element2},
                {2, element3},
                {3, element4},
                {4, element5},
                {5, element6},
            };
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(6));
            Assert.That(space.LearningSpaceLayout.LearningElements, Is.EquivalentTo(expectedDict));
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(2));
            Assert.That(world.UnplacedLearningElements, Has.Exactly(1).EqualTo(element7));
            Assert.That(world.UnplacedLearningElements, Has.Exactly(1).EqualTo(element8));
            Assert.That(world.UnsavedChanges, Is.True);
            Assert.That(space.UnsavedChanges, Is.True);
        });
    }
}