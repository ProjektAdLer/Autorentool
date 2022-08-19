using System.Xml;
using Generator.WorldExport;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.WorldExport;

[TestFixture]
public class UpperCaseUTF8EncodingUt
{
    [Test]
    public void UpperCaseUTF8Encoding_EncodingIsUpperCase()
    {
        //Arrange 

        //Act
        var upperCase = new UpperCaseUtf8Encoding();
        
        var settings = new XmlWriterSettings
        {
            Encoding = new UpperCaseUtf8Encoding(), // Moodle needs Encoding in Uppercase!
            NewLineHandling = NewLineHandling.Replace,
            NewLineOnAttributes = true,
            Indent = true // Generate new lines for each element
        };
        
        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(upperCase.BodyName,Is.EqualTo("utf-8"));
            Assert.That(upperCase.WebName,Is.EqualTo("UTF-8"));
        });
      
    }
}