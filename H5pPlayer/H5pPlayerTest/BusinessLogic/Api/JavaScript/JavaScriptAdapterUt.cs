using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.Domain;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using NSubstitute;

namespace H5pPlayerTest.BusinessLogic.Api.JavaScript;

[TestFixture]
public class JavaScriptAdapterUt
{
    //test fall hone https nicht vergessen !!!!!!!!!!!!!!!!!!!!!!!!
    
    
    
    [TestCase(@"C:\Users\TestUserName\AppData\Roaming\AdLerAuthoring\ContentFiles\Accordion_Test.h5p",
        "https://localhost:8001/H5pStandalone/h5p-folder")]
    [TestCase(@"C:\Users\TestUserName\AppData\Roaming\AdLerAuthoring\ContentFiles\Accordion_Test.h5p",
        "//localhost:8001/H5pStandalone/h5p-folder")]
    public async Task DisplayH5p(string h5pSourcePath, string unzippedH5psPath)
    {
        var h5pEntity = CreateH5pEntity(h5pSourcePath, unzippedH5psPath);
        var fakeJsInterop = Substitute.For<IJSRuntime>();
        var systemUnderTest = CreateJavaScriptAdapter(fakeJsInterop);
        
        await systemUnderTest.DisplayH5p(h5pEntity);
        
        await fakeJsInterop.Received(1).InvokeAsync<IJSVoidResult>(
            "testH5P", 
            Arg.Is<object[]>(
                args => 
                args.Length == 1 && 
                (string)args[0] == "//localhost:8001/H5pStandalone/h5p-folder/Accordion_Test"));
        // following is not possible:
        //await fakeJsInterop.Received().InvokeVoidAsync("testH5P", "//localhost:8001/H5pStandalone/h5p-folder/AbfrageDefinitionen");
    }
 
 
    // just because the Entity is injected instead of an TO
    [TestCase(@"C:\Users\TestUserName\AppData\Roaming\AdLerAuthoring\ContentFiles\Accordion_Test.h5p",
        "https://localhost:8001/H5pStandalone/h5p-folder")]
    [TestCase(@"C:\Users\TestUserName\AppData\Roaming\AdLerAuthoring\ContentFiles\Accordion_Test.h5p",
        "//localhost:8001/H5pStandalone/h5p-folder")]
    public async Task DisplayH5p_NotModifyH5pEntityH5pJsonSourcePath(string h5pSourcePath, string unzippedH5psPath)
    {
        var h5pEntity = CreateH5pEntity(h5pSourcePath, unzippedH5psPath);
        var systemUnderTest = CreateJavaScriptAdapter();

        await systemUnderTest.DisplayH5p(h5pEntity);

        Assert.That(h5pEntity.H5pZipSourcePath, Is.EqualTo(h5pSourcePath));
        Assert.That(h5pEntity.UnzippedH5psPath, Is.EqualTo(unzippedH5psPath));
    }

    private static H5pEntity CreateH5pEntity(string h5pSourcePath, string unzippedH5psPath)
    {
        var h5pEntity = new H5pEntity();
        h5pEntity.H5pZipSourcePath = h5pSourcePath;
        h5pEntity.UnzippedH5psPath = unzippedH5psPath;
        return h5pEntity;
    }
    
    
    private static JavaScriptAdapter CreateJavaScriptAdapter(IJSRuntime? fakeJsInterop = null)
    {
        fakeJsInterop = fakeJsInterop ?? Substitute.For<IJSRuntime>();
        var systemUnderTest = new JavaScriptAdapter(fakeJsInterop);
        return systemUnderTest;
    }
}