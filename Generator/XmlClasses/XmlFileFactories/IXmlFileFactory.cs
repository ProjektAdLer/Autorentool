﻿using Generator.DSL;
using Generator.XmlClasses.Entities._activities.GradeHistory.xml;
using Generator.XmlClasses.Entities._activities.Grades.xml;
using Generator.XmlClasses.Entities._activities.Inforef.xml;
using Generator.XmlClasses.Entities._activities.Module.xml;
using Generator.XmlClasses.Entities._activities.Resource.xml;
using Generator.XmlClasses.Entities._activities.Roles.xml;
using Generator.XmlClasses.Entities._sections.Inforef.xml;
using Generator.XmlClasses.Entities._sections.Section.xml;

namespace Generator.XmlClasses.XmlFileFactories;

public interface IXmlFileFactory
{
    IActivitiesGradesXmlGradeItem ActivitiesGradesXmlGradeItem { get; }
    IActivitiesGradesXmlGradeItems ActivitiesGradesXmlGradeItems { get; }
    IActivitiesGradesXmlActivityGradebook ActivitiesGradesXmlActivityGradebook { get; }
    IActivitiesResourceXmlResource ActivitiesFileResourceXmlResource { get; }
    IActivitiesResourceXmlActivity ActivitiesFileResourceXmlActivity { get; }
    IActivitiesRolesXmlRoles ActivitiesRolesXmlRoles { get; }
    IActivitiesModuleXmlModule ActivitiesModuleXmlModule { get; }
    IActivitiesGradeHistoryXmlGradeHistory ActivitiesGradeHistoryXmlGradeHistory { get; }
    IActivitiesInforefXmlFile ActivitiesInforefXmlFileBlock1 { get; }
    IActivitiesInforefXmlFile ActivitiesInforefXmlFileBlock2 { get; }
    IActivitiesInforefXmlFileref ActivitiesInforefXmlFileref { get; }
    IActivitiesInforefXmlGradeItem ActivitiesInforefXmlGradeItem { get; }
    IActivitiesInforefXmlGradeItemref ActivitiesInforefXmlGradeItemref { get; }
    IActivitiesInforefXmlInforef ActivitiesInforefXmlInforef { get; }
    ISectionsInforefXmlInforef SectionsInforefXmlInforef { get; }
    ISectionsSectionXmlSection SectionsSectionXmlSection { get; }
    IReadDsl ReadDsl { get; }
    void CreateFileFactory();
    void ReadFileListAndSetParameters(List<LearningElementJson> listDslDocument);
    void FileSetParametersFilesXml(string hashCheckSum, string filesize);
    void FileSetParametersActivity();

    /// <summary>
    /// Create Folder section/ in the folder sections. And both files inforef.xml and section.xml
    /// </summary>
    void FileSetParametersSections();

    /// <summary>
    /// Creates a Resource folder in the activity folder. Each activity needs an folder.
    /// </summary>
    /// <param name="moduleId"></param>
    void CreateActivityFolder(string? moduleId);

    /// <summary>
    /// Creates section folders in the sections folder. For every sectionId.
    /// </summary>
    /// <param name="sectionId"></param>
    void CreateSectionsFolder(string? sectionId);
}