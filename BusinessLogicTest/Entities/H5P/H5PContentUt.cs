using BusinessLogic.Entities.LearningContent.H5P;
using NUnit.Framework;
using Shared.H5P;

namespace BusinessLogicTest.Entities.H5P;

[TestFixture]
public class H5PContentUt
{
    private string _basePath;
    [SetUp]
    public void Setup()
    {
        _basePath = OperatingSystem.IsWindows() ? "C:" : "/";
    }
    [Test]
    public void H5PContent_ConstructorWithValidParameter_ConstructsH5PContent()
    {
        var name= "name";
        bool unsavedChanges = false;
        string type= "type";
        string filePath= Path.Combine(_basePath, name);
        bool primitiveH5P = false;
        H5PContentState h5PState = H5PContentState.Unknown;
        
        var systemUnderTest = new H5PContent(h5PState, name, unsavedChanges, type, filePath, primitiveH5P);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Type, Is.EqualTo(type));
            Assert.That(systemUnderTest.Filepath, Is.EqualTo(filePath));
            Assert.That(systemUnderTest.UnsavedChanges, Is.EqualTo(unsavedChanges));
            Assert.That(systemUnderTest.H5PState, Is.EqualTo(h5PState));
            Assert.That(systemUnderTest.IsH5P, Is.EqualTo(primitiveH5P));
            Assert.That(systemUnderTest.IsH5P, Is.EqualTo(false));
        });
    }
    
}
    
