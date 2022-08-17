using System;
using System.Linq;

namespace ElectronWrapper;

public class OpenDialogOptions
{
    public OpenDialogOptions()
    {
        InnerOpenDialogOptions = new ElectronNET.API.Entities.OpenDialogOptions();
    }

    internal OpenDialogOptions(ElectronNET.API.Entities.OpenDialogOptions innerOpenDialogOptions)
    {
        InnerOpenDialogOptions = innerOpenDialogOptions;
    }
    
    internal readonly ElectronNET.API.Entities.OpenDialogOptions InnerOpenDialogOptions;

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

    private OpenDialogProperty MapForward(ElectronNET.API.Entities.OpenDialogProperty prop) => prop switch
    {
        ElectronNET.API.Entities.OpenDialogProperty.openFile => OpenDialogProperty.OpenFile,
        ElectronNET.API.Entities.OpenDialogProperty.openDirectory => OpenDialogProperty.OpenDirectory,
        ElectronNET.API.Entities.OpenDialogProperty.multiSelections => OpenDialogProperty.MultiSelections,
        ElectronNET.API.Entities.OpenDialogProperty.showHiddenFiles => OpenDialogProperty.ShowHiddenFiles,
        ElectronNET.API.Entities.OpenDialogProperty.createDirectory => OpenDialogProperty.CreateDirectory,
        ElectronNET.API.Entities.OpenDialogProperty.promptToCreate => OpenDialogProperty.PromptToCreate,
        ElectronNET.API.Entities.OpenDialogProperty.noResolveAliases => OpenDialogProperty.NoResolveAliases,
        ElectronNET.API.Entities.OpenDialogProperty.treatPackageAsDirectory => OpenDialogProperty
            .TreatPackageAsDirectory,
        _ => throw new ArgumentOutOfRangeException(nameof(prop), prop, null)
    };

    private ElectronNET.API.Entities.OpenDialogProperty MapBackwards(OpenDialogProperty prop) => prop switch
    {
        OpenDialogProperty.OpenFile => ElectronNET.API.Entities.OpenDialogProperty.openFile,
        OpenDialogProperty.OpenDirectory => ElectronNET.API.Entities.OpenDialogProperty.openDirectory,
        OpenDialogProperty.MultiSelections => ElectronNET.API.Entities.OpenDialogProperty.multiSelections,
        OpenDialogProperty.ShowHiddenFiles => ElectronNET.API.Entities.OpenDialogProperty.showHiddenFiles,
        OpenDialogProperty.CreateDirectory => ElectronNET.API.Entities.OpenDialogProperty.createDirectory,
        OpenDialogProperty.PromptToCreate => ElectronNET.API.Entities.OpenDialogProperty.promptToCreate,
        OpenDialogProperty.NoResolveAliases => ElectronNET.API.Entities.OpenDialogProperty.noResolveAliases,
        OpenDialogProperty.TreatPackageAsDirectory => ElectronNET.API.Entities.OpenDialogProperty
            .treatPackageAsDirectory,
        _ => throw new ArgumentOutOfRangeException(nameof(prop), prop, null)
    };
}