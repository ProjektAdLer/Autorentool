using Presentation.PresentationLogic.LearningContent;
using Shared;

namespace Presentation.Components.Forms.Element;

public class ElementModelHandler : IElementModelHandler
{
    //If you add a new ElementModel, you have to add it to the following methods:
    //  - GetIconForElementModel: Add the path to the icon for the new ElementModel
    //  - GetElementModelsForModelType: Add the new ElementModel to the switch statement for each corresponding ContentType
    //  - GetElementModelsForTheme: Add the new ElementModel to the switch statement for each corresponding Theme
    public IEnumerable<ElementModel> GetElementModels(ILearningContentViewModel? learningContentViewModel = null,
        Theme? theme = null)
    {
        var type = learningContentViewModel switch
        {
            IFileContentViewModel fileContentViewModel => ContentTypeHelper.GetContentType(fileContentViewModel.Type),
            ILinkContentViewModel => ContentTypeEnum.Video,
            _ => ContentTypeEnum.H5P
        };

        IComparer<ElementModel> comparer = new ElementModelComparer(type, theme ?? Theme.Campus);

        var elementModels = (ElementModel[]) Enum.GetValues(typeof(ElementModel));

        return elementModels.OrderBy(m => m, comparer);
    }

    public string GetIconForElementModel(ElementModel elementModel)
    {
        return elementModel switch
        {
            ElementModel.l_random => "CustomIcons/ElementModels/random-icon-nobg.png",
            //ArcadeTheme
            ElementModel.l_h5p_blackslotmachine_1 =>
                "CustomIcons/ElementModels/arcadeTheme/l_h5p_blackslotmachine_1.png",
            ElementModel.l_h5p_deskpc_2 => "CustomIcons/ElementModels/arcadeTheme/l_h5p_deskpc_2.png",
            ElementModel.l_h5p_greyslotmachine_1 =>
                "CustomIcons/ElementModels/arcadeTheme/l_h5p_greyslotmachine_1.png",
            ElementModel.l_h5p_purpleslotmachine_1 =>
                "CustomIcons/ElementModels/arcadeTheme/l_h5p_purpleslotmachine_1.png",
            ElementModel.l_h5p_redslotmachine_1 => "CustomIcons/ElementModels/arcadeTheme/l_h5p_redslotmachine_1.png",
            ElementModel.l_image_cardboardcutout_1 =>
                "CustomIcons/ElementModels/arcadeTheme/l_image_cardboardcutout_1.png",
            ElementModel.l_image_gameposter_1 => "CustomIcons/ElementModels/arcadeTheme/l_image_gameposter_1.png",
            ElementModel.l_image_gameposter_2 => "CustomIcons/ElementModels/arcadeTheme/l_image_gameposter_2.png",
            ElementModel.l_text_comicshelfbig_1 =>
                "CustomIcons/ElementModels/arcadeTheme/l_text_comicshelfbig_1.png",
            ElementModel.l_text_comicshelfsmall_1 =>
                "CustomIcons/ElementModels/arcadeTheme/l_text_comicshelfsmall_1.png",
            ElementModel.l_video_vrdesk_1 => "CustomIcons/ElementModels/arcadeTheme/l_video_vrdesk_1.png",
            //CampusTheme
            ElementModel.l_h5p_blackboard_2 => "CustomIcons/ElementModels/campusTheme/l_h5p_blackboard_2.png",
            ElementModel.l_h5p_daylightprojector_1 =>
                "CustomIcons/ElementModels/campusTheme/l_h5p_daylightprojector_1.png",
            ElementModel.l_h5p_deskpc_3 => "CustomIcons/ElementModels/campusTheme/l_h5p_deskpc_3.png",
            ElementModel.l_h5p_drawingtable_2 => "CustomIcons/ElementModels/campusTheme/l_h5p_drawingtable_2.png",
            ElementModel.l_image_sciencebio_1 => "CustomIcons/ElementModels/campusTheme/l_image_sciencebio_1.png",
            ElementModel.l_image_sciencegeo_1 => "CustomIcons/ElementModels/campusTheme/l_image_sciencegeo_1.png",
            ElementModel.l_image_sciencewhiteboard_1 =>
                "CustomIcons/ElementModels/campusTheme/l_image_sciencewhiteboard_1.png",
            ElementModel.l_text_libraryshelf_1 => "CustomIcons/ElementModels/campusTheme/l_text_libraryshelf_1.png",
            ElementModel.l_video_movieprojektor_1 =>
                "CustomIcons/ElementModels/campusTheme/l_video_movieprojektor_1.png",
            //SuburbTheme
            ElementModel.l_picture_painting_1 => "CustomIcons/ElementModels/suburbTheme/l_image_painting_1.png",
            ElementModel.l_picture_painting_2 => "CustomIcons/ElementModels/suburbTheme/l_image_painting_2.png",
            ElementModel.l_picture_paintingeasel_1 =>
                "CustomIcons/ElementModels/suburbTheme/l_image_paintingeasel_1.png",
            ElementModel.l_h5p_deskpc_1 => "CustomIcons/ElementModels/suburbTheme/l_h5p_deskpc_1.png",
            ElementModel.l_h5p_slotmachine_1 => "CustomIcons/ElementModels/suburbTheme/l_h5p_slotmachine_1.png",
            ElementModel.l_h5p_blackboard_1 => "CustomIcons/ElementModels/suburbTheme/l_h5p_blackboard_1.png",
            ElementModel.l_h5p_drawingtable_1 => "CustomIcons/ElementModels/suburbTheme/l_h5p_drawingtable_1.png",
            ElementModel.l_text_bookshelf_1 => "CustomIcons/ElementModels/suburbTheme/l_text_bookshelf_1.png",
            ElementModel.l_text_bookshelf_2 => "CustomIcons/ElementModels/suburbTheme/l_text_bookshelf_2.png",
            ElementModel.l_video_television_1 => "CustomIcons/ElementModels/suburbTheme/l_video_television_1.png",
            _ => throw new ArgumentOutOfRangeException(nameof(elementModel), elementModel, null)
        };
    }

    public static ElementModel GetElementModelRandom()
    {
        return ElementModel.l_random;
    }

    internal static IEnumerable<ElementModel> GetElementModelsForModelType(ContentTypeEnum modelType)
    {
        switch (modelType)
        {
            case ContentTypeEnum.H5P:
                //ArcadeTheme
                yield return ElementModel.l_h5p_blackslotmachine_1;
                yield return ElementModel.l_h5p_deskpc_2;
                yield return ElementModel.l_h5p_greyslotmachine_1;
                yield return ElementModel.l_h5p_purpleslotmachine_1;
                yield return ElementModel.l_h5p_redslotmachine_1;
                //CampusTheme
                yield return ElementModel.l_h5p_blackboard_2;
                yield return ElementModel.l_h5p_daylightprojector_1;
                yield return ElementModel.l_h5p_deskpc_3;
                yield return ElementModel.l_h5p_drawingtable_2;
                //SuburbTheme
                yield return ElementModel.l_h5p_slotmachine_1;
                yield return ElementModel.l_h5p_deskpc_1;
                yield return ElementModel.l_h5p_blackboard_1;
                yield return ElementModel.l_h5p_drawingtable_1;
                break;
            case ContentTypeEnum.Image:
                //ArcadeTheme
                yield return ElementModel.l_image_cardboardcutout_1;
                yield return ElementModel.l_image_gameposter_1;
                yield return ElementModel.l_image_gameposter_2;
                //CampusTheme
                yield return ElementModel.l_image_sciencebio_1;
                yield return ElementModel.l_image_sciencegeo_1;
                yield return ElementModel.l_image_sciencewhiteboard_1;
                //SuburbTheme
                yield return ElementModel.l_picture_painting_1;
                yield return ElementModel.l_picture_painting_2;
                yield return ElementModel.l_picture_paintingeasel_1;
                break;
            case ContentTypeEnum.Text:
                //ArcadeTheme
                yield return ElementModel.l_text_comicshelfbig_1;
                yield return ElementModel.l_text_comicshelfsmall_1;
                //CampusTheme
                yield return ElementModel.l_text_libraryshelf_1;
                //SuburbTheme
                yield return ElementModel.l_text_bookshelf_1;
                yield return ElementModel.l_text_bookshelf_2;
                break;
            case ContentTypeEnum.Video:
                //ArcadeTheme
                yield return ElementModel.l_video_vrdesk_1;
                //CampusTheme
                yield return ElementModel.l_video_movieprojektor_1;
                //SuburbTheme
                yield return ElementModel.l_video_television_1;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(modelType), modelType, null);
        }
    }

    internal static IEnumerable<ElementModel> GetElementModelsForTheme(Theme theme)
    {
        switch (theme)
        {
            case Theme.Arcade:
                yield return ElementModel.l_h5p_blackslotmachine_1;
                yield return ElementModel.l_h5p_deskpc_2;
                yield return ElementModel.l_h5p_greyslotmachine_1;
                yield return ElementModel.l_h5p_purpleslotmachine_1;
                yield return ElementModel.l_h5p_redslotmachine_1;
                yield return ElementModel.l_image_cardboardcutout_1;
                yield return ElementModel.l_image_gameposter_1;
                yield return ElementModel.l_image_gameposter_2;
                yield return ElementModel.l_text_comicshelfbig_1;
                yield return ElementModel.l_text_comicshelfsmall_1;
                yield return ElementModel.l_video_vrdesk_1;
                break;
            case Theme.Campus:
                yield return ElementModel.l_h5p_blackboard_2;
                yield return ElementModel.l_h5p_daylightprojector_1;
                yield return ElementModel.l_h5p_deskpc_3;
                yield return ElementModel.l_h5p_drawingtable_2;
                yield return ElementModel.l_image_sciencebio_1;
                yield return ElementModel.l_image_sciencegeo_1;
                yield return ElementModel.l_image_sciencewhiteboard_1;
                yield return ElementModel.l_text_libraryshelf_1;
                yield return ElementModel.l_video_movieprojektor_1;
                break;
            case Theme.Suburb:
                yield return ElementModel.l_h5p_blackboard_1;
                yield return ElementModel.l_h5p_deskpc_1;
                yield return ElementModel.l_h5p_drawingtable_1;
                yield return ElementModel.l_h5p_slotmachine_1;
                yield return ElementModel.l_picture_painting_1;
                yield return ElementModel.l_picture_painting_2;
                yield return ElementModel.l_picture_paintingeasel_1;
                yield return ElementModel.l_text_bookshelf_1;
                yield return ElementModel.l_text_bookshelf_2;
                yield return ElementModel.l_video_television_1;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(theme), theme, null);
        }
    }

    private class ElementModelComparer : Comparer<ElementModel>
    {
        private readonly Theme _theme;

        private readonly ContentTypeEnum _type;

        public ElementModelComparer(ContentTypeEnum type, Theme theme)
        {
            _type = type;
            _theme = theme;
        }

        public override int Compare(ElementModel x, ElementModel y)
        {
            //ElementModel.l_random is always the first element
            if (x == ElementModel.l_random) return -1;
            if (y == ElementModel.l_random) return 1;

            var xInTheme = GetElementModelsForTheme(_theme).Contains(x);
            var yInTheme = GetElementModelsForTheme(_theme).Contains(y);
            var xInType = GetElementModelsForModelType(_type).Contains(x);
            var yInType = GetElementModelsForModelType(_type).Contains(y);

            return (xInTheme, yInTheme, xInType, yInType) switch
            {
                //If one of the elements is in the theme and the other is not, the one in the theme is first
                (true, false, _, _) => -1,
                (false, true, _, _) => 1,
                //If both elements are in the theme and one of them is in the type, but the other is not, the one in the type is first
                (true, true, true, false) => -1,
                (true, true, false, true) => 1,
                //If both elements are not in the theme and one of them is in the type, but the other is not, the one in the type is first
                (false, false, true, false) => -1,
                (false, false, false, true) => 1,
                //If both elements are or are not in the theme and both are or are not in the type, compare them by their enum value
                _ => x.CompareTo(y)
            };
        }
    }
}