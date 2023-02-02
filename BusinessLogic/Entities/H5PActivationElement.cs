using JetBrains.Annotations;
using Shared;

namespace BusinessLogic.Entities;

public class H5PActivationElement : Element
{
    /// <summary>
    /// Private Constructor for AutoMapper
    /// </summary>
    [UsedImplicitly]
    private H5PActivationElement()
    {
    }

    public H5PActivationElement(string name, string shortname, ISpace? parent,
        Content content, string url, string authors, string description, string goals,
        ElementDifficultyEnum difficulty, int workload = 0, int points = 0, double positionX = 0,
        double positionY = 0) : base(name, shortname, content, url, authors, description, goals, 
        difficulty, parent, workload, points, positionX, positionY)
    {
    }
}