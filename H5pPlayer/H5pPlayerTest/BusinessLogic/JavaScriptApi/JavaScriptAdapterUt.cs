using H5pPlayer.BusinessLogic.Domain;
using H5pPlayer.BusinessLogic.JavaScriptApi;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using NSubstitute;

namespace H5pPlayerTest.BusinessLogic.JavaScriptApi;

[TestFixture]
public class JavaScriptAdapterUt
{
    [TestCase("https://localhost:8001/H5pStandalone/h5p-folder/AbfrageDefinitionen")]
    [TestCase("//localhost:8001/H5pStandalone/h5p-folder/AbfrageDefinitionen")]
    public async Task DisplayH5p(string h5pJsonPath)
    {
        var h5pEntity = CreateH5pEntity(h5pJsonPath);
        var fakeJsInterop = Substitute.For<IJSRuntime>();
        var systemUnderTest = CreateJavaScriptAdapter(fakeJsInterop);
        
        await systemUnderTest.DisplayH5p(h5pEntity);
        
        await fakeJsInterop.Received(1).InvokeAsync<IJSVoidResult>(
            "testH5P", 
            Arg.Is<object[]>(
                args => 
                args.Length == 1 && 
                (string)args[0] == "//localhost:8001/H5pStandalone/h5p-folder/AbfrageDefinitionen"));
        // following is not possible:
        //await fakeJsInterop.Received().InvokeVoidAsync("testH5P", "//localhost:8001/H5pStandalone/h5p-folder/AbfrageDefinitionen");
    }
 

    [Test]
    public async Task DisplayH5p_NotModifyH5pEntityH5pJsonSourcePath()
    {
        var h5pJsonPath = "https://localhost:8001/H5pStandalone/h5p-folder/AbfrageDefinitionen";
        var h5pEntity = CreateH5pEntity(h5pJsonPath);
        var fakeJsInterop = Substitute.For<IJSRuntime>();
        var systemUnderTest = CreateJavaScriptAdapter(fakeJsInterop);

        await systemUnderTest.DisplayH5p(h5pEntity);

        Assert.That(h5pEntity.H5pJsonSourcePath, Is.EqualTo(h5pJsonPath));
    }

    private static H5pEntity CreateH5pEntity(string h5pJsonPath)
    {
        var h5pEntity = new H5pEntity();
        h5pEntity.H5pJsonSourcePath = h5pJsonPath;
        return h5pEntity;
    }
    
    
    private static JavaScriptAdapter CreateJavaScriptAdapter(IJSRuntime? fakeJsInterop = null)
    {
        fakeJsInterop = fakeJsInterop ?? Substitute.For<IJSRuntime>();
        var systemUnderTest = new JavaScriptAdapter(fakeJsInterop);
        return systemUnderTest;
    }
}