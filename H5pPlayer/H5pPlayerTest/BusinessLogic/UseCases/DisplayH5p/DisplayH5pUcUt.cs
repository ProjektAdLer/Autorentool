using System.Reflection.PortableExecutable;
using H5pPlayer.BusinessLogic;
using H5pPlayer.BusinessLogic.JavaScriptApi;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace H5pPlayerTest.BusinessLogic.UseCases.DisplayH5p;

[TestFixture]
public class DisplayH5pUcUt
{

    
    
    [Test]
    public void StartToDisplayH5p()
    {
        var mockJavaScriptAdapter = Substitute.For<IJavaScriptAdapter>();
        var systemUnderTest = CreateStandardSystemUnderTest(mockJavaScriptAdapter);
        const string h5pJsonSourcePath = "C://PathToJson/Source";
        var displayH5pInputTo = CreateDisplayH5pInputTo(h5pJsonSourcePath);


        systemUnderTest.StartToDisplayH5p(displayH5pInputTo);
        
        mockJavaScriptAdapter.Received().DisplayH5p(h5pJsonSourcePath);
    }
 
    
    [TestCase(@"C:\Temp")]
    [TestCase(@"Test\Temp\Tested")]
    public void ValidH5pJsonSourcePath(string validPath)
    {
        var systemUnderTest = CreateStandardSystemUnderTest();
        var displayH5pInputTo = CreateDisplayH5pInputTo(validPath);
        
        Assert.DoesNotThrow(
            () => systemUnderTest.StartToDisplayH5p(displayH5pInputTo));
        Assert.That(systemUnderTest.H5pEntity.H5pJsonSourcePath, Is.EqualTo(validPath));
    }
   
    [Test]
    public void NullH5pJsonSourcePath()
    {
        string invalidPath = null;

        var mockDisplayH5pOutputPort = Substitute.For<IDisplayH5pUcOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(null, mockDisplayH5pOutputPort);
        var displayH5pInputTo = CreateDisplayH5pInputTo(invalidPath);
        const string expectedErrorMessage = "H5P Json Path was wrong!";
        var expectedOutputTo = CreateDisplayH5pErrorOutputTo(invalidPath, expectedErrorMessage);

        systemUnderTest.StartToDisplayH5p(displayH5pInputTo);
     
        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
    }

  

    [TestCase("")]
    [TestCase("   ")]
    public void EmptyOrWhitespaceH5pJsonSourcePath(string pathToCheck)
    {
        var mockDisplayH5pOutputPort = Substitute.For<IDisplayH5pUcOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(null, mockDisplayH5pOutputPort);
        var displayH5pInputTo = CreateDisplayH5pInputTo(pathToCheck);
        const string expectedErrorMessage = "H5P Json Path was wrong!";
        var expectedOutputTo = CreateDisplayH5pErrorOutputTo(pathToCheck, expectedErrorMessage);

        systemUnderTest.StartToDisplayH5p(displayH5pInputTo);

        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
        
    }

    [Test]
    public void InvalidH5pJsonSourcePath([Range(0, 32)] int number)
    {
        var mockDisplayH5pOutputPort = Substitute.For<IDisplayH5pUcOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(null, mockDisplayH5pOutputPort);
        var badChars = Path.GetInvalidPathChars();// 33 elements
        const string validPath = @"C:\Temp\Invalid";
        var invalidPath = validPath + badChars[number];
        var displayH5pInputTo = CreateDisplayH5pInputTo(invalidPath);
        const string expectedErrorMessage = "H5P Json Path was wrong!";
        var expectedOutputTo = CreateDisplayH5pErrorOutputTo(invalidPath, expectedErrorMessage);

       systemUnderTest.StartToDisplayH5p(displayH5pInputTo);
       
       mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
    }
    
    
    private static DisplayH5pInputTo CreateDisplayH5pInputTo(
        string h5pJsonSourcePath = "C://Default_PathToJson/Source")
    {
        var displayH5pTo = new DisplayH5pInputTo(h5pJsonSourcePath);
        return displayH5pTo;
    }
    
    private static DisplayH5pErrorOutputTo CreateDisplayH5pErrorOutputTo(
        string invalidPath, 
        string expectedErrorMessage)
    {
        var expectedOutputTo = new DisplayH5pErrorOutputTo();
        expectedOutputTo.InvalidH5pJsonSourcePath = invalidPath;
        expectedOutputTo.H5pJsonSourcePathErrorText = expectedErrorMessage;
        return expectedOutputTo;
    }

    private static DisplayH5pUc CreateStandardSystemUnderTest(
        IJavaScriptAdapter? fakeJavaScriptAdapter = null,
        IDisplayH5pUcOutputPort? fakeDisplayH5pUcOutputPort = null)
    {
        fakeJavaScriptAdapter ??= Substitute.For<IJavaScriptAdapter>();
        fakeDisplayH5pUcOutputPort ??= Substitute.For<IDisplayH5pUcOutputPort>();
        var systemUnderTest = new DisplayH5pUc(
            fakeJavaScriptAdapter,
            fakeDisplayH5pUcOutputPort);
        return systemUnderTest;
    }
}