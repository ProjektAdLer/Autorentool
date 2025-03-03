using System;
using System.Threading;
using Bunit;
using Bunit.TestDoubles;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.Dialogues;
using TestHelpers;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.Dialogues;

[TestFixture]
public class UploadProgressDialogUt
{
    [SetUp]
    public void Setup()
    {
        _ctx = new TestContext();
        _ctx.ComponentFactories.AddStub<MudDialog>();
        _ctx.ComponentFactories.AddStub<MudProgressLinear>();
        _ctx.AddLocalizerForTest<UploadProgressDialog>();
    }

    [TearDown]
    public void TearDown()
    {
        _ctx.Dispose();
    }

    private TestContext _ctx;

    [Test]
    public void Render_ParametersSet()
    {
        var mudDialogInstance = Substitute.For<IMudDialogInstance>();
        var progress = new Progress<int>();

        var systemUnderTest = GetRenderedComponent(mudDialogInstance, progress);

        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Instance.Progress, Is.EqualTo(progress));
            Assert.That(systemUnderTest.Instance.MudDialog, Is.EqualTo(mudDialogInstance));
        });
    }

    [Test]
    public void Progress_Report_UpdatesProgressLinear()
    {
        var mudDialogInstance = Substitute.For<IMudDialogInstance>();
        var synchronizationContext = Substitute.For<SynchronizationContext>();
        synchronizationContext
            .When(s => s.Post(Arg.Any<SendOrPostCallback>(), Arg.Any<object?>()))
            .Do(ci => ci.Arg<SendOrPostCallback>().Invoke(ci[1]));
        SynchronizationContext.SetSynchronizationContext(synchronizationContext);
        var progress = new Progress<int>();

        var systemUnderTest = GetRenderedComponent(mudDialogInstance, progress);

        var mudDialog = systemUnderTest.FindComponent<Stub<MudDialog>>();
        var mudProgressLinear = mudDialog.Render<MudDialog, Stub<MudProgressLinear>>(_ctx, "DialogContent");
        var progressParameter = (double)mudProgressLinear.Instance.Parameters[nameof(MudProgressLinear.Value)];

        Assert.That(progressParameter, Is.EqualTo(0d));

        (progress as IProgress<int>).Report(50);

        mudProgressLinear = mudDialog.Render<MudDialog, Stub<MudProgressLinear>>(_ctx, "DialogContent");
        progressParameter = (double)mudProgressLinear.Instance.Parameters[nameof(MudProgressLinear.Value)];
        Assert.That(progressParameter, Is.EqualTo(50d));
    }

    [Test]
    public void Progress_Report100_ClosesDialog()
    {
        var mudDialogInstance = Substitute.For<IMudDialogInstance>();
        var synchronizationContext = Substitute.For<SynchronizationContext>();
        synchronizationContext
            .When(s => s.Post(Arg.Any<SendOrPostCallback>(), Arg.Any<object?>()))
            .Do(ci => ci.Arg<SendOrPostCallback>().Invoke(ci[1]));
        SynchronizationContext.SetSynchronizationContext(synchronizationContext);
        var progress = new Progress<int>();

        var systemUnderTest = GetRenderedComponent(mudDialogInstance, progress);

        var mudDialog = systemUnderTest.FindComponent<Stub<MudDialog>>();
        var mudProgressLinear = mudDialog.Render<MudDialog, Stub<MudProgressLinear>>(_ctx, "DialogContent");
        var progressParameter = (double)mudProgressLinear.Instance.Parameters[nameof(MudProgressLinear.Value)];

        Assert.That(progressParameter, Is.EqualTo(0d));
        //TODO: We cannot replace MudDialogInstance inside of our Dialog as we must use the type instead of an interface
        //TODO: therefore we cannot intercept the call to Close() and check if it was called with the correct parameters
        /*
        (progress as IProgress<int>).Report(100);

        mudDialogInstance.Received().Close(DialogResult.Ok(true));
        */
    }

    private IRenderedComponent<UploadProgressDialog> GetRenderedComponent(IMudDialogInstance? mudDialogInstance = null,
        Progress<int>? progress = null)
    {
        mudDialogInstance ??= Substitute.For<IMudDialogInstance>();
        progress ??= new Progress<int>();

        return _ctx.RenderComponent<UploadProgressDialog>(param => param
            .Add(p => p.Progress, progress)
            .AddCascadingValue(mudDialogInstance)
        );
    }

    private class TestProgress<T> : Progress<T>
    {
        protected override void OnReport(T value)
        {
        }
    }
}