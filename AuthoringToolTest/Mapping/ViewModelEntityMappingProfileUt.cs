using AuthoringTool.Mapping;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using BusinessLogic.Entities;
using NUnit.Framework;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.Content;
using Presentation.PresentationLogic.Element;
using Presentation.PresentationLogic.Element.ActivationElement;
using Presentation.PresentationLogic.Element.InteractionElement;
using Presentation.PresentationLogic.Element.TestElement;
using Presentation.PresentationLogic.Element.TransferElement;
using Presentation.PresentationLogic.PathWay;
using Presentation.PresentationLogic.Space;
using Presentation.PresentationLogic.Space.SpaceLayout;
using Presentation.PresentationLogic.World;
using Shared;

namespace AuthoringToolTest.Mapping;

[TestFixture]
public class ViewModelEntityMappingProfileUt
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
            ViewModelEntityMappingProfile.Configure(cfg);
            cfg.AddCollectionMappers();
        });

        Assert.That(() => mapper.AssertConfigurationIsValid(), Throws.Nothing);
    }

    [Test]
    public void MapContentAndContentViewModel_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new Content(Name, Type, Filepath);
        var destination = new ContentViewModel("", "", "bar/baz/buz.txt");

        systemUnderTest.Map(source, destination);

        TestContent(destination, false);

        destination.Name = NewName;
        destination.Type = NewType;
        destination.Filepath = NewFilepath;

        systemUnderTest.Map(destination, source);

        TestContent(source, true);
    }

    [Test]
    public void MapElementAndElementViewModel_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var content = GetTestableContent();
        var source = new Element(Name, Shortname, content, Url, Authors, Description, Goals,
            Difficulty, null, Workload, Points, PositionX, PositionY);
        var destination = new ElementViewModel("", "",
            new ContentViewModel("", "", Filepath), Url, "", "", "", ElementDifficultyEnum.None);

        systemUnderTest.Map(source, destination);

        TestElement(destination, null, false);
        Assert.That(destination.Id, Is.EqualTo(source.Id));

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Content = new ContentViewModel(NewName, NewType, NewFilepath);
        destination.Url = NewUrl;
        destination.Authors = NewAuthors;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.Difficulty = NewDifficulty;
        destination.Workload = NewWorkload;
        destination.Points = NewPoints;
        destination.PositionX = NewPositionX;
        destination.PositionY = NewPositionY;

        systemUnderTest.Map(destination, source);

        TestElement(source, null, true);
        Assert.That(destination.Id, Is.EqualTo(source.Id));
    }

    [Test]
    public void MapSpaceAndSpaceViewModel_WithoutElement_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new Space(Name, Shortname, Authors, Description, Goals, RequiredPoints, null, PositionX,
            PositionY);
        var destination = new SpaceViewModel("", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestSpace(destination, false);
        Assert.That(destination.ContainedElements, Is.Empty);

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.RequiredPoints = NewRequiredPoints;
        destination.SpaceLayout.Elements = Array.Empty<IElementViewModel?>();
        destination.PositionX = NewPositionX;
        destination.PositionY = NewPositionY;

        systemUnderTest.Map(destination, source);

        TestSpace(source, true);
        Assert.That(source.ContainedElements, Is.Empty);
    }

    [TestCase(ElementType.TextTransfer, typeof(TextTransferElementViewModel), typeof(TextTransferElement))]
    [TestCase(ElementType.ImageTransfer, typeof(ImageTransferElementViewModel), typeof(ImageTransferElement))]
    [TestCase(ElementType.VideoTransfer, typeof(VideoTransferElementViewModel), typeof(VideoTransferElement))]
    [TestCase(ElementType.PdfTransfer, typeof(PdfTransferElementViewModel), typeof(PdfTransferElement))]
    [TestCase(ElementType.VideoActivation, typeof(VideoActivationElementViewModel), typeof(VideoActivationElement))]
    [TestCase(ElementType.H5PActivation, typeof(H5PActivationElementViewModel), typeof(H5PActivationElement))]
    [TestCase(ElementType.H5PInteraction, typeof(H5PInteractionElementViewModel), typeof(H5PInteractionElement))]
    [TestCase(ElementType.H5PTest, typeof(H5PTestElementViewModel), typeof(H5PTestElement))]
    public void MapSpaceAndSpaceViewModel_WithElement_TestMappingIsValid
        (ElementType elementType, Type expectedElementViewModelType, Type expectedElementType)
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new Space(Name, Shortname, Authors, Description, Goals, RequiredPoints,
            new SpaceLayout(new IElement?[6], FloorPlanEnum.Rectangle2X3),
            PositionX, PositionY);
        source.SpaceLayout.Elements[0] = GetTestableElementWithParent(source, elementType);
        var destination = new SpaceViewModel("", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestSpace(destination, false);
        Assert.That(destination.ContainedElements.Count(), Is.EqualTo(1));
        Assert.That(destination.ContainedElements.First(), Is.InstanceOf(expectedElementViewModelType));
        Assert.That(destination.Id, Is.EqualTo(source.Id));

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.RequiredPoints = NewRequiredPoints;
        destination.SpaceLayout.Elements = new IElementViewModel?[6]
            {GetTestableElementViewModelWithParent(destination, elementType), null, null, null, null, null};
        destination.PositionX = NewPositionX;
        destination.PositionY = NewPositionY;

        systemUnderTest.Map(destination, source);

        TestSpace(source, true);
        Assert.That(source.ContainedElements.Count(), Is.EqualTo(1));
        Assert.That(source.ContainedElements.First(), Is.InstanceOf(expectedElementType));
        Assert.That(destination.Id, Is.EqualTo(source.Id));
    }

    [Test]
    public void MapWorldAndWorldViewModel_WithoutSpaces_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new World(Name, Shortname, Authors, Language, Description, Goals,
            new List<Space>());
        var destination = new WorldViewModel("", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() => { Assert.That(destination.Spaces, Is.Empty); });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.Spaces = new List<ISpaceViewModel>();

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() => { Assert.That(source.Spaces, Is.Empty); });
    }

    [Test]
    public void MapWorldAndWorldViewModel_WithEmptySpace_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new World(Name, Shortname, Authors, Language, Description, Goals,
            new List<Space>());
        source.Spaces.Add(new Space(Name, Shortname, Authors, Description, Goals, RequiredPoints, null,
            PositionX,
            PositionY));
        var destination = new WorldViewModel("", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() =>
        {
            Assert.That(destination.Spaces, Has.Count.EqualTo(1));
            Assert.That(destination.Spaces.First().ContainedElements, Is.Empty);
        });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.Spaces = new List<ISpaceViewModel>()
        {
            new SpaceViewModel(NewName, NewShortname, NewAuthors, NewDescription, NewGoals, NewRequiredPoints,
                null, NewPositionX, NewPositionY)
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
    public void MapWorldAndWorldViewModel_WithSpace_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new World(Name, Shortname, Authors, Language, Description, Goals,
            new List<Space>());
        source.Spaces.Add(GetTestableSpace());
        var destination = new WorldViewModel("", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() =>
        {
            Assert.That(destination.Spaces, Has.Count.EqualTo(1));
            Assert.That(destination.Spaces.First().ContainedElements.Count, Is.EqualTo(1));
        });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.Spaces = new List<ISpaceViewModel>() {GetTestableNewSpaceViewModel()};

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() =>
        {
            Assert.That(source.Spaces, Has.Count.EqualTo(1));
            Assert.That(source.Spaces[0].ContainedElements.Count(), Is.EqualTo(1));
        });
    }

    [Test]
    public void MapWorldAndWorldViewModel_WithSpacesAndPathWay_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new World(Name, Shortname, Authors, Language, Description, Goals,
            new List<Space>());
        var space1 = GetTestableSpace();
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 3, 2);
        source.Spaces.Add(space1);
        source.PathWayConditions.Add(pathWayCondition);
        var destination = new WorldViewModel("", "", "", "", "", "");

        source.Pathways.Add(new Pathway(space1, pathWayCondition));

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);

        Assert.Multiple(() =>
        {
            Assert.That(destination.Spaces, Has.Count.EqualTo(1));
            Assert.That(destination.PathWayConditions, Has.Count.EqualTo(1));
            Assert.That(destination.Spaces.First().ContainedElements.Count(), Is.EqualTo(1));
            Assert.That(destination.Spaces.First().OutBoundObjects, Has.Count.EqualTo(1));
            Assert.That(destination.PathWayConditions.First().InBoundObjects, Has.Count.EqualTo(1));
            Assert.That(destination.PathWays, Has.Count.EqualTo(1));
        });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;

        var spaceVm1 = GetTestableNewSpaceViewModel();
        var pathWayConditionVm = new PathWayConditionViewModel(ConditionEnum.And, 2, 1);
        destination.Spaces = new List<ISpaceViewModel>() {spaceVm1};
        destination.PathWayConditions = new List<PathWayConditionViewModel>() {pathWayConditionVm};
        destination.PathWays = new List<IPathWayViewModel>();
        destination.PathWays.Add(new PathwayViewModel(spaceVm1, pathWayConditionVm));

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() =>
        {
            Assert.That(source.Spaces, Has.Count.EqualTo(1));
            Assert.That(source.PathWayConditions, Has.Count.EqualTo(1));
            Assert.That(source.Spaces[0].ContainedElements.Count(), Is.EqualTo(1));
            Assert.That(source.Spaces[0].OutBoundObjects, Has.Count.EqualTo(1));
            Assert.That(source.PathWayConditions[0].InBoundObjects, Has.Count.EqualTo(1));
            Assert.That(source.Pathways, Has.Count.EqualTo(1));
        });
    }

    /// <summary>
    /// This test tests whether or not the AutoMapper.Collection configuration works as intended.
    /// See https://github.com/AutoMapper/AutoMapper.Collection
    /// </summary>
    [Test]
    public void MapWorldAndWorldViewModel_WithSpaces_ObjectsStayEqual()
    {
        var elementVm1 =
            new ElementViewModel("el1", Shortname, new ContentViewModel("foo", "bar", Filepath), Url,
                Authors,
                Description, Goals, Difficulty);

        var space = new SpaceViewModel("testSpace", Shortname, Authors, Description, Goals, RequiredPoints,
            new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3)
            {
                Elements = new IElementViewModel?[6] {elementVm1, null, null, null, null, null}
            })
        {
            SelectedElement = elementVm1
        };
        elementVm1.Parent = space;

        var worldVm = new WorldViewModel("world", Shortname, Authors, Language, Description, Goals, true,
            new List<ISpaceViewModel> {space})
        {
            SelectedObject = space
        };


        var systemUnderTest = CreateTestableMapper();

        var worldEntity = systemUnderTest.Map<World>(worldVm);
        worldVm.Spaces.First().SpaceLayout.ContainedElements.First().Authors = "foooooooooo";


        //map back into viewmodel - with update syntax
        systemUnderTest.Map(worldEntity, worldVm);

        //we would expect that the objects are still the same and we retained view specific information
        Assert.That(worldVm.Spaces.First(), Is.EqualTo(space));

        Assert.Multiple(() =>
        {
            Assert.That(worldVm.Spaces.First().SpaceLayout.ContainedElements.Count(), Is.EqualTo(1));
            Assert.That(worldVm.Spaces.First().SpaceLayout.ContainedElements.First().Authors,
                Is.EqualTo("foooooooooo"));
            Assert.That(worldVm.Spaces.First().ContainedElements.First(), Is.EqualTo(elementVm1));
            Assert.That(worldVm.SelectedObject, Is.EqualTo(space));
            Assert.That(worldVm.Spaces.First().SelectedElement, Is.EqualTo(elementVm1));
        });
    }

    [Test]
    public void MapAuthoringToolWorkspaceAndAuthoringToolWorkspaceViewModel_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var world1 = new World("world1", Shortname, Authors, Language, Description, Goals);
        var world2 = new World("world2", Shortname, Authors, Language, Description, Goals);
        var source = new AuthoringToolWorkspace(world2, new List<World> {world1, world2});
        var destination = new AuthoringToolWorkspaceViewModel();

        systemUnderTest.Map(source, destination);

        Assert.That(destination.Worlds, Is.Not.Null);
        Assert.That(destination.Worlds.Count, Is.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(destination.Worlds.First().Name, Is.EqualTo("world1"));
            Assert.That(destination.Worlds.Last().Name, Is.EqualTo("world2"));
            Assert.That(destination.SelectedWorld, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(destination.SelectedWorld!.Name, Is.EqualTo("world2"));
            Assert.That(destination.SelectedWorld, Is.EqualTo(destination.Worlds.Last()));
        });
        destination.SelectedWorld = destination.Worlds.First();

        systemUnderTest.Map(destination, source);

        Assert.That(source.Worlds, Is.Not.Null);
        Assert.That(source.Worlds, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(source.Worlds.First(), Is.EqualTo(world1));
            Assert.That(source.Worlds.Last(), Is.EqualTo(world2));
            Assert.That(source.Worlds.First().Name, Is.EqualTo("world1"));
            Assert.That(source.Worlds.Last().Name, Is.EqualTo("world2"));
            Assert.That(source.SelectedWorld, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(source.SelectedWorld!.Name, Is.EqualTo("world1"));
            Assert.That(source.SelectedWorld, Is.EqualTo(source.Worlds.First()));
        });

        // Test the private constructor from AuthoringToolWorkspace
        var source2 = systemUnderTest.Map<AuthoringToolWorkspace>(destination);

        Assert.That(source2.Worlds, Is.Not.Null);
        Assert.That(source2.Worlds, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(source2.Worlds.First().Name, Is.EqualTo("world1"));
            Assert.That(source2.Worlds.Last().Name, Is.EqualTo("world2"));
            Assert.That(source2.SelectedWorld, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(source2.SelectedWorld!.Name, Is.EqualTo("world1"));
            Assert.That(source2.SelectedWorld, Is.EqualTo(source2.Worlds.First()));
        });
    }

    #region testable Content/Element/Space/World

    private static Content GetTestableContent()
    {
        return new Content(Name, Type, Filepath);
    }

    private static ContentViewModel GetTestableNewContentViewModel()
    {
        return new ContentViewModel(NewName, NewType, NewFilepath);
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

    private static ElementViewModel GetTestableElementViewModelWithParent(SpaceViewModel parent,
        ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.TextTransfer:
                return new TextTransferElementViewModel(NewName, NewShortname, parent,
                    GetTestableNewContentViewModel(), NewUrl, NewAuthors, NewDescription, NewGoals, NewDifficulty,
                    NewWorkload, NewPoints, NewPositionX, NewPositionY);
            case ElementType.ImageTransfer:
                return new ImageTransferElementViewModel(NewName, NewShortname, parent,
                    GetTestableNewContentViewModel(), NewUrl, NewAuthors, NewDescription, NewGoals, NewDifficulty,
                    NewWorkload, NewPoints, NewPositionX, NewPositionY);
            case ElementType.VideoTransfer:
                return new VideoTransferElementViewModel(NewName, NewShortname, parent,
                    GetTestableNewContentViewModel(), NewUrl, NewAuthors, NewDescription, NewGoals, NewDifficulty,
                    NewWorkload, NewPoints, NewPositionX, NewPositionY);
            case ElementType.PdfTransfer:
                return new PdfTransferElementViewModel(NewName, NewShortname, parent,
                    GetTestableNewContentViewModel(), NewUrl, NewAuthors, NewDescription, NewGoals, NewDifficulty,
                    NewWorkload, NewPoints, NewPositionX, NewPositionY);
            case ElementType.VideoActivation:
                return new VideoActivationElementViewModel(NewName, NewShortname, parent,
                    GetTestableNewContentViewModel(), NewUrl, NewAuthors, NewDescription, NewGoals, NewDifficulty,
                    NewWorkload, NewPoints, NewPositionX, NewPositionY);
            case ElementType.H5PActivation:
                return new H5PActivationElementViewModel(NewName, NewShortname, parent,
                    GetTestableNewContentViewModel(), NewUrl, NewAuthors, NewDescription, NewGoals, NewDifficulty,
                    NewWorkload, NewPoints, NewPositionX, NewPositionY);
            case ElementType.H5PInteraction:
                return new H5PInteractionElementViewModel(NewName, NewShortname, parent,
                    GetTestableNewContentViewModel(), NewUrl, NewAuthors, NewDescription, NewGoals, NewDifficulty,
                    NewWorkload, NewPoints, NewPositionX, NewPositionY);
            case ElementType.H5PTest:
                return new H5PTestElementViewModel(NewName, NewShortname, parent,
                    GetTestableNewContentViewModel(), NewUrl, NewAuthors, NewDescription, NewGoals, NewDifficulty,
                    NewWorkload, NewPoints, NewPositionX, NewPositionY);
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

    private static SpaceViewModel GetTestableNewSpaceViewModel()
    {
        var space = new SpaceViewModel(NewName, NewShortname, NewAuthors, NewDescription, NewGoals,
            NewRequiredPoints,
            new SpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3), NewPositionX, NewPositionY);
        var element = GetTestableElementViewModelWithParent(space, ElementType.TextTransfer);
        space.SpaceLayout.PutElement(0, element);
        return space;
    }

    #endregion

    #region static test methods

    private static void TestWorld(object destination, bool useNewFields)
    {
        switch (destination)
        {
            case WorldViewModel world:
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
        }
    }

    private static void TestSpacesList(object worldSpaces, bool useNewFields)
    {
        switch (worldSpaces)
        {
            case List<ISpaceViewModel> spaces:
                Assert.Multiple(() =>
                {
                    foreach (var space in spaces)
                    {
                        TestSpace(space, useNewFields);
                    }
                });
                break;
            case List<Space> spaces:
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
            case SpaceViewModel space:
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
        }
    }

    private static void TestElementsList(object worldElements, object? parent, bool useNewFields)
    {
        switch (worldElements)
        {
            case List<IElementViewModel> elements:
                Assert.Multiple(() =>
                {
                    foreach (var element in elements)
                    {
                        TestElement(element, parent, useNewFields);
                    }
                });
                break;
            case List<Element> elements:
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
            case ElementViewModel element:
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
        }
    }

    private static void TestContent(object elementContent, bool useNewFields)
    {
        switch (elementContent)
        {
            case ContentViewModel content:
                Assert.Multiple(() =>
                {
                    Assert.That(content.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(content.Type, Is.EqualTo(useNewFields ? NewType : Type));
                    Assert.That(content.Filepath, Is.EqualTo(useNewFields ? NewFilepath : Filepath));
                });
                break;
            case Content content:
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
        var mapper = new MapperConfiguration(cfg =>
        {
            ViewModelEntityMappingProfile.Configure(cfg);
            cfg.AddCollectionMappers();
        });
        var systemUnderTest = mapper.CreateMapper();
        return systemUnderTest;
    }
}