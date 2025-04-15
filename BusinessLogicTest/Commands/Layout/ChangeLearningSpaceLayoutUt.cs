using BusinessLogic.Commands.Layout;
using BusinessLogic.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Shared;
using Shared.Theme;
using TestHelpers;

namespace BusinessLogicTest.Commands.Layout;

[TestFixture]
public class ChangeLearningSpaceLayoutUt
{
    [Test]
    // ANF-ID: [AWA0023]
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
            { 0, element1 },
            { 1, element2 },
            { 2, element3 },
            { 3, element4 },
            { 4, element5 },
            { 5, element6 },
            { 6, element7 },
            { 7, element8 },
        }, new Dictionary<int, ILearningElement>(), FloorPlanEnum.R_20X30_8L);
        var space = new LearningSpace("", "", 0, SpaceTheme.CampusAschaffenburg,
            EntityProvider.GetLearningOutcomeCollection(), layout)
        {
            UnsavedChanges = false
        };
        world.LearningSpaces.Add(space);

        var systemUnderTest = new ChangeLearningSpaceLayout(space, world, FloorPlanEnum.R_20X20_6L, _ => { },
            new NullLogger<ChangeLearningSpaceLayout>());

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
                { 0, element1 },
                { 1, element2 },
                { 2, element3 },
                { 3, element4 },
                { 4, element5 },
                { 5, element6 },
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
    // ANF-ID: [AWA0023]
    public void Execute_ReindexesLearningElementsWhenNecessary()
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

        // Note: using high indices to ensure the reindexing happens
        var layout = new LearningSpaceLayout(new Dictionary<int, ILearningElement>
        {
            { 10, element1 },
            { 11, element2 },
            { 12, element3 },
            { 13, element4 },
            { 14, element5 },
            { 15, element6 },
            { 16, element7 },
            { 17, element8 },
        }, new Dictionary<int, ILearningElement>(), FloorPlanEnum.R_20X30_8L);

        var space = new LearningSpace("", "", 0, SpaceTheme.CampusAschaffenburg,
            EntityProvider.GetLearningOutcomeCollection(), layout)
        {
            UnsavedChanges = false
        };
        world.LearningSpaces.Add(space);

        var systemUnderTest = new ChangeLearningSpaceLayout(space, world, FloorPlanEnum.R_20X20_6L, _ => { },
            new NullLogger<ChangeLearningSpaceLayout>());

        systemUnderTest.Execute();

        Assert.Multiple(() =>
        {
            var expectedDict = new Dictionary<int, ILearningElement>
            {
                { 0, element1 },
                { 1, element2 },
                { 2, element3 },
                { 3, element4 },
                { 4, element5 },
                { 5, element6 },
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
            { 0, element1 },
            { 1, element2 },
            { 2, element3 },
            { 3, element4 },
            { 4, element5 },
            { 5, element6 },
            { 6, element7 },
            { 7, element8 },
        }, new Dictionary<int, ILearningElement>(), FloorPlanEnum.R_20X30_8L);
        var space = new LearningSpace("", "", 0, SpaceTheme.CampusAschaffenburg,
            EntityProvider.GetLearningOutcomeCollection(), layout)
        {
            UnsavedChanges = false
        };
        world.LearningSpaces.Add(space);

        var systemUnderTest = new ChangeLearningSpaceLayout(space, world, FloorPlanEnum.R_20X20_6L, _ => { },
            new NullLogger<ChangeLearningSpaceLayout>());

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
                { 0, element1 },
                { 1, element2 },
                { 2, element3 },
                { 3, element4 },
                { 4, element5 },
                { 5, element6 },
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
                { 0, element1 },
                { 1, element2 },
                { 2, element3 },
                { 3, element4 },
                { 4, element5 },
                { 5, element6 },
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
    public void Undo_MementoSpaceLayoutIsNull_ThrowsException()
    {
        var world = EntityProvider.GetLearningWorld(unsavedChanges: false);
        var space = EntityProvider.GetLearningSpace();
        world.LearningSpaces.Add(space);

        var systemUnderTest = new ChangeLearningSpaceLayout(space, world, FloorPlanEnum.R_20X20_6L, _ => { },
            new NullLogger<ChangeLearningSpaceLayout>());

        var ex = Assert.Throws<InvalidOperationException>(() => systemUnderTest.Undo());
        Assert.That(ex!.Message, Is.EqualTo("MementoSpaceLayout is null"));
    }

    [Test]
    public void Undo_MementoSpaceIsNull_ThrowsException()
    {
        var world = EntityProvider.GetLearningWorld(unsavedChanges: false);
        var space = EntityProvider.GetLearningSpace();
        world.LearningSpaces.Add(space);

        var systemUnderTest = new ChangeLearningSpaceLayout(space, world, FloorPlanEnum.R_20X20_6L, _ => { },
            new NullLogger<ChangeLearningSpaceLayout>());

        // Manually setting MementoSpaceLayout to bypass the first check
        var mementoSpaceLayout = space.LearningSpaceLayout.GetMemento();
        systemUnderTest.MementoSpaceLayout = mementoSpaceLayout;

        var ex = Assert.Throws<InvalidOperationException>(() => systemUnderTest.Undo());
        Assert.That(ex!.Message, Is.EqualTo("MementoSpace is null"));
    }

    [Test]
    public void Undo_MementoWorldIsNull_ThrowsException()
    {
        var world = EntityProvider.GetLearningWorld(unsavedChanges: false);
        var space = EntityProvider.GetLearningSpace();
        world.LearningSpaces.Add(space);

        var systemUnderTest = new ChangeLearningSpaceLayout(space, world, FloorPlanEnum.R_20X20_6L, _ => { },
            new NullLogger<ChangeLearningSpaceLayout>());


        // Manually setting MementoSpaceLayout and MementoSpace to bypass the first and second checks
        var mementoSpaceLayout = space.LearningSpaceLayout.GetMemento();
        var mementoSpace = space.GetMemento();

        systemUnderTest.MementoSpaceLayout = mementoSpaceLayout;
        systemUnderTest.MementoSpace = mementoSpace;

        var ex = Assert.Throws<InvalidOperationException>(() => systemUnderTest.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_mementoWorld is null"));
    }
}