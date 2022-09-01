namespace Generator.XmlClasses.Entities._activities.Module.xml;

public interface IActivitiesModuleXmlModule : IXmlSerializablePath
{
    string ModuleName { get; set; }
    
    string SectionId { get; set; }
    
    string SectionNumber { get; set; }
    
    string IdNumber { get; set; }
    
    string Added { get; set; }
    
    string Score { get; set; }
    
    string Indent { get; set; }
    
    string Visible { get; set; }
    
    string VisibleOnCoursePage { get; set; }
    
    string Visibleold { get; set; }
    
    string Groupmode { get; set; }
    
    string GroupingId { get; set; }
    
    string Completion { get; set; }
    
    string Completiongradeitemnumber { get; set; }
    
    string CompletionView { get; set; }
    
    string Completionexpected { get; set; }
    
    string Availability { get; set; }
    
    string ShowDescription { get; set; }
    
    string Tags { get; set; }
    
    string Id { get; set; }
    
    string Version { get; set; }
}