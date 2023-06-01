using Presentation.PresentationLogic.LearningContent;
using Shared;

namespace Presentation.Components.Forms.Element;

public class ElementModelHandler : IElementModelHandler
{
    public IEnumerable<ElementModel> GetElementModels(ILearningContentViewModel? learningContentViewModel = null)
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

    private IEnumerable<ElementModel> GetElementModelsForImage()
    {
        return new List<ElementModel> {ElementModel.L_BILD_WANDBILD_1, ElementModel.L_BILD_WANDBILD_2};
    }

    private IEnumerable<ElementModel> GetElementModelsForH5P()
    {
        return new List<ElementModel>
        {
            ElementModel.L_H5P_SPIELAUTOMAT_1, ElementModel.L_H5P_SCHREIBTISCH_1, ElementModel.L_H5P_TAFEL_1,
            ElementModel.L_H5P_ZEICHENPULT_1
        };
    }

    private static IEnumerable<ElementModel> GetElementModelsForPdf()
    {
        return new List<ElementModel> { };
    }

    private static IEnumerable<ElementModel> GetElementModelsForText()
    {
        return new List<ElementModel> {ElementModel.L_TEXT_BUECHERREGAL_1, ElementModel.L_TEXT_BUECHERREGAL_2};
    }

    private static IEnumerable<ElementModel> GetElementModelsForVideo()
    {
        return new List<ElementModel> {ElementModel.L_VIDEO_FERNSEHER_1};
    }

    public string GetIconForElementModel(ElementModel elementModel)
    {
        return elementModel switch
        {
            ElementModel.L_BILD_WANDBILD_1 => "CustomIcons/ElementModels/l-bild-Wandbild-1.png",
            ElementModel.L_BILD_WANDBILD_2 => "CustomIcons/ElementModels/l-bild-Wandbild-2.png",
            ElementModel.L_H5P_SCHREIBTISCH_1 => "CustomIcons/ElementModels/l-h5p-Schreibtisch-1.png",
            ElementModel.L_H5P_SPIELAUTOMAT_1 => "CustomIcons/ElementModels/l-h5p-Spielautomat-1.png",
            ElementModel.L_H5P_TAFEL_1 => "CustomIcons/ElementModels/l-h5p-Tafel-1.png",
            ElementModel.L_H5P_ZEICHENPULT_1 => "CustomIcons/ElementModels/l-h5p-Zeichenpult-1.png",
            ElementModel.L_TEXT_BUECHERREGAL_1 => "CustomIcons/ElementModels/l-text-Bücherregal-1.png",
            ElementModel.L_TEXT_BUECHERREGAL_2 => "CustomIcons/ElementModels/l-text-Bücherregal-2.png",
            ElementModel.L_VIDEO_FERNSEHER_1 => "CustomIcons/ElementModels/l-video-Fernseher-1.png",
            _ => throw new ArgumentOutOfRangeException(nameof(elementModel), elementModel, null)
        };
    }
}