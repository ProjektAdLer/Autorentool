using AuthoringTool.Mapping;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using BusinessLogic.Entities;
using NUnit.Framework;
using PersistEntities;
using Shared;

namespace AuthoringToolTest.Mapping;

[TestFixture]
public class EntityPersistEntityMappingProfileUt
{
    private const string Name = "name";
    private const string Shortname = "shortname";
    private const string Authors = "authors";
    private const string Language = "language";
    private const string Url = "google";
    private const string Description = "description";
    private const string Goals = "goals";
    private const string Type = "type";
    private static readonly string Filepath = "bar/baz/buz.txt";
    private const ElementDifficultyEnum Difficulty = ElementDifficultyEnum.Easy;
    private const ElementDifficultyEnumPe DifficultyPe = ElementDifficultyEnumPe.Easy;
    private const int Workload = 1;
    private const int Points = 2;
    private const int RequiredPoints = 3;
    private const double PositionX = 1.0;
    private const double PositionY = 2.0;

    private const string NewName = "newName";
    private const string NewShortname = "newShortname";
    private const string NewUrl = "newgoogle";
    private const string NewAuthors = "newAuthors";
    private const string NewLanguage = "newLanguage";
    private const string NewDescription = "newDescription";
    private const string NewGoals = "newGoals";
    private const string NewType = "newType";
    private static readonly string NewFilepath = "/foo/bar/baz.txt";
    private const ElementDifficultyEnum NewDifficulty = ElementDifficultyEnum.Medium;
    private const ElementDifficultyEnumPe NewDifficultyPe = ElementDifficultyEnumPe.Medium;
    private const int NewWorkload = 2;
    private const int NewPoints = 3;
    private const int NewRequiredPoints = 4;
    private const double NewPositionX = 3.0;
    private const double NewPositionY = 4.0;

    [Test]
    public void Constructor_TestConfigurationIsValid()
    {
        var mapper = new MapperConfiguration(cfg=>
        {
            EntityPersistEntityMappingProfile.Configure(cfg);
            cfg.AddCollectionMappers();
        });

        Assert.That(() => mapper.AssertConfigurationIsValid(), Throws.Nothing);
    }

    [Test]
    public void MapContentAndContentPersistEntity_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new Content(Name, Type, Filepath);
        var destination = new ContentPe("", "", "");

        systemUnderTest.Map(source, destination);

        TestContent(destination, false);

        destination.Name = NewName;
        destination.Type = NewType;
        destination.Filepath = NewFilepath;

        systemUnderTest.Map(destination, source);

        TestContent(source, true);
    }

    [Test]
    public void MapElementAndElementPersistEntity_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var content = GetTestableContent();
        var source = new Element(Name, Shortname, content, Url, Authors, Description, Goals,
            Difficulty, null, Workload, Points, PositionX, PositionY);
        var destination = new ElementPe("", "", new ContentPe("", "", "bar/baz/buz.txt"), "google.com",
            "", "", "",
            ElementDifficultyEnumPe.None);

        systemUnderTest.Map(source, destination);

        TestElement(destination, null, false);

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Content = new ContentPe(NewName, NewType, NewFilepath);
        destination.Url = NewUrl;
        destination.Authors = NewAuthors;
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
    public void MapSpaceAndSpacePersistEntity_WithoutElement_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new Space(Name, Shortname, Authors, Description, Goals, RequiredPoints, null,
            PositionX, PositionY, new List<IObjectInPathWay>(), new List<IObjectInPathWay>());
        var destination = new SpacePe("", "", "", "", "", 0);

        systemUnderTest.Map(source, destination);

        TestSpace(destination, false);
        Assert.That(destination.SpaceLayout.ContainedElements, Is.Empty);

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.RequiredPoints = NewRequiredPoints;
        destination.SpaceLayout.Elements = Array.Empty<IElementPe?>();
        destination.PositionX = NewPositionX;
        destination.PositionY = NewPositionY;
        destination.InBoundObjects = new List<IObjectInPathWayPe>();
        destination.OutBoundObjects = new List<IObjectInPathWayPe>();

        systemUnderTest.Map(destination, source);

        TestSpace(source, true);
        Assert.That(source.ContainedElements, Is.Empty);
    }

    [TestCase(ElementType.TextTransfer, typeof(TextTransferElementPe), typeof(TextTransferElement))]
    [TestCase(ElementType.ImageTransfer, typeof(ImageTransferElementPe), typeof(ImageTransferElement))]
    [TestCase(ElementType.VideoTransfer, typeof(VideoTransferElementPe), typeof(VideoTransferElement))]
    [TestCase(ElementType.PdfTransfer, typeof(PdfTransferElementPe), typeof(PdfTransferElement))]
    [TestCase(ElementType.VideoActivation, typeof(VideoActivationElementPe), typeof(VideoActivationElement))]
    [TestCase(ElementType.H5PActivation, typeof(H5PActivationElementPe), typeof(H5PActivationElement))]
    [TestCase(ElementType.H5PInteraction, typeof(H5PInteractionElementPe), typeof(H5PInteractionElement))]
    [TestCase(ElementType.H5PTest, typeof(H5PTestElementPe), typeof(H5PTestElement))]
    public void MapSpaceAndSpacePersistEntity_WithElement_TestMappingIsValid
        (ElementType elementType, Type expectedElementPeType, Type expectedElementType)
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new Space(Name, Shortname, Authors, Description, Goals, RequiredPoints,
            new SpaceLayout(new IElement?[6], FloorPlanEnum.Rectangle2X3),
            PositionX, PositionY, new List<IObjectInPathWay>(), new List<IObjectInPathWay>());
        source.SpaceLayout.Elements[0] = GetTestableElementWithParent(source, elementType);
        var destination = new SpacePe("", "", "", "", "", 0);

        systemUnderTest.Map(source, destination);

        TestSpace(destination, false);
        Assert.That(destination.SpaceLayout.ContainedElements.Count(), Is.EqualTo(1));
        Assert.That(destination.SpaceLayout.ContainedElements.First(),
            Is.InstanceOf(expectedElementPeType));

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.RequiredPoints = NewRequiredPoints;
        destination.SpaceLayout.Elements = new IElementPe[]
            {GetTestableElementPersistEntity(elementType)};
        destination.PositionX = NewPositionX;
        destination.PositionY = NewPositionY;
        destination.InBoundObjects = new List<IObjectInPathWayPe>();
        destination.OutBoundObjects = new List<IObjectInPathWayPe>();

        systemUnderTest.Map(destination, source);

        TestSpace(source, true);
        Assert.That(source.ContainedElements.Count(), Is.EqualTo(1));
        Assert.That(source.ContainedElements.First(), Is.InstanceOf(expectedElementType));
    }

    [Test]
    public void MapWorldAndWorldPersistEntity_WithoutSpaces_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new World(Name, Shortname, Authors, Language, Description, Goals,
            new List<Space>());
        var destination = new WorldPe("", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() => { Assert.That(destination.Spaces, Is.Empty); });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.Spaces = new List<SpacePe>();

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() => { Assert.That(source.Spaces, Is.Empty); });
    }

    [Test]
    public void MapWorldAndWorldPersistEntity_WithEmptySpace_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new World(Name, Shortname, Authors, Language, Description, Goals,
            new List<Space>());
        source.Spaces.Add(new Space(Name, Shortname, Authors, Description, Goals, RequiredPoints,
            null, PositionX, PositionY, new List<IObjectInPathWay>(), new List<IObjectInPathWay>()));
        var destination = new WorldPe("", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() =>
        {
            Assert.That(destination.Spaces, Has.Count.EqualTo(1));
            Assert.That(destination.Spaces[0].SpaceLayout.ContainedElements, Is.Empty);
        });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.Spaces = new List<SpacePe>()
        {
            new SpacePe(NewName, NewShortname, NewAuthors, NewDescription, NewGoals, NewRequiredPoints,
                null, NewPositionX, NewPositionY, new List<IObjectInPathWayPe>(), new List<IObjectInPathWayPe>())
        };

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() =>
        {
            Assert.That(source.Spaces, Has.Count.EqualTo(1));
            Assert.That(source.Spaces[0].ContainedElements, Is.Empty);
        });
    }

    [Test]
    public void MapWorldAndWorldPersistEntity_WithSpaceAndPathWay_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new World(Name, Shortname, Authors, Language, Description, Goals,
            new List<Space>());
        var space1 = GetTestableSpace();
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 3, 2);
        var space2 = GetTestableSpace();
        source.Spaces.Add(space1);
        source.PathWayConditions.Add(pathWayCondition);
        source.Spaces.Add(space2);
        var destination = new WorldPe("", "", "", "", "", "");

        source.Pathways.Add(new Pathway(space1, pathWayCondition));
        source.Pathways.Add(new Pathway(pathWayCondition, space2));
        space1.OutBoundObjects.Add(pathWayCondition);
        pathWayCondition.InBoundObjects.Add(space1);
        pathWayCondition.OutBoundObjects.Add(space2);
        space2.InBoundObjects.Add(pathWayCondition);

        systemUnderTest.Map(source, destination);

        var destinationSpace1 = destination.Spaces[0];
        var destinationCondition = destination.PathWayConditions[0];
        var destinationSpace2 = destination.Spaces[1];
        TestWorld(destination, false);
        Assert.Multiple(() =>
        {
            Assert.That(destination.Spaces, Has.Count.EqualTo(2));
            Assert.That(destination.PathWayConditions, Has.Count.EqualTo(1));
            Assert.That(destinationSpace1.SpaceLayout.ContainedElements.Count(), Is.EqualTo(1));
            Assert.That(destinationSpace1.OutBoundObjects, Does.Contain(destinationCondition));
            Assert.That(destinationSpace2.SpaceLayout.ContainedElements.Count(), Is.EqualTo(1));
            Assert.That(destinationSpace2.InBoundObjects, Does.Contain(destinationCondition));
            Assert.That(destinationCondition.InBoundObjects, Does.Contain(destinationSpace1));
            Assert.That(destinationCondition.OutBoundObjects, Does.Contain(destinationSpace2));
            Assert.That(destination.Pathways, Has.Count.EqualTo(2));
        });


        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        var spacePe1 = GetTestableNewSpacePersistEntity();
        var pathWayConditionPe = new PathWayConditionPe(ConditionEnumPe.And, 2, 1);
        var spacePe2 = GetTestableNewSpacePersistEntity();
        destination.Spaces = new List<SpacePe>() {spacePe1, spacePe2};
        destination.PathWayConditions = new List<PathWayConditionPe>() {pathWayConditionPe};
        destination.Pathways = new List<PathwayPe>
            {new(spacePe1, pathWayConditionPe), new(pathWayConditionPe, spacePe2)};
        ;
        spacePe1.OutBoundObjects.Add(pathWayConditionPe);
        pathWayConditionPe.InBoundObjects.Add(spacePe1);
        pathWayConditionPe.OutBoundObjects.Add(spacePe2);
        spacePe2.InBoundObjects.Add(pathWayConditionPe);

        {
            systemUnderTest.Map(destination, source);
            var sourceSpace1 = source.Spaces[0];
            var sourceCondition = source.PathWayConditions[0];
            var sourceSpace2 = source.Spaces[1];

            TestWorld(source, true);
            Assert.Multiple(() =>
            {
                Assert.That(source.Spaces, Has.Count.EqualTo(2));
                Assert.That(source.PathWayConditions, Has.Count.EqualTo(1));
                Assert.That(sourceSpace1.ContainedElements.Count(), Is.EqualTo(1));
                Assert.That(sourceSpace1.OutBoundObjects, Does.Contain(sourceCondition));
                Assert.That(sourceCondition.InBoundObjects, Does.Contain(sourceSpace1));
                Assert.That(sourceCondition.OutBoundObjects, Does.Contain(sourceSpace2));
                Assert.That(sourceSpace2.ContainedElements.Count(), Is.EqualTo(1));
                Assert.That(sourceSpace2.InBoundObjects, Does.Contain(sourceCondition));
                Assert.That(source.Pathways, Has.Count.EqualTo(2));
            });
        }

        {
            var newSource = systemUnderTest.Map<World>(destination);
            systemUnderTest.Map(destination, newSource);
            var newSourceSpace1 = source.Spaces[0];
            var newSourceCondition = source.PathWayConditions[0];
            var newSourceSpace2 = source.Spaces[1];

            TestWorld(newSource, true);
            Assert.Multiple(() =>
            {
                Assert.That(newSource.Spaces, Has.Count.EqualTo(2));
                Assert.That(newSource.PathWayConditions, Has.Count.EqualTo(1));
                Assert.That(newSourceSpace1.ContainedElements.Count(), Is.EqualTo(1));
                Assert.That(newSourceSpace1.OutBoundObjects, Does.Contain(newSourceCondition));
                Assert.That(newSourceCondition.InBoundObjects, Does.Contain(newSourceSpace1));
                Assert.That(newSourceCondition.OutBoundObjects, Does.Contain(newSourceSpace2));
                Assert.That(newSourceSpace2.ContainedElements.Count(), Is.EqualTo(1));
                Assert.That(newSourceSpace2.InBoundObjects, Does.Contain(newSourceCondition));
                Assert.That(newSource.Pathways, Has.Count.EqualTo(2));
            });
        }
    }

    /// <summary>
    /// Regression test for https://github.com/projektadler/autorentool/issues/254
    /// </summary>
    [Test]
    public void
        MapWorldAndWorldPersistEntity_WithMultipleSpacesAndPathWays_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new World(Name, Shortname, Authors, Language, Description, Goals,
            new List<Space>());
        var space1 = GetTestableSpace();
        space1.Name = "space1";
        var space2 = GetTestableSpace();
        space2.Name = "space2";
        var space3 = GetTestableSpace();
        space3.Name = "space3";
        source.Spaces.Add(space1);
        source.Spaces.Add(space2);
        source.Spaces.Add(space3);
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 2, 1);
        source.PathWayConditions.Add(pathWayCondition);

        source.Pathways.Add(new Pathway(space1, space2));
        space1.OutBoundObjects.Add(space2);
        space2.InBoundObjects.Add(space1);
        source.Pathways.Add(new Pathway(space2, space3));
        space1.OutBoundObjects.Add(space3);
        space2.InBoundObjects.Add(space2);
        source.Pathways.Add(new Pathway(space3, pathWayCondition));
        space3.OutBoundObjects.Add(pathWayCondition);
        pathWayCondition.InBoundObjects.Add(space3);

        var persistEntity = systemUnderTest.Map<WorldPe>(source);
        var restored = systemUnderTest.Map<World>(persistEntity);

        Assert.That(restored.Pathways, Has.Count.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(((Space) restored.Pathways[0].SourceObject).Name, Is.EqualTo("space1"));
            Assert.That(((Space) restored.Pathways[1].SourceObject).Name, Is.EqualTo("space2"));
            Assert.That(((Space) restored.Pathways[0].TargetObject).Name, Is.EqualTo("space2"));
            Assert.That(((Space) restored.Pathways[1].TargetObject).Name, Is.EqualTo("space3"));
            Assert.That(((Space) restored.Pathways[2].SourceObject).Name, Is.EqualTo("space3"));
            Assert.That(((PathWayCondition) restored.Pathways[2].TargetObject).Condition,
                Is.EqualTo(ConditionEnum.And));
        });
    }

    #region testable Content/Element/Space/World

    private static Content GetTestableContent()
    {
        return new Content(Name, Type, Filepath);
    }

    private static ContentPe GetTestableNewContentPersistEntity()
    {
        return new ContentPe(NewName, NewType, NewFilepath);
    }

    private static Element GetTestableElementWithParent(Space parent, ElementType elementType)
    {
        return elementType switch
        {
            ElementType.TextTransfer => new TextTransferElement(Name, Shortname, parent,
                GetTestableContent(), Url, Authors, Description, Goals, Difficulty, Workload, Points, PositionX,
                PositionY),
            ElementType.ImageTransfer => new ImageTransferElement(Name, Shortname, parent,
                GetTestableContent(), Url, Authors, Description, Goals, Difficulty, Workload, Points, PositionX,
                PositionY),
            ElementType.VideoTransfer => new VideoTransferElement(Name, Shortname, parent,
                GetTestableContent(), Url, Authors, Description, Goals, Difficulty, Workload, Points, PositionX,
                PositionY),
            ElementType.PdfTransfer => new PdfTransferElement(Name, Shortname, parent,
                GetTestableContent(), Url, Authors, Description, Goals, Difficulty, Workload, Points, PositionX,
                PositionY),
            ElementType.VideoActivation => new VideoActivationElement(Name, Shortname, parent,
                GetTestableContent(), Url, Authors, Description, Goals, Difficulty, Workload, Points, PositionX,
                PositionY),
            ElementType.H5PActivation => new H5PActivationElement(Name, Shortname, parent,
                GetTestableContent(), Url, Authors, Description, Goals, Difficulty, Workload, Points, PositionX,
                PositionY),
            ElementType.H5PInteraction => new H5PInteractionElement(Name, Shortname, parent,
                GetTestableContent(), Url, Authors, Description, Goals, Difficulty, Workload, Points, PositionX,
                PositionY),
            ElementType.H5PTest => new H5PTestElement(Name, Shortname, parent, GetTestableContent(), Url,
                Authors, Description, Goals, Difficulty, Workload, Points, PositionX, PositionY),
            _ => throw new ArgumentOutOfRangeException(nameof(elementType), elementType, null)
        };
    }

    private static ElementPe GetTestableElementPersistEntity(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.TextTransfer:
                return new TextTransferElementPe(NewName, NewShortname, GetTestableNewContentPersistEntity(), NewUrl,
                    NewAuthors, NewDescription, NewGoals, NewDifficultyPe, NewWorkload, NewPoints, NewPositionX,
                    NewPositionY);
            case ElementType.ImageTransfer:
                return new ImageTransferElementPe(NewName, NewShortname, GetTestableNewContentPersistEntity(), NewUrl,
                    NewAuthors, NewDescription, NewGoals, NewDifficultyPe, NewWorkload, NewPoints, NewPositionX,
                    NewPositionY);
            case ElementType.VideoTransfer:
                return new VideoTransferElementPe(NewName, NewShortname, GetTestableNewContentPersistEntity(), NewUrl,
                    NewAuthors, NewDescription, NewGoals, NewDifficultyPe, NewWorkload, NewPoints, NewPositionX,
                    NewPositionY);
            case ElementType.PdfTransfer:
                return new PdfTransferElementPe(NewName, NewShortname, GetTestableNewContentPersistEntity(), NewUrl,
                    NewAuthors, NewDescription, NewGoals, NewDifficultyPe, NewWorkload, NewPoints, NewPositionX,
                    NewPositionY);
            case ElementType.VideoActivation:
                return new VideoActivationElementPe(NewName, NewShortname, GetTestableNewContentPersistEntity(), NewUrl,
                    NewAuthors, NewDescription, NewGoals, NewDifficultyPe, NewWorkload, NewPoints, NewPositionX,
                    NewPositionY);
            case ElementType.H5PActivation:
                return new H5PActivationElementPe(NewName, NewShortname, GetTestableNewContentPersistEntity(), NewUrl,
                    NewAuthors, NewDescription, NewGoals, NewDifficultyPe, NewWorkload, NewPoints, NewPositionX,
                    NewPositionY);
            case ElementType.H5PInteraction:
                return new H5PInteractionElementPe(NewName, NewShortname, GetTestableNewContentPersistEntity(), NewUrl,
                    NewAuthors, NewDescription, NewGoals, NewDifficultyPe, NewWorkload, NewPoints, NewPositionX,
                    NewPositionY);
            case ElementType.H5PTest:
                return new H5PTestElementPe(NewName, NewShortname, GetTestableNewContentPersistEntity(), NewUrl,
                    NewAuthors, NewDescription, NewGoals, NewDifficultyPe, NewWorkload, NewPoints, NewPositionX,
                    NewPositionY);
            default:
                throw new ArgumentOutOfRangeException(nameof(elementType), elementType, null);
        }
    }

    public enum ElementType
    {
        TextTransfer,
        ImageTransfer,
        VideoTransfer,
        PdfTransfer,
        VideoActivation,
        H5PActivation,
        H5PInteraction,
        H5PTest
    }

    private static Space GetTestableSpace()
    {
        var space = new Space(Name, Shortname, Authors, Description, Goals, RequiredPoints,
            new SpaceLayout(new IElement[6], FloorPlanEnum.Rectangle2X3), PositionX, PositionY);
        var element = GetTestableElementWithParent(space, ElementType.TextTransfer);
        space.SpaceLayout.Elements[0] = element;
        return space;
    }

    private static SpacePe GetTestableNewSpacePersistEntity()
    {
        return new SpacePe(NewName, NewShortname, NewAuthors, NewDescription, NewGoals, NewRequiredPoints,
            new SpaceLayoutPe(
                new IElementPe[] {GetTestableElementPersistEntity(ElementType.TextTransfer)},
                FloorPlanEnumPe.Rectangle2X3), NewPositionX, NewPositionY);
    }

    #endregion

    #region static test methods

    private static void TestWorld(object destination, bool useNewFields)
    {
        switch (destination)
        {
            case World world:
                Assert.Multiple(() =>
                {
                    Assert.That(world.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(world.Shortname, Is.EqualTo(useNewFields ? NewShortname : Shortname));
                    Assert.That(world.Authors, Is.EqualTo(useNewFields ? NewAuthors : Authors));
                    Assert.That(world.Language, Is.EqualTo(useNewFields ? NewLanguage : Language));
                    Assert.That(world.Description, Is.EqualTo(useNewFields ? NewDescription : Description));
                    Assert.That(world.Goals, Is.EqualTo(useNewFields ? NewGoals : Goals));
                    TestSpacesList(world.Spaces, useNewFields);
                });
                break;
            case WorldPe world:
                Assert.Multiple(() =>
                {
                    Assert.That(world.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(world.Shortname, Is.EqualTo(useNewFields ? NewShortname : Shortname));
                    Assert.That(world.Authors, Is.EqualTo(useNewFields ? NewAuthors : Authors));
                    Assert.That(world.Language, Is.EqualTo(useNewFields ? NewLanguage : Language));
                    Assert.That(world.Description, Is.EqualTo(useNewFields ? NewDescription : Description));
                    Assert.That(world.Goals, Is.EqualTo(useNewFields ? NewGoals : Goals));
                    TestSpacesList(world.Spaces, useNewFields);
                });
                break;
        }
    }

    private static void TestSpacesList(object worldSpaces, bool useNewFields)
    {
        switch (worldSpaces)
        {
            case List<Space> spaces:
                Assert.Multiple(() =>
                {
                    foreach (var space in spaces)
                    {
                        TestSpace(space, useNewFields);
                    }
                });
                break;
            case List<SpacePe> spaces:
                Assert.Multiple(() =>
                {
                    foreach (var space in spaces)
                    {
                        TestSpace(space, useNewFields);
                    }
                });
                break;
        }
    }

    private static void TestSpace(object testSpace, bool useNewFields)
    {
        switch (testSpace)
        {
            case Space space:
                Assert.Multiple(() =>
                {
                    Assert.That(space.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(space.Shortname, Is.EqualTo(useNewFields ? NewShortname : Shortname));
                    Assert.That(space.Authors, Is.EqualTo(useNewFields ? NewAuthors : Authors));
                    Assert.That(space.Description, Is.EqualTo(useNewFields ? NewDescription : Description));
                    Assert.That(space.Goals, Is.EqualTo(useNewFields ? NewGoals : Goals));
                    Assert.That(space.RequiredPoints, Is.EqualTo(useNewFields ? NewRequiredPoints : RequiredPoints));
                    TestElementsList(space.ContainedElements, space, useNewFields);
                    Assert.That(space.PositionX, Is.EqualTo(useNewFields ? NewPositionX : PositionX));
                    Assert.That(space.PositionY, Is.EqualTo(useNewFields ? NewPositionY : PositionY));
                });
                break;
            case SpacePe space:
                Assert.Multiple(() =>
                {
                    Assert.That(space.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(space.Shortname, Is.EqualTo(useNewFields ? NewShortname : Shortname));
                    Assert.That(space.Authors, Is.EqualTo(useNewFields ? NewAuthors : Authors));
                    Assert.That(space.Description, Is.EqualTo(useNewFields ? NewDescription : Description));
                    Assert.That(space.Goals, Is.EqualTo(useNewFields ? NewGoals : Goals));
                    Assert.That(space.RequiredPoints, Is.EqualTo(useNewFields ? NewRequiredPoints : RequiredPoints));
                    TestElementsList(space.SpaceLayout.ContainedElements, space, useNewFields);
                    Assert.That(space.PositionX, Is.EqualTo(useNewFields ? NewPositionX : PositionX));
                    Assert.That(space.PositionY, Is.EqualTo(useNewFields ? NewPositionY : PositionY));
                });
                break;
        }
    }

    private static void TestElementsList(object worldElements, object? parent, bool useNewFields)
    {
        switch (worldElements)
        {
            case List<Element> elements:
                Assert.Multiple(() =>
                {
                    foreach (var element in elements)
                    {
                        TestElement(element, parent, useNewFields);
                    }
                });
                break;
            case List<ElementPe> elements:
                Assert.Multiple(() =>
                {
                    foreach (var element in elements)
                    {
                        TestElement(element, parent, useNewFields);
                    }
                });
                break;
        }
    }

    private static void TestElement(object testElement, object? parent, bool useNewFields)
    {
        switch (testElement)
        {
            case Element element:
                Assert.Multiple(() =>
                {
                    Assert.That(element.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(element.Shortname, Is.EqualTo(useNewFields ? NewShortname : Shortname));
                    TestContent(element.Content, useNewFields);
                    Assert.That(element.Url, Is.EqualTo(useNewFields ? NewUrl : Url));
                    Assert.That(element.Authors, Is.EqualTo(useNewFields ? NewAuthors : Authors));
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
            case ElementPe element:
                Assert.Multiple(() =>
                {
                    Assert.That(element.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(element.Shortname, Is.EqualTo(useNewFields ? NewShortname : Shortname));
                    TestContent(element.Content, useNewFields);
                    Assert.That(element.Url, Is.EqualTo(useNewFields ? NewUrl : Url));
                    Assert.That(element.Authors, Is.EqualTo(useNewFields ? NewAuthors : Authors));
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

    private static void TestContent(object elementContent, bool useNewFields)
    {
        switch (elementContent)
        {
            case Content content:
                Assert.Multiple(() =>
                {
                    Assert.That(content.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(content.Type, Is.EqualTo(useNewFields ? NewType : Type));
                    Assert.That(content.Filepath, Is.EqualTo(useNewFields ? NewFilepath : Filepath));
                });
                break;
            case ContentPe content:
                Assert.Multiple(() =>
                {
                    Assert.That(content.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(content.Type, Is.EqualTo(useNewFields ? NewType : Type));
                    Assert.That(content.Filepath, Is.EqualTo(useNewFields ? NewFilepath : Filepath));
                });
                break;
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