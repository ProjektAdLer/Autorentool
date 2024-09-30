using H5pPlayer.BusinessLogic.Domain;

namespace H5pPlayerTest.BusinessLogic.Domain;

[TestFixture]
public class H5pEntityUt
{

    [Test]
    public void H5pEntity_Constructor_AllPropertiesInitialized()
    {
        var systemUnderTest = CrateDefaultSystemUnderTest();
        
        Assert.That(systemUnderTest.ActiveDisplayMode, Is.EqualTo(H5pDisplayMode.Display));
        Assert.That(systemUnderTest.H5pZipSourcePath, Is.EqualTo(string.Empty));
    }
    
    [TestCase(@"C:\Temp")]                 // Windows absolute path
    [TestCase(@"Test\Temp\Tested")]        // Windows relative path
    [TestCase(@"Test/Temp/Tested")]        // Unix/macOS relative path
    [TestCase(@"/usr/local/bin")]          // Unix/macOS absolute path
    [TestCase(@"C:/Program Files/Temp")]   // Mixed style path (Windows)
    [TestCase(@"/tmp/test#folder/")]       // Unix/macOS with special characters
    [TestCase(@"C:\Temp with spaces\file")]// Windows with spaces
    [TestCase(@".\relative\path")]         // Windows relative path with .
    [TestCase(@"..\parent\path")]          // Windows relative path with ..
    [TestCase(@"/path with spaces")]       // Unix/macOS with spaces
    [TestCase(@"/")]                       // Root path Unix/macOS
    [TestCase(@"C:\")]                     // Root path Windows
    public void ValidH5pZipSourcePath(string validPath)
    {
        var systemUnderTest = CrateDefaultSystemUnderTest();

        Assert.DoesNotThrow(
            () => systemUnderTest.H5pZipSourcePath = validPath);
        Assert.That(systemUnderTest.H5pZipSourcePath, Is.EqualTo(validPath));
    }
    
    [Test]
    public void NullH5pZipSourcePath()
    {
        string invalidPath = null;

        var systemUnderTest = CrateDefaultSystemUnderTest();

        var exception = Assert.Throws<ArgumentNullException>(
            () => systemUnderTest.H5pZipSourcePath = invalidPath);
        Assert.That(exception.Message, Is.EqualTo("Value cannot be null. (Parameter 'H5pZipSourcePath')"));
    }
    
    [TestCase("")]
    [TestCase("   ")]
    public void EmptyOrWhitespaceH5pZipSourcePath(string pathToCheck)
    {
        var systemUnderTest = CrateDefaultSystemUnderTest();

        var exception = Assert.Throws<ArgumentException>(
            () => systemUnderTest.H5pZipSourcePath = pathToCheck);
        Assert.That(exception.Message, Contains.Substring("H5pZipSourcePath"));
    }
    
    [Test]
    public void H5pZipSourcePath([Range(0, 32)] int number)
    {
        string validPath = @"C:\Temp\Invalid";
        char[] badChars = Path.GetInvalidPathChars();// 33 elements

        var systemUnderTest = CrateDefaultSystemUnderTest();

        var exception = Assert.Throws<ArgumentException>(
            () => systemUnderTest.H5pZipSourcePath = validPath + badChars[number]);
        Assert.That(exception.Message, Is.EqualTo("H5pZipSourcePath contains invalid path chars!"));
    }

    private static H5pEntity CrateDefaultSystemUnderTest()
    {
        return new H5pEntity();
    }
}