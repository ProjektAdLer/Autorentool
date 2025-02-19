using H5pPlayer.BusinessLogic.Api.FileSystemDataAccess;
using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;
using NSubstitute;

namespace H5pPlayerTest.BusinessLogic.UseCases.StartH5pPlayer;

[TestFixture]
public class StartH5pPlayerUcUT
{
    const string H5pFileEnding = ".h5p";
    private string RootPathDueToOperatingSystem { get;  set; }

    [SetUp]
    public void SetUp()
    {
        RootPathDueToOperatingSystem = OperatingSystem.IsWindows() ? "C:" : "/";
    }

    [Test]
    public async Task CleanH5pFolderInWwwroot()
    {
        var mockFileSystemDataAccess = Substitute.For<IFileSystemDataAccess>();
        var systemUnderTest = CreateStandardSystemUnderTest(
            null, null, null, mockFileSystemDataAccess);
        var directoryForCleaning = Path.Combine("wwwroot", "H5pStandalone", "h5p-folder");
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0();

        await systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);

        mockFileSystemDataAccess.Received().DeleteAllFilesAndDirectoriesIn(
            Arg.Is<string>(path => path.Contains(directoryForCleaning)));
    }


    [Test] 
    [TestCaseSource(nameof(GetValidH5pZipSourcePaths))] 
    public async Task ValidH5pZipSourcePath(string validPath)
    {
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(
            null, null, mockDisplayH5pOutputPort);
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(0, validPath);

        await systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);

        mockDisplayH5pOutputPort.DidNotReceive().ErrorOutput(Arg.Any<StartH5pPlayerErrorOutputTO>());
        Assert.That(systemUnderTest.H5pEntity.H5pZipSourcePath, Is.EqualTo(validPath));
    }
    
    [Test]
    public async Task NullH5pZipSourcePath()
    {
        string invalidPath = null;
        string validPath = Path.Combine(RootPathDueToOperatingSystem, "Temp");
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(
            null, null, mockDisplayH5pOutputPort);
        var startH5pPlayerInputTO = CreateNullableStartH5pPlayerInputT0(0, invalidPath, validPath);

        const string expectedErrorMessage = "Value cannot be null. (Parameter 'H5pZipSourcePath')";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(invalidPath, expectedErrorMessage);

        await systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);

        mockDisplayH5pOutputPort.Received().ErrorOutput(Arg.Is<StartH5pPlayerErrorOutputTO>(x =>
            x.InvalidPath == expectedOutputTo.InvalidPath &&
            x.ErrorTextForInvalidPath == expectedOutputTo.ErrorTextForInvalidPath));
    }

    [TestCase("")]
    [TestCase("   ")]
    public async Task EmptyOrWhitespaceH5pZipSourcePath(string pathToCheck)
    {
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(
            null, null, mockDisplayH5pOutputPort);
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(0, pathToCheck);
        const string expectedErrorMessage = "H5pZipSourcePath was empty or whitespace!";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(pathToCheck, expectedErrorMessage);

        await systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);

        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
    }

    [Test]
    [TestCaseSource(nameof(GetInvalidPathChars))]
    public async Task InvalidPathCharsInH5pZipSourcePath(char badChar)
    {
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(
            null, null, mockDisplayH5pOutputPort);
        var validPath = Path.Combine(RootPathDueToOperatingSystem, "Temp", "Invalid");
        var invalidPath = validPath + badChar + H5pFileEnding;
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(0, invalidPath);
        const string expectedErrorMessage = "H5pZipSourcePath contains invalid path chars!";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(invalidPath, expectedErrorMessage);

        await systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);

        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
    }

    [Test]
    public async Task MissingH5pExtensionInH5pZipSourcePath([Values] H5pDisplayMode displayMode)
    {
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(
            null, null, mockDisplayH5pOutputPort);
        string invalidPath = Path.Combine(RootPathDueToOperatingSystem, "Temp", "Invalid");
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(displayMode, invalidPath);
        const string expectedErrorMessage = "H5pZipSourcePath misses .h5p extension!";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(invalidPath, expectedErrorMessage);

        await systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);

        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
    }


    [TestCase(@"Test\Temp\Tested" + H5pFileEnding)] // Windows relative path
    [TestCase(@"Test/Temp/Tested" + H5pFileEnding)] // Unix/macOS relative path
    [TestCase(@".\relative\path" + H5pFileEnding)] // Windows relative path with .
    [TestCase(@"..\parent\path" + H5pFileEnding)] // Windows relative path with ..
    [TestCase(@"C:Test\Temp\Tested" + H5pFileEnding)] // Windows relative path with drive letter
    [TestCase(@"../parent/path" + H5pFileEnding)] // Unix/macOS relative path with ..
    [TestCase(@"./relative/path" + H5pFileEnding)] // Unix/macOS relative path with .
    [TestCase(@"~/home/user/Tested" + H5pFileEnding)] // Unix/macOS relative path with ~
    public async Task PathIsNotRootedInH5pZipSourcePath(string invalidPath)
    {
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(
            null, null, mockDisplayH5pOutputPort);
        var displayMode = H5pDisplayMode.Display;
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(displayMode, invalidPath);
        const string expectedErrorMessage = "H5pZipSourcePath must be rooted!";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(invalidPath, expectedErrorMessage);

        await systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);

        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
    }


    [TestCase(@"https://example.com/path/to/resource")] // Basic HTTPS URL
    [TestCase(@"https://example.com/resource#with-fragment")] // URL with fragment identifier
    [TestCase(@"https://example.com/path with spaces/resource")] // URL with spaces
    [TestCase(@"https://example.com/path/to/resource?query=parameter")] // URL with query parameters
    [TestCase(@"https://example.com:8080/path/to/resource")] // URL with custom port number
    [TestCase(
        @"https://example.com/path/to/special_characters#frag!ment$123")] // URL with special characters and fragment
    [TestCase(@"https://example.com/path/to/deeply/nested/resource")] // URL with deeply nested path
    [TestCase(@"https://localhost:5000/api/resource")] // Localhost URL with custom port
    [TestCase(@"https://example.com/resource.ext")] // URL with file extension
    [TestCase(@"https://example.com/path/to/resource/file.json")] // URL pointing to a JSON file
    [TestCase(@"https://example.com/path/to/resource/file.xml")] // URL pointing to an XML file
    [TestCase(@"https://example.com/api/v1/resource/123")] // API-style URL with versioning and ID
    public async Task ValidUnzippedH5psPath(string validPath)
    {
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(
            null, null, mockDisplayH5pOutputPort);
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(0, null, validPath);

        await systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);

        mockDisplayH5pOutputPort.DidNotReceive().ErrorOutput(Arg.Any<StartH5pPlayerErrorOutputTO>());
        Assert.That(systemUnderTest.H5pEntity.UnzippedH5psPath, Is.EqualTo(validPath));
    }


    [Test]
    public async Task NullUnzippedH5psPath()
    {
        string invalidPath = null;
        string validPath = Path.Combine(RootPathDueToOperatingSystem, "Temp" + H5pFileEnding);
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(
            null, null, mockDisplayH5pOutputPort);
        var startH5pPlayerInputTO = CreateNullableStartH5pPlayerInputT0(0, validPath, invalidPath);

        const string expectedErrorMessage = "Value cannot be null. (Parameter 'UnzippedH5psPath')";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(invalidPath, expectedErrorMessage);

        await systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);

        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
    }

    [TestCase("")]
    [TestCase("   ")]
    public async Task EmptyOrWhitespaceUnzippedH5psPath(string pathToCheck)
    {
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(
            null, null, mockDisplayH5pOutputPort);
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(0, null, pathToCheck);
        const string expectedErrorMessage = "UnzippedH5psPath was empty or whitespace!";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(pathToCheck, expectedErrorMessage);

        await systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);

        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
    }


    [Test]
    [TestCaseSource(nameof(GetInvalidPathChars))]
    public async Task InvalidPathCharsInUnzippedH5psPath(char badChar)
    {
        var mockDisplayH5pOutputPort = Substitute.For<IStartH5pPlayerUCOutputPort>();
        var systemUnderTest = CreateStandardSystemUnderTest(
            null, null, mockDisplayH5pOutputPort);
        var validPath = Path.Combine(RootPathDueToOperatingSystem, "Temp", "Invalid");
        var invalidPath = validPath + badChar + H5pFileEnding;
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(0, null, invalidPath);
        const string expectedErrorMessage = "UnzippedH5psPath contains invalid path chars!";
        var expectedOutputTo = CreateStartH5pPlayerErrorOutputTO(invalidPath, expectedErrorMessage);

        await systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);

        mockDisplayH5pOutputPort.Received().ErrorOutput(expectedOutputTo);
    }

    [Test]
    public async Task MapDisplayMode([Values] H5pDisplayMode displayMode)
    {
        var systemUnderTest = CreateStandardSystemUnderTest();
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(displayMode);

        await systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);

        Assert.That(systemUnderTest.H5pEntity.ActiveDisplayMode,
            Is.EqualTo(displayMode));
    }

    [Test]
    public async Task ExtractZippedH5pToTemporaryFolder()
    {
        var mockFileSystemDataAccess = Substitute.For<IFileSystemDataAccess>();
        var systemUnderTest = CreateStandardSystemUnderTest(
            null, null, null, mockFileSystemDataAccess);
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0();

        await systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);

        mockFileSystemDataAccess.Received().ExtractZipFile(
            systemUnderTest.H5pEntity.H5pZipSourcePath,
            Arg.Is<string>(path => path.Contains(Path.Combine("wwwroot", "H5pStandalone", "h5p-folder"))));
    }

    [Test]
    public async Task StartH5pPlayerToDisplayH5p()
    {
        var mockDisplayH5pUC = Substitute.For<IDisplayH5pUC>();
        var mockValidateH5pUc = Substitute.For<IValidateH5pUc>();
        var systemUnderTest = CreateStandardSystemUnderTest(mockValidateH5pUc, mockDisplayH5pUC);
        string h5pZipSourcePath = Path.Combine(RootPathDueToOperatingSystem, "PathToZip", "Source" + H5pFileEnding);
        var displayMode = H5pDisplayMode.Display;
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(displayMode, h5pZipSourcePath);

        await systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);

        await mockDisplayH5pUC.Received().StartToDisplayH5p(Arg.Any<H5pEntity>());
        await mockValidateH5pUc.DidNotReceive().StartToValidateH5p(Arg.Any<H5pEntity>());
    }

    [Test]
    public async Task StartH5pPlayerToValidateH5p()
    {
        var mockValidateH5pUc = Substitute.For<IValidateH5pUc>();
        var mockDisplayH5pUC = Substitute.For<IDisplayH5pUC>();
        var systemUnderTest = CreateStandardSystemUnderTest(mockValidateH5pUc, mockDisplayH5pUC);
        string h5pZipSourcePath = Path.Combine(RootPathDueToOperatingSystem, "PathToZip", "Source" + H5pFileEnding);
        var displayMode = H5pDisplayMode.Validate;
        var startH5pPlayerInputTO = CreateStartH5pPlayerInputT0(displayMode, h5pZipSourcePath);

        await systemUnderTest.StartH5pPlayer(startH5pPlayerInputTO);

        await mockValidateH5pUc.Received().StartToValidateH5p(Arg.Any<H5pEntity>());
        await mockDisplayH5pUC.DidNotReceive().StartToDisplayH5p(Arg.Any<H5pEntity>());
    }


    private static StartH5pPlayerInputTO CreateNullableStartH5pPlayerInputT0(
        H5pDisplayMode displayMode,
        string h5pZipSourcePath,
        string unzippedH5psPath)
    {
        var transportObject = new StartH5pPlayerInputTO(displayMode, h5pZipSourcePath, unzippedH5psPath);
        return transportObject;
    }


    private StartH5pPlayerInputTO CreateStartH5pPlayerInputT0(
        H5pDisplayMode displayMode = H5pDisplayMode.Display,
        string? h5pZipSourcePath = null,
        string? unzippedH5psPath = null)
    {
        h5pZipSourcePath ??= Path.Combine(RootPathDueToOperatingSystem, "Default_PathToZip", "Source" + H5pFileEnding);
        unzippedH5psPath ??= "https://localhost:8001/H5pStandalone/h5p-folder/";
        var transportObject = new StartH5pPlayerInputTO(displayMode, h5pZipSourcePath, unzippedH5psPath);
        return transportObject;
    }

    private static StartH5pPlayerErrorOutputTO CreateStartH5pPlayerErrorOutputTO(
        string invalidPath,
        string expectedErrorMessage)
    {
        var expectedOutputTo = new StartH5pPlayerErrorOutputTO(invalidPath, expectedErrorMessage);
        return expectedOutputTo;
    }

    private static IEnumerable<char> GetInvalidPathChars()
    {
        return Path.GetInvalidPathChars();
    }

    private static StartH5pPlayerUC CreateStandardSystemUnderTest(
        IValidateH5pUc? validateH5PUc = null,
        IDisplayH5pUC? displayH5pUc = null,
        IStartH5pPlayerUCOutputPort? outputPort = null,
        IFileSystemDataAccess? dataAccess = null)
    {
        validateH5PUc ??= Substitute.For<IValidateH5pUc>();
        displayH5pUc ??= Substitute.For<IDisplayH5pUC>();
        outputPort ??= Substitute.For<IStartH5pPlayerUCOutputPort>();
        dataAccess ??= Substitute.For<IFileSystemDataAccess>();
        var systemUnderTest = new StartH5pPlayerUC(
            validateH5PUc,
            dataAccess,
            displayH5pUc,
            outputPort);
        return systemUnderTest;
    }
    
    
    private static IEnumerable<string> GetValidH5pZipSourcePaths()
    {
        if (OperatingSystem.IsWindows())
        {
            yield return @"C:\Temp" + H5pFileEnding; // Windows absolute path
            yield return @"C:/Program Files/Temp" + H5pFileEnding; // Mixed style path (Windows)
            yield return @"C:\Temp with spaces\file" + H5pFileEnding; // Windows with spaces
            yield return @"C:\d" + H5pFileEnding; // Root path Windows
        }
        else
        {
            yield return @"/usr/local/bin" + H5pFileEnding; // Unix/macOS absolute path
            yield return @"/tmp/test#folder/d" + H5pFileEnding; // Unix/macOS with special characters
            yield return @"/path with spaces" + H5pFileEnding; // Unix/macOS with spaces
            yield return @"/d" + H5pFileEnding; // Root path Unix/macOS
        }
    }
}