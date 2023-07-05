namespace Generator.XmlClasses.XmlFileFactories;

public interface IXmlH5PFactory
{
    /// <summary>
    /// Create H5P structure in files.xml, folder activity and folder sections for every H5P element in the DSL Document
    /// </summary>
    void CreateH5PFileFactory();
}