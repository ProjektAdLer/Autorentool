using Presentation.PresentationLogic.LearningElement;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLayout;

public interface IAdvancedLearningSpaceLayoutViewModel
{
    IDictionary<int, ILearningElementViewModel> LearningElements { get; set; }
    int Capacity { get; }
    int Count { get; }
    IEnumerable<int> UsedIndices { get; }
    IEnumerable<ILearningElementViewModel> ContainedLearningElements { get; }
    ILearningElementViewModel? GetElement(int index);
    void PutElement(int index, ILearningElementViewModel element);
    void RemoveElement(int index);
    void ClearAllElements();
}