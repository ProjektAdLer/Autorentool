using Generator.WorldExport;
using NUnit.Framework;

namespace GeneratorTest.WorldExport;

[TestFixture]
public class UpperCaseUtf8EncodingUt
{
    [Test]
    // ANF-ID: [GHO11]
    public void UpperCaseUTF8Encoding_EncodingIsUpperCase()
    {
        //Arrange 

        //Act
        var upperCase = new UpperCaseUtf8Encoding();

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(upperCase.BodyName, Is.EqualTo("utf-8"));
            Assert.That(upperCase.WebName, Is.EqualTo("UTF-8"));
        });
    }
}