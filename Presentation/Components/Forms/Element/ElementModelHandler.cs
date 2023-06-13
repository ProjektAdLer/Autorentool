using Presentation.PresentationLogic.LearningContent;
using Shared;

namespace Presentation.Components.Forms.Element;

public class ElementModelHandler : IElementModelHandler
{
    public static IEnumerable<ElementModel> GetElementModels(ILearningContentViewModel? learningContentViewModel = null)
    {
        return learningContentViewModel switch
        {
            IFileContentViewModel fileContentViewModel => fileContentViewModel.Type switch
            {
                "h5p" => GetElementModelsForH5P().Concat(GetElementModels().Except(GetElementModelsForH5P())),
                "txt" or "c" or "h" or "cpp" or "cc" or "c++" or "py" or "cs" or "js" or "php" or "html" or "css" =>
                    GetElementModelsForText().Concat(GetElementModels().Except(GetElementModelsForText())),
                "jpg" or "png" or "webp" or "bmp" => GetElementModelsForImage()
                    .Concat(GetElementModels().Except(GetElementModelsForImage())),
                "pdf" => GetElementModelsForPdf().Concat(GetElementModels().Except(GetElementModelsForPdf())),
                _ => GetElementModels()
            },
            ILinkContentViewModel => GetElementModelsForVideo()
                .Concat(GetElementModels().Except(GetElementModelsForVideo())),
            _ => (ElementModel[]) Enum.GetValues(typeof(ElementModel))
        };
    }

    private static IEnumerable<ElementModel> GetElementModelsForImage()
    {
        return new List<ElementModel> {ElementModel.l_picture_painting_1, ElementModel.l_picture_painting_2};
    }

    private static IEnumerable<ElementModel> GetElementModelsForH5P()
    {
        return new List<ElementModel>
        {
            ElementModel.l_h5p_slotmachine_1, ElementModel.l_h5p_deskpc_1, ElementModel.l_h5p_blackboard_1,
            ElementModel.l_h5p_drawingtable_1
        };
    }

    private static IEnumerable<ElementModel> GetElementModelsForPdf()
    {
        return new List<ElementModel> { };
    }

    private static IEnumerable<ElementModel> GetElementModelsForText()
    {
        return new List<ElementModel> {ElementModel.l_text_bookshelf_1, ElementModel.l_text_bookshelf_2};
    }

    private static IEnumerable<ElementModel> GetElementModelsForVideo()
    {
        return new List<ElementModel> {ElementModel.l_video_television_1};
    }

    public string GetIconForElementModel(ElementModel elementModel)
    {
        return elementModel switch
        {
            ElementModel.l_picture_painting_1 => "CustomIcons/ElementModels/l-bild-Wandbild-1.png",
            ElementModel.l_picture_painting_2 => "CustomIcons/ElementModels/l-bild-Wandbild-2.png",
            ElementModel.l_h5p_deskpc_1 => "CustomIcons/ElementModels/l-h5p-Schreibtisch-1.png",
            ElementModel.l_h5p_slotmachine_1 => "CustomIcons/ElementModels/l-h5p-Spielautomat-1.png",
            ElementModel.l_h5p_blackboard_1 => "CustomIcons/ElementModels/l-h5p-Tafel-1.png",
            ElementModel.l_h5p_drawingtable_1 => "CustomIcons/ElementModels/l-h5p-Zeichenpult-1.png",
            ElementModel.l_text_bookshelf_1 => "CustomIcons/ElementModels/l-text-Bücherregal-1.png",
            ElementModel.l_text_bookshelf_2 => "CustomIcons/ElementModels/l-text-Bücherregal-2.png",
            ElementModel.l_video_television_1 => "CustomIcons/ElementModels/l-video-Fernseher-1.png",
            _ => throw new ArgumentOutOfRangeException(nameof(elementModel), elementModel, null)
        };
    }
}