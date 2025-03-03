using System;
using System.Linq;

namespace ElectronWrapper;

public class OpenDialogOptions
{
    public OpenDialogOptions()
    {
        InnerOpenDialogOptions = new ElectronSharp.API.Entities.OpenDialogOptions();
    }

    internal OpenDialogOptions(ElectronSharp.API.Entities.OpenDialogOptions innerOpenDialogOptions)
    {
        InnerOpenDialogOptions = innerOpenDialogOptions;
    }
    
    internal readonly ElectronSharp.API.Entities.OpenDialogOptions InnerOpenDialogOptions;

    public string Title
    {
        get => InnerOpenDialogOptions.Title;
        set => InnerOpenDialogOptions.Title = value;
    }

    public string DefaultPath
    {
        get => InnerOpenDialogOptions.DefaultPath;
        set => InnerOpenDialogOptions.DefaultPath = value;
    }

    public FileFilter[] Filters
    {
        get => InnerOpenDialogOptions.Filters.Select(f => new FileFilter(f)).ToArray();
        set => InnerOpenDialogOptions.Filters = value.Select(f => f.InnerFileFilter).ToArray();
    }

    public OpenDialogProperty[] Properties
    {
        get => InnerOpenDialogOptions.Properties.Select(MapForward).ToArray();
        set => InnerOpenDialogOptions.Properties = value.Select(MapBackwards).ToArray();
    }

    private OpenDialogProperty MapForward(ElectronSharp.API.Entities.OpenDialogProperty prop) => prop switch
    {
        ElectronSharp.API.Entities.OpenDialogProperty.openFile => OpenDialogProperty.OpenFile,
        ElectronSharp.API.Entities.OpenDialogProperty.openDirectory => OpenDialogProperty.OpenDirectory,
        ElectronSharp.API.Entities.OpenDialogProperty.multiSelections => OpenDialogProperty.MultiSelections,
        ElectronSharp.API.Entities.OpenDialogProperty.showHiddenFiles => OpenDialogProperty.ShowHiddenFiles,
        _ => throw new ArgumentOutOfRangeException(nameof(prop), prop, null)
    };

    private ElectronSharp.API.Entities.OpenDialogProperty MapBackwards(OpenDialogProperty prop) => prop switch
    {
        OpenDialogProperty.OpenFile => ElectronSharp.API.Entities.OpenDialogProperty.openFile,
        OpenDialogProperty.OpenDirectory => ElectronSharp.API.Entities.OpenDialogProperty.openDirectory,
        OpenDialogProperty.MultiSelections => ElectronSharp.API.Entities.OpenDialogProperty.multiSelections,
        OpenDialogProperty.ShowHiddenFiles => ElectronSharp.API.Entities.OpenDialogProperty.showHiddenFiles,
        _ => throw new ArgumentOutOfRangeException(nameof(prop), prop, null)
    };
}