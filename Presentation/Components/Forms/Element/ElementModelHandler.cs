﻿using Shared;
using Shared.Theme;

namespace Presentation.Components.Forms.Element;

public enum ElementModelContentType
{
    Any,
    File,
    Link,
    Adaptivity,
    Story
}

public class ElementModelHandler : IElementModelHandler
{
    //If you add a new ElementModel, you have to add it to the following methods:
    //  - GetIconForElementModel: Add the path to the icon for the new ElementModel
    //  - GetElementModelsForModelType: Add the new ElementModel to the switch statement for each corresponding ContentType
    //  - GetElementModelsForTheme: Add the new ElementModel to the switch statement for each corresponding Theme
    public IEnumerable<ElementModel> GetElementModels(ElementModelContentType contentType, string fileType = "",
        WorldTheme? theme = null)
    {
        var type = contentType switch
        {
            ElementModelContentType.Any => ContentTypeEnum.Text,
            ElementModelContentType.File => ContentTypeHelper.GetContentType(fileType),
            ElementModelContentType.Link => ContentTypeEnum.Video,
            ElementModelContentType.Adaptivity => ContentTypeEnum.Adaptivity,
            ElementModelContentType.Story => ContentTypeEnum.Story,
            _ => throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null)
        };

        IComparer<ElementModel> comparer = new ElementModelComparer(type, theme ?? WorldTheme.CampusAschaffenburg);

        switch (type)
        {
            case ContentTypeEnum.Story:
                return NpcModels.OrderBy(m => m, comparer);
            case ContentTypeEnum.Adaptivity:
                return AdaptivityModels;
            default:
            {
                var elementModels = (ElementModel[])Enum.GetValues(typeof(ElementModel));
                return elementModels.Except(NpcModels).Except(AdaptivityModels).OrderBy(m => m, comparer);
            }
        }
    }

    internal static readonly IEnumerable<ElementModel> NpcModels =
        GetElementModelsForModelType(ContentTypeEnum.Story).ToArray();

    internal static readonly IEnumerable<ElementModel> AdaptivityModels =
        GetElementModelsForModelType(ContentTypeEnum.Adaptivity).ToArray();

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
            ElementModel.l_video_movieprojector_1 =>
                "CustomIcons/ElementModels/campusTheme/l_video_movieprojector_1.png",
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
            //Adaptivity
            ElementModel.a_npc_alerobot => "CustomIcons/AdaptivityElementModels/a_npc_alerobot.png",
            //Story NPCs
            ElementModel.a_npc_sheriffjustice => "CustomIcons/AdaptivityElementModels/npc/a_npc_sheriffjustice.png",
            ElementModel.a_npc_dozentlukas => "CustomIcons/AdaptivityElementModels/npc/a_npc_dozentlukas.png",
            ElementModel.a_npc_defaultnpc => "CustomIcons/AdaptivityElementModels/npc/a_npc_defaultnpc.png",
            ElementModel.a_npc_bullyfemale => "CustomIcons/AdaptivityElementModels/npc/a_npc_bullyfemale.png",
            ElementModel.a_npc_bullymale => "CustomIcons/AdaptivityElementModels/npc/a_npc_bullymale.png",
            ElementModel.a_npc_oldman => "CustomIcons/AdaptivityElementModels/npc/a_npc_oldman.png",
            ElementModel.a_npc_hiphopfemale => "CustomIcons/AdaptivityElementModels/npc/a_npc_hiphopfemale.png",
            ElementModel.a_npc_hiphopmale => "CustomIcons/AdaptivityElementModels/npc/a_npc_hiphopmale.png",
            ElementModel.a_npc_santafemale => "CustomIcons/AdaptivityElementModels/npc/a_npc_santafemale.png",
            ElementModel.a_npc_santamale => "CustomIcons/AdaptivityElementModels/npc/a_npc_santamale.png",
            _ => throw new ArgumentOutOfRangeException(nameof(elementModel), elementModel,
                @"Icon not found for ElementModel")
        };
    }

    public ElementModel GetElementModelRandom()
    {
        return ElementModel.l_random;
    }

    public static ElementModel GetElementModelDefault(ContentTypeEnum modelType)
    {
        return modelType switch
        {
            ContentTypeEnum.Adaptivity => ElementModel.a_npc_alerobot,
            _ => ElementModel.l_random
        };
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
                yield return ElementModel.l_video_movieprojector_1;
                //SuburbTheme
                yield return ElementModel.l_video_television_1;
                break;
            case ContentTypeEnum.Adaptivity:
                //adaptivity
                yield return ElementModel.a_npc_alerobot;
                break;
            case ContentTypeEnum.Story:
                //campus
                yield return ElementModel.a_npc_dozentlukas;
                //arcade
                yield return ElementModel.a_npc_sheriffjustice;
                //suburb
                yield return ElementModel.a_npc_defaultnpc;
                //npc
                yield return ElementModel.a_npc_bullyfemale;
                yield return ElementModel.a_npc_bullymale;
                yield return ElementModel.a_npc_oldman;
                yield return ElementModel.a_npc_hiphopfemale;
                yield return ElementModel.a_npc_hiphopmale;
                yield return ElementModel.a_npc_santafemale;
                yield return ElementModel.a_npc_santamale;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(modelType), modelType, null);
        }
    }

    internal static IEnumerable<ElementModel> GetElementModelsForTheme(WorldTheme worldTheme)
    {
        switch (worldTheme)
        {
            case WorldTheme.Company:
                break;
            case WorldTheme.CampusAschaffenburg:
            case WorldTheme.CampusKempten:
                yield return ElementModel.l_h5p_blackboard_2;
                yield return ElementModel.l_h5p_daylightprojector_1;
                yield return ElementModel.l_h5p_deskpc_3;
                yield return ElementModel.l_h5p_drawingtable_2;
                yield return ElementModel.l_image_sciencebio_1;
                yield return ElementModel.l_image_sciencegeo_1;
                yield return ElementModel.l_image_sciencewhiteboard_1;
                yield return ElementModel.l_text_libraryshelf_1;
                yield return ElementModel.l_video_movieprojector_1;
                break;
            case WorldTheme.Suburb:
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
                throw new ArgumentOutOfRangeException(nameof(worldTheme), worldTheme, null);
        }

        // Models that are in all themes
        yield return ElementModel.a_npc_alerobot;
        yield return ElementModel.a_npc_defaultnpc;
        yield return ElementModel.a_npc_dozentlukas;
        yield return ElementModel.a_npc_sheriffjustice;
        yield return ElementModel.a_npc_bullyfemale;
        yield return ElementModel.a_npc_bullymale;
        yield return ElementModel.a_npc_oldman;
        yield return ElementModel.a_npc_hiphopfemale;
        yield return ElementModel.a_npc_hiphopmale;
        yield return ElementModel.a_npc_santafemale;
        yield return ElementModel.a_npc_santamale;
    }

    private class ElementModelComparer : Comparer<ElementModel>
    {
        private readonly WorldTheme _worldTheme;

        private readonly ContentTypeEnum _type;

        public ElementModelComparer(ContentTypeEnum type, WorldTheme worldTheme)
        {
            _type = type;
            _worldTheme = worldTheme;
        }

        public override int Compare(ElementModel x, ElementModel y)
        {
            //ElementModel.l_random is always the first element
            if (x == ElementModel.l_random) return -1;
            if (y == ElementModel.l_random) return 1;

            var xInTheme = GetElementModelsForTheme(_worldTheme).Contains(x);
            var yInTheme = GetElementModelsForTheme(_worldTheme).Contains(y);
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