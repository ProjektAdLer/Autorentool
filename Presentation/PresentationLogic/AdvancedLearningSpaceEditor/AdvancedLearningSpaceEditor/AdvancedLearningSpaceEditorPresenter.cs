using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components.Web;
using Presentation.Components;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedComponent;
using Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpace;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.SelectedViewModels;
using Shared;

namespace Presentation.PresentationLogic.AdvancedLearningSpaceEditor.AdvancedLearningSpaceEditor;

public class AdvancedLearningSpaceEditorPresenter : IAdvancedLearningSpaceEditorPresenter
{
    private readonly IErrorService _errorService;
    private IAdvancedLearningSpaceViewModel? _advancedLearningSpaceVm;
    private readonly ISelectedViewModelsProvider _selectedViewModelsProvider;

    public AdvancedLearningSpaceEditorPresenter(ILogger<AdvancedLearningSpaceEditorPresenter> logger,
        ISelectedViewModelsProvider selectedViewModelsProvider, IErrorService errorService)
    {
        Logger = logger;
        _selectedViewModelsProvider = selectedViewModelsProvider;
        _selectedViewModelsProvider.PropertyChanged += SelectedViewModelsProviderOnPropertyChanged;
        _errorService = errorService;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private ILogger<AdvancedLearningSpaceEditorPresenter> Logger { get; }

    public IAdvancedLearningSpaceViewModel? AdvancedLearningSpaceViewModel
    {
        get => _advancedLearningSpaceVm;
        private set => SetField(ref _advancedLearningSpaceVm, value);
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void SelectedViewModelsProviderOnPropertyChanged(object? caller, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_selectedViewModelsProvider.LearningObjectInPathWay))
        {
            if (caller is not ISelectedViewModelsProvider)
            {
                LogAndSetError("OnSelectedViewModelsProviderOnPropertyChanged",
                    $"Caller must be of type ISelectedViewModelsProvider, got {caller?.GetType()}",
                    "Caller must be of type ISelectedViewModelsProvider");
                return;
            }

            if (_selectedViewModelsProvider.LearningObjectInPathWay is LearningSpaceViewModel)
                AdvancedLearningSpaceViewModel = null;
            else if (_selectedViewModelsProvider.LearningObjectInPathWay is AdvancedLearningSpaceViewModel advSpace)
                AdvancedLearningSpaceViewModel = advSpace;
            else if (_selectedViewModelsProvider.LearningObjectInPathWay is null)
                AdvancedLearningSpaceViewModel = null;
        }
    }

    private void LogAndSetError(string operation, string errorDetail, string userMessage)
    {
        Logger.LogError("Error in {Operation}: {ErrorDetail}", operation, errorDetail);
        _errorService.SetError("Operation failed", userMessage);
    }


    public void SetAdvancedLearningSpace(AdvancedLearningSpaceViewModel advSpace)
    {
        AdvancedLearningSpaceViewModel = advSpace;
        Logger.LogDebug("LearningSpace set to {Name}", advSpace.Name);
    }

    public void CreateAdvancedLearningElementSlot(double positionX = 50D, double positionY = 50D)
    {
        if (AdvancedLearningSpaceViewModel == null)
            throw new ApplicationException("AdvancedLearningSpaceViewModel is null!");
        var spaceId = AdvancedLearningSpaceViewModel.Id;
        var slotKey = AdvancedLearningSpaceViewModel.AdvancedLearningSpaceLayout.AdvancedLearningElementSlots.Count;

        AdvancedLearningSpaceViewModel.AdvancedLearningSpaceLayout.AddAdvancedLearningElementSlot(spaceId, slotKey,
            positionX, positionY);
    }

    public void CreateAdvancedDecoration(double positionX = 50D, double positionY = 50D)
    {
        if (AdvancedLearningSpaceViewModel == null)
            throw new ApplicationException("AdvancedLearningSpaceViewModel is null!");
        var spaceId = AdvancedLearningSpaceViewModel.Id;
        var decorationKey = AdvancedLearningSpaceViewModel.AdvancedLearningSpaceLayout.AdvancedDecorations.Count;

        AdvancedLearningSpaceViewModel.AdvancedLearningSpaceLayout.AddAdvancedDecoration(spaceId, decorationKey,
            positionX, positionY);
    }

    public void DeleteAdvancedComponent(IAdvancedComponentViewModel advancedComponentViewModel)
    {
        if (AdvancedLearningSpaceViewModel == null)
        {
            throw new ApplicationException("AdvancedLearningSpaceViewModel is null!");
        }

        if (advancedComponentViewModel.GetType() == typeof(AdvancedDecorationViewModel))
        {
            var decorationDictionary = AdvancedLearningSpaceViewModel.AdvancedLearningSpaceLayout.AdvancedDecorations;
            var decorationKey = decorationDictionary.FirstOrDefault(x => x.Value == advancedComponentViewModel).Key;
            decorationDictionary.Remove(decorationKey);
        }
        else if (advancedComponentViewModel.GetType() == typeof(AdvancedLearningElementSlotViewModel))
        {
            var slotDictionary =
                AdvancedLearningSpaceViewModel.AdvancedLearningSpaceLayout.AdvancedLearningElementSlots;
            var slotKey = slotDictionary.FirstOrDefault(x => x.Value == advancedComponentViewModel).Key;
            slotDictionary.Remove(slotKey);
        }
    }

    public void DeleteCornerPoint(DoublePoint cornerPoint)
    {
        var cornerPointDictionary = AdvancedLearningSpaceViewModel!.AdvancedLearningSpaceLayout.AdvancedCornerPoints;
        var cornerPointKey = cornerPointDictionary.FirstOrDefault(x => x.Value == cornerPoint).Key;
        for (int i = cornerPointKey + 1; i < cornerPointDictionary.Count; i++)
        {
            cornerPointDictionary[i - 1] = cornerPointDictionary[i];
        }

        cornerPointDictionary.Remove(cornerPointDictionary.Count - 1);
    }

    public void AddCornerPoint(MouseEventArgs mouseEventArgs)
    {
        // Check if it was a rightclick
        if (mouseEventArgs.Button == 2)
        {
            var mouseLocation = new Vector2((float)mouseEventArgs.OffsetX, (float)mouseEventArgs.OffsetY);
            var cornerPoints = GetSpaceCornerPoints();

            //Compute the wall that is closest to mouse location, then add a corner point to that wall
            var closestWallPoint = new Vector2();
            var closestPoint = new Vector2();
            var shortestDistance = float.MaxValue;
            for (int i = 0; i < cornerPoints!.Count; i++)
            {
                var wallPoint1 = new Vector2((float)cornerPoints[i].X, (float)cornerPoints[i].Y);
                var wallPoint2 = new Vector2((float)cornerPoints[i + 1 < cornerPoints.Count ? i + 1 : 0].X,
                    (float)cornerPoints[i + 1 < cornerPoints.Count ? i + 1 : 0].Y);
                var distance =
                    this.ComputeDistance(mouseLocation, wallPoint1, wallPoint2, out Vector2 closestLocalPoint);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestPoint = closestLocalPoint;
                    closestWallPoint = wallPoint1;
                }
            }

            var closestWallPointIndex = cornerPoints
                .FirstOrDefault(x => x.Value.X == closestWallPoint.X && x.Value.Y == closestWallPoint.Y).Key;
            var closestDoublePoint = new DoublePoint { X = closestPoint.X, Y = closestPoint.Y };
            cornerPoints.Add(cornerPoints.Count, cornerPoints[cornerPoints.Count - 1]);
            for (int i = cornerPoints.Count - 1; i > closestWallPointIndex + 1; i--)
            {
                cornerPoints[i] = cornerPoints[i - 1];
            }

            cornerPoints[closestWallPointIndex + 1] = closestDoublePoint;
        }
    }

    public void RotateAdvancedComponent(IAdvancedComponentViewModel advancedComponent)
    {
        advancedComponent.Rotation += 90;
    }

    public IDictionary<int, DoublePoint> GetSpaceCornerPoints()
    {
        if (AdvancedLearningSpaceViewModel == null)
            throw new ApplicationException("AdvancedLearningSpaceViewModel is null!");
        return AdvancedLearningSpaceViewModel.AdvancedLearningSpaceLayout.AdvancedCornerPoints;
    }

    private float ComputeDistance(Vector2 mousePoint, Vector2 wallPoint1, Vector2 wallPoint2, out Vector2 closestPoint)
    {
        Vector2 wallVector = wallPoint2 - wallPoint1;
        Vector2 mouseVector = mousePoint - wallPoint1;

        float wallProjection = Vector2.Dot(mouseVector, wallVector);
        float wallLength = wallVector.LengthSquared();
        float distance = wallProjection / wallLength;

        if (distance <= 0)
        {
            closestPoint = wallPoint1;
        }
        else if (distance >= 1)
        {
            closestPoint = wallPoint2;
        }
        else
        {
            closestPoint = wallPoint1 + distance * wallVector;
        }

        return Vector2.Distance(mousePoint, closestPoint);
    }
}