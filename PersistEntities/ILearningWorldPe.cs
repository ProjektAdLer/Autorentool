﻿namespace PersistEntities;

public interface ILearningWorldPe
{
    
    List<LearningElementPe> LearningElements { get; set; }
    List<LearningSpacePe>? LearningSpaces { get; set; }
    string Name { get; set; }
    string Description { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Language { get; set; }
    string Goals { get; set; }
}