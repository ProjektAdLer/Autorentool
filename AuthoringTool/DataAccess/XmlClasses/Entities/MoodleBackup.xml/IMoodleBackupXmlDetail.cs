namespace AuthoringTool.DataAccess.XmlClasses.Entities;

public interface IMoodleBackupXmlDetail
{
    string Type { get; set; }
    
    string Format { get; set; }
    
    string Interactive { get; set; }
    
    string Mode { get; set; }
    
    string Execution { get; set; }
    
    string ExecutionTime { get; set; }
    
    string BackupId { get; set; }
}