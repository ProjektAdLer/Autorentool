using System;
using System.Threading;
using System.Threading.Tasks;
using Bunit;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Dialogues;

namespace IntegrationTest.Dialogues;

[TestFixture]
public class UploadProgressDialogIt : MudBlazorTestFixture<UploadProgressDialog>
{
    [Test]
    // ANF-ID: [AHO22]
    public async Task ProgressReports100_DialogCloses()
    {
        var provider = Context.RenderComponent<MudDialogProvider>();
        var synchronizationContext = Substitute.For<SynchronizationContext>();
        synchronizationContext
            .When(s => s.Post(Arg.Any<SendOrPostCallback>(), Arg.Any<object?>()))
            .Do(ci => provider.InvokeAsync(() => ci.Arg<SendOrPostCallback>().Invoke(ci[1])));
        SynchronizationContext.SetSynchronizationContext(synchronizationContext);
        var progress = new Progress<int>();
        var dialogParameters = new DialogParameters { { nameof(UploadProgressDialog.Progress), progress } };

        var service = (DialogService)Context.Services.GetService<IDialogService>()!;
        IDialogReference? dialogReference = null;
        await provider.InvokeAsync(async () =>
        {
            dialogReference = await service.ShowAsync<UploadProgressDialog>("foo", dialogParameters);
        });
        Assert.That(dialogReference, Is.Not.Null);

        (progress as IProgress<int>).Report(100);
        var result = await dialogReference.Result;
        Assert.That(result!.Canceled, Is.False);
        Assert.That(result.Data, Is.EqualTo(true));
    }
}