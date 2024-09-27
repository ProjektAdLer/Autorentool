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
        Assert.That(systemUnderTest.H5pJsonSourcePath, Is.EqualTo(string.Empty));
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
    public void ValidH5pJsonSourcePath(string validPath)
    {
        var systemUnderTest = CrateDefaultSystemUnderTest();

        Assert.DoesNotThrow(
            () => systemUnderTest.H5pJsonSourcePath = validPath);
        Assert.That(systemUnderTest.H5pJsonSourcePath, Is.EqualTo(validPath));
    }
    
    [Test]
    public void NullH5pJsonSourcePath()
    {
        string invalidPath = null;

        var systemUnderTest = CrateDefaultSystemUnderTest();

        var exception = Assert.Throws<ArgumentNullException>(
            () => systemUnderTest.H5pJsonSourcePath = invalidPath);
        Assert.That(exception.Message, Is.EqualTo("Value cannot be null. (Parameter 'H5pJsonSourcePath')"));
    }
    
    [TestCase("")]
    [TestCase("   ")]
    public void EmptyOrWhitespaceH5pJsonSourcePath(string pathToCheck)
    {
        var systemUnderTest = CrateDefaultSystemUnderTest();

        var exception = Assert.Throws<ArgumentException>(
            () => systemUnderTest.H5pJsonSourcePath = pathToCheck);
        Assert.That(exception.Message, Contains.Substring("H5pJsonSourcePath"));
    }
    
    [Test]
    public void H5pJsonSourcePath([Range(0, 32)] int number)
    {
        string validPath = @"C:\Temp\Invalid";
        char[] badChars = Path.GetInvalidPathChars();// 33 elements

        var systemUnderTest = CrateDefaultSystemUnderTest();

        var exception = Assert.Throws<ArgumentException>(
            () => systemUnderTest.H5pJsonSourcePath = validPath + badChars[number]);
        Assert.That(exception.Message, Is.EqualTo("H5pJsonSourcePath contains invalid path chars!"));
    }

    private static H5pEntity CrateDefaultSystemUnderTest()
    {
        return new H5pEntity();
    }
}