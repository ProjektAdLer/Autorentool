namespace Generator.DSL;

/// <summary>
/// The DocumentRoot only needs the world
/// This class is needed, because the definition of the Dsl-Structure has the World as its own tag. 
/// </summary>
public class DocumentRootJson : IDocumentRootJson
{
    public DocumentRootJson(WorldJson world)
    {
        World = world;
    }

    public WorldJson World { get; set; }
    
}