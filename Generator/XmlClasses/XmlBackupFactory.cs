﻿using System.Text;
using Generator.ATF;
using Generator.ATF.AdaptivityElement;
using Generator.XmlClasses.Entities.Gradebook.xml;
using Generator.XmlClasses.Entities.Groups.xml;
using Generator.XmlClasses.Entities.MoodleBackup.xml;
using Generator.XmlClasses.Entities.Outcomes.xml;
using Generator.XmlClasses.Entities.Questions.xml;
using Generator.XmlClasses.Entities.Roles.xml;
using Generator.XmlClasses.Entities.Scales.xml;

namespace Generator.XmlClasses;

/// <summary>
/// sets the Parameter of gradebook.xml, groups.xml, outcomes.xml, questions.xml, scales.xml, roles.xml and
///  moodle_backup.xml files and create it
/// </summary>
public class XmlBackupFactory : IXmlBackupFactory
{
    private readonly int _contextId;
    private readonly string _currentTime;
    private readonly List<IElementJson> _learningElements;
    private readonly ILearningWorldJson _learningWorld;
    internal IGradebookXmlGradeCategories GradebookXmlGradeCategories;
    internal IGradebookXmlGradeCategory GradebookXmlGradeCategory;
    internal IGradebookXmlGradeItem GradebookXmlGradeItem;
    internal IGradebookXmlGradeItems GradebookXmlGradeItems;

    internal List<MoodleBackupXmlActivity> MoodleBackupXmlActivityList;
    internal List<MoodleBackupXmlSection> MoodleBackupXmlSectionList;
    internal List<MoodleBackupXmlSetting> MoodleBackupXmlSettingList;
    public IReadAtf ReadAtf;


    public XmlBackupFactory(IReadAtf readAtf, int contextId, IGradebookXmlGradeItem? gradebookXmlGradeItem = null,
        IGradebookXmlGradeItems? gradebookXmlGradeItems = null,
        IGradebookXmlGradeCategory? gradebookXmlGradeCategory = null,
        IGradebookXmlGradeCategories? gradebookXmlGradeCategories = null,
        IGradebookXmlGradeSetting? gradebookXmlGradebookSetting = null,
        IGradebookXmlGradeSettings? gradebookXmlGradebookSettings = null,
        IGradebookXmlGradebook? gradebookXmlGradebook = null,
        IGroupsXmlGroupingsList? groupsXmlGroupingsList = null, IGroupsXmlGroups? groupsXmlGroups = null,
        IMoodleBackupXmlDetail? moodleBackupXmlDetail = null, IMoodleBackupXmlDetails? moodleBackupXmlDetails = null,
        IMoodleBackupXmlActivities? moodleBackupXmlActivities = null,
        IMoodleBackupXmlSection? moodleBackupXmlSection = null,
        IMoodleBackupXmlSections? moodleBackupXmlSections = null, IMoodleBackupXmlCourse? moodleBackupXmlCourse = null,
        IMoodleBackupXmlContents? moodleBackupXmlContents = null,
        IMoodleBackupXmlSetting? moodleBackupXmlSetting = null,
        IMoodleBackupXmlInformation? moodleBackupXmlInformation = null,
        IMoodleBackupXmlMoodleBackup? moodleBackupXmlMoodleBackup = null,
        IOutcomesXmlOutcomesDefinition? outcomesXmlOutcomesDefinition = null,
        IQuestionsXmlQuestionsCategories? questionsXmlQuestionsCategories = null,
        IRolesXmlRole? rolesXmlRole = null, IRolesXmlRolesDefinition? rolesXmlRolesDefinition = null,
        IScalesXmlScalesDefinition? scalesXmlScalesDefinition = null)
    {
        ReadAtf = readAtf;
        _contextId = contextId;

        GradebookXmlGradeItem = gradebookXmlGradeItem ?? new GradebookXmlGradeItem();
        GradebookXmlGradeItems = gradebookXmlGradeItems ?? new GradebookXmlGradeItems();
        GradebookXmlGradeCategory = gradebookXmlGradeCategory ?? new GradebookXmlGradeCategory();
        GradebookXmlGradeCategories = gradebookXmlGradeCategories ?? new GradebookXmlGradeCategories();
        GradebookXmlGradebookSetting = gradebookXmlGradebookSetting ?? new GradebookXmlGradeSetting();
        GradebookXmlGradebookSettings = gradebookXmlGradebookSettings ?? new GradebookXmlGradeSettings();
        GradebookXmlGradebook = gradebookXmlGradebook ?? new GradebookXmlGradebook();

        GroupsXmlGroupingsList = groupsXmlGroupingsList ?? new GroupsXmlGroupingsList();
        GroupsXmlGroups = groupsXmlGroups ?? new GroupsXmlGroups();

        MoodleBackupXmlDetail = moodleBackupXmlDetail ?? new MoodleBackupXmlDetail();
        MoodleBackupXmlDetails = moodleBackupXmlDetails ?? new MoodleBackupXmlDetails();

        MoodleBackupXmlActivities = moodleBackupXmlActivities ?? new MoodleBackupXmlActivities();

        MoodleBackupXmlSection = moodleBackupXmlSection ?? new MoodleBackupXmlSection();
        MoodleBackupXmlSections = moodleBackupXmlSections ?? new MoodleBackupXmlSections();
        MoodleBackupXmlCourse = moodleBackupXmlCourse ?? new MoodleBackupXmlCourse();
        MoodleBackupXmlContents = moodleBackupXmlContents ?? new MoodleBackupXmlContents();

        MoodleBackupXmlSettingFilename = moodleBackupXmlSetting ?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingImscc11 = moodleBackupXmlSetting ?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingUsers = moodleBackupXmlSetting ?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingAnonymize = moodleBackupXmlSetting ?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingRoleAssignments = moodleBackupXmlSetting ?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingActivities = moodleBackupXmlSetting ?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingBlocks = moodleBackupXmlSetting ?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingFiles = moodleBackupXmlSetting ?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingFilters = moodleBackupXmlSetting ?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingComments = moodleBackupXmlSetting ?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingBadges = moodleBackupXmlSetting ?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingCalendarevents = moodleBackupXmlSetting ?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingUserscompletion = moodleBackupXmlSetting ?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingLogs = moodleBackupXmlSetting ?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingGradeHistories = moodleBackupXmlSetting ?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingQuestionbank = moodleBackupXmlSetting ?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingGroups = moodleBackupXmlSetting ?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingCompetencies = moodleBackupXmlSetting ?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingCustomfield = moodleBackupXmlSetting ?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingContentbankcontent = moodleBackupXmlSetting ?? new MoodleBackupXmlSetting();
        MoodleBackupXmlSettingLegacyfiles = moodleBackupXmlSetting ?? new MoodleBackupXmlSetting();

        MoodleBackupXmlSettings = new MoodleBackupXmlSettings();

        MoodleBackupXmlInformation = moodleBackupXmlInformation ?? new MoodleBackupXmlInformation();
        MoodleBackupXmlMoodleBackup = moodleBackupXmlMoodleBackup ?? new MoodleBackupXmlMoodleBackup();

        OutcomesXmlOutcomesDefinition = outcomesXmlOutcomesDefinition ?? new OutcomesXmlOutcomesDefinition();

        QuestionsXmlQuestionsCategories = questionsXmlQuestionsCategories ?? new QuestionsXmlQuestionsCategories();

        RolesXmlRole = rolesXmlRole ?? new RolesXmlRole();
        RolesXmlRolesDefinition = rolesXmlRolesDefinition ?? new RolesXmlRolesDefinition();

        ScalesXmlScalesDefinition = scalesXmlScalesDefinition ?? new ScalesXmlScalesDefinition();

        _currentTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

        _learningWorld = readAtf.GetLearningWorld();
        _learningElements = readAtf.GetElementsOrderedList();
        MoodleBackupXmlActivityList = new List<MoodleBackupXmlActivity>();
        MoodleBackupXmlSettingList = new List<MoodleBackupXmlSetting>();
        MoodleBackupXmlSectionList = new List<MoodleBackupXmlSection>();
    }

    internal IGradebookXmlGradeSetting GradebookXmlGradebookSetting { get; }
    internal IGradebookXmlGradeSettings GradebookXmlGradebookSettings { get; }
    internal IGradebookXmlGradebook GradebookXmlGradebook { get; }
    internal IGroupsXmlGroupingsList GroupsXmlGroupingsList { get; }
    internal IGroupsXmlGroups GroupsXmlGroups { get; }
    internal IMoodleBackupXmlDetail MoodleBackupXmlDetail { get; }
    internal IMoodleBackupXmlDetails MoodleBackupXmlDetails { get; }
    internal IMoodleBackupXmlActivities MoodleBackupXmlActivities { get; }
    internal IMoodleBackupXmlSection MoodleBackupXmlSection { get; }
    internal IMoodleBackupXmlSections MoodleBackupXmlSections { get; }
    internal IMoodleBackupXmlCourse MoodleBackupXmlCourse { get; }
    internal IMoodleBackupXmlContents MoodleBackupXmlContents { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingFilename { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingImscc11 { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingUsers { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingAnonymize { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingRoleAssignments { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingActivities { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingBlocks { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingFiles { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingFilters { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingComments { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingBadges { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingCalendarevents { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingUserscompletion { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingLogs { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingGradeHistories { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingQuestionbank { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingGroups { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingCompetencies { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingCustomfield { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingContentbankcontent { get; }
    internal IMoodleBackupXmlSetting MoodleBackupXmlSettingLegacyfiles { get; }
    internal IMoodleBackupXmlSettings MoodleBackupXmlSettings { get; }
    internal IMoodleBackupXmlInformation MoodleBackupXmlInformation { get; }
    internal IMoodleBackupXmlMoodleBackup MoodleBackupXmlMoodleBackup { get; }
    internal IOutcomesXmlOutcomesDefinition OutcomesXmlOutcomesDefinition { get; }
    internal IQuestionsXmlQuestionsCategories QuestionsXmlQuestionsCategories { get; }
    internal IScalesXmlScalesDefinition ScalesXmlScalesDefinition { get; }
    internal IRolesXmlRole RolesXmlRole { get; }
    internal IRolesXmlRolesDefinition RolesXmlRolesDefinition { get; }

    /// <summary>
    /// Use all Methods of the current class.
    /// </summary>
    public void CreateXmlBackupFactory()
    {
        //Set parameter and create gradebook.xml
        CreateGradebookXml();

        //Set parameter and create groups.xml
        CreateGroupsXml();

        //Set parameter and create outcomes.xml
        CreateOutcomesXml();

        //Set parameter and create questions.xml
        CreateQuestionsXml();

        //Set parameter and create roles.xml
        CreateRolesXml();

        //Set parameter and create scales.xml
        CreateScalesXml();

        //Set parameter and create moodle_backup.xml
        CreateMoodleBackupXml();
    }


    /// <summary>
    /// Set the parameter of the gradebook.xml file and create it.
    /// </summary>
    public void CreateGradebookXml()
    {
        //set the parameter of gradebook.xml file
        GradebookXmlGradeItem.Timecreated = _currentTime;
        GradebookXmlGradeItem.Timemodified = _currentTime;
        GradebookXmlGradeItems.GradeItem =
            GradebookXmlGradeItem as GradebookXmlGradeItem ?? new GradebookXmlGradeItem();

        GradebookXmlGradeCategory.Timecreated = _currentTime;
        GradebookXmlGradeCategory.Timemodified = _currentTime;
        GradebookXmlGradeCategories.GradeCategory = GradebookXmlGradeCategory as GradebookXmlGradeCategory ??
                                                    new GradebookXmlGradeCategory();

        GradebookXmlGradebookSettings.GradeSetting = GradebookXmlGradebookSetting as GradebookXmlGradeSetting ??
                                                     new GradebookXmlGradeSetting();

        GradebookXmlGradebook.GradeCategories = GradebookXmlGradeCategories as GradebookXmlGradeCategories ??
                                                new GradebookXmlGradeCategories();
        GradebookXmlGradebook.GradeItems =
            GradebookXmlGradeItems as GradebookXmlGradeItems ?? new GradebookXmlGradeItems();
        GradebookXmlGradebook.GradeSettings = GradebookXmlGradebookSettings as GradebookXmlGradeSettings ??
                                              new GradebookXmlGradeSettings();

        //create the gradebook.xml file
        GradebookXmlGradebook.Serialize();
    }

    public void CreateGroupsXml()
    {
        //set the parameter of groups.xml file
        GroupsXmlGroups.GroupingsList =
            GroupsXmlGroupingsList as GroupsXmlGroupingsList ?? new GroupsXmlGroupingsList();

        //create groups.xml file
        GroupsXmlGroups.Serialize();
    }

    public void CreateMoodleBackupXml()
    {
        //set the parameter of the moodle_backup.xml file
        MoodleBackupXmlDetails.Detail = MoodleBackupXmlDetail as MoodleBackupXmlDetail ?? new MoodleBackupXmlDetail();

        MoodleBackupXmlCourse.Title = _learningWorld.WorldName;

        InitializeMoodleBackupSetting();

        AddLearningElementsToBackup();

        AddSectionsToBackup();

        AddBaseLearningElementsToBackup();

        MoodleBackupXmlContents.Activities =
            MoodleBackupXmlActivities as MoodleBackupXmlActivities ?? new MoodleBackupXmlActivities();
        MoodleBackupXmlContents.Sections =
            MoodleBackupXmlSections as MoodleBackupXmlSections ?? new MoodleBackupXmlSections();
        MoodleBackupXmlContents.Course = MoodleBackupXmlCourse as MoodleBackupXmlCourse ?? new MoodleBackupXmlCourse();

        MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingFilename as MoodleBackupXmlSetting ??
                                       new MoodleBackupXmlSetting());
        MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingImscc11 as MoodleBackupXmlSetting ??
                                       new MoodleBackupXmlSetting());
        MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingUsers as MoodleBackupXmlSetting ??
                                       new MoodleBackupXmlSetting());
        MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingAnonymize as MoodleBackupXmlSetting ??
                                       new MoodleBackupXmlSetting());
        MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingRoleAssignments as MoodleBackupXmlSetting ??
                                       new MoodleBackupXmlSetting());
        MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingActivities as MoodleBackupXmlSetting ??
                                       new MoodleBackupXmlSetting());
        MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingBlocks as MoodleBackupXmlSetting ??
                                       new MoodleBackupXmlSetting());
        MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingFiles as MoodleBackupXmlSetting ??
                                       new MoodleBackupXmlSetting());
        MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingFilters as MoodleBackupXmlSetting ??
                                       new MoodleBackupXmlSetting());
        MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingComments as MoodleBackupXmlSetting ??
                                       new MoodleBackupXmlSetting());
        MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingBadges as MoodleBackupXmlSetting ??
                                       new MoodleBackupXmlSetting());
        MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingCalendarevents as MoodleBackupXmlSetting ??
                                       new MoodleBackupXmlSetting());
        MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingUserscompletion as MoodleBackupXmlSetting ??
                                       new MoodleBackupXmlSetting());
        MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingLogs as MoodleBackupXmlSetting ??
                                       new MoodleBackupXmlSetting());
        MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingGradeHistories as MoodleBackupXmlSetting ??
                                       new MoodleBackupXmlSetting());
        MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingQuestionbank as MoodleBackupXmlSetting ??
                                       new MoodleBackupXmlSetting());
        MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingGroups as MoodleBackupXmlSetting ??
                                       new MoodleBackupXmlSetting());
        MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingCompetencies as MoodleBackupXmlSetting ??
                                       new MoodleBackupXmlSetting());
        MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingCustomfield as MoodleBackupXmlSetting ??
                                       new MoodleBackupXmlSetting());
        MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingContentbankcontent as MoodleBackupXmlSetting ??
                                       new MoodleBackupXmlSetting());
        MoodleBackupXmlSettingList.Add(MoodleBackupXmlSettingLegacyfiles as MoodleBackupXmlSetting ??
                                       new MoodleBackupXmlSetting());

        MoodleBackupXmlSettings.Setting = MoodleBackupXmlSettingList;

        //Base information about the moodle_backup.xml file. The value in originalCourseFullName & 
        //OriginalCourseShortName will be displayed, when the .mbz file is uploaded to moodle.
        //Its important that the parameter "includeFiles" is set to "1".
        //With the OriginalCourseFormat the future learningWorld can be individualised
        MoodleBackupXmlInformation.BackupDate = _currentTime;
        MoodleBackupXmlInformation.OriginalCourseFullname = _learningWorld.WorldName;
        MoodleBackupXmlInformation.OriginalCourseShortname = _learningWorld.WorldName;
        MoodleBackupXmlInformation.OriginalCourseStartDate = _currentTime;
        MoodleBackupXmlInformation.OriginalCourseContextId = _contextId.ToString();
        MoodleBackupXmlInformation.Details =
            MoodleBackupXmlDetails as MoodleBackupXmlDetails ?? new MoodleBackupXmlDetails();
        MoodleBackupXmlInformation.Contents =
            MoodleBackupXmlContents as MoodleBackupXmlContents ?? new MoodleBackupXmlContents();
        MoodleBackupXmlInformation.Settings =
            MoodleBackupXmlSettings as MoodleBackupXmlSettings ?? new MoodleBackupXmlSettings();


        MoodleBackupXmlMoodleBackup.Information = MoodleBackupXmlInformation as MoodleBackupXmlInformation ??
                                                  new MoodleBackupXmlInformation();

        //create moodle_backup.xml file
        MoodleBackupXmlMoodleBackup.Serialize();
    }


    public void CreateOutcomesXml()
    {
        //create outcomes.xml file
        OutcomesXmlOutcomesDefinition.Serialize();
    }

    /// <inheritdoc cref="IXmlBackupFactory.CreateQuestionsXml"/>
    public void CreateQuestionsXml()
    {
        var listAdaptivityElements = ReadAtf.GetAdaptivityElementsList();

        if (listAdaptivityElements.Count == 0)
        {
            QuestionsXmlQuestionsCategories.Serialize();
            return;
        }

        var questionCategory3 = new QuestionsXmlQuestionsCategory(3, "top", _contextId, 0);
        var questionCategory4 = new QuestionsXmlQuestionsCategory(4, "Default for name", _contextId, 3);

        var answerId = 1;

        var questions = listAdaptivityElements.SelectMany(ae => ae.AdaptivityContent.AdaptivityTasks)
            .SelectMany(t => t.AdaptivityQuestions).ToList();


        foreach (var questionBankEntryXml in questions.Select(question =>
                     CreateQuestionBankEntryXml(question, question.QuestionId, ref answerId)))
        {
            questionCategory4.QuestionBankEntries.QuestionBankEntries.Add(questionBankEntryXml);
        }

        QuestionsXmlQuestionsCategories.QuestionCategory.Add(questionCategory3);
        QuestionsXmlQuestionsCategories.QuestionCategory.Add(questionCategory4);

        QuestionsXmlQuestionsCategories.Serialize();
    }

    public void CreateRolesXml()
    {
        //set parameters of the roles.xml file
        RolesXmlRolesDefinition.Role = RolesXmlRole as RolesXmlRole ?? new RolesXmlRole();

        //create roles.xml file
        RolesXmlRolesDefinition.Serialize();
    }

    public void CreateScalesXml()
    {
        //create scales.xml file
        ScalesXmlScalesDefinition.Serialize();
    }

    private void AddBaseLearningElementsToBackup()
    {
        if (ReadAtf.GetBaseLearningElementsList().Count > 0)
        {
            var sectionId = (ReadAtf.GetSpaceList().Count + 1).ToString();
            MoodleBackupXmlSectionList.Add(new MoodleBackupXmlSection
            {
                SectionId = sectionId,
                Title = "Hinweise auf externes Lernmaterial",
                Directory = "sections/section_" + sectionId,
            });

            MoodleBackupXmlSections.Section = MoodleBackupXmlSectionList;

            MoodleBackupXmlSettingList.Add(new MoodleBackupXmlSetting("section",
                "section_" + sectionId + "_included", "1",
                "section_" + sectionId, true));

            MoodleBackupXmlSettingList.Add(new MoodleBackupXmlSetting("section",
                "section_" + sectionId + "_userinfo", "0",
                "section_" + sectionId, true));
        }
    }

    private void AddSectionsToBackup()
    {
        MoodleBackupXmlSectionList.Add(new MoodleBackupXmlSection
        {
            SectionId = "0",
            Title = "General",
            Directory = "sections/section_0",
        });

        MoodleBackupXmlSections.Section = MoodleBackupXmlSectionList;

        MoodleBackupXmlSettingList.Add(new MoodleBackupXmlSetting("section",
            "section_0_included", "1",
            "section_0", true));

        MoodleBackupXmlSettingList.Add(new MoodleBackupXmlSetting("section",
            "section_0_userinfo", "0",
            "section_0", true));

        foreach (var space in ReadAtf.GetSpaceList())
        {
            var sectionId = space.SpaceId.ToString();
            var sectionName = space.SpaceName;

            MoodleBackupXmlSectionList.Add(new MoodleBackupXmlSection
            {
                SectionId = sectionId,
                Title = sectionName,
                Directory = "sections/section_" + sectionId,
            });

            MoodleBackupXmlSections.Section = MoodleBackupXmlSectionList;


            MoodleBackupXmlSettingList.Add(new MoodleBackupXmlSetting("section",
                "section_" + sectionId + "_included", "1",
                "section_" + sectionId, true));

            MoodleBackupXmlSettingList.Add(new MoodleBackupXmlSetting("section",
                "section_" + sectionId + "_userinfo", "0",
                "section_" + sectionId, true));
        }
    }

    private void AddLearningElementsToBackup()
    {
        //Every activity needs the following tags in the moodle_backup.xml file
        //The ElementType is different for some element types
        foreach (var element in _learningElements)
        {
            var learningElementId = element.ElementId.ToString();
            var learningElementType = element.ElementFileType;
            var learningElementName = element.ElementName;
            var learningElementSectionId = element switch
            {
                IInternalElementJson internalElementJson => internalElementJson.LearningSpaceParentId.ToString(),
                IBaseLearningElementJson => (ReadAtf.GetSpaceList().Count + 1).ToString(),
                _ => throw new ArgumentOutOfRangeException(nameof(element))
            };

            learningElementType = GetLearningElementType(learningElementType);

            AddActivityToBackup(learningElementId, learningElementType, learningElementName, learningElementSectionId);
        }
    }

    private void AddActivityToBackup(string id, string type, string name, string sectionId)
    {
        MoodleBackupXmlActivityList.Add(new MoodleBackupXmlActivity
        {
            ModuleId = id,
            SectionId = sectionId,
            ModuleName = type,
            Title = name,
            Directory = "activities/" + type + "_" + id,
        });

        MoodleBackupXmlActivities.Activity = MoodleBackupXmlActivityList;

        MoodleBackupXmlSettingList.Add(new MoodleBackupXmlSetting("activity", type + "_" + id + "_included", "1",
            type + "_" + id, false));
        MoodleBackupXmlSettingList.Add(new MoodleBackupXmlSetting("activity", type + "_" + id + "_userinfo", "0",
            type + "_" + id, false));
    }

    private void InitializeMoodleBackupSetting()
    {
        //MoodleBackupXmlSettingSetting are Tags that describe the Moodle Backup Settings.
        //They are the same Options that are displayed, when a backup is created in moodle. 
        //Its very important, that files & activities are imported with the backup (value=1)
        MoodleBackupXmlSettingFilename.Name = "filename";
        MoodleBackupXmlSettingFilename.Value = "C#_AuthoringTool_Created_Backup.mbz";

        MoodleBackupXmlSettingImscc11.Name = "imscc11";

        MoodleBackupXmlSettingUsers.Name = "users";

        MoodleBackupXmlSettingAnonymize.Name = "anonymize";

        MoodleBackupXmlSettingRoleAssignments.Name = "role_assignments";

        MoodleBackupXmlSettingActivities.Name = "activities";
        MoodleBackupXmlSettingActivities.Value = "1";

        MoodleBackupXmlSettingBlocks.Name = "blocks";

        MoodleBackupXmlSettingFiles.Name = "files";
        MoodleBackupXmlSettingFiles.Value = "1";

        MoodleBackupXmlSettingFilters.Name = "filters";

        MoodleBackupXmlSettingComments.Name = "comments";

        MoodleBackupXmlSettingBadges.Name = "badges";

        MoodleBackupXmlSettingCalendarevents.Name = "calendarevents";

        MoodleBackupXmlSettingUserscompletion.Name = "userscompletion";

        MoodleBackupXmlSettingLogs.Name = "logs";

        MoodleBackupXmlSettingGradeHistories.Name = "grade_histories";

        MoodleBackupXmlSettingQuestionbank.Name = "questionbank";

        MoodleBackupXmlSettingQuestionbank.Value = "1";

        MoodleBackupXmlSettingGroups.Name = "groups";

        MoodleBackupXmlSettingCompetencies.Name = "competencies";

        MoodleBackupXmlSettingCustomfield.Name = "customfield";

        MoodleBackupXmlSettingContentbankcontent.Name = "contentbankcontent";

        MoodleBackupXmlSettingLegacyfiles.Name = "legacyfiles";
    }

    private string GetLearningElementType(string learningElementType)
    {
        return learningElementType switch
        {
            "h5p" => "h5pactivity",
            "pdf" or "json" or "jpg" or "jpeg" or "png" or "bmp" or "webp" or "txt" or "c" or "h" or "cpp" or "cc"
                or "c++" or "py" or "cs" or "js" or "php" or "html" or "css" => "resource",
            "adaptivity" => "adleradaptivity",
            _ => learningElementType
        };
    }

    /// <summary>
    /// Creates an XML representation for a given adaptivity question.
    /// </summary>
    /// <param name="question">The adaptivity question to be processed.</param>
    /// <param name="questionId">Reference to the current question ID, used for generating unique IDs.</param>
    /// <param name="answerId">Reference to the current answer ID, used for generating unique IDs for answers.</param>
    /// <returns>Returns the XML representation of the processed adaptivity question.</returns>
    private QuestionsXmlQuestionsCategoryQuestionBankEntry CreateQuestionBankEntryXml(IAdaptivityQuestionJson question,
        int questionId, ref int answerId)
    {
        var questionBankEntryXml =
            new QuestionsXmlQuestionsCategoryQuestionBankEntry(questionId, question.QuestionUUID);
        var questionVersionXml = new QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersions(questionId);
        var questionXml =
            new QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionsQuestion(questionId,
                question.QuestionText);

        var pluginQTypeMultiChoiceQuestionXml =
            new QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQTypeMultichoiceQuestion();
        var answersXml = CreateAnswersXml(question.Choices, ref answerId);

        var singleResponseInt = question.QuestionType is ResponseType.singleResponse ? 1 : 0;

        var incorrectFeedback = CreateIncorrectFeedbackString(question.AdaptivityRules);

        var multiChoiceXml =
            new
                QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQTypeMultichoiceQuestionMultichoice(
                    questionId, singleResponseInt, incorrectFeedback);

        pluginQTypeMultiChoiceQuestionXml.Answers = answersXml;
        pluginQTypeMultiChoiceQuestionXml.Multichoice = multiChoiceXml;
        questionXml.PluginQTypeMultichoiceQuestion = pluginQTypeMultiChoiceQuestionXml;
        questionVersionXml.Questions.Question.Add(questionXml);
        questionBankEntryXml.QuestionVersion.QuestionVersions.Add(questionVersionXml);

        return questionBankEntryXml;
    }

    private string CreateIncorrectFeedbackString(List<IAdaptivityRuleJson> questionAdaptivityRules)
    {
        var incorrectFeedback = new StringBuilder("Diese Antwort ist falsch. <br>");

        if (questionAdaptivityRules.Count == 0)
            return incorrectFeedback.ToString();

        incorrectFeedback.Append("Hinweis: ");
        foreach (var rule in questionAdaptivityRules)
        {
            switch (rule.AdaptivityAction)
            {
                case CommentActionJson commentActionJson:
                    incorrectFeedback.Append(commentActionJson.CommentText).Append("<br>");
                    break;
                case ContentReferenceActionJson contentReferenceActionJson:
                    var baseElementName = ReadAtf.GetBaseLearningElementsList()
                        .First(x => x.ElementId == contentReferenceActionJson.ElementId).ElementName;
                    incorrectFeedback.Append("Schaue dir noch mal das Lernelement ").Append(baseElementName)
                        .Append(" an. <br>");
                    incorrectFeedback.Append(string.IsNullOrEmpty(contentReferenceActionJson.HintText)
                        ? ""
                        : contentReferenceActionJson.HintText + "<br>");
                    break;
                case ElementReferenceActionJson elementReferenceActionJson:
                    var learningElementName = ReadAtf.GetElementsOrderedList()
                        .First(x => x.ElementId == elementReferenceActionJson.ElementId).ElementName;
                    var spaceName = ReadAtf.GetSpaceList()
                        .First(x => x.SpaceSlotContents.Any(y => y == elementReferenceActionJson.ElementId)).SpaceName;
                    incorrectFeedback.Append("Schaue dir noch mal das Lernelement ").Append(learningElementName)
                        .Append(" in Raum \"").Append(spaceName).Append("\" an. <br>");
                    incorrectFeedback.Append(string.IsNullOrEmpty(elementReferenceActionJson.HintText)
                        ? ""
                        : elementReferenceActionJson.HintText + "<br>");
                    break;
            }
        }

        return incorrectFeedback.ToString();
    }


    /// <summary>
    /// Creates an XML representation for the answers of a given adaptivity question.
    /// </summary>
    /// <param name="choices">A list of choices representing possible answers for the adaptivity question.</param>
    /// <param name="answerId">Reference to the current answer ID, used for generating unique IDs for answers.</param>
    /// <returns>Returns the XML representation of the answers for the adaptivity question.</returns>
    private static
        QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQTypeMultichoiceQuestionAnswers
        CreateAnswersXml(List<IChoiceJson> choices, ref int answerId)
    {
        var answersXml =
            new
                QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQTypeMultichoiceQuestionAnswers();
        var numCorrectChoices = choices.Count(x => x.IsCorrect);
        var numIncorrectChoices = choices.Count(x => !x.IsCorrect);

        foreach (var choice in choices)
        {
            var fraction = choice.IsCorrect ? 1.0 / numCorrectChoices : -1.0 / numIncorrectChoices;
            answersXml.Answer.Add(
                new
                    QuestionsXmlQuestionsCategoryQuestionBankEntryQuestionVersionQuestionPluginQTypeMultichoiceQuestionAnswer(
                        answerId, choice.AnswerText, fraction));
            answerId++;
        }

        return answersXml;
    }
}