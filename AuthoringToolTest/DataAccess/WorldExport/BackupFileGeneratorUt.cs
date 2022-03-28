using AuthoringTool.DataAccess.XmlClasses;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.WorldExport;

[TestFixture]
public class BackupFileGeneratorUt
{
    [Test]
    public void WorldExport_WriteXMLFiles_AllFilesCreated()
    {
        //Arrange (Initialize Objects) 
        var xmlEntityManager = new XmlEntityManager();

        //Act
        xmlEntityManager.GetFactories();
        
        //Assert 

    }
    
    
}