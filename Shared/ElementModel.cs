namespace Shared;

public enum ElementModel
{
    // ReSharper disable InconsistentNaming
    l_random,

    //ArcadeTheme
    l_h5p_blackslotmachine_1,
    l_h5p_deskpc_2,
    l_h5p_greyslotmachine_1,
    l_h5p_purpleslotmachine_1,
    l_h5p_redslotmachine_1,

    l_image_cardboardcutout_1,
    l_image_gameposter_1,
    l_image_gameposter_2,

    l_text_comicshelfbig_1,
    l_text_comicshelfsmall_1,

    l_video_vrdesk_1,

    //CampusTheme
    l_h5p_blackboard_2,
    l_h5p_daylightprojector_1,
    l_h5p_deskpc_3,
    l_h5p_drawingtable_2,

    l_image_sciencebio_1,
    l_image_sciencegeo_1,
    l_image_sciencewhiteboard_1,

    l_text_libraryshelf_1,

    l_video_movieprojector_1,

    //SuburbTheme
    l_h5p_blackboard_1,
    l_h5p_deskpc_1,
    l_h5p_drawingtable_1,
    l_h5p_slotmachine_1,

    l_picture_painting_1,
    l_picture_painting_2,
    l_picture_paintingeasel_1,

    l_text_bookshelf_1,
    l_text_bookshelf_2,

    l_video_television_1,
    
    
    //Adaptivity
    a_npc_alerobot,
    
    //NPCs
    
    a_npc_defaultdark_female,
    a_npc_defaultdark_male,
    a_npc_bully_female,
    a_npc_bully_male,
    a_npc_studentdark_female,
    a_npc_studentdark_male,
    a_npc_studentlight_female,
    a_npc_studentlight_male,
    a_npc_oldiedark_female,
    a_npc_oldiedark_male,
    a_npc_oldielight_female,
    a_npc_oldielight_male,
    a_npc_nerddark_female,
    a_npc_nerddark_male,
    a_npc_nerdlight_female,
    a_npc_nerdlight_male,
    a_npc_hiphop_female,
    a_npc_hiphop_male,
    a_npc_santa_female,
    a_npc_santa_male,
    a_npc_sheriffjustice,
    a_npc_dozentantonia,
    a_npc_dozentdaniel,
    a_npc_dozentgeorg,
    a_npc_dozentjoerg,
    a_npc_dozentlukas,
    
    //TODO Check if this is correct
    [Obsolete("Use a_npc_defaultdark_female instead", error: false)]
    a_npc_defaultnpc=a_npc_defaultdark_female,
    [Obsolete("Use a_npc_defaultdark_female instead", error: false)]
    a_npc_defaultfemale=a_npc_defaultdark_female,
    [Obsolete("Use a_npc_defaultdark_male instead", error: false)]
    a_npc_defaultmale = a_npc_defaultdark_male,
    [Obsolete("Use a_npc_bully_female instead", error: false)]
    a_npc_bullyfemale = a_npc_bully_female,
    [Obsolete("Use a_npc_bully_male instead", error: false)]
    a_npc_bullymale = a_npc_bully_male,
    [Obsolete("Use a_npc_oldiedark_male instead", error: false)]
    a_npc_oldman = a_npc_oldiedark_male,
    [Obsolete("Use a_npc_hiphop_female instead", error: false)]
    a_npc_hiphopfemale = a_npc_hiphop_female,
    [Obsolete("Use a_npc_hiphop_male instead", error: false)]
    a_npc_hiphopmale = a_npc_hiphop_male,
    [Obsolete("Use a_npc_santa_female instead", error: false)]
    a_npc_santafemale = a_npc_santa_female,
    [Obsolete("Use a_npc_santa_male instead", error: false)]
    a_npc_santamale = a_npc_santa_male,

    // ReSharper restore InconsistentNaming
    
}