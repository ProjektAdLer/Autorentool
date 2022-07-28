namespace AuthoringTool.DataAccess.XmlClasses.Entities;

public interface IMoodleBackupXmlActivity
{

    string ModuleId { get; set; }
    
    string SectionId { get; set; }
    
    string ModuleName { get; set; }
    
    string Title { get; set; }
    
    string Directory { get; set; }
}