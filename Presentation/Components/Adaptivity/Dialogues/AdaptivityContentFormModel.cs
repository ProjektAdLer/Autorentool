using Presentation.Components.Forms.Models;
using Presentation.PresentationLogic.LearningContent.AdaptivityContent;

namespace Presentation.Components.Adaptivity.Dialogues;

public class AdaptivityContentFormModel : ILearningContentFormModel
{
    public AdaptivityContentFormModel()
    {
        Tasks = new List<IAdaptivityTaskViewModel>();
        Name = "";
    }

    // ReSharper disable once CollectionNeverUpdated.Global
    public ICollection<IAdaptivityTaskViewModel> Tasks { get; set; }
    public string Name { get; set; }
}