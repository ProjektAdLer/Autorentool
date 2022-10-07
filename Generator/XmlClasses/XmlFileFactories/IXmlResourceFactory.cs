using Generator.DSL;
using Generator.XmlClasses.Entities._activities.GradeHistory.xml;
using Generator.XmlClasses.Entities._activities.Grades.xml;
using Generator.XmlClasses.Entities._activities.Inforef.xml;
using Generator.XmlClasses.Entities._activities.Module.xml;
using Generator.XmlClasses.Entities._activities.Resource.xml;
using Generator.XmlClasses.Entities._activities.Roles.xml;

namespace Generator.XmlClasses.XmlFileFactories;

public interface IXmlResourceFactory
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
    IReadDsl ReadDsl { get; }
    void CreateResourceFactory();
    //void ReadFileListAndSetParameters(List<LearningElementJson> listDslDocument);
    void ReadFileListAndSetParametersResource(List<LearningElementJson> listPdfDocument);
    void ResourceSetParametersFilesXml(string hashCheckSum, string filesize, string mimeType);
    void FileSetParametersActivity();

    /// <summary>
    /// Creates a Resource folder in the activity folder. Each activity needs an folder.
    /// </summary>
    /// <param name="moduleId"></param>
    void CreateActivityFolder(string moduleId);

}