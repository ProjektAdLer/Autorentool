namespace AuthoringTool.DataAccess.XmlClasses.Entities._activities.Inforef.xml;

public interface IActivitiesInforefXmlInforef : IXmlSerializablePath
{
    ActivitiesInforefXmlFileref Fileref { get; set; }
    
    ActivitiesInforefXmlGradeItemref GradeItemref { get; set; }
}