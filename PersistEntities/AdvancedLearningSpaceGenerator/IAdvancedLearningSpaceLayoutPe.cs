using Presentation.PresentationLogic.AdvancedLearningSpaceEditor;
using Shared;

namespace PersistEntities.AdvancedLearningSpaceGenerator;

public interface IAdvancedLearningSpaceLayoutPe
{
    IDictionary<int, ILearningElementPe> LearningElements { get; set; }
    IDictionary<int, Coordinate> AdvancedLearningElementSlots { get; set; }
    IDictionary<int, Coordinate> AdvancedDecorations { get; set; }
    IDictionary<int, DoublePoint> AdvancedCornerPoints { get; set; }
    DoublePoint EntryDoorPosition { get; set; }
    DoublePoint ExitDoorPosition { get; set; }
    IEnumerable<ILearningElementPe> ContainedLearningElements { get; }
    IEnumerable<Coordinate> ContainedAdvancedLearningElementSlots { get; }
    IEnumerable<Coordinate> ContainedAdvancedDecorations { get; }
    IEnumerable<DoublePoint> ContainedAdvancedCornerPoints { get; }
}