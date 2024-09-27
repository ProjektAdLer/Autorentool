using H5pPlayer.BusinessLogic.Domain;
using H5pPlayer.BusinessLogic.JavaScriptApi;
using H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;
using NSubstitute;

namespace H5pPlayerTest.BusinessLogic.UseCases.DisplayH5p;

[TestFixture]
public class StartH5pPlayerUcUT
{

    
    
    
       
    [TestCase(@"C:\Temp")]
    [TestCase(@"Test\Temp\Tested")]
    public void ValidH5pJsonSourcePath(string validPath)
    {
        var systemUnderTest = CreateStandardSystemUnderTest();
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(validPath);
        
        Assert.DoesNotThrow(
            () => systemUnderTest.StartToDisplayH5p(startH5pPlayerInputTO));
        Assert.That(systemUnderTest.H5pEntity.H5pJsonSourcePath, Is.EqualTo(validPath));
    }
   
    [Test]
    public void NullH5pJsonSourcePath()
    {
        string invalidPath = null;

        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(null, mockDisplayH5pOutputPort);
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(invalidPath);
        const string expectedErrorMessage = "H5P Json Path was wrong!";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(invalidPath, expectedErrorMessage);

        systemUnderTest.StartToDisplayH5p(startH5pPlayerInputTO);
     
        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
    }

  

    [TestCase("")]
    [TestCase("   ")]
    public void EmptyOrWhitespaceH5pJsonSourcePath(string pathToCheck)
    {
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(null, mockDisplayH5pOutputPort);
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(pathToCheck);
        const string expectedErrorMessage = "H5P Json Path was wrong!";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(pathToCheck, expectedErrorMessage);

        systemUnderTest.StartToDisplayH5p(startH5pPlayerInputTO);

        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
        
    }

    [Test]
    public void InvalidH5pJsonSourcePath([Range(0, 32)] int number)
    {
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(null, mockDisplayH5pOutputPort);
        var badChars = Path.GetInvalidPathChars();// 33 elements
        const string validPath = @"C:\Temp\Invalid";
        var invalidPath = validPath + badChars[number];
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(invalidPath);
        const string expectedErrorMessage = "H5P Json Path was wrong!";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(invalidPath, expectedErrorMessage);

        systemUnderTest.StartToDisplayH5p(startH5pPlayerInputTO);
       
        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
    }
    
    // hier weiter extract ziped H5p


    [Test]
    public void ExtractZippedH5p()
    {
        var systemUnderTest = CreateStandardSystemUnderTest();
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0();
        
        systemUnderTest.StartToDisplayH5p(startH5pPlayerInputTO);
        
    }
    
    
    [Test]
    public void StartToDisplayH5p()
    {
        var mockJavaScriptAdapter = Substitute.For<IJavaScriptAdapter>();
        var systemUnderTest = CreateStandardSystemUnderTest(mockJavaScriptAdapter);
        const string h5pJsonSourcePath = "C://PathToJson/Source";
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(h5pJsonSourcePath);


        systemUnderTest.StartToDisplayH5p(startH5pPlayerInputTO);
        
        mockJavaScriptAdapter.Received().DisplayH5p(Arg.Any<H5pEntity>());
    }
 
    
    
    
    
    private static StartH5pPlayerInputTO CreateStartH5pPlayerInputT0(
        string h5pJsonSourcePath = "C://Default_PathToJson/Source")
    {
        var transportObject = new StartH5pPlayerInputTO(h5pJsonSourcePath);
        return transportObject;
    }
    
    private static StartH5pPlayerErrorOutputTO CreateStartH5pPlayerErrorOutputTO(
        string invalidPath, 
        string expectedErrorMessage)
    {
        var expectedOutputTo = new StartH5pPlayerErrorOutputTO();
        expectedOutputTo.InvalidH5pJsonSourcePath = invalidPath;
        expectedOutputTo.H5pJsonSourcePathErrorText = expectedErrorMessage;
        return expectedOutputTo;
    }

    private static StartH5pPlayerUC CreateStandardSystemUnderTest(
        IJavaScriptAdapter? fakeAdapter = null,
        IStartH5pPlayerUCOutputPort? outputPort = null)
    {
        fakeAdapter ??= Substitute.For<IJavaScriptAdapter>();
        outputPort ??= Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = new StartH5pPlayerUC(
            fakeAdapter,
            outputPort);
        return systemUnderTest;
    }
}