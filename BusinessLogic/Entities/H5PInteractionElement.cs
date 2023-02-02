using JetBrains.Annotations;
using Shared;

namespace BusinessLogic.Entities;

public class H5PInteractionElement : Element
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private H5PInteractionElement()
    {
    }

    public H5PInteractionElement(string name, string shortname, ISpace? parent,
        Content content, string url, string authors, string description, string goals,
        ElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY) : base(
        name, shortname, content, url, authors, description, goals, difficulty, parent, workload, points, positionX,
        positionY)
    {
    }
}