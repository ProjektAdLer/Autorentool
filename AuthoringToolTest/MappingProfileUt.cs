using AuthoringTool;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
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
    private const string Description = "description";
    private const string Goals = "goals";
    private const string Type = "type";
    private static readonly byte[] Content = new byte[] {0x01, 0x02, 0x03};
    private const LearningElementDifficultyEnum Difficulty = LearningElementDifficultyEnum.Easy;
    private const LearningElementDifficultyEnumPe DifficultyPe = LearningElementDifficultyEnumPe.Easy;
    private const int Workload = 1;
    private const double PositionX = 1.0;
    private const double PositionY = 2.0;

    private const string NewName = "newName";
    private const string NewShortname = "newShortname";
    private const string NewAuthors = "newAuthors";
    private const string NewLanguage = "newLanguage";
    private const string NewDescription = "newDescription";
    private const string NewGoals = "newGoals";
    private const string NewType = "newType";
    private static readonly byte[] NewContent = new byte[] {0x04, 0x05, 0x06};
    private const LearningElementDifficultyEnum NewDifficulty = LearningElementDifficultyEnum.Medium;
    private const LearningElementDifficultyEnumPe NewDifficultyPe = LearningElementDifficultyEnumPe.Medium;
    private const int NewWorkload = 2;
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
        var source = new LearningContent(Name, Type, Content);
        var destination = new LearningContentViewModel("", "", Array.Empty<byte>());

        systemUnderTest.Map(source, destination);

        TestContent(destination, false);

        destination.Name = NewName;
        destination.Type = NewType;
        destination.Content = NewContent;

        systemUnderTest.Map(destination, source);

        TestContent(source, true);
    }

    [Test]
    public void MapLearningContentAndLearningContentPersistEntity_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningContent(Name, Type, Content);
        var destination = new LearningContentPe("", "", Array.Empty<byte>());

        systemUnderTest.Map(source, destination);

        TestContent(destination, false);

        destination.Name = NewName;
        destination.Type = NewType;
        destination.Content = NewContent;

        systemUnderTest.Map(destination, source);

        TestContent(source, true);
    }

    [Test]
    public void MapLearningElementAndLearningElementViewModel_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var content = GetTestableContent();
        var source = new LearningElement(Name, Shortname, content, Authors, Description, Goals,
            Difficulty, null, Workload, PositionX, PositionY);
        var destination = new LearningElementViewModel("", "",
            new LearningContentViewModel("", "", Array.Empty<byte>()), "", "", "", LearningElementDifficultyEnum.None);

        systemUnderTest.Map(source, destination);

        TestElement(destination, null, false);
        Assert.That(destination.Id, Is.EqualTo(source.Id));

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.LearningContent = new LearningContentViewModel(NewName, NewType, NewContent);
        destination.Authors = NewAuthors;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.Difficulty = NewDifficulty;
        destination.Workload = NewWorkload;
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
        var source = new LearningElement(Name, Shortname, content, Authors, Description, Goals,
            Difficulty, null, Workload, PositionX, PositionY);
        var destination = new LearningElementPe("", "", new LearningContentPe("", "", Array.Empty<byte>()), "", "", "",
            LearningElementDifficultyEnumPe.None);

        systemUnderTest.Map(source, destination);

        TestElement(destination, null, false);

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.LearningContent = new LearningContentPe(NewName, NewType, NewContent);
        destination.Authors = NewAuthors;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.Difficulty = NewDifficultyPe;
        destination.Workload = NewWorkload;
        destination.PositionX = NewPositionX;
        destination.PositionY = NewPositionY;

        systemUnderTest.Map(destination, source);

        TestElement(source, null, true);
    }

    [Test]
    public void MapLearningSpaceAndLearningSpaceViewModel_WithoutLearningElement_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningSpace(Name, Shortname, Authors, Description, Goals, null, PositionX, PositionY);
        var destination = new LearningSpaceViewModel("", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestSpace(destination, false);
        Assert.That(destination.LearningElements, Is.Empty);

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
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
        var source = new LearningSpace(Name, Shortname, Authors, Description, Goals, null, PositionX, PositionY);
        var destination = new LearningSpacePe("", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestSpace(destination, false);
        Assert.That(destination.LearningElements, Is.Empty);

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
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
        var source = new LearningSpace(Name, Shortname, Authors, Description, Goals, new List<LearningElement>(),
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
        var source = new LearningSpace(Name, Shortname, Authors, Description, Goals, new List<LearningElement>(),
            PositionX, PositionY);
        source.LearningElements.Add(GetTestableElementWithParent(source));
        var destination = new LearningSpacePe("", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestSpace(destination, false);
        Assert.That(destination.LearningElements, Has.Count.EqualTo(1));

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.LearningElements = new List<LearningElementPe>() {GetTestableNewElementPersistEntity()};
        destination.PositionX = NewPositionX;
        destination.PositionY = NewPositionY;

        systemUnderTest.Map(destination, source);

        TestSpace(source, true);
        Assert.That(source.LearningElements, Has.Count.EqualTo(1));
    }

    [Test]
    public void MapLearningWorldAndLearningWorldViewModel_WithoutLearningSpacesAndElements_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            new List<LearningElement>(), new List<LearningSpace>());
        var destination = new LearningWorldViewModel("", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() =>
        {
            Assert.That(destination.LearningSpaces, Is.Empty);
            Assert.That(destination.LearningElements, Is.Empty);
        });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.LearningSpaces = new List<ILearningSpaceViewModel>();
        destination.LearningElements = new List<ILearningElementViewModel>();

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() =>
        {
            Assert.That(source.LearningSpaces, Is.Empty);
            Assert.That(source.LearningElements, Is.Empty);
        });
    }

    [Test]
    public void MapLearningWorldAndLearningWorldPersistEntity_WithoutLearningSpacesAndElements_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            new List<LearningElement>(), new List<LearningSpace>());
        var destination = new LearningWorldPe("", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() =>
        {
            Assert.That(destination.LearningSpaces, Is.Empty);
            Assert.That(destination.LearningElements, Is.Empty);
        });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.LearningSpaces = new List<LearningSpacePe>();
        destination.LearningElements = new List<LearningElementPe>();

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() =>
        {
            Assert.That(source.LearningSpaces, Is.Empty);
            Assert.That(source.LearningElements, Is.Empty);
        });
    }

    [Test]
    public void MapLearningWorldAndLearningWorldViewModel_WithLearningElement_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            new List<LearningElement>(), new List<LearningSpace>());
        source.LearningElements.Add(GetTestableElementWithParent(source));
        var destination = new LearningWorldViewModel("", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() =>
        {
            Assert.That(destination.LearningSpaces, Is.Empty);
            Assert.That(destination.LearningElements, Has.Count.EqualTo(1));
        });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.LearningSpaces = new List<ILearningSpaceViewModel>();
        destination.LearningElements = new List<ILearningElementViewModel>()
            {GetTestableNewElementViewModelWithParent(destination)};

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() =>
        {
            Assert.That(source.LearningSpaces, Is.Empty);
            Assert.That(source.LearningElements, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void MapLearningWorldAndLearningWorldPersistEntity_WithLearningElement_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            new List<LearningElement>(), new List<LearningSpace>());
        source.LearningElements.Add(GetTestableElementWithParent(source));
        var destination = new LearningWorldPe("", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() =>
        {
            Assert.That(destination.LearningSpaces, Is.Empty);
            Assert.That(destination.LearningElements, Has.Count.EqualTo(1));
        });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.LearningSpaces = new List<LearningSpacePe>();
        destination.LearningElements = new List<LearningElementPe>() {GetTestableNewElementPersistEntity()};

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() =>
        {
            Assert.That(source.LearningSpaces, Is.Empty);
            Assert.That(source.LearningElements, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void MapLearningWorldAndLearningWorldViewModel_WithEmptyLearningSpace_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            new List<LearningElement>(), new List<LearningSpace>());
        source.LearningSpaces.Add(new LearningSpace(Name, Shortname, Authors, Description, Goals, null, PositionX,
            PositionY));
        var destination = new LearningWorldViewModel("", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() =>
        {
            Assert.That(destination.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(destination.LearningSpaces[0].LearningElements, Is.Empty);
            Assert.That(destination.LearningElements, Is.Empty);
        });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.LearningSpaces = new List<ILearningSpaceViewModel>()
        {
            new LearningSpaceViewModel(NewName, NewShortname, NewAuthors, NewDescription, NewGoals, null, NewPositionX,
                NewPositionY)
        };
        destination.LearningElements = new List<ILearningElementViewModel>();

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() =>
        {
            Assert.That(source.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(source.LearningSpaces[0].LearningElements, Is.Empty);
            Assert.That(source.LearningElements, Is.Empty);
        });
    }

    [Test]
    public void MapLearningWorldAndLearningWorldPersistEntity_WithEmptyLearningSpace_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            new List<LearningElement>(), new List<LearningSpace>());
        source.LearningSpaces.Add(new LearningSpace(Name, Shortname, Authors, Description, Goals, null, PositionX,
            PositionY));
        var destination = new LearningWorldPe("", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() =>
        {
            Assert.That(destination.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(destination.LearningSpaces[0].LearningElements, Is.Empty);
            Assert.That(destination.LearningElements, Is.Empty);
        });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.LearningSpaces = new List<LearningSpacePe>()
        {
            new LearningSpacePe(NewName, NewShortname, NewAuthors, NewDescription, NewGoals, null, NewPositionX,
                NewPositionY)
        };
        destination.LearningElements = new List<LearningElementPe>();

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() =>
        {
            Assert.That(source.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(source.LearningSpaces[0].LearningElements, Is.Empty);
            Assert.That(source.LearningElements, Is.Empty);
        });
    }

    [Test]
    public void MapLearningWorldAndLearningWorldViewModel_WithLearningSpaceAndElement_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            new List<LearningElement>(), new List<LearningSpace>());
        source.LearningSpaces.Add(GetTestableSpace());
        source.LearningElements.Add(GetTestableElementWithParent(source));
        var destination = new LearningWorldViewModel("", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() =>
        {
            Assert.That(destination.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(destination.LearningSpaces[0].LearningElements, Has.Count.EqualTo(1));
            Assert.That(destination.LearningElements, Has.Count.EqualTo(1));
        });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.LearningSpaces = new List<ILearningSpaceViewModel>() {GetTestableNewSpaceViewModel()};
        destination.LearningElements = new List<ILearningElementViewModel>()
            {GetTestableNewElementViewModelWithParent(destination)};

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() =>
        {
            Assert.That(source.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(source.LearningSpaces[0].LearningElements, Has.Count.EqualTo(1));
            Assert.That(source.LearningElements, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void MapLearningWorldAndLearningWorldPersistEntity_WithLearningSpaceAndElement_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            new List<LearningElement>(), new List<LearningSpace>());
        source.LearningSpaces.Add(GetTestableSpace());
        source.LearningElements.Add(GetTestableElementWithParent(source));
        var destination = new LearningWorldPe("", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() =>
        {
            Assert.That(destination.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(destination.LearningSpaces[0].LearningElements, Has.Count.EqualTo(1));
            Assert.That(destination.LearningElements, Has.Count.EqualTo(1));
        });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.LearningSpaces = new List<LearningSpacePe>() {GetTestableNewSpacePersistEntity()};
        destination.LearningElements = new List<LearningElementPe>() {GetTestableNewElementPersistEntity()};

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() =>
        {
            Assert.That(source.LearningSpaces, Has.Count.EqualTo(1));
            Assert.That(source.LearningSpaces[0].LearningElements, Has.Count.EqualTo(1));
            Assert.That(source.LearningElements, Has.Count.EqualTo(1));
        });
    }

    /// <summary>
    /// This test tests whether or not the AutoMapper.Collection configuration works as intended.
    /// See https://github.com/AutoMapper/AutoMapper.Collection
    /// </summary>
    [Test]
    public void MapLearningWorldAndLearningWorldViewModel_WithSpacesAndElements_ObjectsStayEqual()
    {
        var elementVm1 =
            new LearningElementViewModel("el1", Shortname, new LearningContentViewModel("foo", "bar", Content), Authors,
                Description, Goals, Difficulty);
        var elementVm2 =
            new LearningElementViewModel("el2", Shortname, new LearningContentViewModel("foo", "bar", Content), Authors,
                Description, Goals, Difficulty);

        var space = new LearningSpaceViewModel("space", Shortname, Authors, Description, Goals,
            new List<ILearningElementViewModel> { elementVm1 })
        {
            SelectedLearningObject = elementVm1
        };
        elementVm1.Parent = space;

        var world = new LearningWorldViewModel("world", Shortname, Authors, Language, Description, Goals, true,
        new List<ILearningElementViewModel> { elementVm2 }, new List<ILearningSpaceViewModel> { space })
        {
            SelectedLearningObject = space
        };


        var systemUnderTest = CreateTestableMapper();

        var entity = systemUnderTest.Map<LearningWorld>(world);
        
        //map back into viewmodel - with update syntax
        systemUnderTest.Map(entity, world);
        
        //we would expect that the objects are still the same and we retained view specific information
        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces.First(), Is.EqualTo(space));
            Assert.That(world.LearningElements.First(), Is.EqualTo(elementVm2));
        });
        Assert.Multiple(() =>
        {
            Assert.That(world.LearningSpaces.First().LearningElements.First(), Is.EqualTo(elementVm1));
            Assert.That(world.SelectedLearningObject, Is.EqualTo(space));
            Assert.That(world.LearningSpaces.First().SelectedLearningObject, Is.EqualTo(elementVm1));
        });
    }
    
    [Test]
    public void MapAuthoringToolWorkspaceAndAuthoringToolWorkspaceViewModel_TestMappingIsValid(){
        var systemUnderTest = CreateTestableMapper();
        var world1 = new LearningWorld("world1", Shortname, Authors, Language, Description, Goals);
        var world2 = new LearningWorld("world2", Shortname, Authors, Language, Description, Goals);
        var source = new AuthoringToolWorkspace(world2, new List<LearningWorld> {world1, world2});
        var destination = new AuthoringToolWorkspaceViewModel();
        
        systemUnderTest.Map(source, destination);
        
        Assert.Multiple(() =>
        {
            Assert.That(destination.LearningWorlds, Is.Not.Null);
            Assert.That(destination.LearningWorlds.Count, Is.EqualTo(2));
            Assert.That(destination.LearningWorlds.First().Name, Is.EqualTo("world1"));
            Assert.That(destination.LearningWorlds.Last().Name, Is.EqualTo("world2"));
            Assert.That(destination.SelectedLearningWorld, Is.Not.Null);
            Assert.That(destination.SelectedLearningWorld.Name, Is.EqualTo("world2"));
            Assert.That(destination.SelectedLearningWorld, Is.EqualTo(destination.LearningWorlds.Last()));
        });
        
        destination.SelectedLearningWorld = destination.LearningWorlds.First();
        
        systemUnderTest.Map(destination, source);
        
        Assert.Multiple(() =>
        {
            Assert.That(source.LearningWorlds, Is.Not.Null);
            Assert.That(source.LearningWorlds.Count, Is.EqualTo(2));
            Assert.That(source.LearningWorlds.First().Name, Is.EqualTo("world1"));
            Assert.That(source.LearningWorlds.Last().Name, Is.EqualTo("world2"));
            Assert.That(source.SelectedLearningWorld, Is.Not.Null);
            Assert.That(source.SelectedLearningWorld.Name, Is.EqualTo("world1"));
            Assert.That(source.SelectedLearningWorld, Is.EqualTo(source.LearningWorlds.First()));
        });
    }

    #region testable Content/Element/Space/World

    private static LearningContent GetTestableContent()
    {
        return new LearningContent(Name, Type, Content);
    }

    private static LearningContentViewModel GetTestableNewContentViewModel()
    {
        return new LearningContentViewModel(NewName, NewType, NewContent);
    }

    private static LearningContentPe GetTestableNewContentPersistEntity()
    {
        return new LearningContentPe(NewName, NewType, NewContent);
    }

    private static LearningElement GetTestableElementWithParent(object parent)
    {
        return parent switch
        {
            LearningWorld world => new LearningElement(Name, Shortname, GetTestableContent(), Authors, Description,
                Goals, Difficulty, world, Workload, PositionX, PositionY),
            LearningSpace space => new LearningElement(Name, Shortname, GetTestableContent(), Authors, Description,
                Goals, Difficulty, space, Workload, PositionX, PositionY),
            _ => throw new ArgumentException($"{parent.GetType().Name} is not a valid parent type")
        };
    }

    private static LearningElementViewModel GetTestableNewElementViewModelWithParent(object parent)
    {
        return parent switch
        {
            LearningWorldViewModel world => new LearningElementViewModel(NewName, NewShortname,
                GetTestableNewContentViewModel(), NewAuthors, NewDescription, NewGoals, NewDifficulty, world,
                NewWorkload, NewPositionX, NewPositionY),
            LearningSpaceViewModel space => new LearningElementViewModel(NewName, NewShortname,
                GetTestableNewContentViewModel(), NewAuthors, NewDescription, NewGoals, NewDifficulty, space,
                NewWorkload, NewPositionX, NewPositionY),
            _ => throw new ArgumentException($"{parent.GetType().Name} is not a valid parent type")
        };
    }

    private static LearningElementPe GetTestableNewElementPersistEntity()
    {
        return new LearningElementPe(NewName, NewShortname,
            GetTestableNewContentPersistEntity(), NewAuthors, NewDescription, NewGoals, NewDifficultyPe, NewWorkload,
            NewPositionX, NewPositionY);
    }

    private static LearningSpace GetTestableSpace()
    {
        var space = new LearningSpace(Name, Shortname, Authors, Description, Goals,
            new List<LearningElement>(), PositionX, PositionY);
        var element = GetTestableElementWithParent(space);
        space.LearningElements.Add(element);
        return space;
    }

    private static LearningSpaceViewModel GetTestableNewSpaceViewModel()
    {
        var space = new LearningSpaceViewModel(NewName, NewShortname, NewAuthors, NewDescription, NewGoals,
            new List<ILearningElementViewModel>(), NewPositionX, NewPositionY);
        var element = GetTestableNewElementViewModelWithParent(space);
        space.LearningElements.Add(element);
        return space;
    }

    private static LearningSpacePe GetTestableNewSpacePersistEntity()
    {
        return new LearningSpacePe(NewName, NewShortname, NewAuthors, NewDescription, NewGoals,
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
                    TestElementsList(world.LearningElements, world, useNewFields);
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
                    TestElementsList(world.LearningElements, world, useNewFields);
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
                    TestElementsList(world.LearningElements, world, useNewFields);
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
                    Assert.That(element.Authors, Is.EqualTo(useNewFields ? NewAuthors : Authors));
                    Assert.That(element.Description, Is.EqualTo(useNewFields ? NewDescription : Description));
                    Assert.That(element.Goals, Is.EqualTo(useNewFields ? NewGoals : Goals));
                    Assert.That(element.Difficulty, Is.EqualTo(useNewFields ? NewDifficulty : Difficulty));
                    Assert.That(element.Parent, Is.EqualTo(parent));
                    Assert.That(element.Workload, Is.EqualTo(useNewFields ? NewWorkload : Workload));
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
                    Assert.That(element.Authors, Is.EqualTo(useNewFields ? NewAuthors : Authors));
                    Assert.That(element.Description, Is.EqualTo(useNewFields ? NewDescription : Description));
                    Assert.That(element.Goals, Is.EqualTo(useNewFields ? NewGoals : Goals));
                    Assert.That(element.Difficulty, Is.EqualTo(useNewFields ? NewDifficulty : Difficulty));
                    Assert.That(element.Parent, Is.EqualTo(parent));
                    Assert.That(element.Workload, Is.EqualTo(useNewFields ? NewWorkload : Workload));
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
                    Assert.That(element.Authors, Is.EqualTo(useNewFields ? NewAuthors : Authors));
                    Assert.That(element.Description, Is.EqualTo(useNewFields ? NewDescription : Description));
                    Assert.That(element.Goals, Is.EqualTo(useNewFields ? NewGoals : Goals));
                    Assert.That(element.Difficulty, Is.EqualTo(useNewFields ? NewDifficultyPe : DifficultyPe));
                    Assert.That(element.Workload, Is.EqualTo(useNewFields ? NewWorkload : Workload));
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
                    Assert.That(content.Content, Is.EqualTo(useNewFields ? NewContent : Content));
                });
                break;
            case LearningContent content:
                Assert.Multiple(() =>
                {
                    Assert.That(content.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(content.Type, Is.EqualTo(useNewFields ? NewType : Type));
                    Assert.That(content.Content, Is.EqualTo(useNewFields ? NewContent : Content));
                });
                break;
            case LearningContentPe content:
                Assert.Multiple(() =>
                {
                    Assert.That(content.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(content.Type, Is.EqualTo(useNewFields ? NewType : Type));
                    Assert.That(content.Content, Is.EqualTo(useNewFields ? NewContent : Content));
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