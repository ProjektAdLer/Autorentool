using System.IO.Abstractions;
using System.Text.Json;
using FileSystem = System.IO.Abstractions.FileSystem;

namespace AuthoringTool.DataAccess.DSL;

public class ReadDSL : IReadDSL
{
    public List<LearningElementJson>? ListH5PElements;
    public LearningWorldJson? LearningWorldJson;
    private string filepathDSL;
    private IFileSystem _fileSystem;

    public void ReadLearningWorld(string dslPath, IFileSystem? fileSystem=null)
    {
        _fileSystem = fileSystem?? new FileSystem();
        filepathDSL = dslPath;
        
        ListH5PElements = new List<LearningElementJson>();
        string jsonString = _fileSystem.File.ReadAllText(filepathDSL);
        DocumentRootJson? rootJson = JsonSerializer.Deserialize<DocumentRootJson>(jsonString);
        GetH5PElements(rootJson);
        SetLearningWorld(rootJson);
    }

    private void SetLearningWorld(DocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null) LearningWorldJson = documentRootJson.learningWorld;
    }

    public LearningWorldJson? GetLearningWorld()
    {
        return LearningWorldJson;
    }
    
    private void GetH5PElements(DocumentRootJson? documentRootJson)
    {
        if (documentRootJson != null && documentRootJson.learningWorld != null)
            if (documentRootJson.learningWorld.learningElements != null)
                foreach (var element in documentRootJson.learningWorld.learningElements)
                {
                    if (element.elementType == "H5P")
                    {
                        if (ListH5PElements != null) ListH5PElements.Add(element);
                    }
                }
    }

    public List<LearningElementJson>? GetH5PElementsList()
    {
        return ListH5PElements;
    }
    
    
}