using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands.Layout;

[TestFixture]
public class ChangeLearningSpaceLayoutUt
{
    [Test]
    public void Execute_ChangesLayout_PutsExtraLearningElementsIntoWorld()
    {
        var world = new LearningWorld("", "", "", "", "", "")
        {
            UnsavedChanges = false
        };
        var element1 = new LearningElement("", null!, "", "", LearningElementDifficultyEnum.None)
        {
            UnsavedChanges = false
        };
        var element2 = new LearningElement("", null!, "", "", LearningElementDifficultyEnum.None)
        {
            UnsavedChanges = false
        };
        var element3 = new LearningElement("", null!, "", "", LearningElementDifficultyEnum.None)
        {
            UnsavedChanges = false
        };
        var element4 = new LearningElement("", null!, "", "", LearningElementDifficultyEnum.None)
        {
            UnsavedChanges = false
        };
        var element5 = new LearningElement("", null!, "", "", LearningElementDifficultyEnum.None)
        {
            UnsavedChanges = false
        };
        var layout = new LearningSpaceLayout(new Dictionary<int, ILearningElement>
        {
            {0, element1},
            {1, element2},
            {2, element3},
            {3, element4},
            {4, element5},
            
        }, FloorPlanEnum.LShape3L2);
        var space = new LearningSpace("", "", "", 0, layout)
        {
            UnsavedChanges = false
        };
        world.LearningSpaces.Add(space);
        
        var systemUnderTest = new ChangeLearningSpaceLayout(space, world, FloorPlanEnum.Rectangle2X2, _ => { });
        
        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(5));
            Assert.That(world.UnplacedLearningElements, Is.Empty);
            Assert.That(world.UnsavedChanges, Is.False);
            Assert.That(space.UnsavedChanges, Is.False);
        });
        
        systemUnderTest.Execute();
        
        Assert.Multiple(() =>
        {
            var expectedDict = new Dictionary<int, ILearningElement>
            {
                { 0, element1 },
                { 1, element2 },
                { 2, element3 },
                { 3, element4 },
            };
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(4));
            Assert.That(space.LearningSpaceLayout.LearningElements, Is.EquivalentTo(expectedDict));
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements, Has.Exactly(1).EqualTo(element5));
            Assert.That(world.UnsavedChanges, Is.True);
            Assert.That(space.UnsavedChanges, Is.True);
        });
    }
    [Test]
    public void UndoRedo_UndoesAndRedoesChanges()
    {
        var world = new LearningWorld("", "", "", "", "", "")
        {
            UnsavedChanges = false
        };
        var element1 = new LearningElement("", null!, "", "", LearningElementDifficultyEnum.None)
        {
            UnsavedChanges = false
        };
        var element2 = new LearningElement("", null!, "", "", LearningElementDifficultyEnum.None)
        {
            UnsavedChanges = false
        };
        var element3 = new LearningElement("", null!, "", "", LearningElementDifficultyEnum.None)
        {
            UnsavedChanges = false
        };
        var element4 = new LearningElement("", null!, "", "", LearningElementDifficultyEnum.None)
        {
            UnsavedChanges = false
        };
        var element5 = new LearningElement("", null!, "", "", LearningElementDifficultyEnum.None)
        {
            UnsavedChanges = false
        };
        var layout = new LearningSpaceLayout(new Dictionary<int, ILearningElement>
        {
            {0, element1},
            {1, element2},
            {2, element3},
            {3, element4},
            {4, element5},
            
        }, FloorPlanEnum.LShape3L2);
        var space = new LearningSpace("", "", "", 0, layout)
        {
            UnsavedChanges = false
        };
        world.LearningSpaces.Add(space);
        
        var systemUnderTest = new ChangeLearningSpaceLayout(space, world, FloorPlanEnum.Rectangle2X2, _ => { });
        
        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(5));
            Assert.That(world.UnplacedLearningElements, Is.Empty);
            Assert.That(world.UnsavedChanges, Is.False);
            Assert.That(space.UnsavedChanges, Is.False);
        });
        
        systemUnderTest.Execute();
        
        Assert.Multiple(() =>
        {
            var expectedDict = new Dictionary<int, ILearningElement>
            {
                { 0, element1 },
                { 1, element2 },
                { 2, element3 },
                { 3, element4 },
            };
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(4));
            Assert.That(space.LearningSpaceLayout.LearningElements, Is.EquivalentTo(expectedDict));
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements, Has.Exactly(1).EqualTo(element5));
            Assert.That(world.UnsavedChanges, Is.True);
            Assert.That(space.UnsavedChanges, Is.True);
        });
        
        systemUnderTest.Undo();
        
        Assert.Multiple(() =>
        {
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(5));
            Assert.That(world.UnplacedLearningElements, Is.Empty);
            Assert.That(world.UnsavedChanges, Is.False);
            Assert.That(space.UnsavedChanges, Is.False);
        });
        
        systemUnderTest.Redo();
        
        Assert.Multiple(() =>
        {
            var expectedDict = new Dictionary<int, ILearningElement>
            {
                { 0, element1 },
                { 1, element2 },
                { 2, element3 },
                { 3, element4 },
            };
            Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(4));
            Assert.That(space.LearningSpaceLayout.LearningElements, Is.EquivalentTo(expectedDict));
            Assert.That(world.UnplacedLearningElements, Has.Count.EqualTo(1));
            Assert.That(world.UnplacedLearningElements, Has.Exactly(1).EqualTo(element5));
            Assert.That(world.UnsavedChanges, Is.True);
            Assert.That(space.UnsavedChanges, Is.True);
        });
    }
}