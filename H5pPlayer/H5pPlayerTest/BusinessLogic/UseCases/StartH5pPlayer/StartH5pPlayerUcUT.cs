﻿using H5pPlayer.BusinessLogic.Domain;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;
using NSubstitute;

namespace H5pPlayerTest.BusinessLogic.UseCases.StartH5pPlayer;

[TestFixture]
public class StartH5pPlayerUcUT
{
  
    const string H5pFileEnding = ".h5p";
       
    [TestCase(@"C:\Temp"+ H5pFileEnding)]                 // Windows absolute path
    [TestCase(@"/usr/local/bin" + H5pFileEnding)]          // Unix/macOS absolute path
    [TestCase(@"C:/Program Files/Temp" + H5pFileEnding)]   // Mixed style path (Windows)
    [TestCase(@"/tmp/test#folder/d" + H5pFileEnding)]       // Unix/macOS with special characters
    [TestCase(@"C:\Temp with spaces\file" + H5pFileEnding)]// Windows with spaces
    [TestCase(@"/path with spaces" + H5pFileEnding)]       // Unix/macOS with spaces
    [TestCase(@"/d" + H5pFileEnding)]                       // Root path Unix/macOS
    [TestCase(@"C:\d" + H5pFileEnding)]                     // Root path Windows
    public void ValidH5pZipSourcePath(string validPath)
    {
        var systemUnderTest = CreateStandardSystemUnderTest();
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(0,validPath);
        
        Assert.DoesNotThrow(() => systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO));
        Assert.That(systemUnderTest.H5pEntity.H5pZipSourcePath, Is.EqualTo(validPath));
    }
   
    [Test]
    public void NullH5pZipSourcePath()
    {
        string invalidPath = null;
        string validPath = @"C:\Temp";
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(null, mockDisplayH5pOutputPort);
        var startH5pPlayerInputTO = CreateNullableStartH5pPlayerInputT0(0,invalidPath, validPath);
       
        const string expectedErrorMessage =   "Value cannot be null. (Parameter 'H5pZipSourcePath')";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(invalidPath, expectedErrorMessage);

        systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);
     
        mockDisplayH5pOutputPort.Received().ErrorOutput(Arg.Is<StartH5pPlayerErrorOutputTO>(x =>
            x.InvalidPath == expectedOutputTo.InvalidPath &&
            x.ErrorTextForInvalidPath == expectedOutputTo.ErrorTextForInvalidPath));
    }

    [TestCase("")]
    [TestCase("   ")]
    public void EmptyOrWhitespaceH5pZipSourcePath(string pathToCheck)
    {
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(null, mockDisplayH5pOutputPort);
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(0,pathToCheck);
        const string expectedErrorMessage = "H5pZipSourcePath was empty or whitespace!";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(pathToCheck, expectedErrorMessage);

        systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);

        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
    }

    [Test]
    public void InvalidPathCharsInH5pZipSourcePath([Range(0, 32)] int number)
    {
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(null, mockDisplayH5pOutputPort);
        var badChars = Path.GetInvalidPathChars();// 33 elements
        const string validPath = @"C:\Temp\Invalid";
        var invalidPath = validPath + badChars[number] + H5pFileEnding;
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(0,invalidPath);
        const string expectedErrorMessage = "H5pZipSourcePath contains invalid path chars!";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(invalidPath, expectedErrorMessage);

        systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);
       
        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
    }

    [Test]
    public void MissingH5pExtensionInH5pZipSourcePath([Values]H5pDisplayMode displayMode)
    {
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(null, mockDisplayH5pOutputPort);
        const string invalidPath = @"C:\Temp\Invalid";
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(displayMode,invalidPath);
        const string expectedErrorMessage = "H5pZipSourcePath misses .h5p extension!";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(invalidPath, expectedErrorMessage);

        systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);
       
        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
    }

           
    
    [TestCase(@"Test\Temp\Tested" + H5pFileEnding)]        // Windows relative path
    [TestCase(@"Test/Temp/Tested" + H5pFileEnding)]        // Unix/macOS relative path
    [TestCase(@".\relative\path" + H5pFileEnding)]         // Windows relative path with .
    [TestCase(@"..\parent\path" + H5pFileEnding)]          // Windows relative path with ..
    public void PathIsNotRootedInH5pZipSourcePath(string invalidPath)
    {
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(null, mockDisplayH5pOutputPort);
        var displayMode = H5pDisplayMode.Display;
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(displayMode,invalidPath);
        const string expectedErrorMessage = "H5pZipSourcePath must be rooted!";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(invalidPath, expectedErrorMessage);

        systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);
       
        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
    }

    
    [TestCase(@"C:\Temp"+ H5pFileEnding)]                 // Windows absolute path
    [TestCase(@"/usr/local/bin" + H5pFileEnding)]          // Unix/macOS absolute path
    [TestCase(@"C:/Program Files/Temp" + H5pFileEnding)]   // Mixed style path (Windows)
    [TestCase(@"/tmp/test#folder/d" + H5pFileEnding)]       // Unix/macOS with special characters
    [TestCase(@"C:\Temp with spaces\file" + H5pFileEnding)]// Windows with spaces
    [TestCase(@"/path with spaces" + H5pFileEnding)]       // Unix/macOS with spaces
    [TestCase(@"/d" + H5pFileEnding)]                       // Root path Unix/macOS
    [TestCase(@"C:\d" + H5pFileEnding)]                     // Root path Windows
    public void ValidUnzippedH5psPath(string validPath)
    {
        var systemUnderTest = CreateStandardSystemUnderTest();
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(0,null, validPath);
        
        Assert.DoesNotThrow(() => systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO));
        Assert.That(systemUnderTest.H5pEntity.UnzippedH5psPath, Is.EqualTo(validPath));
    }
    
    
    [Test]
    public void NullUnzippedH5psPath()
    {
        string invalidPath = null;
        string validPath = @"C:\Temp" + H5pFileEnding;
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(null, mockDisplayH5pOutputPort);
        var startH5pPlayerInputTO = CreateNullableStartH5pPlayerInputT0(0,validPath, invalidPath);
       
        const string expectedErrorMessage =   "Value cannot be null. (Parameter 'UnzippedH5psPath')";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(invalidPath, expectedErrorMessage);

        systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);
     
        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
    }

    [TestCase("")]
    [TestCase("   ")]
    public void EmptyOrWhitespaceUnzippedH5psPath(string pathToCheck)
    {
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(null, mockDisplayH5pOutputPort);
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(0,null, pathToCheck);
        const string expectedErrorMessage = "UnzippedH5psPath was empty or whitespace!";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(pathToCheck, expectedErrorMessage);

        systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);

        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
    }
    
    
    [Test]
    public void InvalidPathCharsInUnzippedH5psPath([Range(0, 32)] int number)
    {
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(null, mockDisplayH5pOutputPort);
        var badChars = Path.GetInvalidPathChars();// 33 elements
        const string startPath = @"C:\Temp\Invalid";
        var invalidPath = startPath + badChars[number] + H5pFileEnding;
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(0,null,invalidPath);
        const string expectedErrorMessage = "UnzippedH5psPath contains invalid path chars!";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(invalidPath, expectedErrorMessage);

        systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);
       
        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
    }
    
    
    [TestCase(@"Test\Temp\Tested" + H5pFileEnding)]        // Windows relative path
    [TestCase(@"Test/Temp/Tested" + H5pFileEnding)]        // Unix/macOS relative path
    [TestCase(@".\relative\path" + H5pFileEnding)]         // Windows relative path with .
    [TestCase(@"..\parent\path" + H5pFileEnding)]          // Windows relative path with ..
    public void PathIsNotRootedInUnzippedH5psPath(string invalidPath)
    {
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(null, mockDisplayH5pOutputPort);
        var displayMode = H5pDisplayMode.Display;
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(displayMode,null,invalidPath);
        const string expectedErrorMessage = "UnzippedH5psPath must be rooted!";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(invalidPath, expectedErrorMessage);

        systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);
       
        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
    }
    
    
    
    
    [Test]
    public void MapDisplayMode([Values]H5pDisplayMode displayMode)
    {
        var systemUnderTest = CreateStandardSystemUnderTest();
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(displayMode);
        
        systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);
        
        Assert.That(systemUnderTest.H5pEntity.ActiveDisplayMode, 
            Is.EqualTo(displayMode));
    }
    
    
    // hier weiter extract ziped H5p

    
    

    [Test]
    public void ExtractZippedH5pToTemporaryFolder()
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
        const string h5pZipSourcePath = "C://PathToZip/Source" + H5pFileEnding;
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(0,h5pZipSourcePath);

        systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);
        
        mockDisplayH5pUC.Received().StartToDisplayH5pUC(Arg.Any<H5pEntity>());
    }
 
    
    private static StartH5pPlayerInputTO CreateNullableStartH5pPlayerInputT0(
        H5pDisplayMode displayMode,
        string h5pZipSourcePath,
        string unzippedH5psPath)
    {
        var transportObject = new StartH5pPlayerInputTO(displayMode, h5pZipSourcePath, unzippedH5psPath);
        return transportObject;
    }
    
    
    private static StartH5pPlayerInputTO CreateStartH5pPlayerInputT0(
        H5pDisplayMode displayMode = H5pDisplayMode.Display,
        string h5pZipSourcePath = null,
        string unzippedH5psPath = null)
    {
        h5pZipSourcePath ??= "C://Default_PathToZip/Source" + H5pFileEnding;
        unzippedH5psPath ??= "https://localhost:8001/H5pStandalone/h5p-folder/AbfrageDefinitionen";
        var transportObject = new StartH5pPlayerInputTO(displayMode, h5pZipSourcePath, unzippedH5psPath);
        return transportObject;
    }
    
    private static StartH5pPlayerErrorOutputTO CreateStartH5pPlayerErrorOutputTO(
        string invalidPath, 
        string expectedErrorMessage)
    {
        var expectedOutputTo = new StartH5pPlayerErrorOutputTO();
        expectedOutputTo.InvalidPath = invalidPath;
        expectedOutputTo.ErrorTextForInvalidPath = expectedErrorMessage;
        return expectedOutputTo;
    }

    private static StartH5pPlayerUC CreateStandardSystemUnderTest(
        IDisplayH5pUC? fakeDisplayH5PUc = null,
        IStartH5pPlayerUCOutputPort? outputPort = null)
    {
        fakeDisplayH5PUc ??= Substitute.For<IDisplayH5pUC>();
        outputPort ??= Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = new StartH5pPlayerUC(
            fakeDisplayH5PUc,
            outputPort);
        return systemUnderTest;
    }
}