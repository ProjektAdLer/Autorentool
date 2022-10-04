using System.IO.Abstractions;
using System.Text.Json;
using Generator.WorldExport;
using PersistEntities;

namespace Generator.DSL;

public class CreateDsl : ICreateDsl
{
    public List<LearningElementPe> ListLearningElements;
    public List<LearningSpacePe> ListLearningSpaces;
    public LearningWorldJson? LearningWorldJson;
    private List<int> _listLearningSpaceContent;
    private IFileSystem _fileSystem;
    public string Uuid;
    private string _dslPath;
    private string _xmlFilesForExportPath;

    /// <summary>
    /// Read the AuthoringToolLib Entities and create a Dsl Document with a specified syntax.
    /// </summary>
    /// <param name="fileSystem"></param>
#pragma warning disable CS8618 //@Dimitri_Bigler Lists are always initiated, Constructor just doesnt know.
    public CreateDsl(IFileSystem fileSystem)
#pragma warning restore CS8618
    {
        Initialize();
        _fileSystem = fileSystem;
       
    }

    private void Initialize()
    {
        ListLearningElements = new List<LearningElementPe>();
        ListLearningSpaces = new List<LearningSpacePe>();
        _listLearningSpaceContent = new List<int>();
        Guid guid = Guid.NewGuid();
        Uuid = guid.ToString();
    }
    
    /// <summary>
    /// Reads the LearningWord Entity and creates an DSL Document with the given information.
    /// </summary>
    /// <param name="learningWorld"></param> Information about the learningWorld, topics, spaces and elements
    public string WriteLearningWorld(LearningWorldPe learningWorld)
    {
        Initialize();
        
        //Initialise learningWorldJson with empty values, will be filled with information later in the method.
        LearningWorldJson = new LearningWorldJson(Uuid, new IdentifierJson("name", learningWorld.Name), new List<int>(),
            new List<TopicJson>(), new List<LearningSpaceJson>(), new List<LearningElementJson>(), 
            learningWorld.Description, learningWorld.Goals);

        // Create Learning Spaces & fill into Learning World
        // The learningSpaceId defines what the starting Id for Spaces should be. 
        // Search for Learning Elements in Spaces and add to listLearningElements
        ListLearningSpaces.AddRange(learningWorld.LearningSpaces);
        
        int learningSpaceId = 1;
        // Starts with 2, because the DSL Document always has Element ID = 1. Therefore all other elements have to start with 2.
        int learningSpaceElementId = 1;
        foreach (var learningSpace in ListLearningSpaces)
        {
            _listLearningSpaceContent = new List<int>();
            IdentifierJson learningSpaceIdentifier = new IdentifierJson("name", learningSpace.Name);
            
            //Searching for Learning Elements in each Space
            foreach (var element in learningSpace.LearningElements)
            {
                var elementCategory = "";
                switch (element.LearningContent.Type)
                {
                    case "png" or "jpg" or "bmp" or "webp":
                        elementCategory = "image";
                        break;
                    case "mp4":
                        elementCategory = "video";
                        break;
                    case "txt" or "c" or "h" or "cpp" or "cc" or "c++" or "py" or "js" or "php" or "html" or "css":
                        elementCategory = "text";
                        break;
                    case "h5p":
                        elementCategory = "h5p";
                        break;
                    case "pdf":
                        elementCategory = "pdf";
                        break;
                    default:
                        ArgumentException e = new ArgumentException("The given LearningContent Type is not supported - in CreateDsl.");
                        break;
                }
                IdentifierJson learningElementIdentifier = new IdentifierJson("FileName", element.Name);
                List<LearningElementValueJson> learningElementValueList = new List<LearningElementValueJson>();
                LearningElementValueJson learningElementValueJson = new LearningElementValueJson("Points", element.Points);
                learningElementValueList.Add(learningElementValueJson);

                LearningElementJson learningElementJson = new LearningElementJson(learningSpaceElementId,
                    learningElementIdentifier, "", elementCategory,element.LearningContent.Type, learningSpaceId, learningElementValueList, 
                    element.Description, element.Goals);
                ListLearningElements.Add(element);
                //int elementIndex = ListLearningElements.IndexOf(element) + 1;
                _listLearningSpaceContent.Add(learningSpaceElementId);
                learningSpaceElementId++;
                LearningWorldJson.LearningElements.Add(learningElementJson);
            }
            
            
            // Add Learning Space to Learning World
            LearningWorldJson.LearningSpaces.Add(new LearningSpaceJson(learningSpaceId,
                learningSpaceIdentifier, _listLearningSpaceContent, 
                learningSpace.RequiredPoints, 
                learningSpace.LearningElements.Sum(element => element.Points),
                learningSpace.Description, learningSpace.Goals));

            learningSpaceId++;
        }

        // Create DocumentRoot & JSON Document
        // And add the learningWorldJson to the DocumentRoot
        // The structure of the DSL needs DocumentRoot, because the learningWorld has its own tag
        DocumentRootJson rootJson = new DocumentRootJson(LearningWorldJson);

        var options = new JsonSerializerOptions { WriteIndented = true,  PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
        var jsonFile = JsonSerializer.Serialize(rootJson,options);
        
        //Create Backup Folder structure and the DSL Document in it
        var currentDirectory = _fileSystem.Directory.GetCurrentDirectory();
        _xmlFilesForExportPath = _fileSystem.Path.Join(currentDirectory, "XMLFilesForExport");
        _dslPath = _fileSystem.Path.Join(currentDirectory, "XMLFilesForExport", "DSL_Document.json");
        
        if (_fileSystem.Directory.Exists(_xmlFilesForExportPath))
        {
            _fileSystem.Directory.Delete(_xmlFilesForExportPath, true);
        }
        
        BackupFileGenerator createFolders = new BackupFileGenerator(_fileSystem);
        createFolders.CreateBackupFolders();
        
        //All LearningElements are created at the specified location = Easier access to files in further Export-Operations.
        //After the files are added to the Backup-Structure, these Files will be deleted.
        foreach (var learningElement in ListLearningElements)
        {
            _fileSystem.File.WriteAllBytes(_fileSystem.Path.Join("XMLFilesForExport", learningElement.Name), learningElement.LearningContent.Content);
        }

        _fileSystem.File.WriteAllText(_dslPath, jsonFile);
        Console.WriteLine(jsonFile);
        return _dslPath;
    }
}