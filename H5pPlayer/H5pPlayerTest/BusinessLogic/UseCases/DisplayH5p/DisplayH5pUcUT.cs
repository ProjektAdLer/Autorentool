using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using NSubstitute;

namespace H5pPlayerTest.BusinessLogic.UseCases.DisplayH5p;

[TestFixture]
public class DisplayH5pUcUT
{
    [Test]
    public async Task StartToDisplayH5pUC_CallJavaScriptAdapter()
    {
        var mockJavaScriptAdapter = Substitute.For<IJavaScriptAdapter>();
        var systemUnderTest = new DisplayH5pUC(mockJavaScriptAdapter);
        var unzippedH5psPath = @"C:\ValidPath1.h5p";
        var h5pZipSourcePath = @"C:\ValidPath2.h5p";
        var h5pEntity = CreateH5pEntity(unzippedH5psPath, h5pZipSourcePath);
        
        await systemUnderTest.StartToDisplayH5pUC(h5pEntity);

        var javaScriptAdapterTO = CreateJavaScriptAdapterTO(unzippedH5psPath, h5pZipSourcePath);
        await mockJavaScriptAdapter.Received().DisplayH5p(Arg.Is(javaScriptAdapterTO));
        await mockJavaScriptAdapter.Received().DisplayH5p(Arg.Any<JavaScriptAdapterTO>());
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