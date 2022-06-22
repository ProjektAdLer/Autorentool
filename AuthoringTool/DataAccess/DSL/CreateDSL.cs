using System.IO.Abstractions;
using System.Text.Json;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.Entities;
using FileSystem = System.IO.Abstractions.FileSystem;

namespace AuthoringTool.DataAccess.DSL;

public class CreateDSL : ICreateDSL
{
    
    public List<LearningElement>? listLearningElements;
    public List<LearningSpace>? listLearningSpaces;
    public List<int>? listLearningSpaceContent;
    public LearningWorldJson? learningWorldJson;
    private readonly IFileSystem _fileSystem;

    public CreateDSL(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }


    /// <summary>
    /// Reads the LearningWord Entity and creates an DSL Document with the given information.
    /// </summary>
    /// <param name="learningWorld"></param> Information about the learningWorld, topics, spaces and elements
    public string WriteLearningWorld(LearningWorld learningWorld)
    {
        
        //Create Empty Learning World, 
        //learningWorldJson will be filled with information later
        learningWorldJson = new LearningWorldJson()
        {
            identifier = new IdentifierJson()
            {
                type = "name",
                value = learningWorld.Name
            },
            learningWorldContent = new List<int>(),
            topics = new List<TopicJson>(),
            learningSpaces = new List<LearningSpaceJson>(),
            learningElements = new List<LearningElementJson>(),
        };
        
        
        // Create Learning Spaces & fill into Learning World
        // Search for Learning Elements in Spaces and add to listLearningElements
        listLearningElements = new List<LearningElement>();
        listLearningSpaces = new List<LearningSpace>();
        listLearningSpaces = learningWorld.LearningSpaces;
        listLearningSpaceContent = new List<int>();
        int learningSpaceId = 1;

        if (listLearningSpaces != null)
            foreach (var learningSpace in listLearningSpaces)
            {
                IdentifierJson learningSpaceIdentifier = new IdentifierJson()
                {
                    type = "name",
                    value = learningSpace.Name
                };

                //Which Learning Elements are in each Space
                foreach (var element in learningSpace.LearningElements)
                {
                    listLearningElements.Add(element);
                    int elementIndex = listLearningElements.IndexOf(element)+1;
                    listLearningSpaceContent.Add(elementIndex);
                }

                LearningSpaceJson learningSpaceJson = new LearningSpaceJson()
                {
                    spaceId = learningSpaceId,
                    identifier = learningSpaceIdentifier,
                    learningSpaceName = learningSpace.Name,
                    learningSpaceContent = listLearningSpaceContent
                };

                learningSpaceId++;

                // Add Learning Space to Learning World
                learningWorldJson.learningSpaces.Add(learningSpaceJson);
            }


        // Create Learning Elements & Fill into Learning World
        // This Part only adds learning Elements which are in the Learning World (not into Spaces)
        // But it creates all elements in the List (the List includes Elements from Spaces & learning World)
        foreach (var element in learningWorld.LearningElements)
        {
            listLearningElements.Add(element);
        }
        int learningElementId = 1;

        foreach (var learningElement in listLearningElements)
        {
            if (learningElement.Content.Type == ".h5p")
            {
                learningElement.Content.Type = "H5P";
            }
            IdentifierJson learningElementIdentifier = new IdentifierJson()
            {
                type = "FileName",
                value = learningElement.Name
            };

            LearningElementJson learningElementJson = new LearningElementJson()
            {
                id = learningElementId,
                identifier = learningElementIdentifier,
                elementType = learningElement.Content.Type,
            };
            
            learningElementId++;
            
            //Add Learning Elements to Learning World
            learningWorldJson.learningElements.Add(learningElementJson);
        }
        
        //Add another Element to the LearningElementList, representation for the DSL Document
        IdentifierJson dslDocumentIdentifier = new IdentifierJson()
        {
            type = "FileName",
            value = "DSL Dokument"
        };

        LearningElementJson dslDocumentJson = new LearningElementJson()
        {
            id = learningElementId,
            identifier = dslDocumentIdentifier,
            elementType = "json",
        };
        
        learningWorldJson.learningElements.Add(dslDocumentJson);


        // Create DocumentRoot & JSON Document
        // And add the learningWorldJson to the DocumentRoot
        // The structure of the DSL needs DocumentRoot, because the learningWorld has its own tag
        DocumentRootJson rootJson = new DocumentRootJson()
        {
            learningWorld = learningWorldJson
        };
        
        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonFile = JsonSerializer.Serialize(rootJson,options);
        
        //Create Backup Folder structure and the DSL Document in it
        BackupFileGenerator createFolders = new BackupFileGenerator(_fileSystem);
        createFolders.CreateBackupFolders();
        foreach (var learningElement in listLearningElements)
        {
            _fileSystem.File.WriteAllBytes(_fileSystem.Path.Join("XMLFilesForExport", learningElement.Name), learningElement.Content.Content);
        }
        var dslPath = _fileSystem.Path.Join("XMLFilesForExport", "DSL_Document.json");
        _fileSystem.File.WriteAllText(dslPath, jsonFile);
        Console.WriteLine(jsonFile);
        return dslPath;

    }
}