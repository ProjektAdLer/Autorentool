namespace Generator.XmlClasses.XmlFileFactories;

public interface IXmlResourceFactory
{
    /// <summary>
    /// Creates a resource factory, reading a list of resource elements and setting parameters accordingly.
    /// This method also manages XML file lists.
    /// </summary>
    void CreateResourceFactory();
}