using Generator.DSL;

namespace Generator.XmlClasses;

public interface IXmlEntityManager
{
    void GetFactories(IReadDsl readDsl, string dslpath);
    
}