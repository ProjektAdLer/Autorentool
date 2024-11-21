using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using NSubstitute;

namespace H5pPlayerTest.BusinessLogic.Api.JavaScript;

[TestFixture]
public class JavaScriptAdapterUt
{
    [TestCase(@"C:\Users\TestUserName\AppData\Roaming\AdLerAuthoring\ContentFiles\Accordion_Test.h5p",
        "https://localhost:8001/H5pStandalone/h5p-folder/")]
    [TestCase(@"C:\Users\TestUserName\AppData\Roaming\AdLerAuthoring\ContentFiles\Accordion_Test.h5p",
        "//localhost:8001/H5pStandalone/h5p-folder/")]
    public async Task DisplayH5p(string h5pSourcePath, string unzippedH5psPath)
    {
        var javaScriptAdapterTO = CreateJavaScriptAdapterTO(unzippedH5psPath, h5pSourcePath);
        var fakeJsInterop = Substitute.For<IJSRuntime>();
        var systemUnderTest = CreateJavaScriptAdapter(fakeJsInterop);

        await systemUnderTest.DisplayH5p(javaScriptAdapterTO);

        await fakeJsInterop.Received(1).InvokeAsync<IJSVoidResult>(
            "displayH5p",
            Arg.Is<object[]>(
                args =>
                    args.Length == 1 &&
                    (string)args[0] == "//localhost:8001/H5pStandalone/h5p-folder/Accordion_Test"));
        // following is not possible:
        //await fakeJsInterop.Received().InvokeVoidAsync("testH5P", "//localhost:8001/H5pStandalone/h5p-folder/AbfrageDefinitionen");
    }


    // just because the Entity is injected instead of an TO
    [TestCase(@"C:\Users\TestUserName\AppData\Roaming\AdLerAuthoring\ContentFiles\Accordion_Test.h5p",
        "https://localhost:8001/H5pStandalone/h5p-folder/")]
    [TestCase(@"C:\Users\TestUserName\AppData\Roaming\AdLerAuthoring\ContentFiles\Accordion_Test.h5p",
        "//localhost:8001/H5pStandalone/h5p-folder/")]
    public async Task DisplayH5p_NotModifyH5pEntityH5pJsonSourcePath(string h5pSourcePath, string unzippedH5psPath)
    {
        var javaScriptAdapterTO = CreateJavaScriptAdapterTO(unzippedH5psPath, h5pSourcePath);

        var systemUnderTest = CreateJavaScriptAdapter();

        await systemUnderTest.DisplayH5p(javaScriptAdapterTO);

        Assert.That(javaScriptAdapterTO.H5pZipSourcePath, Is.EqualTo(h5pSourcePath));
        Assert.That(javaScriptAdapterTO.UnzippedH5psPath, Is.EqualTo(unzippedH5psPath));
    }


    [Test]
    public async Task TerminateH5pJavaScriptPlayer()
    {
        var mockJsRuntime = Substitute.For<IJSRuntime>();
        var systemUnderTest = CreateJavaScriptAdapter(mockJsRuntime);

        await systemUnderTest.TerminateH5pJavaScriptPlayer();

        await mockJsRuntime.Received().InvokeVoidAsync("terminateH5pStandalone");
    }

    
    private static JavaScriptAdapterTO CreateJavaScriptAdapterTO(string unzippedH5psPath, string h5pZipSourcePath)
    {
        var javaScriptAdapterTO = new JavaScriptAdapterTO(unzippedH5psPath, h5pZipSourcePath);
        return javaScriptAdapterTO;
    }


    private static JavaScriptAdapter CreateJavaScriptAdapter(IJSRuntime? fakeJsInterop = null)
    {
        fakeJsInterop = fakeJsInterop ?? Substitute.For<IJSRuntime>();
        var systemUnderTest = new JavaScriptAdapter(fakeJsInterop);
        return systemUnderTest;
    }
}