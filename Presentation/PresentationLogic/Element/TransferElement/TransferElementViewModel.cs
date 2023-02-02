using Presentation.PresentationLogic.Content;
using Presentation.PresentationLogic.Space;
using Shared;

namespace Presentation.PresentationLogic.Element.TransferElement;

public abstract class TransferElementViewModel : ElementViewModel
{ 
        /// <summary>
        /// Protected Constructor for AutoMapper
        /// </summary>
        protected TransferElementViewModel()
        {
        }

        protected TransferElementViewModel(string name, string shortname, ISpaceViewModel? parent,
                ContentViewModel content, string url, string authors, string description, string goals,
                ElementDifficultyEnum difficulty, int workload, int points, double positionX, double positionY)
                : base(name, shortname, content, url, authors, description, goals, difficulty, parent, workload, points,
                        positionX, positionY)
        {
        }
}