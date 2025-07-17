using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bunit;
using BusinessLogic.ErrorManagement.DataAccess;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Presentation.Components.ContentFiles;
using Presentation.Components.Dialogues;
using Presentation.Components.Forms.Content;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;

namespace IntegrationTest.Components.ContentFiles;

[TestFixture]
public class ContentFilesAddIt : MudBlazorTestFixture<ContentFilesAdd>
{
    [SetUp]
    public new void Setup()
    {
        _dialogService = Substitute.For<IDialogService>();
        _presentationLogic = Substitute.For<IPresentationLogic>();
        _errorService = Substitute.For<IErrorService>();
        _h5PPlayerPluginManager = Substitute.For<IH5PPlayerPluginManager>();
        
        Context.Services.AddSingleton(_dialogService);
        Context.Services.AddSingleton(_presentationLogic);
        Context.Services.AddSingleton(_errorService);
        Context.Services.AddSingleton(_h5PPlayerPluginManager);
        Context.ComponentFactories.AddStub<AddLinkForm>();
        Context.RenderComponent<MudPopoverProvider>();
    }

    private IDialogService _dialogService = null!;
    private IPresentationLogic _presentationLogic = null!;
    private IErrorService _errorService = null!;
    private IH5PPlayerPluginManager _h5PPlayerPluginManager = null!;

    [Test]
    public void OnInitialized_DependenciesInjected()
    {
        var systemUnderTest = GetRenderedComponent();

        Assert.That(systemUnderTest.Instance.DialogService, Is.EqualTo(_dialogService));
        Assert.That(systemUnderTest.Instance.PresentationLogic, Is.EqualTo(_presentationLogic));
        Assert.That(systemUnderTest.Instance.ErrorService, Is.EqualTo(_errorService));
        Assert.That(systemUnderTest.Instance.Logger, Is.Not.Null);
        Assert.That(systemUnderTest.Instance.Localizer, Is.Not.Null);
    }

    [Test]
    // ANF-ID: [AWA0036]
    public async Task OnFilesChanged_ProperFileUploaded_FileProcessed()
    {
        // Arrange
        var systemUnderTest = GetRenderedComponent();
        var browserFile = new MockBrowserFile("testFileName.txt");
        var fileUpload = systemUnderTest.FindComponent<MudFileUpload<IReadOnlyList<IBrowserFile>>>();

        // Act
        await systemUnderTest.InvokeAsync(() =>
            fileUpload.Instance.OnFilesChanged.InvokeAsync(new InputFileChangeEventArgs(new[] { browserFile })));

        // Assert
        await _presentationLogic.Received(1).LoadLearningContentViewModelAsync(Arg.Any<string>(), Arg.Any<Stream>());
    }

    [Test]
    // ANF-ID: [AWA0036]
    public async Task OnFilesChanged_DuplicateFileUploaded_FileNotProcessedAndDialog()
    {
        // Arrange
        var systemUnderTest = GetRenderedComponent();
        var browserFile = new MockBrowserFile("testFileName.txt");
        var fileUpload = systemUnderTest.FindComponent<MudFileUpload<IReadOnlyList<IBrowserFile>>>();
        _presentationLogic.LoadLearningContentViewModelAsync(Arg.Any<string>(), Arg.Any<Stream>())
            .Throws(new HashExistsException("duplicateFileName.txt", "/bogus/path/to/duplicateFileName.txt"));

        // Act
        await systemUnderTest.InvokeAsync(() =>
            fileUpload.Instance.OnFilesChanged.InvokeAsync(new InputFileChangeEventArgs(new[] { browserFile })));

        // Assert
        await _presentationLogic.Received(1).LoadLearningContentViewModelAsync(Arg.Any<string>(), Arg.Any<Stream>());
        await _dialogService.Received(1).ShowMessageBox("DialogService.MessageBox.Duplicate.Title",
            "DialogService.MessageBox.Duplicate.TexttestFileName.txt");
    }

    [Test]
    // ANF-ID: [AWA0036]
    public async Task OnFilesChanged_ErrorFileUploaded_FileNotProcessedAndCallsErrorService()
    {
        // Arrange
        var systemUnderTest = GetRenderedComponent();
        var browserFile = new MockBrowserFile("testFileName.txt");
        var fileUpload = systemUnderTest.FindComponent<MudFileUpload<IReadOnlyList<IBrowserFile>>>();
        _presentationLogic.LoadLearningContentViewModelAsync(Arg.Any<string>(), Arg.Any<Stream>())
            .Throws(new IOException("Some error"));

        // Act
        await systemUnderTest.InvokeAsync(() =>
            fileUpload.Instance.OnFilesChanged.InvokeAsync(new InputFileChangeEventArgs(new[] { browserFile })));

        // Assert
        await _presentationLogic.Received(1).LoadLearningContentViewModelAsync(Arg.Any<string>(), Arg.Any<Stream>());
        _errorService.Received(1).SetError("ContentFilesAdd.ErrorMessage.LoadingMaterial", "Some error");
    }

    [Test]
    // ANF-ID: [AWA0047]
    public async Task OnFilesChanged_ZipFileUploaded_WithNoEntries_ShowsDialog()
    {
        // Arrange
        var systemUnderTest = GetRenderedComponent();
        var browserFile = new MockZipBrowserFile("testFileName.zip");
        var fileUpload = systemUnderTest.FindComponent<MudFileUpload<IReadOnlyList<IBrowserFile>>>();

        // Act
        await systemUnderTest.InvokeAsync(() =>
            fileUpload.Instance.OnFilesChanged.InvokeAsync(new InputFileChangeEventArgs(new[] { browserFile })));

        // Assert
        await _presentationLogic.DidNotReceive()
            .LoadLearningContentViewModelAsync(Arg.Any<string>(), Arg.Any<Stream>());

        await _dialogService.Received(1).ShowMessageBox("ContentFilesAdd.EmptyFile.Title",
            "ContentFilesAdd.EmptyFile.Text");
    }

    [Test]
    // ANF-ID: [AWA0047]
    public async Task OnFilesChanged_ZipFileUploaded_WithEntries_ShowsDialog()
    {
        // Arrange
        var systemUnderTest = GetRenderedComponent();
        var browserFile = new MockZipBrowserFile("testFileName.zip", new List<string> { "testFileName.txt" },
            new List<string> { "duplicateFileName.txt" }, new List<string> { "unsupportedFileName" },
            new List<string> { "errorFileName.txt" });
        var fileUpload = systemUnderTest.FindComponent<MudFileUpload<IReadOnlyList<IBrowserFile>>>();
        _presentationLogic.When(x =>
                x.LoadLearningContentViewModelAsync(Arg.Is<string>(s => s.StartsWith("DUP_")), Arg.Any<Stream>()))
            .Throw(args => new HashExistsException(args.ArgAt<string>(0), "/bogus/path"));
        _presentationLogic.When(x =>
                x.LoadLearningContentViewModelAsync(Arg.Is<string>(s => s.StartsWith("ERR_")), Arg.Any<Stream>()))
            .Throw(args => new IOException(args.ArgAt<string>(0)));

        // Act
        await systemUnderTest.InvokeAsync(() =>
            fileUpload.Instance.OnFilesChanged.InvokeAsync(new InputFileChangeEventArgs(new[] { browserFile })));

        // Assert
        await _presentationLogic.Received(3)
            .LoadLearningContentViewModelAsync(Arg.Any<string>(), Arg.Any<Stream>());

        await _dialogService.Received(1).ShowAsync<ImportZipDialog>("DialogService.MessageBox.Import.Title", Arg.Is<DialogParameters>(p =>
            p.Get<string>(nameof(ImportZipDialog.FileName)) == "testFileName.zip" &&
            p.Get<List<string>>(nameof(ImportZipDialog.SuccessfulFiles))!.Count == 1 &&
            p.Get<List<string>>(nameof(ImportZipDialog.SuccessfulFiles))![0] == "NEW_testFileName.txt" &&
            p.Get<List<string>>(nameof(ImportZipDialog.DuplicateFiles))!.Count == 1 &&
            p.Get<List<string>>(nameof(ImportZipDialog.DuplicateFiles))![0] == "DUP_duplicateFileName.txt" &&
            p.Get<List<string>>(nameof(ImportZipDialog.UnsupportedFiles))!.Count == 1 &&
            p.Get<List<string>>(nameof(ImportZipDialog.UnsupportedFiles))![0] ==
            "unsupportedFileName" + ".unsupported" &&
            p.Get<List<string>>(nameof(ImportZipDialog.ErrorFiles))!.Count == 1 &&
            p.Get<List<string>>(nameof(ImportZipDialog.ErrorFiles))![0] == "ERR_errorFileName.txt"
        ), Arg.Any<DialogOptions>());
    }


    private class MockBrowserFile : IBrowserFile
    {
        public MockBrowserFile(string fileName)
        {
            Name = fileName;
            ContentType = "application/text";
        }

        DateTimeOffset IBrowserFile.LastModified => DateTimeOffset.Now;

        public long Size => 1024;
        public string ContentType { get; }

        public string Name { get; }

        public Stream OpenReadStream(long maxAllowedSize = 134217728, CancellationToken cancellationToken = default)
        {
            var stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write("This is a dummy file content");
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }

    private class MockZipBrowserFile : IBrowserFile
    {
        private readonly List<string> _duplicateEntries;
        private readonly List<string> _errorEntries;

        private readonly List<string> _newEntries;
        private readonly List<string> _unsupportedEntries;

        public MockZipBrowserFile(string fileName, List<string>? newEntries = null,
            List<string>? duplicateEntries = null,
            List<string>? unsupportedEntries = null, List<string>? errorEntries = null)
        {
            Name = fileName;
            ContentType = "application/zip";
            _newEntries = newEntries ?? new List<string>();
            _duplicateEntries = duplicateEntries ?? new List<string>();
            _unsupportedEntries = unsupportedEntries ?? new List<string>();
            _errorEntries = errorEntries ?? new List<string>();
        }

        DateTimeOffset IBrowserFile.LastModified => DateTimeOffset.Now;

        public long Size => 1024;
        public string ContentType { get; }

        public string Name { get; }

        public Stream OpenReadStream(long maxAllowedSize = 134217728, CancellationToken cancellationToken = default)
        {
            var stream = new MemoryStream();
            using (var archive =
                   new ZipArchive(stream, ZipArchiveMode.Create, true))
            {
                foreach (var demoFile in _newEntries.Select(newEntry => archive.CreateEntry("NEW_" + newEntry)))
                {
                    using var entryStream = demoFile.Open();
                    using var streamWriter = new StreamWriter(entryStream);
                    streamWriter.Write("This is a dummy file content inside zip file");
                }

                foreach (var demoFile in _duplicateEntries.Select(duplicateEntry =>
                             archive.CreateEntry("DUP_" + duplicateEntry)))
                {
                    using var entryStream = demoFile.Open();
                    using var streamWriter = new StreamWriter(entryStream);
                    streamWriter.Write("This is a dummy file content inside zip file");
                }

                foreach (var demoFile in _unsupportedEntries.Select(unsupportedEntry =>
                             archive.CreateEntry(unsupportedEntry + ".unsupported")))
                {
                    using var entryStream = demoFile.Open();
                    using var streamWriter = new StreamWriter(entryStream);
                    streamWriter.Write("This is a dummy file content inside zip file");
                }

                foreach (var demoFile in _errorEntries.Select(errorEntry => archive.CreateEntry("ERR_" + errorEntry)))
                {
                    using var entryStream = demoFile.Open();
                    using var streamWriter = new StreamWriter(entryStream);
                    streamWriter.Write("This is a dummy file content inside zip file");
                }
            }

            stream.Position = 0;
            return stream;
        }
    }

    private IRenderedComponent<ContentFilesAdd> GetRenderedComponent()
    {
        return Context.RenderComponent<ContentFilesAdd>();
    }
}