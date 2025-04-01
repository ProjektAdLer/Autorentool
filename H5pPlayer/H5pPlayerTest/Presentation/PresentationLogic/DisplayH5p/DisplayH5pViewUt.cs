using H5pPlayer.Presentation.PresentationLogic.DisplayH5p;
using H5pPlayer.Presentation.View;
using NSubstitute;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using TestContext = Bunit.TestContext;
namespace H5pPlayerTest.Presentation.PresentationLogic.DisplayH5p;


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