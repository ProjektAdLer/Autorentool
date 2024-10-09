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
        Assert.That(systemUnderTest.UnzippedH5psPath, Is.EqualTo(string.Empty));
    }
    
   
    private static H5pEntity CrateDefaultSystemUnderTest()
    {
        return new H5pEntity();
    }
}