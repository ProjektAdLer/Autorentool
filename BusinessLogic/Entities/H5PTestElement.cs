using JetBrains.Annotations;
using Shared;

namespace BusinessLogic.Entities;

public class H5PTestElement : Element
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private H5PTestElement()
    {
    }

    public H5PTestElement(string name, string shortname, ISpace? parent,
        Content content, string url, string authors, string description, string goals,
        ElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY) : base(
        name, shortname, content, url, authors, description, goals, difficulty, parent, workload, points, positionX,
        positionY)
    {
    }
}