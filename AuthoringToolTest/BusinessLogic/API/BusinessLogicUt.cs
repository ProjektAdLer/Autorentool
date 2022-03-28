using AuthoringTool.API.Configuration;
using NUnit.Framework;
using AuthoringTool.BusinessLogic.API;
using AuthoringTool.DataAccess.API;
using AuthoringTool.Entities;
using NSubstitute;

namespace AuthoringToolTest.BusinessLogic.API;

[TestFixture]
public class BusinessLogicUt
{
    [Test]
    public void BusinessLogic_StandardConstructor_AllPropertiesInitialized()
    {
        IAuthoringToolConfiguration mockConfiguration = Substitute.For<IAuthoringToolConfiguration>();
        IDataAccess mockDataAccess = Substitute.For<IDataAccess>();  
        var systemUnderTest = CreateStandardBusinessLogic(mockConfiguration, mockDataAccess);
        
        Assert.AreEqual(mockConfiguration, systemUnderTest.Configuration);
        Assert.AreEqual(mockDataAccess, systemUnderTest.DataAccess);
    }
    
    [Test]
    public void BusinessLogic_ConstructBackup_CallsDataAccess()
    {
        IDataAccess mockDataAccess = Substitute.For<IDataAccess>();
        
        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);
        
        systemUnderTest.ConstructBackup();
        
        mockDataAccess.Received().ConstructBackup();
    }
    
    [Test]
    public void BusinessLogic_SaveLearningWorld_CallsDataAccess()
    {
        IDataAccess mockDataAccess = Substitute.For<IDataAccess>();
        var learningWorld = new LearningWorld("fa", "a", "f", "f", "f", "f");
        
        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);
        
        systemUnderTest.SaveLearningWorld(learningWorld, "foobar");
        
        mockDataAccess.Received().SaveLearningWorldToFile(learningWorld, "foobar");
    }
    
    [Test]
    public void BusinessLogic_LoadLearningWorld_CallsDataAccess()
    {
        IDataAccess mockDataAccess = Substitute.For<IDataAccess>();
        
        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        systemUnderTest.LoadLearningWorld("foobar");

        mockDataAccess.Received().LoadLearningWorldFromFile("foobar");
    }

    [Test]
    public void BusinessLogic_LoadLearningWorld_ReturnsLearningWorld()
    {
        IDataAccess mockDataAccess = Substitute.For<IDataAccess>();
        var learningWorld = new LearningWorld("fa", "a", "f", "f", "f", "f");
        mockDataAccess.LoadLearningWorldFromFile("foobar").Returns(learningWorld);
        
        var systemUnderTest = CreateStandardBusinessLogic(null, mockDataAccess);

        var learningWorldActual = systemUnderTest.LoadLearningWorld("foobar");
        
        Assert.That(learningWorldActual, Is.EqualTo(learningWorld));
    }
    
    private AuthoringTool.BusinessLogic.API.BusinessLogic CreateStandardBusinessLogic(
        IAuthoringToolConfiguration? fakeConfiguration = null,
        IDataAccess? fakeDataAccess = null)
    {
        fakeConfiguration ??= Substitute.For<IAuthoringToolConfiguration>();
        fakeDataAccess ??= Substitute.For<IDataAccess>();
        return new AuthoringTool.BusinessLogic.API.BusinessLogic(fakeConfiguration, fakeDataAccess);
    }
}