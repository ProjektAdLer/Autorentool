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
        var h5pEntity = new H5pEntity();
        
        await systemUnderTest.StartToDisplayH5pUC(h5pEntity);

        await mockJavaScriptAdapter.Received().DisplayH5p(h5pEntity);
    }
}