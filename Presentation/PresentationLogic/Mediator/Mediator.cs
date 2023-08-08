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
    private bool _worldOverViewOpen;
    private bool _worldViewOpen;
    private bool _advancedLearningSpaceChecked;

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
        WorldViewOpen = false;
        WorldOverviewOpen = false;
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

    public bool AdvancedLearningSpaceChecked
    {
        get => _advancedLearningSpaceChecked;
        private set => SetField(ref _advancedLearningSpaceChecked, value);
    }

    #endregion

    #region right side

    public bool WorldViewOpen
    {
        get => _worldViewOpen;
        private set => SetField(ref _worldViewOpen, value);
    }

    public bool WorldOverviewOpen
    {
        get => _worldOverViewOpen;
        private set => SetField(ref _worldOverViewOpen, value);
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

    public void RequestOpenWorldView()
    {
        CloseAllRightSide();
        WorldViewOpen = true;
    }

    public void RequestOpenWorldOverview()
    {
        CloseAllRightSide();
        WorldOverviewOpen = true;
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

    public void RequestToggleWorldView()
    {
        if (WorldViewOpen)
        {
            WorldViewOpen = false;
        }
        else
        {
            RequestOpenWorldView();
        }
    }

    public void RequestToggleWorldOverview()
    {
        if (WorldOverviewOpen)
        {
            WorldOverviewOpen = false;
        }
        else
        {
            RequestOpenWorldOverview();
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