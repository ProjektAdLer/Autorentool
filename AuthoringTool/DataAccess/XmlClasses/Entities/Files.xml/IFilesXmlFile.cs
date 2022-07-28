namespace AuthoringTool.DataAccess.XmlClasses.Entities;

public interface IFilesXmlFile
{

    string ContentHash { get; set; }
    

    string ContextId { get; set; }

    string Component { get; set; }
    

    string FileArea { get; set; }
    

    string ItemId { get; set; }
    

    string Filepath { get; set; }
    

    string Filename { get; set; }
    

    string Userid { get; set; }
    

    string Filesize { get; set; }
    

    string Mimetype { get; set; }
    

    string Status { get; set; }
    

    string Timecreated { get; set; }
    

    string Timemodified { get; set; }
    
    //Has the same Value as Filename
    string Source { get; set; }
    

    string Author { get; set; }
    

    string License { get; set; }
    

    string Sortorder { get; set; }


    string RepositoryType { get; set; }


    string RepositoryId { get; set; }


    string Reference { get; set; }
    

    string Id { get; set; }
}