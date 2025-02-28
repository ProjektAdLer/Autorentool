using System.Threading.Tasks;
using Bunit;
using BusinessLogic.Entities.LearningOutcome;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.LearningOutcomes;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningSpace.LearningOutcomeViewModel;

namespace IntegrationTest.Dialogues.Outcomes;

[TestFixture]
public class CreateEditManualLearningOutcomeUt : MudDialogTestFixture<CreateEditManualLearningOutcome>
{
    private IPresentationLogic PresentationLogic { get; set; }
    private LearningOutcomeCollectionViewModel? Collection { get; set; }
    private ManualLearningOutcome? Outcome { get; set; }
    public IDialogReference Dialog { get; set; } = null!;

    [SetUp]
    public new void Setup()
    {
        PresentationLogic = Substitute.For<IPresentationLogic>();
        Context.Services.AddSingleton(PresentationLogic);
    }

    private async Task GetDialogAsync()
    {
        var dialogParameters = GetDialogParameters(Collection, Outcome);

        Dialog = await OpenDialogAndGetDialogReferenceAsync(parameters:dialogParameters);
    }


    [Test]
    // ANF-ID: [AHO02]
    public async Task ClickSubmit_NoCurrentOutcome_ShouldCallPresentationLogicAddOutcome()
    {
        await GetDialogAsync();

        const string str = "You will become a millionaire!";
        // var tf = DialogProvider.FindComponent<MudTextField<string>>();
        DialogProvider.Render();
        var tf = DialogProvider;
        var ta = tf.Find("textarea");
        ta.Change(str);
        var mudBtn = DialogProvider.FindComponent<MudButton>();
        var btn = mudBtn.Find("button");
        btn.Click();
        
        PresentationLogic.Received().AddManualLearningOutcome(Arg.Any<LearningOutcomeCollectionViewModel>(), str);
    }

    private DialogParameters GetDialogParameters(
        LearningOutcomeCollectionViewModel? collection = null, ManualLearningOutcome? outcome = null)
    {
        collection ??= new LearningOutcomeCollectionViewModel();
        return new DialogParameters<CreateEditManualLearningOutcome>
        {
            { nameof(CreateEditManualLearningOutcome.LearningOutcomeCollection), collection },
            { nameof(CreateEditManualLearningOutcome.CurrentManualLearningOutcome), outcome }
        };
    }
}