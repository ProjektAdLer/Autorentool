using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Presentation.PresentationLogic.Mediator;

public class Mediator : IMediator
{
    private bool _contentDialogOpen;
    private bool _elementDialogOpen;
    private bool _overwriteElementEdit;
    private bool _spaceDialogOpen;
    private bool _worldDialogOpen;
    private bool _worldTreeViewOpen;
    private bool _worldPathwayViewOpen;

    public void CloseBothSides()
    {
        CloseAllLeftSide();
        CloseAllRightSide();
    }

    /// <summary>
    /// Closes all dialogs on the left side.
    /// </summary>
    private void CloseAllLeftSide()
    {
        WorldDialogOpen = false;
        SpaceDialogOpen = false;
        ElementDialogOpen = false;
        ContentDialogOpen = false;
    }

    /// <summary>
    /// Closes all dialogs on the right side.
    /// </summary>
    private void CloseAllRightSide()
    {
        WorldPathwayViewOpen = false;
        WorldTreeViewOpen = false;
    }

    #region left side

    public bool WorldDialogOpen
    {
        get => _worldDialogOpen;
        private set => SetField(ref _worldDialogOpen, value);
    }

    public bool SpaceDialogOpen
    {
        get => _spaceDialogOpen;
        private set => SetField(ref _spaceDialogOpen, value);
    }

    public bool ElementDialogOpen
    {
        get => _elementDialogOpen;
        private set => SetField(ref _elementDialogOpen, value);
    }

    public bool OverwriteElementEdit
    {
        get => _overwriteElementEdit;
        set => SetField(ref _overwriteElementEdit, value);
    }

    public bool ContentDialogOpen
    {
        get => _contentDialogOpen;
        private set => SetField(ref _contentDialogOpen, value);
    }

    #endregion

    #region right side

    public bool WorldPathwayViewOpen
    {
        get => _worldPathwayViewOpen;
        private set => SetField(ref _worldPathwayViewOpen, value);
    }

    public bool WorldTreeViewOpen
    {
        get => _worldTreeViewOpen;
        private set => SetField(ref _worldTreeViewOpen, value);
    }

    #endregion

    #region RequestOpen

    public void RequestOpenWorldDialog()
    {
        CloseAllLeftSide();
        WorldDialogOpen = true;
    }

    public void RequestOpenSpaceDialog()
    {
        CloseAllLeftSide();
        SpaceDialogOpen = true;
    }

    public void RequestOpenElementDialog()
    {
        CloseAllLeftSide();
        ElementDialogOpen = true;
    }

    public void RequestOpenNewElementDialog()
    {
        CloseAllLeftSide();
        ElementDialogOpen = true;
        OverwriteElementEdit = true;
    }

    public void RequestOpenContentDialog()
    {
        CloseAllLeftSide();
        ContentDialogOpen = true;
    }

    public void RequestOpenPathwayView()
    {
        CloseAllRightSide();
        WorldPathwayViewOpen = true;
    }

    public void RequestOpenWorldTreeView()
    {
        CloseAllRightSide();
        WorldTreeViewOpen = true;
    }

    #endregion

    #region RequestToggle

    public void RequestToggleWorldDialog()
    {
        if (WorldDialogOpen)
        {
            WorldDialogOpen = false;
        }
        else
        {
            RequestOpenWorldDialog();
        }
    }

    public void RequestToggleSpaceDialog()
    {
        if (SpaceDialogOpen)
        {
            SpaceDialogOpen = false;
        }
        else
        {
            RequestOpenSpaceDialog();
        }
    }

    public void RequestToggleElementDialog()
    {
        if (ElementDialogOpen)
        {
            ElementDialogOpen = false;
        }
        else
        {
            RequestOpenElementDialog();
        }
    }

    public void RequestToggleContentDialog()
    {
        if (ContentDialogOpen)
        {
            ContentDialogOpen = false;
        }
        else
        {
            RequestOpenContentDialog();
        }
    }

    public void RequestToggleWorldPathwayView()
    {
        if (WorldPathwayViewOpen)
        {
            WorldPathwayViewOpen = false;
        }
        else
        {
            RequestOpenPathwayView();
        }
    }

    public void RequestToggleWorldTreeView()
    {
        if (WorldTreeViewOpen)
        {
            WorldTreeViewOpen = false;
        }
        else
        {
            RequestOpenWorldTreeView();
        }
    }

    #endregion


    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return;
        field = value;
        OnPropertyChanged(propertyName);
    }

    #endregion
}