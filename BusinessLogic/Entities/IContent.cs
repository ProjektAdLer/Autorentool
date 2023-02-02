namespace BusinessLogic.Entities;

public interface IContent
{
    string Name { get; set; }
    string Type { get; set; }
    string Filepath { get; set; }
}