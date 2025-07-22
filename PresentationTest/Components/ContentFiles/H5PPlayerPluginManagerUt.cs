using System;
using System.Threading.Tasks;
using H5pPlayer.Api;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.ContentFiles;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.LearningContent.FileContent;
using Shared.H5P;

namespace PresentationTest.Components.ContentFiles;

[TestFixture]
public class H5PPlayerPluginManagerTests
{
  

    [Test]
    public async Task StartH5pPlayerToValidateAsync_WithH5pFile_ShouldCallDialogAndSetStates()
    {
        var fileContentVm = Substitute.For<IFileContentViewModel>();
        fileContentVm.Name.Returns("test.h5p");
        var parseTo = CreateParseH5PFileTo(fileContentVm: fileContentVm);

        await _systemUnderTest.StartH5pPlayerToValidateAsync(parseTo);

        fileContentVm.Received(1).IsH5P = true;
        fileContentVm.Received(1).H5PState = H5PContentState.Completable;
        _presentationLogic.Received(1).EditH5PFileContent(fileContentVm);
        await _dialogService.Received(1)
            .ShowAsync<PlayerH5p>(
                Arg.Any<string>(),
                Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>());
    }

    [Test]
    public async Task StartH5pPlayerToValidateAsync_WithNonH5pFile_ShouldNotCallDialog()
    {
        var fileContentVm = Substitute.For<IFileContentViewModel>();
        fileContentVm.Name.Returns("test.pdf");
        var parseTo = CreateParseH5PFileTo(fileContentVm: fileContentVm);

        await _systemUnderTest.StartH5pPlayerToValidateAsync(parseTo);

        await _dialogService.DidNotReceive()
            .ShowAsync<PlayerH5p>(Arg.Any<string>(), Arg.Any<DialogParameters>(), Arg.Any<DialogOptions>());
    }
    
    
    
    private ILogger<H5PPlayerPluginManager> _logger;
    private IJSRuntime _jsRuntime;
    private IDialogService _dialogService;
    private IPresentationLogic _presentationLogic;
    private IDialogReference _dialogReference;
    private H5PPlayerPluginManager _systemUnderTest;

    [SetUp]
    public void Setup()
    {
        _logger = Substitute.For<ILogger<H5PPlayerPluginManager>>();
        _jsRuntime = Substitute.For<IJSRuntime>();
        _dialogService = Substitute.For<IDialogService>();
        _presentationLogic = Substitute.For<IPresentationLogic>();
        _dialogReference = Substitute.For<IDialogReference>();

        ConfigureDialogServiceFake();

        _systemUnderTest = new H5PPlayerPluginManager(_logger, _jsRuntime, _dialogService, _presentationLogic);
    }

    private void ConfigureDialogServiceFake()
    {
        // DialogService simulates Callback with "Completable"
        _dialogService
            .When(service => service.ShowAsync<PlayerH5p>(
                Arg.Any<string>(),
                Arg.Any<DialogParameters>(),
                Arg.Any<DialogOptions>()))
            .Do(callInfo =>
            {
                var parameters = callInfo.ArgAt<DialogParameters>(1);
                var callback = (Action<H5pPlayerResultTO>)parameters["OnPlayerFinished"];
                callback.Invoke(new H5pPlayerResultTO("Completable"));
            });

        _dialogService
            .ShowAsync<PlayerH5p>(Arg.Any<string>(), Arg.Any<DialogParameters>(), Arg.Any<DialogOptions>())
            .Returns(Task.FromResult(_dialogReference));
    }
    
    private static StartH5PPlayerTO CreateParseH5PFileTo(IFileContentViewModel? fileContentVm = null)
    {
        return new StartH5PPlayerTO
        {
            NavigationManager = new FakeNavigationManager(),
            FileContentVm = fileContentVm ?? Substitute.For<IFileContentViewModel>()
        };
    }
    
    /// <summary>
    /// Substitute<NavigationManager> drops null exception -> inner state needed.
    /// dont need this state during tests!
    /// -> created own Fake:
    /// </summary>
    private class FakeNavigationManager : NavigationManager
    {
        public FakeNavigationManager()
        {
            Initialize("http://localhost/", "http://localhost/test");
        }

        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            // no-op
        }
        
    }
}
