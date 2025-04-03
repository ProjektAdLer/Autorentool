using BusinessLogic.Entities.LearningContent.H5P;
using NUnit.Framework;

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
    public void H5PContent_ConstructorWithValidParameter_ConstructsH5PContent(
        [Values] H5PContentState state)
    {
        var name= "name";
        bool unsavedChanges = false;
        string type= "type";
        string filePath= Path.Combine(_basePath, name);
        bool primitiveH5P = false;
        
        var systemUnderTest = new H5PContent(state, name, unsavedChanges, type, filePath, primitiveH5P);
        
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.Name, Is.EqualTo(name));
            Assert.That(systemUnderTest.Type, Is.EqualTo(type));
            Assert.That(systemUnderTest.Filepath, Is.EqualTo(filePath));
            Assert.That(systemUnderTest.UnsavedChanges, Is.EqualTo(unsavedChanges));
            Assert.That(systemUnderTest.State, Is.EqualTo(state));
            Assert.That(systemUnderTest.PrimitiveH5P, Is.EqualTo(primitiveH5P));
           // Assert.That(systemUnderTest.PrimitiveH5P, Is.EqualTo(false));
        });
    }
    
}
    
