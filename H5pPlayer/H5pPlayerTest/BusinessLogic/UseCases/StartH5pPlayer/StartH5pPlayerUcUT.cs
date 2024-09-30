using H5pPlayer.BusinessLogic.Domain;
using H5pPlayer.BusinessLogic.JavaScriptApi;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;
using NSubstitute;

namespace H5pPlayerTest.BusinessLogic.UseCases.StartH5pPlayer;

[TestFixture]
public class StartH5pPlayerUcUT
{

    
    
    
       
    [TestCase(@"C:\Temp")]
    [TestCase(@"Test\Temp\Tested")]
    public void MapTOtoEntity_ValidH5pZipSourcePath(string validPath)
    {
        var systemUnderTest = CreateStandardSystemUnderTest();
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(0,validPath);
        
        Assert.DoesNotThrow(
            () => systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO));
        Assert.That(systemUnderTest.H5pEntity.H5pZipSourcePath, Is.EqualTo(validPath));
    }
   
    [Test]
    public void MapTOtoEntity_NullH5pZipSourcePath()
    {
        string invalidPath = null;

        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(null, mockDisplayH5pOutputPort);
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(0,invalidPath);
        const string expectedErrorMessage = "H5P Zip Path was wrong!";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(invalidPath, expectedErrorMessage);

        systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);
     
        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
    }

  

    [TestCase("")]
    [TestCase("   ")]
    public void MapTOtoEntity_EmptyOrWhitespaceH5pZipSourcePath(string pathToCheck)
    {
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(null, mockDisplayH5pOutputPort);
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(0,pathToCheck);
        const string expectedErrorMessage = "H5P Zip Path was wrong!";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(pathToCheck, expectedErrorMessage);

        systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);

        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
        
    }

    [Test]
    public void MapTOtoEntity_InvalidH5pZipSourcePath([Range(0, 32)] int number)
    {
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(null, mockDisplayH5pOutputPort);
        var badChars = Path.GetInvalidPathChars();// 33 elements
        const string validPath = @"C:\Temp\Invalid";
        var invalidPath = validPath + badChars[number];
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(0,invalidPath);
        const string expectedErrorMessage = "H5P Zip Path was wrong!";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(invalidPath, expectedErrorMessage);

        systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);
       
        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
    }


    [Test]
    public void MapTOtoEntity_DisplayMode([Values]H5pDisplayMode displayMode)
    {
        var systemUnderTest = CreateStandardSystemUnderTest();
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(displayMode);
        
        systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);
        
        Assert.That(systemUnderTest.H5pEntity.ActiveDisplayMode, 
            Is.EqualTo(displayMode));
    }
    
    
    // hier weiter extract ziped H5p

    
    

    [Test]
    public void ExtractZippedH5p()
    {
        var systemUnderTest = CreateStandardSystemUnderTest();
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0();
        
        systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);
        
    }
    
    
    [Test]
    public void StartH5pPlayerToDisplayH5p()
    {
        var mockDisplayH5pUC = Substitute.For<IDisplayH5pUC>();
        var systemUnderTest = CreateStandardSystemUnderTest(mockDisplayH5pUC);
        const string h5pZipSourcePath = "C://PathToZip/Source";
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(0,h5pZipSourcePath);

        systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);
        
        mockDisplayH5pUC.Received().StartToDisplayH5pUC(Arg.Any<H5pEntity>());
    }
 
    
    
    
    
    private static StartH5pPlayerInputTO CreateStartH5pPlayerInputT0(
        H5pDisplayMode displayMode = H5pDisplayMode.Display,
        string h5pZipSourcePath = "C://Default_PathToZip/Source")
    {
        var transportObject = new StartH5pPlayerInputTO(displayMode, h5pZipSourcePath);
        return transportObject;
    }
    
    private static StartH5pPlayerErrorOutputTO CreateStartH5pPlayerErrorOutputTO(
        string invalidPath, 
        string expectedErrorMessage)
    {
        var expectedOutputTo = new StartH5pPlayerErrorOutputTO();
        expectedOutputTo.InvalidH5pZipSourcePath = invalidPath;
        expectedOutputTo.H5pZipSourcePathErrorText = expectedErrorMessage;
        return expectedOutputTo;
    }

    private static StartH5pPlayerUC CreateStandardSystemUnderTest(
        IDisplayH5pUC? fakeUc = null,
        IStartH5pPlayerUCOutputPort? outputPort = null)
    {
        fakeUc ??= Substitute.For<IDisplayH5pUC>();
        outputPort ??= Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = new StartH5pPlayerUC(
            fakeUc,
            outputPort);
        return systemUnderTest;
    }
}