using System.IO.Abstractions;
using AuthoringTool.DataAccess.DSL;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.GradeHistory.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Grades.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.H5PActivity.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Inforef.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Module.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities._activities.Roles.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities.Files.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities.Sections.Inforef.xml;
using AuthoringTool.DataAccess.XmlClasses.Entities.Sections.Section.xml;

namespace AuthoringTool.DataAccess.XmlClasses.XmlFileFactories;

public interface IXmlH5PFactory
{
    IFilesXmlFiles FilesXmlFiles { get; }
    IFilesXmlFile FilesXmlFileBlock1 { get; }
    IFilesXmlFile FilesXmlFileBlock2 { get; }
    IActivitiesGradesXmlGradeItem ActivitiesGradesXmlGradeItem { get; }
    IActivitiesGradesXmlGradeItems ActivitiesGradesXmlGradeItems { get; }
    IActivitiesGradesXmlActivityGradebook ActivitiesGradesXmlActivityGradebook { get; }
    IActivitiesH5PActivityXmlActivity ActivitiesH5PActivityXmlActivity { get; }
    IActivitiesH5PActivityXmlH5PActivity ActivitiesH5PActivityXmlH5PActivity { get; }
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
    IReadDSL? ReadDsl { get; }

    void CreateH5PFileFactory();
    void ReadH5PListAndSetParameters(List<LearningElementJson> h5pElementsList);
    void H5PSetParametersFilesXml(string? hashCheckSum, string? filesize);

    void H5PSetParametersActivity();

    void H5PSetParametersSections();

    void CreateActivityFolder(string? moduleId);

    void CreateSectionsFolder(string? sectionId);
}