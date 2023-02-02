using JetBrains.Annotations;
using Shared;

namespace BusinessLogic.Entities;

public class TextTransferElement : Element
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private TextTransferElement()
    {
    }
    
    internal TextTransferElement(string name, string shortname, ISpace? parent,
        Content content, string url, string authors, string description, string goals,
        ElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY) : base(
        name, shortname, content, url, authors, description, goals, difficulty, parent, workload, points, positionX,
        positionY)
    {
    }
}