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
        dialogService.Received().ShowMessageBox("title",  Arg.Is<MarkupString>(x => x.Value == "message"));
    }
}