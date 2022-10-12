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
public class LearningWorldPe : ILearningWorldPe, IExtensibleDataObject
{
    public LearningWorldPe(string name, string shortname, string authors, string language, string description,
        string goals, List<LearningSpacePe>? learningSpaces = null,  List<LearningPathwayPe>? learningPathWays = null)
    {
        Name = name;
        Shortname = shortname;
        Authors = authors;
        Language = language;
        Description = description;
        Goals = goals;
        LearningSpaces = learningSpaces ?? new List<LearningSpacePe>();
        LearningPathWays = learningPathWays ?? new List<LearningPathwayPe>();
    }

    /// <summary>
    /// Constructor for serialization. DO NOT USE FOR NORMAL INITIALIZATION.
    /// </summary>
    private LearningWorldPe()
    {
        Name = "";
        Shortname = "";
        Authors = "";
        Language = "";
        Description = "";
        Goals = "";
        LearningSpaces = new List<LearningSpacePe>();
        LearningPathWays = new List<LearningPathwayPe>();
    }

    [DataMember]
    public List<LearningSpacePe> LearningSpaces { get; set; }
    [DataMember]
    public List<LearningPathwayPe> LearningPathWays { get; set; }
    [DataMember]
    public string Name { get; set; }
    [DataMember]
    public string Shortname { get; set; }
    [DataMember]
    public string Authors { get; set; }
    [DataMember]
    public string Language { get; set; }
    [DataMember]
    public string Description { get; set; }
    [DataMember]
    public string Goals { get; set; }
    public ExtensionDataObject? ExtensionData { get; set; }
}