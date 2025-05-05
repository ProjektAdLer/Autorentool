using TestContext = Bunit.TestContext;
namespace H5pPlayerTest.Presentation.View;


[TestFixture]
public class DisplayH5pViewUt
{
    
    private TestContext _testContext;




    
    

    
    
    [SetUp]
    public void Setup()
    {
        _testContext = new TestContext();
        
  
    }

    [TearDown]
    public void TearDown()
    {
        _testContext.Dispose();
    }

}