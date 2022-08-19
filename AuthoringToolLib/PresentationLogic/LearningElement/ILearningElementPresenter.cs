using AuthoringToolLib.PresentationLogic.LearningContent;

namespace AuthoringToolLib.PresentationLogic.LearningElement;

public interface ILearningElementPresenter
{
    LearningElementViewModel EditLearningElement(LearningElementViewModel element, string name, string shortname,
        ILearningElementViewModelParent parent, string authors, string description,
        string goals, LearningElementDifficultyEnum difficulty, int workload, double? posx = null, double? posy = null);
    
    public LearningElementViewModel CreateNewTransferElement(string name, string shortname, 
        ILearningElementViewModelParent parent, ContentTypeEnum contentType, LearningContentViewModel learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload,
        double posx = 0f, double posy = 0f);
        
    public LearningElementViewModel CreateNewActivationElement(string name, string shortname,
        ILearningElementViewModelParent parent, ContentTypeEnum contentType, LearningContentViewModel learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload,
        double posx = 0f, double posy = 0f);
   public LearningElementViewModel CreateNewInteractionElement(string name, string shortname,
        ILearningElementViewModelParent parent, ContentTypeEnum contentType, LearningContentViewModel learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload,
        double posx = 0f, double posy = 0f);
   public LearningElementViewModel CreateNewTestElement(string name, string shortname,
        ILearningElementViewModelParent parent, ContentTypeEnum contentType, LearningContentViewModel learningContent,
        string authors, string description, string goals, LearningElementDifficultyEnum difficulty, int workload,
        double posx = 0f, double posy = 0f);

    void RemoveLearningElementFromParentAssignment(LearningElementViewModel element);
}