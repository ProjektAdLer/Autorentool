
/*
namespace AuthoringToolTest.DataAccess.WorldExport.XmlClasses;

[TestFixture]
public class XmlSectionFactoryUt
{
    
    [Test]
    public void XmlSectionFactory_Constructor_AllPropertiesSet()
    {
        //Arrange
        
        //Act
        var xmlSectionFactory = CreateStandardXmlSectionFactory();

        //Assert
        Assert.That(xmlSectionFactory.SectionsSectionXmlSection, Is.Not.Null);
    }
    
    [Test]
    public void CreateSectionSectionXml_Default_ParametersSetAndSerialized()
    {
        //Arrange 
        var mockSectionXmlSection = Substitute.For<ISectionsSectionXmlSection>();
        var xmlSectionFactory = CreateTestableXmlCourseFactory(mockSectionXmlSection);

        //Act
        xmlSectionFactory.CreateSectionSectionXml();
        
        //Assert
        mockSectionXmlSection.Received().SetParameters(Arg.Any<string>(), Arg.Any<string>());
        mockSectionXmlSection.Received().Serialize();

    }
    
    public XmlSectionFactory CreateStandardXmlSectionFactory()
    {
        return new XmlSectionFactory();
    }

    public XmlSectionFactory CreateTestableXmlCourseFactory(ISectionsSectionXmlSection sectionsSectionXmlSection = null)
    {
        sectionsSectionXmlSection ??= Substitute.For<ISectionsSectionXmlSection>();
       
        return new XmlSectionFactory(sectionsSectionXmlSection);
    }
}*/