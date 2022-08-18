using AuthoringTool.DataAccess.DSL;

namespace AuthoringTool.DataAccess;

public interface IXmlEntityManager
{
    void GetFactories(IReadDsl readDsl, string dslpath);
    
}