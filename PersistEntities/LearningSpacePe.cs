﻿namespace PersistEntities;

[Serializable]
public class LearningSpacePe : ILearningSpacePe
{
    public LearningSpacePe(string name, string shortname, string authors, string description,
        string goals, List<LearningElementPe>? learningElements = null, double positionX = 0, double positionY = 0)
    {
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Description = description;
        Goals = goals;
        LearningElements = learningElements ?? new List<LearningElementPe>();
        PositionX = positionX;
        PositionY = positionY;
    }

    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private LearningSpacePe()
    {
        Name = "";
        Shortname = "";
        Authors = "";
        Description = "";
        Goals = "";
        LearningElements = new List<LearningElementPe>();
        PositionX = 0;
        PositionY = 0;
    }


    public string Name { get; set; }
    public string Description { get; set; }
    public string Shortname { get; set; }
    public string Authors { get; set; }
    public string Goals { get; set; }
    public List<LearningElementPe> LearningElements { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
}