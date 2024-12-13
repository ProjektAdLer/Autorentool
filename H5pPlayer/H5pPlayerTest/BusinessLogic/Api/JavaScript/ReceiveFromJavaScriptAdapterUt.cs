using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using NSubstitute;

namespace H5pPlayerTest.BusinessLogic.Api.JavaScript;

[TestFixture]
public class ReceiveFromJavaScriptAdapterUt
{
    
    
    // [Test]
    // public async Task ReceiveJsonData()
    // {
    //     var jsonData = "";
    //     var mockInputPort = Substitute.For<IValidateH5pUc>();
    //     var validationCompletedTO = new ValidationCompletedTO(true);
    //     var systemUnderTest = CreateJavaScriptAdapter();
    //     
    //      JavaScriptReceiveAdapter.ReceiveJsonData(jsonData);
    //      JavaScriptReceiveAdapter.ReceiveJsonData(jsonData);
    //
    // }

    
   


    private static ReceiveFromJavaScriptAdapter CreateReceiveFromJavaScriptAdapter(
        IJSRuntime? fakeJsInterop = null,
        IValidateH5pUc fakeValidateH5pUc = null)
    {
        var systemUnderTest = new ReceiveFromJavaScriptAdapter();
        return systemUnderTest;
    }
}

