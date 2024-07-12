using System;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.AuthoringToolWorkspace;

namespace PresentationTest.PresentationLogic.AuthoringToolWorkspace;

[TestFixture]
public class ErrorServiceUt
{
    [Test]
    public void SetError_CallsDialogManager()
    {
        var dialogService = Substitute.For<IDialogService>();
        var systemUnderTest = new ErrorService(dialogService);

        systemUnderTest.SetError("title", "message");
        dialogService.Received().ShowMessageBox("title", Arg.Is<MarkupString>(x => x.Value == "message"));
    }

    [Test]
    public void SetError_WithException_CallsDialogManager()
    {
        // Arrange
        var dialogService = Substitute.For<IDialogService>();
        var systemUnderTest = new ErrorService(dialogService);
        var exception = new Exception("Outer exception", new Exception("Inner exception"));
        var expectedMessage = "Outer exception<br/><br/>Inner exception:<br/>Inner exception<br/><br/>";

        // Act
        systemUnderTest.SetError(exception);

        // Assert
        dialogService.Received().ShowMessageBox(
            "An error has occurred",
            Arg.Is<MarkupString>(x =>
                x.Value.Replace("\r", "").Replace("\n", "") == expectedMessage.Replace("\r", "").Replace("\n", ""))
        );
    }
}