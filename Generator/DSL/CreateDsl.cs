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

    /// <summary>
    /// Read the AuthoringToolLib Entities and create a Dsl Document with a specified syntax.
    /// </summary>
    /// <param name="fileSystem"></param>
#pragma warning disable CS8618 @Dimitri_Bigler Lists are always initiated, Constructor just doesnt know.
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
            new List<TopicJson>(), new List<LearningSpaceJson>(), new List<LearningElementJson>());

        // All learningElements that have no learningSpace are added to the learningWorld (With the LearningSpaceParentId=0)
        var learningWorldElements = new LearningSpacePe("Freie Lernelemente", "FEE", "Dimitri",
            "Diese Lernelemente sind keinem Lernraum zugeordnet", "", learningWorld.LearningElements,1,1);
        ListLearningSpaces.Add(learningWorldElements);
        
        // Create Learning Spaces & fill into Learning World
        // The learningSpaceId defines what the starting Id for Spaces should be. 
        // Search for Learning Elements in Spaces and add to listLearningElements
        if (learningWorld.LearningSpaces != null) ListLearningSpaces.AddRange(learningWorld.LearningSpaces);
        
        int learningSpaceId = 0;
        // Starts with 2, because the DSL Document always has Element ID = 1. Therefore all other elements have to start with 2.
        int learningSpaceElementId = 2;
        foreach (var learningSpace in ListLearningSpaces)
        {
            _listLearningSpaceContent = new List<int>();
            IdentifierJson learningSpaceIdentifier = new IdentifierJson("name", learningSpace.Name);
            
            //Add the DSL Document to the first LearningSpace
            if (learningSpaceId == 0)
            {
                IdentifierJson dslDocumentIdentifier = new IdentifierJson("FileName", "DSL_Document");
                LearningElementJson dslDocumentJson = new LearningElementJson(1, dslDocumentIdentifier, "json", 0);
                LearningWorldJson.LearningElements.Add(dslDocumentJson);
                _listLearningSpaceContent.Add(1);
            }
            //Searching for Learning Elements in each Space
            foreach (var element in learningSpace.LearningElements)
            {
                IdentifierJson learningElementIdentifier = new IdentifierJson("FileName", element.Name);
                LearningElementJson learningElementJson = new LearningElementJson(learningSpaceElementId,
                    learningElementIdentifier, element.LearningContent.Type, learningSpaceId);
                ListLearningElements.Add(element);
                //int elementIndex = ListLearningElements.IndexOf(element) + 1;
                _listLearningSpaceContent.Add(learningSpaceElementId);
                learningSpaceElementId++;
                LearningWorldJson.LearningElements.Add(learningElementJson);
            }

            
            // Add Learning Space to Learning World
            LearningWorldJson.LearningSpaces.Add(new LearningSpaceJson(learningSpaceId, learningSpace.Name,
                learningSpaceIdentifier, _listLearningSpaceContent));

            learningSpaceId++;
        }

        // Create DocumentRoot & JSON Document
        // And add the learningWorldJson to the DocumentRoot
        // The structure of the DSL needs DocumentRoot, because the learningWorld has its own tag
        DocumentRootJson rootJson = new DocumentRootJson(LearningWorldJson);

        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonFile = JsonSerializer.Serialize(rootJson,options);
        
        //Create Backup Folder structure and the DSL Document in it
        BackupFileGenerator createFolders = new BackupFileGenerator(_fileSystem);
        createFolders.CreateBackupFolders();
        
        //All LearningElements are created at the specified location = Easier access to files in further Export-Operations.
        //After the files are added to the Backup-Structure, these Files will be deleted.
        foreach (var learningElement in ListLearningElements)
        {
            _fileSystem.File.WriteAllBytes(_fileSystem.Path.Join("XMLFilesForExport", learningElement.Name), learningElement.LearningContent.Content);
        }
        var dslPath = _fileSystem.Path.Join("XMLFilesForExport", "DSL_Document");
        _fileSystem.File.WriteAllText(dslPath, jsonFile);
        Console.WriteLine(jsonFile);
        return dslPath;
    }
}