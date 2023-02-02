namespace BusinessLogic.Entities;

public class Content : IContent
{
    public Content(string name, string type, string filepath)
    {
        Name = name;
        Type = type;
        Filepath = filepath;
    }
    
    /// <summary>
    /// Constructor for Automapper. DO NOT USE.
    /// </summary>
    internal Content()
    {
        Name = "";
        Type = "";
        Filepath = "";
    }
    
    public string Name { get; set; }
    public string Type { get; set; }
    public string Filepath { get; set; }
}