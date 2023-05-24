﻿using AuthoringTool.Mapping;
using AutoMapper;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using FluentAssertions;
using NUnit.Framework;
using PersistEntities;
using PersistEntities.LearningContent;
using Shared;

namespace AuthoringToolTest.Mapping;

[TestFixture]
public class EntityPersistEntityMappingProfileUt
{
    private const string Name = "name";
    private const string Shortname = "shortname";
    private const string Authors = "authors";
    private const string Language = "language";
    private const string Description = "description";
    private const string Goals = "goals";
    private const string SavePath = "foo/bar/baz.txt";
    private const string Type = "type";
    private static readonly string Filepath = "bar/baz/buz.txt";
    private const LearningElementDifficultyEnum Difficulty = LearningElementDifficultyEnum.Easy;
    private const LearningElementDifficultyEnumPe DifficultyPe = LearningElementDifficultyEnumPe.Easy;
    private const int Workload = 1;
    private const int Points = 2;
    private const int RequiredPoints = 3;
    private const double PositionX = 1.0;
    private const double PositionY = 2.0;

    private const string NewName = "newName";
    private const string NewShortname = "newShortname";
    private const string NewAuthors = "newAuthors";
    private const string NewLanguage = "newLanguage";
    private const string NewDescription = "newDescription";
    private const string NewGoals = "newGoals";
    private const string NewSavePath = "faa/bur/buz.txt";
    private const string NewType = "newType";
    private static readonly string NewFilepath = "/foo/bar/baz.txt";
    private const LearningElementDifficultyEnum NewDifficulty = LearningElementDifficultyEnum.Medium;
    private const LearningElementDifficultyEnumPe NewDifficultyPe = LearningElementDifficultyEnumPe.Medium;
    private const int NewWorkload = 2;
    private const int NewPoints = 3;
    private const int NewRequiredPoints = 4;
    private const double NewPositionX = 3.0;
    private const double NewPositionY = 4.0;

    [Test]
    public void Constructor_TestConfigurationIsValid()
    {
        var mapper = new MapperConfiguration(cfg =>
        {
            EntityPersistEntityMappingProfile.Configure(cfg);
            cfg.AddCollectionMappersOnce();
        });

        Assert.That(() => mapper.AssertConfigurationIsValid(), Throws.Nothing);
    }

    [Test]
    public void MapLearningContentAndLearningContentPersistEntity_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new FileContent(Name, Type, Filepath);
        var destination = new FileContentPe("", "", "");

        systemUnderTest.Map(source, destination);

        TestContent(destination, false);

        destination.Name = NewName;
        destination.Type = NewType;
        destination.Filepath = NewFilepath;

        systemUnderTest.Map(destination, source);

        TestContent(source, true);
    }

    [Test]
    public void MapLearningElementAndLearningElementPersistEntity_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var content = GetTestableContent();
        var source = new LearningElement(Name, content, Description, Goals,
            Difficulty, null, workload: Workload, points: Points, positionX: PositionX, positionY: PositionY);
        var destination = new LearningElementPe("", new FileContentPe("", "", "bar/baz/buz.txt"), "", "",
            LearningElementDifficultyEnumPe.None);

        systemUnderTest.Map(source, destination);

        TestElement(destination, null, false);

        destination.Name = NewName;
        destination.LearningContent = new FileContentPe(NewName, NewType, NewFilepath);
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.Difficulty = NewDifficultyPe;
        destination.Workload = NewWorkload;
        destination.Points = NewPoints;
        destination.PositionX = NewPositionX;
        destination.PositionY = NewPositionY;

        systemUnderTest.Map(destination, source);

        TestElement(source, null, true);
    }

    [Test]
    public void MapLearningSpaceAndLearningSpacePersistEntity_WithoutLearningElement_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningSpace(Name, Description, Goals, RequiredPoints, Theme.Campus, null,
            positionX: PositionX, positionY: PositionY, inBoundSpaces: new List<IObjectInPathWay>(),
            outBoundSpaces: new List<IObjectInPathWay>());
        var destination = new LearningSpacePe("", "", "", 0, Theme.Campus);

        systemUnderTest.Map(source, destination);

        TestSpace(destination, false);
        Assert.That(destination.LearningSpaceLayout.ContainedLearningElements, Is.Empty);

        destination.Name = NewName;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.RequiredPoints = NewRequiredPoints;
        destination.LearningSpaceLayout.LearningElements = new Dictionary<int, ILearningElementPe>();
        destination.PositionX = NewPositionX;
        destination.PositionY = NewPositionY;
        destination.InBoundObjects = new List<IObjectInPathWayPe>();
        destination.OutBoundObjects = new List<IObjectInPathWayPe>();

        systemUnderTest.Map(destination, source);

        TestSpace(source, true);
        Assert.That(source.ContainedLearningElements, Is.Empty);
    }

    [Test]
    public void MapLearningSpaceAndLearningSpacePersistEntity_WithLearningElement_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningSpace(Name, Description, Goals, RequiredPoints, Theme.Campus,
            new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.R20X308L),
            positionX: PositionX, positionY: PositionY, inBoundSpaces: new List<IObjectInPathWay>(),
            outBoundSpaces: new List<IObjectInPathWay>());
        source.LearningSpaceLayout.LearningElements[0] = GetTestableElementWithParent(source);
        var destination = new LearningSpacePe("", "", "", 0, Theme.Campus);

        systemUnderTest.Map(source, destination);

        TestSpace(destination, false);
        Assert.That(destination.LearningSpaceLayout.ContainedLearningElements.Count(), Is.EqualTo(1));

        destination.Name = NewName;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.RequiredPoints = NewRequiredPoints;
        destination.LearningSpaceLayout.LearningElements = new Dictionary<int, ILearningElementPe>
        {
            {
                0,
                GetTestableElementPersistEntity()
            }
        };
        destination.PositionX = NewPositionX;
        destination.PositionY = NewPositionY;
        destination.InBoundObjects = new List<IObjectInPathWayPe>();
        destination.OutBoundObjects = new List<IObjectInPathWayPe>();

        systemUnderTest.Map(destination, source);

        TestSpace(source, true);
        Assert.That(source.ContainedLearningElements.Count(), Is.EqualTo(1));
    }

    [Test]
    public void MapLearningWorldAndLearningWorldPersistEntity_WithoutLearningSpaces_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals, SavePath,
            new List<ILearningSpace>());
        var destination = new LearningWorldPe("", "", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() => { Assert.That(destination.LearningSpaces, Is.Empty); });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.SavePath = NewSavePath;
        destination.LearningSpaces = new List<LearningSpacePe>();

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() => { Assert.That(source.LearningSpaces, Is.Empty); });
    }

    [Test]
    public void MapLearningWorldAndLearningWorldPersistEntity_WithUnplacedLearningElements_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals, SavePath);
        source.UnplacedLearningElements.Add(new LearningElement(Name, GetTestableContent(), Description, Goals,
            Difficulty, null, Workload, Points));
        var destination = new LearningWorldPe("", "", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() =>
        {
            Assert.That(destination.LearningSpaces, Has.Count.EqualTo(0));
            Assert.That(destination.UnplacedLearningElements, Has.Count.EqualTo(1));
        });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.SavePath = NewSavePath;
        destination.UnplacedLearningElements = new List<ILearningElementPe>()
        {
            new LearningElementPe(NewName, GetTestableNewContentPersistEntity(), NewDescription, NewGoals,
                NewDifficultyPe, NewWorkload, NewPoints)
        };

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() =>
        {
            Assert.That(source.LearningSpaces, Has.Count.EqualTo(0));
            Assert.That(source.UnplacedLearningElements, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void MapLearningWorldAndLearningWorldPersistEntity_WithEmptyLearningSpace_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals, SavePath,
            new List<ILearningSpace>());
        source.LearningSpaces.Add(new LearningSpace(Name, Description, Goals, RequiredPoints, Theme.Campus,
            null, positionX: PositionX, positionY: PositionY, inBoundSpaces: new List<IObjectInPathWay>(),
            outBoundSpaces: new List<IObjectInPathWay>()));
        var destination = new LearningWorldPe("", "", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() =>
        {
            Assert.That(destination.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(destination.LearningSpaces[0].LearningSpaceLayout.ContainedLearningElements, Is.Empty);
        });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.SavePath = NewSavePath;
        destination.LearningSpaces = new List<LearningSpacePe>()
        {
            new LearningSpacePe(NewName, NewDescription, NewGoals, NewRequiredPoints, Theme.Campus,
                null, positionX: NewPositionX, positionY: NewPositionY, inBoundObjects: new List<IObjectInPathWayPe>(),
                outBoundObjects: new List<IObjectInPathWayPe>())
        };

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() =>
        {
            Assert.That(source.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(source.LearningSpaces[0].ContainedLearningElements, Is.Empty);
        });
    }

    [Test]
    public void MapLearningWorldAndLearningWorldPersistEntity_WithLearningSpaceAndLearningPathWay_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals, SavePath,
            new List<ILearningSpace>());
        var space1 = GetTestableSpace();
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 3, 2);
        var space2 = GetTestableSpace();
        source.LearningSpaces.Add(space1);
        source.PathWayConditions.Add(pathWayCondition);
        source.LearningSpaces.Add(space2);
        var destination = new LearningWorldPe("", "", "", "", "", "", "");

        source.LearningPathways.Add(new LearningPathway(space1, pathWayCondition));
        source.LearningPathways.Add(new LearningPathway(pathWayCondition, space2));
        space1.OutBoundObjects.Add(pathWayCondition);
        pathWayCondition.InBoundObjects.Add(space1);
        pathWayCondition.OutBoundObjects.Add(space2);
        space2.InBoundObjects.Add(pathWayCondition);

        systemUnderTest.Map(source, destination);

        var destinationSpace1 = destination.LearningSpaces[0];
        var destinationCondition = destination.PathWayConditions[0];
        var destinationSpace2 = destination.LearningSpaces[1];
        TestWorld(destination, false);
        Assert.Multiple(() =>
        {
            Assert.That(destination.LearningSpaces, Has.Count.EqualTo(2));
            Assert.That(destination.PathWayConditions, Has.Count.EqualTo(1));
            Assert.That(destinationSpace1.LearningSpaceLayout.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(destinationSpace1.OutBoundObjects, Does.Contain(destinationCondition));
            Assert.That(destinationSpace2.LearningSpaceLayout.ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(destinationSpace2.InBoundObjects, Does.Contain(destinationCondition));
            Assert.That(destinationCondition.InBoundObjects, Does.Contain(destinationSpace1));
            Assert.That(destinationCondition.OutBoundObjects, Does.Contain(destinationSpace2));
            Assert.That(destination.LearningPathways, Has.Count.EqualTo(2));
        });


        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.SavePath = NewSavePath;
        var spacePe1 = GetTestableNewSpacePersistEntity();
        var pathWayConditionPe = new PathWayConditionPe(ConditionEnumPe.And, 2, 1);
        var spacePe2 = GetTestableNewSpacePersistEntity();
        destination.LearningSpaces = new List<LearningSpacePe>() { spacePe1, spacePe2 };
        destination.PathWayConditions = new List<PathWayConditionPe>() { pathWayConditionPe };
        destination.LearningPathways = new List<LearningPathwayPe>
            { new(spacePe1, pathWayConditionPe), new(pathWayConditionPe, spacePe2) };
        spacePe1.OutBoundObjects.Add(pathWayConditionPe);
        pathWayConditionPe.InBoundObjects.Add(spacePe1);
        pathWayConditionPe.OutBoundObjects.Add(spacePe2);
        spacePe2.InBoundObjects.Add(pathWayConditionPe);

        {
            systemUnderTest.Map(destination, source);
            var sourceSpace1 = source.LearningSpaces[0];
            var sourceCondition = source.PathWayConditions[0];
            var sourceSpace2 = source.LearningSpaces[1];

            TestWorld(source, true);
            Assert.Multiple(() =>
            {
                Assert.That(source.LearningSpaces, Has.Count.EqualTo(2));
                Assert.That(source.PathWayConditions, Has.Count.EqualTo(1));
                Assert.That(sourceSpace1.ContainedLearningElements.Count(), Is.EqualTo(1));
                Assert.That(sourceSpace1.OutBoundObjects, Does.Contain(sourceCondition));
                Assert.That(sourceCondition.InBoundObjects, Does.Contain(sourceSpace1));
                Assert.That(sourceCondition.OutBoundObjects, Does.Contain(sourceSpace2));
                Assert.That(sourceSpace2.ContainedLearningElements.Count(), Is.EqualTo(1));
                Assert.That(sourceSpace2.InBoundObjects, Does.Contain(sourceCondition));
                Assert.That(source.LearningPathways, Has.Count.EqualTo(2));
            });
        }

        {
            var newSource = systemUnderTest.Map<LearningWorld>(destination);
            systemUnderTest.Map(destination, newSource);
            var newSourceSpace1 = source.LearningSpaces[0];
            var newSourceCondition = source.PathWayConditions[0];
            var newSourceSpace2 = source.LearningSpaces[1];

            TestWorld(newSource, true);
            Assert.Multiple(() =>
            {
                Assert.That(newSource.LearningSpaces, Has.Count.EqualTo(2));
                Assert.That(newSource.PathWayConditions, Has.Count.EqualTo(1));
                Assert.That(newSourceSpace1.ContainedLearningElements.Count(), Is.EqualTo(1));
                Assert.That(newSourceSpace1.OutBoundObjects, Does.Contain(newSourceCondition));
                Assert.That(newSourceCondition.InBoundObjects, Does.Contain(newSourceSpace1));
                Assert.That(newSourceCondition.OutBoundObjects, Does.Contain(newSourceSpace2));
                Assert.That(newSourceSpace2.ContainedLearningElements.Count(), Is.EqualTo(1));
                Assert.That(newSourceSpace2.InBoundObjects, Does.Contain(newSourceCondition));
                Assert.That(newSource.LearningPathways, Has.Count.EqualTo(2));
            });
        }
    }

    /// <summary>
    /// Regression test for https://github.com/projektadler/autorentool/issues/254
    /// </summary>
    [Test]
    public void
        MapLearningWorldAndLearningWorldPersistEntity_WithMultipleLearningSpacesAndLearningPathWays_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals, SavePath,
            new List<ILearningSpace>());
        var space1 = GetTestableSpace();
        space1.Name = "space1";
        var space2 = GetTestableSpace();
        space2.Name = "space2";
        var space3 = GetTestableSpace();
        space3.Name = "space3";
        source.LearningSpaces.Add(space1);
        source.LearningSpaces.Add(space2);
        source.LearningSpaces.Add(space3);
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 2, 1);
        source.PathWayConditions.Add(pathWayCondition);

        source.LearningPathways.Add(new LearningPathway(space1, space2));
        space1.OutBoundObjects.Add(space2);
        space2.InBoundObjects.Add(space1);
        source.LearningPathways.Add(new LearningPathway(space2, space3));
        space1.OutBoundObjects.Add(space3);
        space2.InBoundObjects.Add(space2);
        source.LearningPathways.Add(new LearningPathway(space3, pathWayCondition));
        space3.OutBoundObjects.Add(pathWayCondition);
        pathWayCondition.InBoundObjects.Add(space3);

        var persistEntity = systemUnderTest.Map<LearningWorldPe>(source);
        var restored = systemUnderTest.Map<LearningWorld>(persistEntity);

        Assert.That(restored.LearningPathways, Has.Count.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(((LearningSpace)restored.LearningPathways[0].SourceObject).Name, Is.EqualTo("space1"));
            Assert.That(((LearningSpace)restored.LearningPathways[1].SourceObject).Name, Is.EqualTo("space2"));
            Assert.That(((LearningSpace)restored.LearningPathways[0].TargetObject).Name, Is.EqualTo("space2"));
            Assert.That(((LearningSpace)restored.LearningPathways[1].TargetObject).Name, Is.EqualTo("space3"));
            Assert.That(((LearningSpace)restored.LearningPathways[2].SourceObject).Name, Is.EqualTo("space3"));
            Assert.That(((PathWayCondition)restored.LearningPathways[2].TargetObject).Condition,
                Is.EqualTo(ConditionEnum.And));
        });
    }

    [Test]
    public void MapLearningWorldToPeAndBack_EquivalentObjectsExceptUnsavedChangesFalse()
    {
        var world = new LearningWorld("saveme", "", "", "", "", "");
        Assert.That(world.UnsavedChanges);

        var systemUnderTest = CreateTestableMapper();

        var pe = systemUnderTest.Map<LearningWorldPe>(world);
        var restoredWorld = systemUnderTest.Map<LearningWorld>(pe);

        restoredWorld.Should().BeEquivalentTo(world, opt => opt.IgnoringCyclicReferences()
            .Excluding(obj => obj.Id)
            .Excluding(obj => obj.UnsavedChanges)
            .Excluding(obj => obj.InternalUnsavedChanges)
        );
        Assert.That(restoredWorld.UnsavedChanges, Is.False);
    }

    #region testable Content/Element/Space/World

    private static FileContent GetTestableContent()
    {
        return new FileContent(Name, Type, Filepath);
    }

    private static FileContentPe GetTestableNewContentPersistEntity()
    {
        return new FileContentPe(NewName, NewType, NewFilepath);
    }

    private static LearningElement GetTestableElementWithParent(LearningSpace parent)
    {
        return new LearningElement(Name, GetTestableContent(), Description, Goals, Difficulty, parent, Workload,
            Points, PositionX,
            PositionY);
    }

    private static LearningElementPe GetTestableElementPersistEntity()
    {
        return new LearningElementPe(NewName, GetTestableNewContentPersistEntity(),
            NewDescription, NewGoals, NewDifficultyPe, NewWorkload, NewPoints, NewPositionX,
            NewPositionY);
    }

    private static LearningSpace GetTestableSpace()
    {
        var space = new LearningSpace(Name, Description, Goals, RequiredPoints, Theme.Campus,
            new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.R20X308L),
            positionX: PositionX, positionY: PositionY);
        var element = GetTestableElementWithParent(space);
        space.LearningSpaceLayout.LearningElements[0] = element;
        return space;
    }

    private static LearningSpacePe GetTestableNewSpacePersistEntity()
    {
        return new LearningSpacePe(NewName, NewDescription, NewGoals, NewRequiredPoints, Theme.Campus,
            new LearningSpaceLayoutPe(
                new Dictionary<int, ILearningElementPe>()
                {
                    {
                        0,
                        GetTestableElementPersistEntity()
                    }
                },
                FloorPlanEnum.R20X308L), positionX: NewPositionX, positionY: NewPositionY);
    }

    #endregion

    #region static test methods

    private static void TestWorld(object destination, bool useNewFields)
    {
        switch (destination)
        {
            case LearningWorld world:
                Assert.Multiple(() =>
                {
                    Assert.That(world.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(world.Shortname, Is.EqualTo(useNewFields ? NewShortname : Shortname));
                    Assert.That(world.Authors, Is.EqualTo(useNewFields ? NewAuthors : Authors));
                    Assert.That(world.Language, Is.EqualTo(useNewFields ? NewLanguage : Language));
                    Assert.That(world.Description, Is.EqualTo(useNewFields ? NewDescription : Description));
                    Assert.That(world.Goals, Is.EqualTo(useNewFields ? NewGoals : Goals));
                    Assert.That(world.SavePath, Is.EqualTo(useNewFields ? NewSavePath : SavePath));
                    TestSpacesList(world.LearningSpaces, useNewFields);
                    TestElementsList(world.UnplacedLearningElements, null, useNewFields);
                });
                break;
            case LearningWorldPe world:
                Assert.Multiple(() =>
                {
                    Assert.That(world.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(world.Shortname, Is.EqualTo(useNewFields ? NewShortname : Shortname));
                    Assert.That(world.Authors, Is.EqualTo(useNewFields ? NewAuthors : Authors));
                    Assert.That(world.Language, Is.EqualTo(useNewFields ? NewLanguage : Language));
                    Assert.That(world.Description, Is.EqualTo(useNewFields ? NewDescription : Description));
                    Assert.That(world.Goals, Is.EqualTo(useNewFields ? NewGoals : Goals));
                    Assert.That(world.SavePath, Is.EqualTo(useNewFields ? NewSavePath : SavePath));
                    TestSpacesList(world.LearningSpaces, useNewFields);
                    TestElementsList(world.UnplacedLearningElements, null, useNewFields);
                });
                break;
        }
    }

    private static void TestSpacesList(object worldLearningSpaces, bool useNewFields)
    {
        switch (worldLearningSpaces)
        {
            case List<LearningSpace> learningSpaces:
                Assert.Multiple(() =>
                {
                    foreach (var learningSpace in learningSpaces)
                    {
                        TestSpace(learningSpace, useNewFields);
                    }
                });
                break;
            case List<LearningSpacePe> learningSpaces:
                Assert.Multiple(() =>
                {
                    foreach (var learningSpace in learningSpaces)
                    {
                        TestSpace(learningSpace, useNewFields);
                    }
                });
                break;
        }
    }

    private static void TestSpace(object learningSpace, bool useNewFields)
    {
        switch (learningSpace)
        {
            case LearningSpace space:
                Assert.Multiple(() =>
                {
                    Assert.That(space.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(space.Description, Is.EqualTo(useNewFields ? NewDescription : Description));
                    Assert.That(space.Goals, Is.EqualTo(useNewFields ? NewGoals : Goals));
                    Assert.That(space.RequiredPoints, Is.EqualTo(useNewFields ? NewRequiredPoints : RequiredPoints));
                    TestElementsList(space.ContainedLearningElements, space, useNewFields);
                    Assert.That(space.PositionX, Is.EqualTo(useNewFields ? NewPositionX : PositionX));
                    Assert.That(space.PositionY, Is.EqualTo(useNewFields ? NewPositionY : PositionY));
                });
                break;
            case LearningSpacePe space:
                Assert.Multiple(() =>
                {
                    Assert.That(space.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(space.Description, Is.EqualTo(useNewFields ? NewDescription : Description));
                    Assert.That(space.Goals, Is.EqualTo(useNewFields ? NewGoals : Goals));
                    Assert.That(space.RequiredPoints, Is.EqualTo(useNewFields ? NewRequiredPoints : RequiredPoints));
                    TestElementsList(space.LearningSpaceLayout.ContainedLearningElements, space, useNewFields);
                    Assert.That(space.PositionX, Is.EqualTo(useNewFields ? NewPositionX : PositionX));
                    Assert.That(space.PositionY, Is.EqualTo(useNewFields ? NewPositionY : PositionY));
                });
                break;
        }
    }

    private static void TestElementsList(object worldLearningElements, object? parent, bool useNewFields)
    {
        switch (worldLearningElements)
        {
            case List<LearningElement> learningElements:
                Assert.Multiple(() =>
                {
                    foreach (var learningElement in learningElements)
                    {
                        TestElement(learningElement, parent, useNewFields);
                    }
                });
                break;
            case List<LearningElementPe> learningElements:
                Assert.Multiple(() =>
                {
                    foreach (var learningElement in learningElements)
                    {
                        TestElement(learningElement, parent, useNewFields);
                    }
                });
                break;
        }
    }

    private static void TestElement(object learningElement, object? parent, bool useNewFields)
    {
        switch (learningElement)
        {
            case LearningElement element:
                Assert.Multiple(() =>
                {
                    Assert.That(element.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    TestContent(element.LearningContent, useNewFields);
                    Assert.That(element.Description, Is.EqualTo(useNewFields ? NewDescription : Description));
                    Assert.That(element.Goals, Is.EqualTo(useNewFields ? NewGoals : Goals));
                    Assert.That(element.Difficulty, Is.EqualTo(useNewFields ? NewDifficulty : Difficulty));
                    Assert.That(element.Parent, Is.EqualTo(parent));
                    Assert.That(element.Workload, Is.EqualTo(useNewFields ? NewWorkload : Workload));
                    Assert.That(element.Points, Is.EqualTo(useNewFields ? NewPoints : Points));
                    Assert.That(element.PositionX, Is.EqualTo(useNewFields ? NewPositionX : PositionX));
                    Assert.That(element.PositionY, Is.EqualTo(useNewFields ? NewPositionY : PositionY));
                });
                break;
            case LearningElementPe element:
                Assert.Multiple(() =>
                {
                    Assert.That(element.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    TestContent(element.LearningContent, useNewFields);
                    Assert.That(element.Description, Is.EqualTo(useNewFields ? NewDescription : Description));
                    Assert.That(element.Goals, Is.EqualTo(useNewFields ? NewGoals : Goals));
                    Assert.That(element.Difficulty, Is.EqualTo(useNewFields ? NewDifficultyPe : DifficultyPe));
                    Assert.That(element.Workload, Is.EqualTo(useNewFields ? NewWorkload : Workload));
                    Assert.That(element.Points, Is.EqualTo(useNewFields ? NewPoints : Points));
                    Assert.That(element.PositionX, Is.EqualTo(useNewFields ? NewPositionX : PositionX));
                    Assert.That(element.PositionY, Is.EqualTo(useNewFields ? NewPositionY : PositionY));
                });
                break;
        }
    }

    private static void TestContent(object elementLearningContent, bool useNewFields)
    {
        switch (elementLearningContent)
        {
            case FileContent content:
                Assert.Multiple(() =>
                {
                    Assert.That(content.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(content.Type, Is.EqualTo(useNewFields ? NewType : Type));
                    Assert.That(content.Filepath, Is.EqualTo(useNewFields ? NewFilepath : Filepath));
                });
                break;
            case FileContentPe content:
                Assert.Multiple(() =>
                {
                    Assert.That(content.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(content.Type, Is.EqualTo(useNewFields ? NewType : Type));
                    Assert.That(content.Filepath, Is.EqualTo(useNewFields ? NewFilepath : Filepath));
                });
                break;
            default:
                throw new NotImplementedException();
        }
    }

    #endregion

    private static IMapper CreateTestableMapper()
    {
        var mapper = new MapperConfiguration(EntityPersistEntityMappingProfile.Configure);
        var systemUnderTest = mapper.CreateMapper();
        return systemUnderTest;
    }
}