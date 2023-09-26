namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;

public interface IAdvancedDecorationViewModel : IAdvancedComponentViewModel
{
    string Identifier { get;}
    Guid SpaceId { get; }
    int DecorationKey { get; }
}