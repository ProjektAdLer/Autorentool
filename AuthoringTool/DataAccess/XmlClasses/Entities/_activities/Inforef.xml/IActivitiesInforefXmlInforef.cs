namespace AuthoringTool.DataAccess.XmlClasses.Entities.activities;

public interface IActivitiesInforefXmlInforef : IXmlSerializablePath
{
    ActivitiesInforefXmlFileref Fileref { get; set; }
    
    ActivitiesInforefXmlGradeItemref GradeItemref { get; set; }
}