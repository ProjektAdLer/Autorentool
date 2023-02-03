using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningSpace;
using Shared;

namespace Presentation.PresentationLogic.LearningElement.TransferElement;

public abstract class TransferElementViewModel : LearningElementViewModel
{ 
        /// <summary>
        /// Protected Constructor for AutoMapper
        /// </summary>
        protected TransferElementViewModel()
        {
        }

        protected TransferElementViewModel(string name, string shortname, ILearningSpaceViewModel? parent,
                LearningContentViewModel content, string url, string authors, string description, string goals,
                LearningElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY)
                : base(name, shortname, content, url, authors, description, goals, difficulty, parent, workload, points,
                        positionX, positionY)
        {
        }
}