using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ElectronWrapper;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.ElectronNET;

namespace PresentationTest.PresentationLogic.ElectronNET;

[TestFixture]
public class ElectronDialogManagerUt
{
    [Test]
    public void ElectronDialogManager_Constructor_AllPropertiesSet()
    {
        var windowManager = Substitute.For<IWindowManagerWrapper>();
        var dialogWrapper = Substitute.For<IDialogWrapper>();
        
        var systemUnderTest = GetElectronDialogManagerForTest(windowManager, dialogWrapper);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.WindowManager, Is.EqualTo(windowManager));
            Assert.That(systemUnderTest.DialogWrapper, Is.EqualTo(dialogWrapper));
        });
    }
    
    [Test]
    public async Task ElectronDialogManager_ShowSaveAsDialog_CallsDialogWrapperAndGetsWindowFromWindowManager()
    {
        var windowManager = Substitute.For<IWindowManagerWrapper>();
        var browserWindow = new BrowserWindow();
        windowManager.BrowserWindows.Returns(new[]{browserWindow});
        var dialogWrapper = Substitute.For<IDialogWrapper>();
        var path = Path.Combine("/", "super", "awesome", "path");
        const string title = "title";
        dialogWrapper.ShowSaveDialogAsync(Arg.Any<BrowserWindow>(), Arg.Any<SaveDialogOptions>())
            .Returns(path)
            .AndDoes(info => {
                var saveDialogOptions = info.Arg<SaveDialogOptions>();
                Assert.Multiple(() =>
                {
                    Assert.That(info.Arg<BrowserWindow>(), Is.EqualTo(browserWindow));
                    Assert.That(saveDialogOptions.Filters, Is.Empty);
                    Assert.That(saveDialogOptions.Title, Is.EqualTo(title));
                    Assert.That(saveDialogOptions.DefaultPath, Is.EqualTo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)));
                });
            });
            
        var systemUnderTest = GetElectronDialogManagerForTest(windowManager, dialogWrapper);
        
        var retVal = await systemUnderTest.ShowSaveAsDialogAsync(title);
        
        await dialogWrapper.Received(1).ShowSaveDialogAsync(browserWindow, Arg.Any<SaveDialogOptions>());
        Assert.That(retVal, Is.EqualTo(path));
    }

    [Test]
    public void ElectronDialogManager_ShowSaveAsDialog_ThrowsWhenNoWindowFound()
    {
        var windowManager = Substitute.For<IWindowManagerWrapper>();
        var dialogWrapper = Substitute.For<IDialogWrapper>();

        var systemUnderTest = GetElectronDialogManagerForTest(windowManager, dialogWrapper);
        
        var exception = Assert.ThrowsAsync<Exception>(async () => await systemUnderTest.ShowSaveAsDialogAsync("title"));
        Assert.That(exception!.Message, Is.EqualTo("BrowserWindow was unexpectedly null"));
    }

    [Test]
    public async Task ElectronDialogManager_ShowSaveAsDialog_PassesDefaultPathToDialogWrapper()
    {
        var windowManager = Substitute.For<IWindowManagerWrapper>();
        var browserWindow = new BrowserWindow();
        windowManager.BrowserWindows.Returns(new[]{browserWindow});
        var dialogWrapper = Substitute.For<IDialogWrapper>();
        const string defaultPath = "/super/awesome/path";
        dialogWrapper.ShowSaveDialogAsync(Arg.Any<BrowserWindow>(), Arg.Any<SaveDialogOptions>())
            .Returns("foo")
            .AndDoes(info =>
            {
                var actualPath = info.Arg<SaveDialogOptions>().DefaultPath;
                Assert.That(actualPath, Is.EqualTo(defaultPath));
            });
        
        var systemUnderTest = GetElectronDialogManagerForTest(windowManager, dialogWrapper);
        
        await systemUnderTest.ShowSaveAsDialogAsync("title", defaultPath);

        await dialogWrapper.Received().ShowSaveDialogAsync(Arg.Any<BrowserWindow>(), Arg.Any<SaveDialogOptions>());
    }

    [Test]
    public void ElectronDialogManager_ShowSaveAsDialog_ThrowsOperationCanceledExceptionWhenUserCancels()
    {
        var windowManager = Substitute.For<IWindowManagerWrapper>();
        var browserWindow = new BrowserWindow();
        windowManager.BrowserWindows.Returns(new[]{browserWindow});
        var dialogWrapper = Substitute.For<IDialogWrapper>();
        dialogWrapper.ShowSaveDialogAsync(Arg.Any<BrowserWindow>(), Arg.Any<SaveDialogOptions>())
            .Returns("");
        
        var systemUnderTest = GetElectronDialogManagerForTest(windowManager, dialogWrapper);
        
        var exception = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.ShowSaveAsDialogAsync("title"));
        Assert.That(exception!.Message, Is.EqualTo("Cancelled by user"));
    }

    [Test]
    public async Task ElectronDialogManager_ShowOpenDialog_CallsDialogWrapperAndGetsWindowFromWindowManager()
    {
        var windowManager = Substitute.For<IWindowManagerWrapper>();
        var browserWindow = new BrowserWindow();
        windowManager.BrowserWindows.Returns(new[]{browserWindow});
        var dialogWrapper = Substitute.For<IDialogWrapper>();
        var path = Path.Combine("/", "super", "awesome", "path");
        const string title = "title";
        dialogWrapper.ShowOpenDialogAsync(browserWindow, Arg.Any<OpenDialogOptions>())
            .Returns(new[] { path })
            .AndDoes(info => {
                var saveDialogOptions = info.Arg<OpenDialogOptions>();
                Assert.Multiple(() =>
                {
                    Assert.That(info.Arg<BrowserWindow>(), Is.EqualTo(browserWindow));
                    Assert.That(saveDialogOptions.Filters, Is.Empty);
                    Assert.That(saveDialogOptions.Title, Is.EqualTo(title));
                    Assert.That(saveDialogOptions.DefaultPath, Is.EqualTo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)));
                });
            });
        
        var systemUnderTest = GetElectronDialogManagerForTest(windowManager, dialogWrapper);
        
        var retVal = await systemUnderTest.ShowOpenDialogAsync(title);
        
        await dialogWrapper.Received(1).ShowOpenDialogAsync(browserWindow, Arg.Any<OpenDialogOptions>());
        Assert.That(retVal.Count(), Is.EqualTo(1));
        Assert.That(retVal.First(), Is.EqualTo(path));
    }
    
    [Test]
    public void ElectronDialogManager_ShowOpenDialog_ThrowsWhenNoWindowFound()
    {
        var windowManager = Substitute.For<IWindowManagerWrapper>();
        var dialogWrapper = Substitute.For<IDialogWrapper>();

        var systemUnderTest = GetElectronDialogManagerForTest(windowManager, dialogWrapper);
        
        var exception = Assert.ThrowsAsync<Exception>(async () => await systemUnderTest.ShowOpenDialogAsync("title"));
        Assert.That(exception!.Message, Is.EqualTo("BrowserWindow was unexpectedly null"));
    }

    [Test]
    public async Task ElectronDialogManager_ShowOpenDialog_PassesDirectoryAndMultiSelectAndDefaultPathToDialogWrapper()
    {
        var windowManager = Substitute.For<IWindowManagerWrapper>();
        var browserWindow = new BrowserWindow();
        windowManager.BrowserWindows.Returns(new[]{browserWindow});
        var dialogWrapper = Substitute.For<IDialogWrapper>();
        const string defaultPath = "/super/awesome/path";
        dialogWrapper.ShowOpenDialogAsync(Arg.Any<BrowserWindow>(), Arg.Any<OpenDialogOptions>())
            .Returns(new[] { "foo" })
            .AndDoes(info =>
            {
                var options = info.Arg<OpenDialogOptions>();
                Assert.That(options.DefaultPath, Is.EqualTo(defaultPath));
                Assert.That(options.Properties.Any(property => property == OpenDialogProperty.MultiSelections));
                Assert.That(options.Properties.Any(property => property == OpenDialogProperty.OpenDirectory));
            });
        
        var systemUnderTest = GetElectronDialogManagerForTest(windowManager, dialogWrapper); 
        
        await systemUnderTest.ShowOpenDialogAsync("title", true, true, defaultPath);
        
        await dialogWrapper.Received(1).ShowOpenDialogAsync(browserWindow, Arg.Any<OpenDialogOptions>());
    }

    [Test]
    public void ElectronDialogManager_ShowOpenDialog_ThrowsOperationCancelledExceptionWhenUserCancels()
    {
        var windowManager = Substitute.For<IWindowManagerWrapper>();
        var browserWindow = new BrowserWindow();
        windowManager.BrowserWindows.Returns(new[]{browserWindow});
        var dialogWrapper = Substitute.For<IDialogWrapper>();
        dialogWrapper.ShowOpenDialogAsync(Arg.Any<BrowserWindow>(), Arg.Any<OpenDialogOptions>())
            .Returns(Array.Empty<string>());
        
        var systemUnderTest = GetElectronDialogManagerForTest(windowManager, dialogWrapper);
        
        var exception = Assert.ThrowsAsync<OperationCanceledException>(async () => await systemUnderTest.ShowOpenDialogAsync("title"));
        Assert.That(exception!.Message, Is.EqualTo("Cancelled by user"));
    }


    private ElectronDialogManager GetElectronDialogManagerForTest(IWindowManagerWrapper? windowManager = null,
        IDialogWrapper? dialogWrapper = null)
    {
        windowManager ??= Substitute.For<IWindowManagerWrapper>();
        dialogWrapper ??= Substitute.For<IDialogWrapper>();
        
        return new ElectronDialogManager(windowManager, dialogWrapper);
    }
}