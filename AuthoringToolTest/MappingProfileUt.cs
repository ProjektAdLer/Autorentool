using AuthoringTool;
using AutoMapper;
using BusinessLogic.Entities;
using NUnit.Framework;
using PersistEntities;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
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
        Assert.That(destination.LearningElements, Is.Empty);

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.RequiredPoints = NewRequiredPoints;
        destination.LearningElements = new List<ILearningElementViewModel>();
        destination.PositionX = NewPositionX;
        destination.PositionY = NewPositionY;

        systemUnderTest.Map(destination, source);

        TestSpace(source, true);
        Assert.That(source.LearningElements, Is.Empty);
    }

    [Test]
    public void MapLearningSpaceAndLearningSpacePersistEntity_WithoutLearningElement_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningSpace(Name, Shortname, Authors, Description, Goals, RequiredPoints, null, PositionX, PositionY);
        var destination = new LearningSpacePe("", "", "", "", "", 0);

        systemUnderTest.Map(source, destination);

        TestSpace(destination, false);
        Assert.That(destination.LearningElements, Is.Empty);

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.RequiredPoints = NewRequiredPoints;
        destination.LearningElements = new List<LearningElementPe>();
        destination.PositionX = NewPositionX;
        destination.PositionY = NewPositionY;

        systemUnderTest.Map(destination, source);

        TestSpace(source, true);
        Assert.That(source.LearningElements, Is.Empty);
    }

    [Test]
    public void MapLearningSpaceAndLearningSpaceViewModel_WithLearningElement_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningSpace(Name, Shortname, Authors, Description, Goals, RequiredPoints, new List<LearningElement>(),
            PositionX, PositionY);
        source.LearningElements.Add(GetTestableElementWithParent(source));
        var destination = new LearningSpaceViewModel("", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestSpace(destination, false);
        Assert.That(destination.LearningElements, Has.Count.EqualTo(1));
        Assert.That(destination.Id, Is.EqualTo(source.Id));

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.RequiredPoints = NewRequiredPoints;
        destination.LearningElements = new List<ILearningElementViewModel>()
            {GetTestableNewElementViewModelWithParent(destination)};
        destination.PositionX = NewPositionX;
        destination.PositionY = NewPositionY;

        systemUnderTest.Map(destination, source);

        TestSpace(source, true);
        Assert.That(source.LearningElements, Has.Count.EqualTo(1));
        Assert.That(destination.Id, Is.EqualTo(source.Id));
    }

    [Test]
    public void MapLearningSpaceAndLearningSpacePersistEntity_WithLearningElement_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningSpace(Name, Shortname, Authors, Description, Goals, RequiredPoints, new List<LearningElement>(),
            PositionX, PositionY);
        source.LearningElements.Add(GetTestableElementWithParent(source));
        var destination = new LearningSpacePe("", "", "", "", "", 0);

        systemUnderTest.Map(source, destination);

        TestSpace(destination, false);
        Assert.That(destination.LearningElements, Has.Count.EqualTo(1));

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.RequiredPoints = NewRequiredPoints;
        destination.LearningElements = new List<LearningElementPe>() {GetTestableNewElementPersistEntity()};
        destination.PositionX = NewPositionX;
        destination.PositionY = NewPositionY;

        systemUnderTest.Map(destination, source);

        TestSpace(source, true);
        Assert.That(source.LearningElements, Has.Count.EqualTo(1));
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
            Assert.That(destination.LearningSpaces.First().LearningElements, Is.Empty);
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
            Assert.That(source.LearningSpaces[0].LearningElements, Is.Empty);
        });
    }

    [Test]
    public void MapLearningWorldAndLearningWorldPersistEntity_WithEmptyLearningSpace_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            new List<LearningSpace>());
        source.LearningSpaces.Add(new LearningSpace(Name, Shortname, Authors, Description, Goals, RequiredPoints,
            null, PositionX, PositionY));
        var destination = new LearningWorldPe("", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() =>
        {
            Assert.That(destination.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(destination.LearningSpaces[0].LearningElements, Is.Empty);
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
                null, NewPositionX, NewPositionY)
        };

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() =>
        {
            Assert.That(source.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(source.LearningSpaces[0].LearningElements, Is.Empty);
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
            Assert.That(destination.LearningSpaces.First().LearningElements, Has.Count.EqualTo(1));
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
            Assert.That(source.LearningSpaces[0].LearningElements, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void MapLearningWorldAndLearningWorldPersistEntity_WithLearningSpace_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            new List<LearningSpace>());
        source.LearningSpaces.Add(GetTestableSpace());
        var destination = new LearningWorldPe("", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() =>
        {
            Assert.That(destination.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(destination.LearningSpaces[0].LearningElements, Has.Count.EqualTo(1));
        });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.LearningSpaces = new List<LearningSpacePe>() {GetTestableNewSpacePersistEntity()};

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() =>
        {
            Assert.That(source.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(source.LearningSpaces[0].LearningElements, Has.Count.EqualTo(1));
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
            new List<ILearningElementViewModel> { elementVm1 })
        {
            SelectedLearningElement = elementVm1
        };
        elementVm1.Parent = space;

        var world = new LearningWorldViewModel("world", Shortname, Authors, Language, Description, Goals, true,
        new List<ILearningSpaceViewModel> { space })
        {
            SelectedLearningSpace = space
        };


        var systemUnderTest = CreateTestableMapper();

        var entity = systemUnderTest.Map<LearningWorld>(world);
        
        //map back into viewmodel - with update syntax
        systemUnderTest.Map(entity, world);
        
        //we would expect that the objects are still the same and we retained view specific information
        Assert.That(world.LearningSpaces.First(), Is.EqualTo(space));
        
        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces.First().LearningElements.First(), Is.EqualTo(elementVm1));
            Assert.That(world.SelectedLearningSpace, Is.EqualTo(space));
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

    private static LearningElement GetTestableElementWithParent(object parent)
    {
        return parent switch
        {
            LearningSpace space => new LearningElement(Name, Shortname, GetTestableContent(), Url, Authors, Description,
                Goals, Difficulty, space, Workload, Points, PositionX, PositionY),
            _ => throw new ArgumentException($"{parent.GetType().Name} is not a valid parent type")
        };
    }

    private static LearningElementViewModel GetTestableNewElementViewModelWithParent(object parent)
    {
        return parent switch
        {
            LearningSpaceViewModel space => new LearningElementViewModel(NewName, NewShortname,
                GetTestableNewContentViewModel(), NewUrl, NewAuthors, NewDescription, NewGoals, NewDifficulty, space,
                NewWorkload, NewPoints, NewPositionX, NewPositionY),
            _ => throw new ArgumentException($"{parent.GetType().Name} is not a valid parent type")
        };
    }

    private static LearningElementPe GetTestableNewElementPersistEntity()
    {
        return new LearningElementPe(NewName, NewShortname,
            GetTestableNewContentPersistEntity(), NewUrl, NewAuthors, NewDescription, NewGoals, NewDifficultyPe, NewWorkload,
            NewPoints, NewPositionX, NewPositionY);
    }

    private static LearningSpace GetTestableSpace()
    {
        var space = new LearningSpace(Name, Shortname, Authors, Description, Goals, RequiredPoints,
            new List<LearningElement>(), PositionX, PositionY);
        var element = GetTestableElementWithParent(space);
        space.LearningElements.Add(element);
        return space;
    }

    private static LearningSpaceViewModel GetTestableNewSpaceViewModel()
    {
        var space = new LearningSpaceViewModel(NewName, NewShortname, NewAuthors, NewDescription, NewGoals, NewRequiredPoints,
            new List<ILearningElementViewModel>(), NewPositionX, NewPositionY);
        var element = GetTestableNewElementViewModelWithParent(space);
        space.LearningElements.Add(element);
        return space;
    }

    private static LearningSpacePe GetTestableNewSpacePersistEntity()
    {
        return new LearningSpacePe(NewName, NewShortname, NewAuthors, NewDescription, NewGoals, NewRequiredPoints,
            new List<LearningElementPe>() {GetTestableNewElementPersistEntity()}, NewPositionX, NewPositionY);
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
                    TestElementsList(space.LearningElements, space, useNewFields);
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
                    TestElementsList(space.LearningElements, space, useNewFields);
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
                    TestElementsList(space.LearningElements, space, useNewFields);
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