using AuthoringTool;
using AutoMapper;
using BusinessLogic.Entities;
using NUnit.Framework;
using PersistEntities;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningElement.ActivationElement;
using Presentation.PresentationLogic.LearningElement.InteractionElement;
using Presentation.PresentationLogic.LearningElement.TestElement;
using Presentation.PresentationLogic.LearningElement.TransferElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningSpace.SpaceLayout;
using Presentation.PresentationLogic.LearningWorld;
using Shared;
using MapperConfiguration = AutoMapper.MapperConfiguration;

namespace AuthoringToolTest;

[TestFixture]
public class MappingProfileUt
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
    private const LearningElementDifficultyEnum Difficulty = LearningElementDifficultyEnum.Easy;
    private const LearningElementDifficultyEnumPe DifficultyPe = LearningElementDifficultyEnumPe.Easy;
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
        var mapper = new MapperConfiguration(MappingProfile.Configure);

        Assert.That(() => mapper.AssertConfigurationIsValid(), Throws.Nothing);
    }

    [Test]
    public void MapLearningContentAndLearningContentViewModel_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningContent(Name, Type, Filepath);
        var destination = new LearningContentViewModel("", "", "bar/baz/buz.txt");

        systemUnderTest.Map(source, destination);

        TestContent(destination, false);

        destination.Name = NewName;
        destination.Type = NewType;
        destination.Filepath = NewFilepath;

        systemUnderTest.Map(destination, source);

        TestContent(source, true);
    }

    [Test]
    public void MapLearningContentAndLearningContentPersistEntity_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningContent(Name, Type, Filepath);
        var destination = new LearningContentPe("", "", "");

        systemUnderTest.Map(source, destination);

        TestContent(destination, false);

        destination.Name = NewName;
        destination.Type = NewType;
        destination.Filepath = NewFilepath;

        systemUnderTest.Map(destination, source);

        TestContent(source, true);
    }

    [Test]
    public void MapLearningElementAndLearningElementViewModel_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var content = GetTestableContent();
        var source = new LearningElement(Name, Shortname, content, Url, Authors, Description, Goals,
            Difficulty, null, Workload, Points, PositionX, PositionY);
        var destination = new LearningElementViewModel("", "",
            new LearningContentViewModel("", "", Filepath), Url, "", "", "", LearningElementDifficultyEnum.None);

        systemUnderTest.Map(source, destination);

        TestElement(destination, null, false);
        Assert.That(destination.Id, Is.EqualTo(source.Id));

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.LearningContent = new LearningContentViewModel(NewName, NewType, NewFilepath);
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
    public void MapLearningElementAndLearningElementPersistEntity_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var content = GetTestableContent();
        var source = new LearningElement(Name, Shortname, content, Url, Authors, Description, Goals,
            Difficulty, null, Workload, Points, PositionX, PositionY);
        var destination = new LearningElementPe("", "", new LearningContentPe("", "", "bar/baz/buz.txt"), "google.com","", "", "",
            LearningElementDifficultyEnumPe.None);

        systemUnderTest.Map(source, destination);

        TestElement(destination, null, false);

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.LearningContent = new LearningContentPe(NewName, NewType, NewFilepath);
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
    public void MapLearningSpaceAndLearningSpaceViewModel_WithoutLearningElement_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningSpace(Name, Shortname, Authors, Description, Goals, RequiredPoints, null, PositionX, PositionY);
        var destination = new LearningSpaceViewModel("", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestSpace(destination, false);
        Assert.That(destination.ContainedLearningElements, Is.Empty);

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.RequiredPoints = NewRequiredPoints;
        destination.LearningSpaceLayout.LearningElements = Array.Empty<ILearningElementViewModel?>();
        destination.PositionX = NewPositionX;
        destination.PositionY = NewPositionY;

        systemUnderTest.Map(destination, source);

        TestSpace(source, true);
        Assert.That(source.ContainedLearningElements, Is.Empty);
    }

    [Test]
    public void MapLearningSpaceAndLearningSpacePersistEntity_WithoutLearningElement_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningSpace(Name, Shortname, Authors, Description, Goals, RequiredPoints, null, 
            PositionX, PositionY, new List<IObjectInPathWay>(), new List<IObjectInPathWay>());
        var destination = new LearningSpacePe("", "", "", "", "", 0);

        systemUnderTest.Map(source, destination);

        TestSpace(destination, false);
        Assert.That(destination.LearningSpaceLayout.ContainedLearningElements, Is.Empty);

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.RequiredPoints = NewRequiredPoints;
        destination.LearningSpaceLayout.LearningElements = Array.Empty<ILearningElementPe?>();
        destination.PositionX = NewPositionX;
        destination.PositionY = NewPositionY;
        destination.InBoundObjects = new List<IObjectInPathWayPe>();
        destination.OutBoundObjects = new List<IObjectInPathWayPe>();

        systemUnderTest.Map(destination, source);

        TestSpace(source, true);
        Assert.That(source.ContainedLearningElements, Is.Empty);
    }

    [TestCase(ElementType.TextTransfer, typeof(TextTransferElementViewModel), typeof(TextTransferElement))]
    [TestCase(ElementType.ImageTransfer, typeof(ImageTransferElementViewModel), typeof(ImageTransferElement))]
    [TestCase(ElementType.VideoTransfer, typeof(VideoTransferElementViewModel), typeof(VideoTransferElement))]
    [TestCase(ElementType.PdfTransfer, typeof(PdfTransferElementViewModel), typeof(PdfTransferElement))]
    [TestCase(ElementType.VideoActivation, typeof(VideoActivationElementViewModel), typeof(VideoActivationElement))]
    [TestCase(ElementType.H5PActivation, typeof(H5PActivationElementViewModel), typeof(H5PActivationElement))]
    [TestCase(ElementType.H5PInteraction, typeof(H5PInteractionElementViewModel), typeof(H5PInteractionElement))]
    [TestCase(ElementType.H5PTest, typeof(H5PTestElementViewModel), typeof(H5PTestElement))]
    public void MapLearningSpaceAndLearningSpaceViewModel_WithLearningElement_TestMappingIsValid
        (ElementType elementType, Type expectedElementViewModelType, Type expectedElementType)
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningSpace(Name, Shortname, Authors, Description, Goals, RequiredPoints, new LearningSpaceLayout(new ILearningElement?[6], FloorPlanEnum.Rectangle2X3),
            PositionX, PositionY);
        source.LearningSpaceLayout.LearningElements[0] = GetTestableElementWithParent(source, elementType);
        var destination = new LearningSpaceViewModel("", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestSpace(destination, false);
        Assert.That(destination.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(destination.ContainedLearningElements.First(), Is.InstanceOf(expectedElementViewModelType));
        Assert.That(destination.Id, Is.EqualTo(source.Id));

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.RequiredPoints = NewRequiredPoints;
        destination.LearningSpaceLayout.LearningElements = new ILearningElementViewModel?[6]
            {GetTestableElementViewModelWithParent(destination, elementType), null, null, null, null, null};
        destination.PositionX = NewPositionX;
        destination.PositionY = NewPositionY;

        systemUnderTest.Map(destination, source);

        TestSpace(source, true);
        Assert.That(source.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(source.ContainedLearningElements.First(), Is.InstanceOf(expectedElementType));
        Assert.That(destination.Id, Is.EqualTo(source.Id));
    }

    [TestCase(ElementType.TextTransfer, typeof(TextTransferElementPe), typeof(TextTransferElement))]
    [TestCase(ElementType.ImageTransfer, typeof(ImageTransferElementPe), typeof(ImageTransferElement))]
    [TestCase(ElementType.VideoTransfer, typeof(VideoTransferElementPe), typeof(VideoTransferElement))]
    [TestCase(ElementType.PdfTransfer, typeof(PdfTransferElementPe), typeof(PdfTransferElement))]
    [TestCase(ElementType.VideoActivation, typeof(VideoActivationElementPe), typeof(VideoActivationElement))]
    [TestCase(ElementType.H5PActivation, typeof(H5PActivationElementPe), typeof(H5PActivationElement))]
    [TestCase(ElementType.H5PInteraction, typeof(H5PInteractionElementPe), typeof(H5PInteractionElement))]
    [TestCase(ElementType.H5PTest, typeof(H5PTestElementPe), typeof(H5PTestElement))]
    public void MapLearningSpaceAndLearningSpacePersistEntity_WithLearningElement_TestMappingIsValid
        (ElementType elementType, Type expectedElementPeType, Type expectedElementType)
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningSpace(Name, Shortname, Authors, Description, Goals, RequiredPoints, new LearningSpaceLayout(new ILearningElement?[6], FloorPlanEnum.Rectangle2X3),
            PositionX, PositionY, new List<IObjectInPathWay>(), new List<IObjectInPathWay>());
        source.LearningSpaceLayout.LearningElements[0] = GetTestableElementWithParent(source, elementType);
        var destination = new LearningSpacePe("", "", "", "", "", 0);

        systemUnderTest.Map(source, destination);

        TestSpace(destination, false);
        Assert.That(destination.LearningSpaceLayout.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(destination.LearningSpaceLayout.ContainedLearningElements.First(), Is.InstanceOf(expectedElementPeType));

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.RequiredPoints = NewRequiredPoints;
        destination.LearningSpaceLayout.LearningElements = new ILearningElementPe[] {GetTestableElementPersistEntity(elementType)};
        destination.PositionX = NewPositionX;
        destination.PositionY = NewPositionY;
        destination.InBoundObjects = new List<IObjectInPathWayPe>();
        destination.OutBoundObjects = new List<IObjectInPathWayPe>();

        systemUnderTest.Map(destination, source);

        TestSpace(source, true);
        Assert.That(source.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(source.ContainedLearningElements.First(), Is.InstanceOf(expectedElementType));
    }

    [Test]
    public void MapLearningWorldAndLearningWorldViewModel_WithoutLearningSpaces_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            new List<LearningSpace>());
        var destination = new LearningWorldViewModel("", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() =>
        {
            Assert.That(destination.LearningSpaces, Is.Empty);
        });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.LearningSpaces = new List<ILearningSpaceViewModel>();

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() =>
        {
            Assert.That(source.LearningSpaces, Is.Empty);
        });
    }

    [Test]
    public void MapLearningWorldAndLearningWorldPersistEntity_WithoutLearningSpaces_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            new List<LearningSpace>());
        var destination = new LearningWorldPe("", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() =>
        {
            Assert.That(destination.LearningSpaces, Is.Empty);
        });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.LearningSpaces = new List<LearningSpacePe>();

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() =>
        {
            Assert.That(source.LearningSpaces, Is.Empty);
        });
    }

    [Test]
    public void MapLearningWorldAndLearningWorldViewModel_WithEmptyLearningSpace_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            new List<LearningSpace>());
        source.LearningSpaces.Add(new LearningSpace(Name, Shortname, Authors, Description, Goals, RequiredPoints, null, PositionX,
            PositionY));
        var destination = new LearningWorldViewModel("", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() =>
        {
            Assert.That(destination.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(destination.LearningSpaces.First().ContainedLearningElements, Is.Empty);
        });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.LearningSpaces = new List<ILearningSpaceViewModel>()
        {
            new LearningSpaceViewModel(NewName, NewShortname, NewAuthors, NewDescription, NewGoals, NewRequiredPoints,
                null, NewPositionX, NewPositionY)
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
    public void MapLearningWorldAndLearningWorldPersistEntity_WithEmptyLearningSpace_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            new List<LearningSpace>());
        source.LearningSpaces.Add(new LearningSpace(Name, Shortname, Authors, Description, Goals, RequiredPoints,
            null, PositionX, PositionY, new List<IObjectInPathWay>(), new List<IObjectInPathWay>()));
        var destination = new LearningWorldPe("", "", "", "", "", "");

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
        destination.LearningSpaces = new List<LearningSpacePe>()
        {
            new LearningSpacePe(NewName, NewShortname, NewAuthors, NewDescription, NewGoals, NewRequiredPoints, 
                null, NewPositionX, NewPositionY, new List<IObjectInPathWayPe>(), new List<IObjectInPathWayPe>())
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
    public void MapLearningWorldAndLearningWorldViewModel_WithLearningSpace_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            new List<LearningSpace>());
        source.LearningSpaces.Add(GetTestableSpace());
        var destination = new LearningWorldViewModel("", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() =>
        {
            Assert.That(destination.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(destination.LearningSpaces.First().ContainedLearningElements.Count, Is.EqualTo(1));
        });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.LearningSpaces = new List<ILearningSpaceViewModel>() {GetTestableNewSpaceViewModel()};

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() =>
        {
            Assert.That(source.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(source.LearningSpaces[0].ContainedLearningElements.Count(), Is.EqualTo(1));
        });
    }
    
    [Test]
    public void MapLearningWorldAndLearningWorldViewModel_WithLearningSpacesAndLearningPathWay_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            new List<LearningSpace>());
        var space1 = GetTestableSpace();
        var pathWayCondition = new PathWayCondition(ConditionEnum.And,3,2);
        source.LearningSpaces.Add(space1);
        source.PathWayConditions.Add(pathWayCondition);
        var destination = new LearningWorldViewModel("", "", "", "", "", "");
        
        source.LearningPathways.Add(new LearningPathway(space1, pathWayCondition));

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);

        Assert.Multiple(() =>
        {
            Assert.That(destination.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(destination.PathWayConditions, Has.Count.EqualTo(1));
            Assert.That(destination.LearningSpaces.First().ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(destination.LearningSpaces.First().OutBoundObjects, Has.Count.EqualTo(1));
            Assert.That(destination.PathWayConditions.First().InBoundObjects, Has.Count.EqualTo(1));
            Assert.That(destination.LearningPathWays, Has.Count.EqualTo(1));
        });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;

        var spaceVm1 = GetTestableNewSpaceViewModel();
        var pathWayConditionVm = new PathWayConditionViewModel(ConditionEnum.And,2,1);
        destination.LearningSpaces = new List<ILearningSpaceViewModel>() {spaceVm1};
        destination.PathWayConditions = new List<PathWayConditionViewModel>() {pathWayConditionVm};
        destination.LearningPathWays = new List<ILearningPathWayViewModel>();
        destination.LearningPathWays.Add(new LearningPathwayViewModel(spaceVm1, pathWayConditionVm));

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() =>
        {
            Assert.That(source.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(source.PathWayConditions, Has.Count.EqualTo(1));
            Assert.That(source.LearningSpaces[0].ContainedLearningElements.Count(), Is.EqualTo(1));
            Assert.That(source.LearningSpaces[0].OutBoundObjects, Has.Count.EqualTo(1));
            Assert.That(source.PathWayConditions[0].InBoundObjects, Has.Count.EqualTo(1));
            Assert.That(source.LearningPathways, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void MapLearningWorldAndLearningWorldPersistEntity_WithLearningSpaceAndLearningPathWay_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            new List<LearningSpace>());
        var space1 = GetTestableSpace();
        var pathWayCondition = new PathWayCondition(ConditionEnum.And,3,2);
        var space2 = GetTestableSpace();
        source.LearningSpaces.Add(space1);
        source.PathWayConditions.Add(pathWayCondition);
        source.LearningSpaces.Add(space2);
        var destination = new LearningWorldPe("", "", "", "", "", "");
        
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
        var spacePe1 = GetTestableNewSpacePersistEntity();
        var pathWayConditionPe = new PathWayConditionPe(ConditionEnumPe.And,2,1);
        var spacePe2 = GetTestableNewSpacePersistEntity();
        destination.LearningSpaces = new List<LearningSpacePe>() {spacePe1, spacePe2};
        destination.PathWayConditions = new List<PathWayConditionPe>() {pathWayConditionPe};
        destination.LearningPathways = new List<LearningPathwayPe> { new(spacePe1, pathWayConditionPe), new(pathWayConditionPe, spacePe2) }; ;
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
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            new List<LearningSpace>());
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
            Assert.That(((PathWayCondition)restored.LearningPathways[2].TargetObject).Condition, Is.EqualTo(ConditionEnum.And));
        });
    }

    /// <summary>
    /// This test tests whether or not the AutoMapper.Collection configuration works as intended.
    /// See https://github.com/AutoMapper/AutoMapper.Collection
    /// </summary>
    [Test]
    public void MapLearningWorldAndLearningWorldViewModel_WithSpaces_ObjectsStayEqual()
    {
        var elementVm1 =
            new LearningElementViewModel("el1", Shortname, new LearningContentViewModel("foo", "bar", Filepath), Url, Authors,
                Description, Goals, Difficulty);
        var elementVm2 =
            new LearningElementViewModel("el2", Shortname, new LearningContentViewModel("foo", "bar", Filepath), Url, Authors,
                Description, Goals, Difficulty);

        var space = new LearningSpaceViewModel("space", Shortname, Authors, Description, Goals, RequiredPoints,
            new LearningSpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3) { LearningElements = new ILearningElementViewModel?[6] { elementVm1, null, null, null, null, null } })
        {
            SelectedLearningElement = elementVm1
        };
        elementVm1.Parent = space;

        var world = new LearningWorldViewModel("world", Shortname, Authors, Language, Description, Goals, true,
        new List<ILearningSpaceViewModel> { space })
        {
            SelectedLearningObject = space
        };


        var systemUnderTest = CreateTestableMapper();

        var entity = systemUnderTest.Map<LearningWorld>(world);
        world.LearningSpaces.First().LearningSpaceLayout.ContainedLearningElements.First().Authors = "foooooooooo";
        
        
        //map back into viewmodel - with update syntax
        systemUnderTest.Map(entity, world);
        
        //we would expect that the objects are still the same and we retained view specific information
        Assert.That(world.LearningSpaces.First(), Is.EqualTo(space));
        
        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces.First().ContainedLearningElements.First(), Is.EqualTo(elementVm1));
            Assert.That(world.SelectedLearningObject, Is.EqualTo(space));
            Assert.That(world.LearningSpaces.First().SelectedLearningElement, Is.EqualTo(elementVm1));
        });
    }
    
    [Test]
    public void MapAuthoringToolWorkspaceAndAuthoringToolWorkspaceViewModel_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var world1 = new LearningWorld("world1", Shortname, Authors, Language, Description, Goals);
        var world2 = new LearningWorld("world2", Shortname, Authors, Language, Description, Goals);
        var source = new AuthoringToolWorkspace(world2, new List<LearningWorld> {world1, world2});
        var destination = new AuthoringToolWorkspaceViewModel();
        
        systemUnderTest.Map(source, destination);
        
        Assert.That(destination.LearningWorlds, Is.Not.Null);
        Assert.That(destination.LearningWorlds.Count, Is.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(destination.LearningWorlds.First().Name, Is.EqualTo("world1"));
            Assert.That(destination.LearningWorlds.Last().Name, Is.EqualTo("world2"));
            Assert.That(destination.SelectedLearningWorld, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(destination.SelectedLearningWorld!.Name, Is.EqualTo("world2"));
            Assert.That(destination.SelectedLearningWorld, Is.EqualTo(destination.LearningWorlds.Last()));
        });
        destination.SelectedLearningWorld = destination.LearningWorlds.First();
        
        systemUnderTest.Map(destination, source);
        
        Assert.That(source.LearningWorlds, Is.Not.Null);
        Assert.That(source.LearningWorlds, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(source.LearningWorlds.First(), Is.EqualTo(world1));
            Assert.That(source.LearningWorlds.Last(), Is.EqualTo(world2));
            Assert.That(source.LearningWorlds.First().Name, Is.EqualTo("world1"));
            Assert.That(source.LearningWorlds.Last().Name, Is.EqualTo("world2"));
            Assert.That(source.SelectedLearningWorld, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(source.SelectedLearningWorld!.Name, Is.EqualTo("world1"));
            Assert.That(source.SelectedLearningWorld, Is.EqualTo(source.LearningWorlds.First()));
        });
        
        // Test the private constructor from AuthoringToolWorkspace
        var source2 = systemUnderTest.Map<AuthoringToolWorkspace>(destination);
        
        Assert.That(source2.LearningWorlds, Is.Not.Null);
        Assert.That(source2.LearningWorlds, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(source2.LearningWorlds.First().Name, Is.EqualTo("world1"));
            Assert.That(source2.LearningWorlds.Last().Name, Is.EqualTo("world2"));
            Assert.That(source2.SelectedLearningWorld, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(source2.SelectedLearningWorld!.Name, Is.EqualTo("world1"));
            Assert.That(source2.SelectedLearningWorld, Is.EqualTo(source2.LearningWorlds.First()));
        });
    }

    #region testable Content/Element/Space/World

    private static LearningContent GetTestableContent()
    {
        return new LearningContent(Name, Type, Filepath);
    }

    private static LearningContentViewModel GetTestableNewContentViewModel()
    {
        return new LearningContentViewModel(NewName, NewType, NewFilepath);
    }

    private static LearningContentPe GetTestableNewContentPersistEntity()
    {
        return new LearningContentPe(NewName, NewType, NewFilepath);
    }

    private static LearningElement GetTestableElementWithParent(LearningSpace parent, ElementType elementType)
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

    private static LearningElementViewModel GetTestableElementViewModelWithParent(LearningSpaceViewModel parent, ElementType elementType)
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

    private static LearningElementPe GetTestableElementPersistEntity(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.TextTransfer:
                return new TextTransferElementPe(NewName, NewShortname, GetTestableNewContentPersistEntity(), NewUrl,
                    NewAuthors, NewDescription, NewGoals, NewDifficultyPe, NewWorkload, NewPoints, NewPositionX, NewPositionY);
            case ElementType.ImageTransfer:
                return new ImageTransferElementPe(NewName, NewShortname, GetTestableNewContentPersistEntity(), NewUrl,
                    NewAuthors, NewDescription, NewGoals, NewDifficultyPe, NewWorkload, NewPoints, NewPositionX, NewPositionY);
            case ElementType.VideoTransfer:
                return new VideoTransferElementPe(NewName, NewShortname, GetTestableNewContentPersistEntity(), NewUrl,
                    NewAuthors, NewDescription, NewGoals, NewDifficultyPe, NewWorkload, NewPoints, NewPositionX, NewPositionY);
            case ElementType.PdfTransfer:
                return new PdfTransferElementPe(NewName, NewShortname, GetTestableNewContentPersistEntity(), NewUrl,
                    NewAuthors, NewDescription, NewGoals, NewDifficultyPe, NewWorkload, NewPoints, NewPositionX, NewPositionY);
            case ElementType.VideoActivation:
                return new VideoActivationElementPe(NewName, NewShortname, GetTestableNewContentPersistEntity(), NewUrl,
                    NewAuthors, NewDescription, NewGoals, NewDifficultyPe, NewWorkload, NewPoints, NewPositionX, NewPositionY);
            case ElementType.H5PActivation:
                return new H5PActivationElementPe(NewName, NewShortname, GetTestableNewContentPersistEntity(), NewUrl,
                    NewAuthors, NewDescription, NewGoals, NewDifficultyPe, NewWorkload, NewPoints, NewPositionX, NewPositionY);
            case ElementType.H5PInteraction:
                return new H5PInteractionElementPe(NewName, NewShortname, GetTestableNewContentPersistEntity(), NewUrl,
                    NewAuthors, NewDescription, NewGoals, NewDifficultyPe, NewWorkload, NewPoints, NewPositionX, NewPositionY);
            case ElementType.H5PTest:
                return new H5PTestElementPe(NewName, NewShortname, GetTestableNewContentPersistEntity(), NewUrl,
                    NewAuthors, NewDescription, NewGoals, NewDifficultyPe, NewWorkload, NewPoints, NewPositionX, NewPositionY);
            default:
                throw new ArgumentOutOfRangeException(nameof(elementType), elementType, null);
        }
    }
    
    public enum ElementType
    {
        TextTransfer, ImageTransfer, VideoTransfer, PdfTransfer, VideoActivation, H5PActivation, H5PInteraction, H5PTest
    }

    private static LearningSpace GetTestableSpace()
    {
        var space = new LearningSpace(Name, Shortname, Authors, Description, Goals, RequiredPoints,
            new LearningSpaceLayout(new ILearningElement[6], FloorPlanEnum.Rectangle2X3), PositionX, PositionY);
        var element = GetTestableElementWithParent(space, ElementType.TextTransfer);
        space.LearningSpaceLayout.LearningElements[0] = element;
        return space;
    }

    private static LearningSpaceViewModel GetTestableNewSpaceViewModel()
    {
        var space = new LearningSpaceViewModel(NewName, NewShortname, NewAuthors, NewDescription, NewGoals, NewRequiredPoints,
            new LearningSpaceLayoutViewModel(FloorPlanEnum.Rectangle2X3), NewPositionX, NewPositionY);
        var element = GetTestableElementViewModelWithParent(space, ElementType.TextTransfer);
        space.LearningSpaceLayout.PutElement(0, element);
        return space;
    }

    private static LearningSpacePe GetTestableNewSpacePersistEntity()
    {
        return new LearningSpacePe(NewName, NewShortname, NewAuthors, NewDescription, NewGoals, NewRequiredPoints,
            new LearningSpaceLayoutPe(new ILearningElementPe[] {GetTestableElementPersistEntity(ElementType.TextTransfer)}, FloorPlanEnumPe.Rectangle2X3), NewPositionX, NewPositionY);
    }

    #endregion

    #region static test methods

    private static void TestWorld(object destination, bool useNewFields)
    {
        switch (destination)
        {
            case LearningWorldViewModel world:
                Assert.Multiple(() =>
                {
                    Assert.That(world.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(world.Shortname, Is.EqualTo(useNewFields ? NewShortname : Shortname));
                    Assert.That(world.Authors, Is.EqualTo(useNewFields ? NewAuthors : Authors));
                    Assert.That(world.Language, Is.EqualTo(useNewFields ? NewLanguage : Language));
                    Assert.That(world.Description, Is.EqualTo(useNewFields ? NewDescription : Description));
                    Assert.That(world.Goals, Is.EqualTo(useNewFields ? NewGoals : Goals));
                    TestSpacesList(world.LearningSpaces, useNewFields);
                });
                break;

            case LearningWorld world:
                Assert.Multiple(() =>
                {
                    Assert.That(world.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(world.Shortname, Is.EqualTo(useNewFields ? NewShortname : Shortname));
                    Assert.That(world.Authors, Is.EqualTo(useNewFields ? NewAuthors : Authors));
                    Assert.That(world.Language, Is.EqualTo(useNewFields ? NewLanguage : Language));
                    Assert.That(world.Description, Is.EqualTo(useNewFields ? NewDescription : Description));
                    Assert.That(world.Goals, Is.EqualTo(useNewFields ? NewGoals : Goals));
                    TestSpacesList(world.LearningSpaces, useNewFields);
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
                    TestSpacesList(world.LearningSpaces, useNewFields);
                });
                break;
        }
    }

    private static void TestSpacesList(object worldLearningSpaces, bool useNewFields)
    {
        switch (worldLearningSpaces)
        {
            case List<ILearningSpaceViewModel> learningSpaces:
                Assert.Multiple(() =>
                {
                    foreach (var learningSpace in learningSpaces)
                    {
                        TestSpace(learningSpace, useNewFields);
                    }
                });
                break;
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
            case LearningSpaceViewModel space:
                Assert.Multiple(() =>
                {
                    Assert.That(space.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(space.Shortname, Is.EqualTo(useNewFields ? NewShortname : Shortname));
                    Assert.That(space.Authors, Is.EqualTo(useNewFields ? NewAuthors : Authors));
                    Assert.That(space.Description, Is.EqualTo(useNewFields ? NewDescription : Description));
                    Assert.That(space.Goals, Is.EqualTo(useNewFields ? NewGoals : Goals));
                    Assert.That(space.RequiredPoints, Is.EqualTo(useNewFields ? NewRequiredPoints : RequiredPoints));
                    TestElementsList(space.ContainedLearningElements, space, useNewFields);
                    Assert.That(space.PositionX, Is.EqualTo(useNewFields ? NewPositionX : PositionX));
                    Assert.That(space.PositionY, Is.EqualTo(useNewFields ? NewPositionY : PositionY));
                });
                break;
            case LearningSpace space:
                Assert.Multiple(() =>
                {
                    Assert.That(space.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(space.Shortname, Is.EqualTo(useNewFields ? NewShortname : Shortname));
                    Assert.That(space.Authors, Is.EqualTo(useNewFields ? NewAuthors : Authors));
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
                    Assert.That(space.Shortname, Is.EqualTo(useNewFields ? NewShortname : Shortname));
                    Assert.That(space.Authors, Is.EqualTo(useNewFields ? NewAuthors : Authors));
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
            case List<ILearningElementViewModel> learningElements:
                Assert.Multiple(() =>
                {
                    foreach (var learningElement in learningElements)
                    {
                        TestElement(learningElement, parent, useNewFields);
                    }
                });
                break;
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
            case LearningElementViewModel element:
                Assert.Multiple(() =>
                {
                    Assert.That(element.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(element.Shortname, Is.EqualTo(useNewFields ? NewShortname : Shortname));
                    TestContent(element.LearningContent, useNewFields);
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
            case LearningElement element:
                Assert.Multiple(() =>
                {
                    Assert.That(element.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(element.Shortname, Is.EqualTo(useNewFields ? NewShortname : Shortname));
                    TestContent(element.LearningContent, useNewFields);
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
            case LearningElementPe element:
                Assert.Multiple(() =>
                {
                    Assert.That(element.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(element.Shortname, Is.EqualTo(useNewFields ? NewShortname : Shortname));
                    TestContent(element.LearningContent, useNewFields);
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

    private static void TestContent(object elementLearningContent, bool useNewFields)
    {
        switch (elementLearningContent)
        {
            case LearningContentViewModel content:
                Assert.Multiple(() =>
                {
                    Assert.That(content.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(content.Type, Is.EqualTo(useNewFields ? NewType : Type));
                    Assert.That(content.Filepath, Is.EqualTo(useNewFields ? NewFilepath : Filepath));
                });
                break;
            case LearningContent content:
                Assert.Multiple(() =>
                {
                    Assert.That(content.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(content.Type, Is.EqualTo(useNewFields ? NewType : Type));
                    Assert.That(content.Filepath, Is.EqualTo(useNewFields ? NewFilepath : Filepath));
                });
                break;
            case LearningContentPe content:
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
        var mapper = new MapperConfiguration(MappingProfile.Configure);
        var systemUnderTest = mapper.CreateMapper();
        return systemUnderTest;
    }
}