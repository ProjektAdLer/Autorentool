using MudBlazor;
using Presentation.Components.LearningOutcomes;
using Presentation.PresentationLogic.LearningOutcome;

namespace Presentation.Components.Forms;

public static class LearningOutcomesOverviewHelper
{
    public static async Task ShowOverviewAsync(IDialogService dialogService,
        ILearningOutcomeCollectionViewModel learningOutcomeCollection, Type entityType)
    {
        var options = new DialogOptions
        {
            BackdropClick = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            CloseButton = true,
        };
        var parameters = new DialogParameters
        {
            { nameof(LearningOutcomesOverview.LearningOutcomeCollection), learningOutcomeCollection },
            { nameof(LearningOutcomesOverview.EntityType), entityType }
        };
        var dialog = await dialogService.ShowAsync<LearningOutcomesOverview>("", parameters, options);
        _ = await dialog.Result;
    }
}