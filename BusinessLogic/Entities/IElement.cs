using Shared;

namespace BusinessLogic.Entities;

public interface IElement
{
    Guid Id { get; }
    string Name { get; set; }
    string Shortname { get; set; }
    public ISpace? Parent { get; set; }
    Content Content { get; set; }
    string Url { get; set; }
    string Authors { get; set; }
    string Description { get; set; }
    string Goals { get; set; }
    int Workload { get; set; }
    int Points { get; set; }
    ElementDifficultyEnum Difficulty { get; set; }
}