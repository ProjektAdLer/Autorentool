using ElectronWrapper;
using NSubstitute;
using NUnit.Framework;

namespace PresentationTest.PresentationLogic.ElectronNET;

[TestFixture]
public class ElectronShutdownManagerUt
{
    [Test]
    public void ElectronShutdownManager_BeginShutdown_CallsBeforeThenOnShutdownHandlers()
    {
        var BeforeCallbackCalled = false;
        var OnCallbackCalled = false;
        
        var systemUnderTest = CreateElectronShutdownManagerForTest();
        systemUnderTest.BeforeShutdown += (_,_) =>
        {
            if (OnCallbackCalled) Assert.Fail("OnShutdown callback called before BeforeShutdown callback");
            if (BeforeCallbackCalled) Assert.Fail("BeforeShutdown callback called twice");
            BeforeCallbackCalled = true;
        };
        systemUnderTest.OnShutdown += _ =>
        {
            if (!BeforeCallbackCalled) Assert.Fail("BeforeShutdown callback not called before OnShutdown callback");
            if (OnCallbackCalled) Assert.Fail("OnShutdown callback called twice");
            OnCallbackCalled = true;
        };

        systemUnderTest.BeginShutdown();
        
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
        };
        systemUnderTest.OnShutdown += _ =>
        {
            Assert.Fail("OnShutdown called despite cancelling shutdown");
        };

        systemUnderTest.BeginShutdown();

        Assert.That(BeforeCallbackCalled, Is.True);
        appWrapper.Received().DidNotReceive().Quit();
    }

    [Test]
    public void ElectronShutdownManager_BeginShutdown_CallsAppWrapper()
    {
        var appWrapper = Substitute.For<IAppWrapper>();

        var systemUnderTest = CreateElectronShutdownManagerForTest(appWrapper);

        systemUnderTest.BeginShutdown();
        appWrapper.Received().Exit();
    }

    private IShutdownManager CreateElectronShutdownManagerForTest(IAppWrapper? appWrapper = null)
    {
        appWrapper ??= Substitute.For<IAppWrapper>();
        return new ElectronShutdownManager(appWrapper);
    }
}