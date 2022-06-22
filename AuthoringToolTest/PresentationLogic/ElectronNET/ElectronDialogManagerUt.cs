using AuthoringTool.PresentationLogic.ElectronNET;
using ElectronWrapper;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolTest.PresentationLogic.ElectronNET;

[TestFixture]
public class ElectronDialogManagerUt
{
    [Test]
    public void ElectronDialogManager_Constructor_AllPropertiesSet()
    {
        var windowManager = Substitute.For<IWindowManagerWrapper>();
        var dialogWrapper = Substitute.For<IDialogWrapper>();
        
        var systemUnderTest = GetElectronDialogManagerForTest(windowManager, dialogWrapper);
        
        Assert.That(systemUnderTest.WindowManager, Is.EqualTo(windowManager));
        Assert.That(systemUnderTest.DialogWrapper, Is.EqualTo(dialogWrapper));
    }

    private ElectronDialogManager GetElectronDialogManagerForTest(IWindowManagerWrapper? windowManager = null,
        IDialogWrapper? dialogWrapper = null)
    {
        windowManager ??= Substitute.For<IWindowManagerWrapper>();
        dialogWrapper ??= Substitute.For<IDialogWrapper>();
        
        return new ElectronDialogManager(windowManager, dialogWrapper);
    }
}