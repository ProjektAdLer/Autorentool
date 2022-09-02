using Generator.DSL;
using Generator.XmlClasses.Entities._activities.GradeHistory.xml;
using Generator.XmlClasses.Entities._activities.Grades.xml;
using Generator.XmlClasses.Entities._activities.H5PActivity.xml;
using Generator.XmlClasses.Entities._activities.Inforef.xml;
using Generator.XmlClasses.Entities._activities.Module.xml;
using Generator.XmlClasses.Entities._activities.Roles.xml;
using Generator.XmlClasses.Entities._sections.Inforef.xml;
using Generator.XmlClasses.Entities._sections.Section.xml;
using Generator.XmlClasses.Entities.Files.xml;

namespace Generator.XmlClasses.XmlFileFactories;

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
    IReadDsl? ReadDsl { get; }

    /// <summary>
    /// Create H5P structure in files.xml, folder activity and folder sections for every H5P element in the DSL Document
    /// </summary>
    void CreateH5PFileFactory();

    void ReadH5PListAndSetParameters(List<LearningElementJson> h5PElementsList,
        List<LearningSpaceJson> learningSpaceJsons);

    /// <summary>
    /// Setting Parameters for h5p element in files.xml, 
    /// </summary>
    /// <param name="hashCheckSum"></param> SHA1 Hash value for the file
    /// <param name="filesize"></param> Byte Filesize for the file
    void H5PSetParametersFilesXml(string hashCheckSum, string filesize);

    /// <summary>
    /// Create Folder Activity and the needed Activity Files
    /// </summary>
    void H5PSetParametersActivity();
    
    /// <summary>
    /// Creates a h5p folder in the activity folder. Each activity needs an folder.
    /// </summary>
    /// <param name="moduleId"></param>
    void CreateActivityFolder(string? moduleId);
    
}