using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;
using NSubstitute;

namespace H5pPlayerTest.BusinessLogic.UseCases.ValidateH5p;

[TestFixture]
public class ValidateH5pUcUt
{
    [Test]
    public async Task  ValidateH5p()
    {
        var mockJavaScriptAdapter = Substitute.For<IJavaScriptAdapter>();
        var systemUnderTest = new ValidateH5pUc(mockJavaScriptAdapter);
        var unzippedH5psPath = @"C:\ValidPath1.h5p";
        var h5pZipSourcePath = @"C:\ValidPath2.h5p";
        var h5pEntity = CreateH5pEntity(unzippedH5psPath, h5pZipSourcePath);

        await systemUnderTest.StartToValidateH5p(h5pEntity);

        var javaScriptAdapterTO = CreateJavaScriptAdapterTO(unzippedH5psPath, h5pZipSourcePath);
        await mockJavaScriptAdapter.Received().ValidateH5p(Arg.Is(javaScriptAdapterTO));
        await mockJavaScriptAdapter.Received().ValidateH5p(Arg.Any<JavaScriptAdapterTO>());
    }

    private static JavaScriptAdapterTO CreateJavaScriptAdapterTO(string unzippedH5psPath, string h5pZipSourcePath)
    {
        var javaScriptAdapterTO = new JavaScriptAdapterTO(unzippedH5psPath, h5pZipSourcePath);
        return javaScriptAdapterTO;
    }
    
    
    private static H5pEntity CreateH5pEntity(
        string unzippedH5psPath, string h5pZipSourcePath)
    {
        var h5pEntity = new H5pEntity();
        h5pEntity.UnzippedH5psPath = unzippedH5psPath;
        h5pEntity.H5pZipSourcePath = h5pZipSourcePath;
        return h5pEntity;
    }
}