using AuthoringTool.DataAccess.DSL;

namespace AuthoringTool.DataAccess;

public interface IXmlEntityManager
{
    void GetFactories(IReadDSL readDsl, string dslpath);
    
}