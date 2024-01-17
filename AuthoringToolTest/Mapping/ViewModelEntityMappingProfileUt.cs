using AuthoringTool.Mapping;
using AutoMapper;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent.Adaptivity;
using BusinessLogic.Entities.LearningContent.Adaptivity.Action;
using BusinessLogic.Entities.LearningContent.Adaptivity.Question;
using BusinessLogic.Entities.LearningContent.Adaptivity.Trigger;
using BusinessLogic.Entities.LearningContent.FileContent;
using BusinessLogic.Entities.LearningContent.LinkContent;
using NUnit.Framework;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent.Action;
using Presentation.PresentationLogic.LearningContent.FileContent;
using Presentation.PresentationLogic.LearningContent.LinkContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningPathway;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningSpace.SpaceLayout;
using Presentation.PresentationLogic.LearningWorld;
using Shared;
using Shared.Adaptivity;
using TestHelpers;

namespace AuthoringToolTest.Mapping;

[TestFixture]
public class ViewModelEntityMappingProfileUt
{
    private const string Name = "name";
    private const string Shortname = "shortname";
    private const string Authors = "authors";
    private const string Language = "language";
    private const string Description = "description";
    private const string Goals = "goals";
    private const string EvaluationLink = "evaluationLink";
    private const string EnrolmentKey = "enrolmentKey";
    private const string SavePath = "foo/bar/baz.txt";
    private const string Type = "type";
    private static readonly string Filepath = "bar/baz/buz.txt";
    private const LearningElementDifficultyEnum Difficulty = LearningElementDifficultyEnum.Easy;
    private const ElementModel SelectedElementModel = ElementModel.l_h5p_slotmachine_1;
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
    private const string NewEvaluationLink = "newEvaluationLink";
    private const string NewEnrolmentKey = "newEnrolmentKey";
    private const string NewSavePath = "faa/bur/buz.txt";
    private const string NewType = "newType";
    private static readonly string NewFilepath = "/foo/bar/baz.txt";
    private const LearningElementDifficultyEnum NewDifficulty = LearningElementDifficultyEnum.Medium;
    private const ElementModel NewSelectedElementModel = ElementModel.l_h5p_blackboard_1;
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
            ViewModelEntityMappingProfile.Configure(cfg);
            cfg.AddCollectionMappersOnce();
        });

        Assert.That(() => mapper.AssertConfigurationIsValid(), Throws.Nothing);
    }

    [Test]
    public void MapLearningContentAndLearningContentViewModel_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new FileContent(Name, Type, Filepath);
        var destination = new FileContentViewModel("", "", "bar/baz/buz.txt");

        systemUnderTest.Map(source, destination);

        TestContent(destination, false);

        systemUnderTest.Map(destination, source);

        TestContent(source, false);
    }

    [Test]
    public void MapLearningElementAndLearningElementViewModel_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var content = GetTestableContent();
        var source = new LearningElement(Name, content, Description, Goals,
            Difficulty, ElementModel.l_h5p_slotmachine_1, workload: Workload, points: Points, positionX: PositionX,
            positionY: PositionY);
        var destination = new LearningElementViewModel("",
            new FileContentViewModel("", "", Filepath), "", "", LearningElementDifficultyEnum.None,
            ElementModel.l_h5p_blackboard_1);

        systemUnderTest.Map(source, destination);

        TestElement(destination, null, false);
        Assert.That(destination.Id, Is.EqualTo(source.Id));

        destination.Name = NewName;
        destination.LearningContent = new FileContentViewModel(NewName, NewType, NewFilepath);
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
    public void MapLearningSpaceAndLearningSpaceViewModel_WithoutLearningElement_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningSpace(Name, Description, RequiredPoints, Theme.Campus,
            positionX: PositionX,
            positionY: PositionY);
        var destination = new LearningSpaceViewModel("", "", Theme.Campus);

        systemUnderTest.Map(source, destination);

        TestSpace(destination, false);
        Assert.That(destination.ContainedLearningElements, Is.Empty);

        destination.Name = NewName;
        destination.Description = NewDescription;
        destination.RequiredPoints = NewRequiredPoints;
        destination.LearningSpaceLayout.LearningElements = new Dictionary<int, ILearningElementViewModel>();
        destination.PositionX = NewPositionX;
        destination.PositionY = NewPositionY;

        systemUnderTest.Map(destination, source);

        TestSpace(source, true);
        Assert.That(source.ContainedLearningElements, Is.Empty);
    }

    [Test]
    public void MapLearningSpaceAndLearningSpaceViewModel_WithLearningElement_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningSpace(Name, Description, RequiredPoints, Theme.Campus,
            EntityProvider.GetLearningOutcomes(),
            new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.R_20X30_8L),
            positionX: PositionX, positionY: PositionY);
        source.LearningSpaceLayout.LearningElements[0] = GetTestableElementWithParent(source);
        var destination = new LearningSpaceViewModel("", "", Theme.Campus);

        systemUnderTest.Map(source, destination);

        TestSpace(destination, false);
        Assert.That(destination.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(destination.Id, Is.EqualTo(source.Id));

        destination.Name = NewName;
        destination.Description = NewDescription;
        destination.RequiredPoints = NewRequiredPoints;
        destination.LearningSpaceLayout.LearningElements = new Dictionary<int, ILearningElementViewModel>
        {
            {
                0,
                GetTestableElementViewModelWithParent(destination)
            }
        };
        destination.PositionX = NewPositionX;
        destination.PositionY = NewPositionY;

        systemUnderTest.Map(destination, source);

        TestSpace(source, true);
        Assert.That(source.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(destination.Id, Is.EqualTo(source.Id));
    }

    [Test]
    public void MapLearningWorldAndLearningWorldViewModel_WithoutLearningSpaces_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            evaluationLink: EvaluationLink, EnrolmentKey, savePath: SavePath, new List<ILearningSpace>());
        var destination = new LearningWorldViewModel("", "", "", "", "", "", "", "");

        systemUnderTest.Map(source, destination);

        TestWorld(destination, false);
        Assert.Multiple(() => { Assert.That(destination.LearningSpaces, Is.Empty); });

        destination.Name = NewName;
        destination.Shortname = NewShortname;
        destination.Authors = NewAuthors;
        destination.Language = NewLanguage;
        destination.Description = NewDescription;
        destination.Goals = NewGoals;
        destination.EvaluationLink = NewEvaluationLink;
        destination.EnrolmentKey = NewEnrolmentKey;
        destination.SavePath = NewSavePath;
        destination.LearningSpaces = new List<ILearningSpaceViewModel>();

        systemUnderTest.Map(destination, source);

        TestWorld(source, true);
        Assert.Multiple(() => { Assert.That(source.LearningSpaces, Is.Empty); });
    }

    [Test]
    public void MapLearningWorldAndLearningWorldViewModel_WithEmptyLearningSpace_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var source = new LearningWorld(Name, Shortname, Authors, Language, Description, Goals,
            EvaluationLink, EnrolmentKey, savePath: SavePath, new List<ILearningSpace>());
        source.LearningSpaces.Add(new LearningSpace(Name, Description, RequiredPoints, Theme.Campus,
            positionX: PositionX,
            positionY: PositionY));
        var destination = new LearningWorldViewModel("", "", "", "", "", "", "", "");

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
        destination.EvaluationLink = NewEvaluationLink;
        destination.EnrolmentKey = NewEnrolmentKey;
        destination.SavePath = NewSavePath;
        destination.LearningSpaces = new List<ILearningSpaceViewModel>
        {
            new LearningSpaceViewModel(NewName, NewDescription, Theme.Campus, NewRequiredPoints,
                positionX: NewPositionX, positionY: NewPositionY)
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
            EvaluationLink, EnrolmentKey, savePath: SavePath, new List<ILearningSpace>());
        source.LearningSpaces.Add(GetTestableSpace());
        var destination = new LearningWorldViewModel("", "", "", "", "", "", "", "");

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
        destination.EvaluationLink = NewEvaluationLink;
        destination.EnrolmentKey = NewEnrolmentKey;
        destination.SavePath = NewSavePath;
        destination.LearningSpaces = new List<ILearningSpaceViewModel> { GetTestableNewSpaceViewModel() };

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
            EvaluationLink, EnrolmentKey, savePath: SavePath, new List<ILearningSpace>());
        var space1 = GetTestableSpace();
        var pathWayCondition = new PathWayCondition(ConditionEnum.And, 3, 2);
        source.LearningSpaces.Add(space1);
        source.PathWayConditions.Add(pathWayCondition);
        var destination = new LearningWorldViewModel("", "", "", "", "", "", "", "");

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
        destination.EvaluationLink = NewEvaluationLink;
        destination.EnrolmentKey = NewEnrolmentKey;
        destination.SavePath = NewSavePath;

        var spaceVm1 = GetTestableNewSpaceViewModel();
        var pathWayConditionVm = new PathWayConditionViewModel(ConditionEnum.And, false, 2, 1);
        destination.LearningSpaces = new List<ILearningSpaceViewModel> { spaceVm1 };
        destination.PathWayConditions = new List<PathWayConditionViewModel> { pathWayConditionVm };
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

    /// <summary>
    /// This test tests whether or not the AutoMapper.Collection configuration works as intended.
    /// See https://github.com/AutoMapper/AutoMapper.Collection
    /// </summary>
    [Test]
    public void MapLearningWorldAndLearningWorldViewModel_WithSpaces_ObjectsStayEqual()
    {
        var elementVm1 =
            new LearningElementViewModel("el1", new FileContentViewModel("foo", "bar", Filepath),
                Description, Goals, Difficulty, ElementModel.l_h5p_slotmachine_1);

        var space = new LearningSpaceViewModel("space", Description, Theme.Campus, RequiredPoints,
                ViewModelProvider.GetLearningOutcomes(),
                new LearningSpaceLayoutViewModel(FloorPlanEnum.R_20X30_8L)
                {
                    LearningElements = new Dictionary<int, ILearningElementViewModel>
                    {
                        {
                            0,
                            elementVm1
                        }
                    }
                })
            ;
        elementVm1.Parent = space;

        var worldVm = new LearningWorldViewModel("world", Shortname, Authors, Language, Description, Goals,
            EvaluationLink, EnrolmentKey, SavePath,
            true,
            new List<ILearningSpaceViewModel> { space });


        var systemUnderTest = CreateTestableMapper();

        var worldEntity = systemUnderTest.Map<LearningWorld>(worldVm);
        worldEntity.LearningSpaces.First().LearningSpaceLayout.ContainedLearningElements.First().Name = "foooooooooo";


        //map back into viewmodel - with update syntax
        systemUnderTest.Map(worldEntity, worldVm);

        //we would expect that the objects are still the same and we retained view specific information
        Assert.That(worldVm.LearningSpaces.First(), Is.EqualTo(space));

        Assert.Multiple(() =>
        {
            Assert.That(worldVm.LearningSpaces.First().LearningSpaceLayout.ContainedLearningElements.Count(),
                Is.EqualTo(1));
            Assert.That(worldVm.LearningSpaces.First().LearningSpaceLayout.ContainedLearningElements.First().Name,
                Is.EqualTo(
                    worldEntity.LearningSpaces.First().LearningSpaceLayout.ContainedLearningElements.First().Name));
            Assert.That(worldVm.LearningSpaces.First().ContainedLearningElements.First(), Is.EqualTo(elementVm1));
        });
    }

    [Test]
    public void MapAuthoringToolWorkspaceAndAuthoringToolWorkspaceViewModel_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var world1 = new LearningWorld("world1", Shortname, Authors, Language, Description, Goals, EvaluationLink,
            EnrolmentKey);
        var world2 = new LearningWorld("world2", Shortname, Authors, Language, Description, Goals, EvaluationLink,
            EnrolmentKey);
        var source = new AuthoringToolWorkspace(new List<ILearningWorld> { world1, world2 });
        var destination = new AuthoringToolWorkspaceViewModel();

        systemUnderTest.Map(source, destination);

        Assert.That(destination.LearningWorlds, Is.Not.Null);
        Assert.That(destination.LearningWorlds.Count, Is.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(destination.LearningWorlds.First().Name, Is.EqualTo("world1"));
            Assert.That(destination.LearningWorlds.Last().Name, Is.EqualTo("world2"));
        });

        systemUnderTest.Map(destination, source);

        Assert.That(source.LearningWorlds, Is.Not.Null);
        Assert.That(source.LearningWorlds, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(source.LearningWorlds.First(), Is.EqualTo(world1));
            Assert.That(source.LearningWorlds.Last(), Is.EqualTo(world2));
            Assert.That(source.LearningWorlds.First().Name, Is.EqualTo("world1"));
            Assert.That(source.LearningWorlds.Last().Name, Is.EqualTo("world2"));
        });

        // Test the private constructor from AuthoringToolWorkspace
        var source2 = systemUnderTest.Map<AuthoringToolWorkspace>(destination);

        Assert.That(source2.LearningWorlds, Is.Not.Null);
        Assert.That(source2.LearningWorlds, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(source2.LearningWorlds.First().Name, Is.EqualTo("world1"));
            Assert.That(source2.LearningWorlds.Last().Name, Is.EqualTo("world2"));
        });
    }

    [Test]
    public void AdaptivityTrigger_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var correctnessTriggerVm = ViewModelProvider.GetCorrectnessTrigger();
        var timeTriggerVm = ViewModelProvider.GetTimeTrigger();
        var compositeTriggerVm = ViewModelProvider.GetCompositeTrigger();

        var correctnessTrigger = systemUnderTest.Map<CorrectnessTrigger>(correctnessTriggerVm);
        var timeTrigger = systemUnderTest.Map<TimeTrigger>(timeTriggerVm);
        var compositeTrigger = systemUnderTest.Map<CompositeTrigger>(compositeTriggerVm);

        correctnessTrigger.ExpectedAnswer = AnswerResult.Incorrect;
        timeTrigger.Expected = 123;
        compositeTrigger.Condition = ConditionEnum.Or;

        systemUnderTest.Map(correctnessTrigger, correctnessTriggerVm);
        systemUnderTest.Map(timeTrigger, timeTriggerVm);
        systemUnderTest.Map(compositeTrigger, compositeTriggerVm);

        Assert.Multiple(() =>
        {
            Assert.That(correctnessTriggerVm.ExpectedAnswer, Is.EqualTo(AnswerResult.Incorrect));
            Assert.That(timeTriggerVm.Expected, Is.EqualTo(123));
            Assert.That(compositeTriggerVm.Condition, Is.EqualTo(ConditionEnum.Or));
        });
    }

    [Test]
    public void AdaptivityAction_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var commentActionVm = ViewModelProvider.GetCommentAction();
        var elementReferenceActionVm = ViewModelProvider.GetElementReferenceAction();
        var contentReferenceActionVm = ViewModelProvider.GetContentReferenceAction();

        var commentAction = systemUnderTest.Map<CommentAction>(commentActionVm);
        var elementReferenceAction = systemUnderTest.Map<ElementReferenceAction>(elementReferenceActionVm);
        var contentReferenceAction = systemUnderTest.Map<ContentReferenceAction>(contentReferenceActionVm);

        commentAction.Comment = "another comment";
        elementReferenceAction.ElementId = Guid.NewGuid();
        contentReferenceAction.Content = new LinkContent("a name", "a link");

        systemUnderTest.Map(commentAction, commentActionVm);
        systemUnderTest.Map(elementReferenceAction, elementReferenceActionVm);
        systemUnderTest.Map(contentReferenceAction, contentReferenceActionVm);

        Assert.Multiple(() =>
        {
            Assert.That(commentActionVm.Comment, Is.EqualTo("another comment"));
            Assert.That(elementReferenceActionVm.ElementId, Is.EqualTo(elementReferenceAction.ElementId));
            Assert.That(contentReferenceActionVm.Content, Is.TypeOf<LinkContentViewModel>());
        });
        var linkContentVm = (LinkContentViewModel)contentReferenceActionVm.Content;
        Assert.Multiple(() =>
        {
            Assert.That(linkContentVm.Name, Is.EqualTo("a name"));
            Assert.That(linkContentVm.Link, Is.EqualTo("a link"));
        });
    }

    [Test]
    public void AdaptivityRule_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var ruleVm = ViewModelProvider.GetRule();

        var rule = systemUnderTest.Map<AdaptivityRule>(ruleVm);

        rule.Action = EntityProvider.GetContentReferenceAction();

        systemUnderTest.Map(rule, ruleVm);

        Assert.That(ruleVm.Action, Is.TypeOf<ContentReferenceActionViewModel>());
    }

    [Test]
    public void MultipleChoiceSingleResponseQuestion_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();
        var questionVm = ViewModelProvider.GetMultipleChoiceSingleResponseQuestion();

        Assert.Multiple(() =>
        {
            Assert.That(questionVm.CorrectChoice, Is.EqualTo(questionVm.Choices.First()));
            Assert.That(questionVm.CorrectChoices, Is.EquivalentTo(questionVm.Choices));
        });

        var questionEntity = systemUnderTest.Map<MultipleChoiceSingleResponseQuestion>(questionVm);

        Assert.Multiple(() =>
        {
            Assert.That(questionEntity.CorrectChoice, Is.EqualTo(questionEntity.Choices.First()));
            Assert.That(questionEntity.CorrectChoices, Is.EquivalentTo(questionEntity.Choices));
        });

        Assert.That(() => systemUnderTest.Map(questionEntity, questionVm), Throws.Nothing);
    }

    [Test]
    public void AdaptivityElement_FullStructure_TestMappingIsValid()
    {
        var systemUnderTest = CreateTestableMapper();

        var elementVm = ViewModelProvider.GetLearningElement();
        var adaptivityContent = ViewModelProvider.GetAdaptivityContent();
        var tasks = adaptivityContent.Tasks.ToList();

        elementVm.LearningContent = adaptivityContent;

        var element = systemUnderTest.Map<LearningElement>(elementVm);

        Assert.That(element.LearningContent, Is.TypeOf<AdaptivityContent>());

        systemUnderTest.Map(element, elementVm);

        Assert.Multiple(() =>
        {
            Assert.That(elementVm.LearningContent, Is.EqualTo(adaptivityContent));
            Assert.That(tasks.First(),
                Is.EqualTo(((AdaptivityContentViewModel)elementVm.LearningContent).Tasks.First()));
        });
    }

    private static FileContent GetTestableContent()
    {
        return new FileContent(Name, Type, Filepath);
    }

    private static FileContentViewModel GetTestableNewContentViewModel()
    {
        return new FileContentViewModel(NewName, NewType, NewFilepath);
    }

    private static LearningElement GetTestableElementWithParent(LearningSpace parent)
    {
        return new LearningElement(Name,
            GetTestableContent(), Description, Goals, Difficulty, SelectedElementModel, parent, Workload, Points,
            PositionX,
            PositionY);
    }

    private static LearningElementViewModel GetTestableElementViewModelWithParent(LearningSpaceViewModel parent)
    {
        return new LearningElementViewModel(NewName,
            GetTestableNewContentViewModel(), NewDescription, NewGoals, NewDifficulty, NewSelectedElementModel, parent,
            NewWorkload, NewPoints, NewPositionX, NewPositionY);
    }

    private static LearningSpace GetTestableSpace()
    {
        var space = new LearningSpace(Name, Description, RequiredPoints, Theme.Campus,
            EntityProvider.GetLearningOutcomes(),
            new LearningSpaceLayout(new Dictionary<int, ILearningElement>(), FloorPlanEnum.R_20X30_8L),
            positionX: PositionX, positionY: PositionY);
        var element = GetTestableElementWithParent(space);
        space.LearningSpaceLayout.LearningElements[0] = element;
        return space;
    }

    private static LearningSpaceViewModel GetTestableNewSpaceViewModel()
    {
        var space = new LearningSpaceViewModel(NewName, NewDescription, Theme.Campus,
            NewRequiredPoints, ViewModelProvider.GetLearningOutcomes(),
            new LearningSpaceLayoutViewModel(FloorPlanEnum.R_20X30_8L), positionX: NewPositionX,
            positionY: NewPositionY);
        var element = GetTestableElementViewModelWithParent(space);
        space.LearningSpaceLayout.PutElement(0, element);
        return space;
    }

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
                    Assert.That(world.EvaluationLink, Is.EqualTo(useNewFields ? NewEvaluationLink : EvaluationLink));
                    Assert.That(world.EnrolmentKey, Is.EqualTo(useNewFields ? NewEnrolmentKey : EnrolmentKey));
                    Assert.That(world.SavePath, Is.EqualTo(useNewFields ? NewSavePath : SavePath));
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
                    Assert.That(world.EvaluationLink, Is.EqualTo(useNewFields ? NewEvaluationLink : EvaluationLink));
                    Assert.That(world.EnrolmentKey, Is.EqualTo(useNewFields ? NewEnrolmentKey : EnrolmentKey));
                    Assert.That(world.SavePath, Is.EqualTo(useNewFields ? NewSavePath : SavePath));
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
                    Assert.That(space.Description, Is.EqualTo(useNewFields ? NewDescription : Description));
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
                    Assert.That(space.Description, Is.EqualTo(useNewFields ? NewDescription : Description));
                    Assert.That(space.RequiredPoints, Is.EqualTo(useNewFields ? NewRequiredPoints : RequiredPoints));
                    TestElementsList(space.ContainedLearningElements, space, useNewFields);
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
        }
    }

    private static void TestContent(object elementLearningContent, bool useNewFields)
    {
        switch (elementLearningContent)
        {
            case FileContentViewModel content:
                Assert.Multiple(() =>
                {
                    Assert.That(content.Name, Is.EqualTo(useNewFields ? NewName : Name));
                    Assert.That(content.Type, Is.EqualTo(useNewFields ? NewType : Type));
                    Assert.That(content.Filepath, Is.EqualTo(useNewFields ? NewFilepath : Filepath));
                });
                break;
            case FileContent content:
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

    private static IMapper CreateTestableMapper()
    {
        var mapper = new MapperConfiguration(cfg =>
        {
            ViewModelEntityMappingProfile.Configure(cfg);
            cfg.AddCollectionMappersOnce();
        });
        var systemUnderTest = mapper.CreateMapper();
        return systemUnderTest;
    }
}