﻿using System.Runtime.Serialization;

namespace PersistEntities;

[Serializable]
[DataContract]
[KnownType(typeof(H5PActivationElementPe))]
[KnownType(typeof(H5PInteractionElementPe))]
[KnownType(typeof(H5PTestElementPe))]
[KnownType(typeof(ImageTransferElementPe))]
[KnownType(typeof(PdfTransferElementPe))]
[KnownType(typeof(TextTransferElementPe))]
[KnownType(typeof(VideoActivationElementPe))]
[KnownType(typeof(VideoTransferElementPe))]
public class LearningSpacePe : ILearningSpacePe, IExtensibleDataObject
{
    public LearningSpacePe(string name, string shortname, string authors, string description, string goals,
        int requiredPoints, List<LearningElementPe>? learningElements = null, double positionX = 0, double positionY = 0)
    {
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Description = description;
        Goals = goals;
        RequiredPoints = requiredPoints;
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
        RequiredPoints = 0;
        LearningElements = new List<LearningElementPe>();
        PositionX = 0;
        PositionY = 0;
    }


    [DataMember]
    public string Name { get; set; }
    [DataMember]
    public string Description { get; set; }
    [DataMember]
    public string Shortname { get; set; }
    [DataMember]
    public string Authors { get; set; }
    [DataMember]
    public string Goals { get; set; }
    [DataMember]
    public int RequiredPoints { get; set; }
    [DataMember]
    public List<LearningElementPe> LearningElements { get; set; }
    [DataMember]
    public double PositionX { get; set; }
    [DataMember]
    public double PositionY { get; set; }
    public ExtensionDataObject? ExtensionData { get; set; }
}