using System.Threading.Tasks;
using ElectronWrapper;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic;
using Presentation.PresentationLogic.ElectronNET;

namespace PresentationTest.PresentationLogic.ElectronNET;

[TestFixture]
public class ElectronShutdownManagerUt
{
    [Test]
    // ANF-ID: [ASN0025]
    public void ElectronShutdownManager_BeginShutdown_CallsBeforeThenOnShutdownHandlers()
    {
        var BeforeCallbackCalled = false;
        var OnCallbackCalled = false;

        var systemUnderTest = CreateElectronShutdownManagerForTest();
        systemUnderTest.BeforeShutdown += (_, _) =>
        {
            if (OnCallbackCalled) Assert.Fail("OnShutdown callback called before BeforeShutdown callback");
            if (BeforeCallbackCalled) Assert.Fail("BeforeShutdown callback called twice");
            BeforeCallbackCalled = true;
            return Task.CompletedTask;
        };
        systemUnderTest.OnShutdown += _ =>
        {
            if (!BeforeCallbackCalled) Assert.Fail("BeforeShutdown callback not called before OnShutdown callback");
            if (OnCallbackCalled) Assert.Fail("OnShutdown callback called twice");
            OnCallbackCalled = true;
            return Task.CompletedTask;
        };

        systemUnderTest.RequestShutdownAsync();

        Assert.Multiple(() =>
        {
            Assert.That(BeforeCallbackCalled, Is.True);
            Assert.That(OnCallbackCalled, Is.True);
        });
    }

    [Test]
    public void ElectronShutdownManager_BeginShutdown_CancelShutdownWhenEventIsCalled()
    {
        var BeforeCallbackCalled = false;
        var appWrapper = Substitute.For<IAppWrapper>();

        var systemUnderTest = CreateElectronShutdownManagerForTest(appWrapper);
        systemUnderTest.BeforeShutdown += (_, args) =>
        {
            BeforeCallbackCalled = true;
            args.CancelShutdown();
            return Task.CompletedTask;
        };
        systemUnderTest.OnShutdown += _ =>
        {
            Assert.Fail("OnShutdown called despite cancelling shutdown");
            return Task.CompletedTask;
        };

        systemUnderTest.RequestShutdownAsync();

        Assert.That(BeforeCallbackCalled, Is.True);
        appWrapper.Received().DidNotReceive().Quit();
    }

    [Test]
    public void ElectronShutdownManager_BeginShutdown_CallsAppWrapper()
    {
        var appWrapper = Substitute.For<IAppWrapper>();

        var systemUnderTest = CreateElectronShutdownManagerForTest(appWrapper);

        systemUnderTest.RequestShutdownAsync();
        appWrapper.Received().Exit();
    }

    private IShutdownManager CreateElectronShutdownManagerForTest(IAppWrapper? appWrapper = null)
    {
        appWrapper ??= Substitute.For<IAppWrapper>();
        return new ElectronShutdownManager(appWrapper);
    }
}