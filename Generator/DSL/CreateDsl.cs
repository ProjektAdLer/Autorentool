using System.IO.Abstractions;
using System.Text.Json;
using Generator.WorldExport;
using PersistEntities;

namespace Generator.DSL;

public class CreateDsl : ICreateDsl
{
    public readonly List<LearningElementPe> ListLearningElements;
    public List<LearningSpacePe> ListLearningSpaces;
    public List<LearningElementJson> ListLearningElementsJson;
    private readonly List<int> _listLearningSpaceContent;
    private readonly IFileSystem _fileSystem;
    private readonly string _uuid;

    /// <summary>
    /// Read the AuthoringToolLib Entities and create a Dsl Document with a specified syntax.
    /// </summary>
    /// <param name="fileSystem"></param>
    public CreateDsl(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
        ListLearningElementsJson = new List<LearningElementJson>();
        ListLearningElements = new List<LearningElementPe>();
        ListLearningSpaces = new List<LearningSpacePe>();
        _listLearningSpaceContent = new List<int>();
        Guid guid = Guid.NewGuid();
        _uuid = guid.ToString();
    }
    
    /// <summary>
    /// Reads the LearningWord Entity and creates an DSL Document with the given information.
    /// </summary>
    /// <param name="learningWorld"></param> Information about the learningWorld, topics, spaces and elements
    public string WriteLearningWorld(LearningWorldPe learningWorld)
    {
        
        //Initialise learningWorldJson with empty values, they will be filled with information later in the method.
        var learningWorldJson = new LearningWorldJson(_uuid, new IdentifierJson("name", learningWorld.Name), new List<int>(),
            new List<TopicJson>(), new List<LearningSpaceJson>(), new List<LearningElementJson>());


        // Create Learning Spaces & fill into Learning World
        // The learningSpaceId defines what the starting Id for Spaces should be. 
        // Search for Learning Elements in Spaces and add to listLearningElements
        if (learningWorld.LearningSpaces != null) ListLearningSpaces = learningWorld.LearningSpaces;
        int learningSpaceId = 1;
        int learningElementId_Space = 1;
        foreach (var learningSpace in ListLearningSpaces)
        {
            IdentifierJson learningSpaceIdentifier = new IdentifierJson("name", learningSpace.Name);

            //Searching for Learning Elements in each Space
            foreach (var element in learningSpace.LearningElements)
            {
                IdentifierJson learningElementIdentifier = new IdentifierJson("FileName", element.Name);
                LearningElementJson learningElementJson = new LearningElementJson(learningElementId_Space,
                    learningElementIdentifier, element.Content.Type, learningSpaceId);
                ListLearningElementsJson.Add(learningElementJson);
                ListLearningElements.Add(element);
                //int elementIndex = ListLearningElements.IndexOf(element) + 1;
                _listLearningSpaceContent.Add(learningElementId_Space);
                learningElementId_Space++;
                learningWorldJson.LearningElements.Add(learningElementJson);
            }

            LearningSpaceJson learningSpaceJson = new LearningSpaceJson(learningSpaceId, learningSpace.Name,
                learningSpaceIdentifier, _listLearningSpaceContent);

            learningSpaceId++;

            // Add Learning Space to Learning World
            learningWorldJson.LearningSpaces.Add(learningSpaceJson);
        }


        // Create Learning Elements & fill into Learning World
        // This Part only adds learning Elements which are in the Learning World (not into Spaces)
        // (the List includes Elements from Spaces & learning World)
        // learningElementId defines what the starting Id for Elements should be. 
        /*foreach (var element in learningWorld.LearningElements)
        {
            ListLearningElements.Add(element);
        }
        int learningElementId = 1;*/

        foreach (var element in learningWorld.LearningElements)
        {
            /*if (element.Content.Type == ".h5p")
            {
                element.Content.Type = "H5P";
            }*/

            IdentifierJson learningElementIdentifier = new IdentifierJson("FileName", element.Name);

            LearningElementJson learningElementJson = new LearningElementJson(learningElementId_Space,
                learningElementIdentifier, element.Content.Type, 0);

            learningElementId_Space++;
            
            //Add Learning Elements to Learning World
            learningWorldJson.LearningElements.Add(learningElementJson);
            ListLearningElements.Add(element);
        }
        
        //Add another Element to the LearningElementList, representation for the DSL Document
        IdentifierJson dslDocumentIdentifier = new IdentifierJson("FileName", "DSL Dokument");

        LearningElementJson dslDocumentJson = new LearningElementJson(learningElementId_Space, dslDocumentIdentifier, "json", 0);

        learningWorldJson.LearningElements.Add(dslDocumentJson);

        // Create DocumentRoot & JSON Document
        // And add the learningWorldJson to the DocumentRoot
        // The structure of the DSL needs DocumentRoot, because the learningWorld has its own tag
        DocumentRootJson rootJson = new DocumentRootJson(learningWorldJson);

        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonFile = JsonSerializer.Serialize(rootJson,options);
        
        //Create Backup Folder structure and the DSL Document in it
        BackupFileGenerator createFolders = new BackupFileGenerator(_fileSystem);
        createFolders.CreateBackupFolders();
        
        //All LearningElements are created at the specified location = Easier access to files in further Export-Operations.
        //After the files are added to the Backup-Structure, these Files will be deleted.
        foreach (var learningElement in ListLearningElements)
        {
            _fileSystem.File.WriteAllBytes(_fileSystem.Path.Join("XMLFilesForExport", learningElement.Name), learningElement.Content.Content);
        }
        var dslPath = _fileSystem.Path.Join("XMLFilesForExport", "DSL_Document.json");
        _fileSystem.File.WriteAllText(dslPath, jsonFile);
        Console.WriteLine(jsonFile);
        return dslPath;
    }
}