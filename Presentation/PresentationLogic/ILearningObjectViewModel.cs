﻿namespace Presentation.PresentationLogic;

public interface ILearningObjectViewModel
{
    Guid Id { get; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
}