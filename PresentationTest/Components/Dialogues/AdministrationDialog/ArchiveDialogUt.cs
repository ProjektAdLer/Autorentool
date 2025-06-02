using System;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MudBlazor;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Presentation.Components.Dialogues.AdministrationDialog;
using Presentation.PresentationLogic.API;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.SelectedViewModels;
using Shared;
using Shared.Exceptions;
using TestHelpers;
using TestContext = Bunit.TestContext;

namespace PresentationTest.Components.Dialogues.AdministrationDialog;

[TestFixture]
public class ArchiveDialogUt
{
    [SetUp]
    public void SetUp()
    {
        _ctx = new TestContext();
        _ctx.ComponentFactories.AddStub<MudPopover>();
        _presentationLogic = Substitute.For<IPresentationLogic>();
        _localizer = Substitute.For<IStringLocalizer<ArchiveDialog>>();
        _logger = Substitute.For<ILogger<ArchiveDialog>>();
        _selectedViewModelsProvider = Substitute.For<ISelectedViewModelsProvider>();
        _errorService = Substitute.For<IErrorService>();
        _snackbar = Substitute.For<ISnackbar>();


        _ctx.Services.AddSingleton(_presentationLogic);
        _ctx.Services.AddSingleton(_localizer);
        _ctx.Services.AddSingleton(_logger);
        _ctx.Services.AddSingleton(_selectedViewModelsProvider);
        _ctx.Services.AddSingleton(_errorService);
        _ctx.Services.AddSingleton(_snackbar);
    }

    [TearDown]
    public void TearDown()
    {
        _snackbar.Dispose();
        _ctx.Dispose();
    }

    private TestContext _ctx = null!;
    private IPresentationLogic _presentationLogic = null!;
    private IStringLocalizer<ArchiveDialog> _localizer = null!;
    private ILogger<ArchiveDialog> _logger = null!;
    private ISelectedViewModelsProvider _selectedViewModelsProvider = null!;
    private IErrorService _errorService = null!;
    private ISnackbar _snackbar = null!;

    [Test]
    public void RenderArchiveDialog_AllParametersSet()
    {
        var systemUnderTest = _ctx.RenderComponent<ArchiveDialog>();

        Assert.That(systemUnderTest.Instance, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(systemUnderTest.Instance.PresentationLogic, Is.EqualTo(_presentationLogic));
            Assert.That(systemUnderTest.Instance.ErrorService, Is.EqualTo(_errorService));
        }
    }

    [Test]
    public void ClickExport_LearningWorldIsNull_ButtonIsDisabled()
    {
        _selectedViewModelsProvider.LearningWorld.Returns((ILearningWorldViewModel?)null);
        var systemUnderTest = _ctx.RenderComponent<ArchiveDialog>();


        var exportButton = systemUnderTest.FindComponent<MudButton>();
        Assert.That(exportButton.Instance.Disabled, Is.True);
    }

    [Test]
    public void ClickExport_LearningWorldIsNotNull_CallsExportToZipArchive()
    {
        var world = ViewModelProvider.GetLearningWorld();
        _presentationLogic.ValidateLearningWorldForExport(world).Returns(new ValidationResult());
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        
        var systemUnderTest = _ctx.RenderComponent<ArchiveDialog>();
        var exportButtons = systemUnderTest.FindAll("button");
        
        exportButtons[0].Click();
        _presentationLogic.Received().ExportLearningWorldToZipArchiveAsync(world);
    }
    
    [Test]
    public void ClickExport_OperationCanceled_Throws()
    {
        var world = ViewModelProvider.GetLearningWorld();
        _presentationLogic.ValidateLearningWorldForExport(world).Returns(new ValidationResult());
        _presentationLogic.ExportLearningWorldToZipArchiveAsync(world).Throws<OperationCanceledException>();
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        
        var systemUnderTest = _ctx.RenderComponent<ArchiveDialog>();
        var exportButtons = systemUnderTest.FindAll("button");
        
        exportButtons[0].Click();
        _snackbar.Received().Add(Arg.Any<string>(), Severity.Warning);
    }
    
    [Test]
    public void ClickExport_Zip_LearningWorldIsNotValid_CallsErrorService()
    {
        var world = ViewModelProvider.GetLearningWorld();
        _presentationLogic.ValidateLearningWorldForExport(world).Returns(new ValidationResult()
        {
            Errors = { "<li>Error 1</li>", "<li>Error 2</li>" }
        });
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        
        var systemUnderTest = _ctx.RenderComponent<ArchiveDialog>();
        var exportButtons = systemUnderTest.FindAll("button");
        
        exportButtons[0].Click();
        _presentationLogic.DidNotReceive().ExportLearningWorldToZipArchiveAsync(world);
        _errorService.Received().SetError(null!, "<li>Error 1</li><li>Error 2</li>");
    }
    
    [Test]
    public void ClickExport_LearningWorldIsNotNull_CallsExportToMoodleArchive()
    {
        var world = ViewModelProvider.GetLearningWorld();
        _presentationLogic.ValidateLearningWorldForExport(world).Returns(new ValidationResult());
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        
        var systemUnderTest = _ctx.RenderComponent<ArchiveDialog>();
        var exportButtons = systemUnderTest.FindAll("button");
        
        exportButtons[1].Click();
        _presentationLogic.Received().ExportLearningWorldToMoodleArchiveAsync(world);
    }
    
    [Test]
    public void ClickExport_Moodle_OperationCanceled_Throws()
    {
        var world = ViewModelProvider.GetLearningWorld();
        _presentationLogic.ValidateLearningWorldForExport(world).Returns(new ValidationResult());
        _presentationLogic.ExportLearningWorldToMoodleArchiveAsync(world).Throws<OperationCanceledException>();
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        
        var systemUnderTest = _ctx.RenderComponent<ArchiveDialog>();
        var exportButtons = systemUnderTest.FindAll("button");
        
        exportButtons[1].Click();
        _snackbar.Received().Add(Arg.Any<string>(), Severity.Warning);
    }
    
    [Test]
    public void ClickExport_Moodle_GeneratorException_Throws()
    {
        var world = ViewModelProvider.GetLearningWorld();
        _presentationLogic.ValidateLearningWorldForExport(world).Returns(new ValidationResult());
        _presentationLogic.ExportLearningWorldToMoodleArchiveAsync(world).Throws<GeneratorException>();
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        
        var systemUnderTest = _ctx.RenderComponent<ArchiveDialog>();
        var exportButtons = systemUnderTest.FindAll("button");
        
        exportButtons[1].Click();
        _errorService.Received().SetError("An Error has occured during creation of a Backup File", Arg.Any<string>());
    }
    
    [Test]
    public void ClickExport_Moodle_Exception_Throws()
    {
        var world = ViewModelProvider.GetLearningWorld();
        _presentationLogic.ValidateLearningWorldForExport(world).Returns(new ValidationResult());
        _presentationLogic.ExportLearningWorldToMoodleArchiveAsync(world).Throws<Exception>();
        _selectedViewModelsProvider.LearningWorld.Returns(world);
        
        var systemUnderTest = _ctx.RenderComponent<ArchiveDialog>();
        var exportButtons = systemUnderTest.FindAll("button");
        
        exportButtons[1].Click();
        _errorService.Received().SetError(Arg.Any<Exception>());
    }
}