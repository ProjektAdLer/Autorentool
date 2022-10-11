﻿namespace BusinessLogic.Entities;

public interface ILearningWorld
{
    
    List<LearningSpace> LearningSpaces { get; set; }
    List<LearningPathway> LearningPathways { get; set; }
    string Description { get; set; }
    string Shortname { get; set; }
    string Authors { get; set; }
    string Language { get; set; }
    string Goals { get; set; }
}