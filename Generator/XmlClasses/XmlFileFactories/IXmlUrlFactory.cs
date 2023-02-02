using Generator.DSL;
using Generator.XmlClasses.Entities._activities.GradeHistory.xml;
using Generator.XmlClasses.Entities._activities.Grades.xml;
using Generator.XmlClasses.Entities._activities.Inforef.xml;
using Generator.XmlClasses.Entities._activities.Module.xml;
using Generator.XmlClasses.Entities._activities.Roles.xml;
using Generator.XmlClasses.Entities._activities.Url.xml;

namespace Generator.XmlClasses.XmlFileFactories;

public interface IXmlUrlFactory
{
    IActivitiesGradesXmlGradeItem ActivitiesGradesXmlGradeItem { get; }
    IActivitiesGradesXmlGradeItems ActivitiesGradesXmlGradeItems { get; }
    IActivitiesGradesXmlActivityGradebook ActivitiesGradesXmlActivityGradebook { get; }
    IActivitiesUrlXmlActivity ActivitiesUrlXmlActivity { get; }
    IActivitiesUrlXmlUrl ActivitiesUrlXmlUrl { get; }
    IActivitiesRolesXmlRoles ActivitiesRolesXmlRoles { get; }
    IActivitiesModuleXmlModule ActivitiesModuleXmlModule { get; }
    IActivitiesGradeHistoryXmlGradeHistory ActivitiesGradeHistoryXmlGradeHistory { get; }
    IActivitiesInforefXmlFileref ActivitiesInforefXmlFileref { get; }
    IActivitiesInforefXmlGradeItem ActivitiesInforefXmlGradeItem { get; }
    IActivitiesInforefXmlGradeItemref ActivitiesInforefXmlGradeItemref { get; }
    IActivitiesInforefXmlInforef ActivitiesInforefXmlInforef { get; }
    IReadDsl ReadDsl { get; }
    void CreateUrlFactory();
    void UrlSetParameters(List<ElementJson> urlList);
    void SetParametersActivityUrl();
    void CreateActivityFolder(string moduleId);
}