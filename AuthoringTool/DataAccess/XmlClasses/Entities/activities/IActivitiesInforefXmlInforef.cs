namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesInforefXmlInforef : IXmlSerializablePath
{
    void SetParameters(ActivitiesInforefXmlFileref? fileref, ActivitiesInforefXmlGradeItemref? gradeItemref);
}