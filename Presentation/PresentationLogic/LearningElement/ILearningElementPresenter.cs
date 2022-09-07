﻿using Presentation.PresentationLogic.LearningContent;
using Shared;

namespace Presentation.PresentationLogic.LearningElement;

public interface ILearningElementPresenter
{
    void RemoveLearningElementFromParentAssignment(LearningElementViewModel element);
}