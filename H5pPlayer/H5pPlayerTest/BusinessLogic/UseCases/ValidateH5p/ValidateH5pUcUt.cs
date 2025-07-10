using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;
using NSubstitute;

namespace H5pPlayerTest.BusinessLogic.UseCases.ValidateH5p;

[TestFixture]
public class ValidateH5pUcUt
{
    private string _basePath;

    [SetUp]
    public void SetUp()
    {
        _basePath = OperatingSystem.IsWindows() ? "C:" : "/";
    }

    /// <summary>
    /// this constructor unit-test is explicitly needed,
    /// because we do not unit-test the real interaction between
    /// the <see cref="ValidateH5pUc"/> and the <see cref="ReceiveFromJavaScriptAdapter"/>
    ///
    /// with this test we ensure the possibility to interact
    /// from <see cref="ReceiveFromJavaScriptAdapter"/>
    /// to the CORRECT INSTANCE of <see cref="ValidateH5pUc"/>
    /// </summary>
    [Test]
    // ANF-ID: [HSE3]
    public void EnsureBackCallOpportunityOfJsAdapterToCorrectInstanceOfValidateUc()
    {
        var systemUnderTest = CreateValidateH5PUc();

        Assert.That(ReceiveFromJavaScriptAdapter.ValidateH5pUc, Is.EqualTo(systemUnderTest));
    }

    [Test]
    // ANF-ID: [HSE3]
    public async Task StartToValidateH5p()
    {
        var mockJavaScriptAdapter = Substitute.For<ICallJavaScriptAdapter>();
        var systemUnderTest = CreateValidateH5PUc(null, mockJavaScriptAdapter);
        var unzippedH5psPath = Path.Combine(_basePath, "ValidPath1.h5p");
        var h5pZipSourcePath = @Path.Combine(_basePath, "ValidPath2.h5p");
        var h5pEntity = CreateH5pEntity(unzippedH5psPath, h5pZipSourcePath);
        var javaScriptAdapterTO = CreateJavaScriptAdapterTO(unzippedH5psPath, h5pZipSourcePath);

        await systemUnderTest.StartToValidateH5p(h5pEntity);

        await mockJavaScriptAdapter.Received().ValidateH5p(Arg.Is(javaScriptAdapterTO));
        await mockJavaScriptAdapter.Received().ValidateH5p(Arg.Any<CallJavaScriptAdapterTO>());
    }


    [Test]
    // ANF-ID: [HSE4]
    public void SetH5pIsCompletable()
    {
        var mockValidateH5pUcOutputPort = Substitute.For<IValidateH5pUcOutputPort>();
        var systemUnderTest = CreateValidateH5PUc(mockValidateH5pUcOutputPort);
        var validateH5pTO = new ValidateH5pTO(true);

        systemUnderTest.ValidateH5p(validateH5pTO);

        mockValidateH5pUcOutputPort.Received().SetH5pIsCompletable();
    }

    [Test]
    // ANF-ID: [HSE4]
    public void H5pIsNotCompletedAlready()
    {
        var mockValidateH5pUcOutputPort = Substitute.For<IValidateH5pUcOutputPort>();
        var systemUnderTest = CreateValidateH5PUc(mockValidateH5pUcOutputPort);
        var validateH5pTO = new ValidateH5pTO(false);

        systemUnderTest.ValidateH5p(validateH5pTO);

        mockValidateH5pUcOutputPort.DidNotReceive().SetH5pIsCompletable();
    }

    
    [Test]
    public async Task StartToValidateH5p_SetsH5pEntity()
    {
        var systemUnderTest = CreateValidateH5PUc();
        var unzippedH5psPath = Path.Combine(_basePath, "ValidPath1.h5p");
        var h5pZipSourcePath = @Path.Combine(_basePath, "ValidPath2.h5p");
        var h5pEntity = CreateH5pEntity(unzippedH5psPath, h5pZipSourcePath);
        
        await systemUnderTest.StartToValidateH5p(h5pEntity);


        Assert.That(systemUnderTest.H5pEntity, Is.EqualTo(h5pEntity));
    }

    [Test]
    public void SetActiveH5pStateToNotUsable()
    {
        var mockValidateH5pUcOutputPort = Substitute.For<IValidateH5pUcOutputPort>();
        var systemUnderTest = CreateValidateH5PUc(mockValidateH5pUcOutputPort);
        var unzippedH5psPath = Path.Combine(_basePath, "ValidPath1.h5p");
        var h5pZipSourcePath = @Path.Combine(_basePath, "ValidPath2.h5p");
        var h5pEntity = CreateH5pEntity(unzippedH5psPath, h5pZipSourcePath);
        systemUnderTest.H5pEntity = h5pEntity;
        
        systemUnderTest.SetActiveH5pStateToNotUsable();
        
        Assert.That(systemUnderTest.H5pEntity.ActiveH5pState, Is.EqualTo(H5pState.NotUsable));
        mockValidateH5pUcOutputPort.Received().SetH5pActiveStateToNotUsable();
    }
    
    [Test]
    public void SetActiveH5pStateToPrimitive()
    {
        var mockValidateH5pUcOutputPort = Substitute.For<IValidateH5pUcOutputPort>();
        var systemUnderTest = CreateValidateH5PUc(mockValidateH5pUcOutputPort);
        var unzippedH5psPath = Path.Combine(_basePath, "ValidPath1.h5p");
        var h5pZipSourcePath = @Path.Combine(_basePath, "ValidPath2.h5p");
        var h5pEntity = CreateH5pEntity(unzippedH5psPath, h5pZipSourcePath);
        systemUnderTest.H5pEntity = h5pEntity;
        
        systemUnderTest.SetActiveH5pStateToPrimitive();
        
        Assert.That(systemUnderTest.H5pEntity.ActiveH5pState, Is.EqualTo(H5pState.Primitive)); 
        mockValidateH5pUcOutputPort.Received().SetH5pActiveStateToPrimitive();
    }
    

    [Test]
    public void SetActiveH5pStateToCompletable()
    {
        var mockValidateH5pUcOutputPort = Substitute.For<IValidateH5pUcOutputPort>();
        var systemUnderTest = CreateValidateH5PUc(mockValidateH5pUcOutputPort);
        var unzippedH5psPath = Path.Combine(_basePath, "ValidPath1.h5p");
        var h5pZipSourcePath = @Path.Combine(_basePath, "ValidPath2.h5p");
        var h5pEntity = CreateH5pEntity(unzippedH5psPath, h5pZipSourcePath);
        systemUnderTest.H5pEntity = h5pEntity;
        
        systemUnderTest.SetActiveH5pStateToCompletable();
        
        Assert.That(systemUnderTest.H5pEntity.ActiveH5pState, Is.EqualTo(H5pState.Completable)); 
        mockValidateH5pUcOutputPort.Received().SetH5pActiveStateToCompletable();
    }




    private static ValidateH5pUc CreateValidateH5PUc(
        IValidateH5pUcOutputPort? fakeValidateH5PUcOutputPort = null,
        ICallJavaScriptAdapter? fakeJavaScriptAdapter = null,
        ITerminateH5pPlayerUcPort? fakeTerminateH5pPlayerUc = null)
    {
        fakeValidateH5PUcOutputPort ??= Substitute.For<IValidateH5pUcOutputPort>();
        fakeJavaScriptAdapter ??= Substitute.For<ICallJavaScriptAdapter>();
        fakeTerminateH5pPlayerUc ??= Substitute.For<ITerminateH5pPlayerUcPort>();
        return new ValidateH5pUc(
            fakeValidateH5PUcOutputPort, 
            fakeJavaScriptAdapter,
            fakeTerminateH5pPlayerUc);
    }


    private static CallJavaScriptAdapterTO CreateJavaScriptAdapterTO(string unzippedH5psPath, string h5pZipSourcePath)
    {
        var javaScriptAdapterTO = new CallJavaScriptAdapterTO(unzippedH5psPath, h5pZipSourcePath);
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