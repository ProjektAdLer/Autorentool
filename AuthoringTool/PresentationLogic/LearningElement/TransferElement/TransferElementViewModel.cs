using AuthoringTool.PresentationLogic.LearningContent;
using AuthoringTool.PresentationLogic.LearningElement.TestElement;

namespace AuthoringTool.PresentationLogic.LearningElement.TransferElement;

public abstract class TransferElementViewModel : LearningElementViewModel
{ 
        /// <summary>
        /// Protected Constructor for AutoMapper
        /// </summary>
        protected TransferElementViewModel() : base()
        {
        }
        
        protected TransferElementViewModel(string name, string shortname, ILearningElementViewModelParent? parent,
        LearningContentViewModel content, string authors, string description, string goals,
        LearningElementDifficultyEnum difficulty, int workload, double positionX, double positionY) : base(name,
                shortname, content, authors, description, goals, difficulty, parent, workload, positionX, positionY)
        {
        }
}