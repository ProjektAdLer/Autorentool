using System.Collections.Generic;
using NUnit.Framework;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.PathWay;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.Space.SpaceLayout;
using Presentation.PresentationLogic.World;
using Shared;

namespace PresentationTest.PresentationLogic.World;

[TestFixture]
public class WorldViewModelUt
{
    
    [Test]
    public void Constructor_InitializesAllProperties()
    {
        var name = "asdf";
        var shortname = "jkl;";
        var authors = "ben and jerry";
        var language = "german";
        var description = "very cool element";
        var goals = "learn very many things";
        var space1 = new SpaceViewModel("ff", "ff", "ff", "ff", "ff");
        var spaces = new List<ISpaceViewModel> { space1 };
        var condition = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        var pathWayConditions = new List<PathWayConditionViewModel> { condition };
        var pathWay = new PathwayViewModel(space1, condition);
        var pathways = new List<IPathWayViewModel> { pathWay };

        var systemUnderTest = new WorldViewModel(name, shortname, authors, language, description, goals, 
            unsavedChanges: false, spaces: spaces, pathWayConditions: pathWayConditions,
            pathWays: pathways);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Shortname, Is.EqualTo(shortname));
            Assert.That(systemUnderTest.Authors, Is.EqualTo(authors));
            Assert.That(systemUnderTest.Language, Is.EqualTo(language)); 
            Assert.That(systemUnderTest.Description, Is.EqualTo(description));
            Assert.That(systemUnderTest.Goals, Is.EqualTo(goals));
            Assert.That(systemUnderTest.UnsavedChanges, Is.False);
            Assert.That(systemUnderTest.Spaces, Is.EqualTo(spaces));
            Assert.That(systemUnderTest.PathWayConditions, Is.EqualTo(pathWayConditions));
            Assert.That(systemUnderTest.PathWays, Is.EqualTo(pathways));
        });
    }

    [Test]
    public void FileEnding_ReturnsCorrectEnding()
    {
        const string expectedFileEnding = "awf";
        var systemUnderTest = new WorldViewModel("foo", "foo", "foo", "foo", "foo", "foo");
        Assert.That(systemUnderTest.FileEnding, Is.EqualTo(expectedFileEnding));
    }

    [Test]
    public void Workload_ReturnsCorrectWorkload()
    {
        var systemUnderTest = new WorldViewModel("foo", "foo", "foo", "foo", "foo", "foo");
        var space = new SpaceViewModel("a", "b", "c", "d", "e", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var spaceElement = new ElementViewModel("a", "b", null!, "url","c", "d", "e",
            ElementDifficultyEnum.Easy, space, 6);
        
        space.SpaceLayout.PutElement(0, spaceElement);
        systemUnderTest.Spaces.Add(space);

        Assert.That(systemUnderTest.Workload, Is.EqualTo(6));
    }
    
    [Test]
    public void Points_ReturnsCorrectSum()
    {
        var systemUnderTest = new WorldViewModel("foo", "foo", "foo", "foo", "foo", "foo");
        var space = new SpaceViewModel("a", "b", "c", "d", "e", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var spaceElement = new ElementViewModel("a", "b", null!, "url","c", "d", "e",
            ElementDifficultyEnum.Easy, space, 6,7);
        var space2 = new SpaceViewModel("a", "b", "c", "d", "e", layoutViewModel: new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3));
        var spaceElement2 = new ElementViewModel("a", "b", null!, "url","c", "d", "e",
            ElementDifficultyEnum.Easy, space, 4,5);
        
        space.SpaceLayout.PutElement(0, spaceElement);
        space2.SpaceLayout.PutElement(0, spaceElement2);
        systemUnderTest.Spaces.Add(space);
        systemUnderTest.Spaces.Add(space2);

        Assert.That(systemUnderTest.Points, Is.EqualTo(12));

        systemUnderTest.Spaces.Remove(space);
        
        Assert.That(systemUnderTest.Points, Is.EqualTo(5));
    }
}