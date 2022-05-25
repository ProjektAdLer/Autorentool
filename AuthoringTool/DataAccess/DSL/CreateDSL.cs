using System.Text.Json;
using AuthoringTool.DataAccess.WorldExport;
using AuthoringTool.Entities;
using Microsoft.VisualBasic;

namespace AuthoringTool.DataAccess.DSL;

public class CreateDSL : ICreateDSL
{
    
    private List<LearningElement> listLearningElements;
    private List<LearningSpace> listLearningSpaces;
    private List<int> listLearningSpaceContent;


    public void WriteLearningWorld(LearningWorld learningWorld)
    {

        //Create Empty Learning World
        LearningWorldJson learningWorldJson = new LearningWorldJson()
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
        
        
        // Create Learning Spaces & Fill into Learning World
        // Search for Learning Elements in Spaces and add to listLearningElements
        listLearningElements = new List<LearningElement>();
        listLearningSpaces = new List<LearningSpace>();
        listLearningSpaces = learningWorld.LearningSpaces;
        listLearningSpaceContent = new List<int>();
        int learningSpaceId = 0;

        foreach (var learningSpace in listLearningSpaces)
        {
            IdentifierJson learningSpaceIdentifier = new IdentifierJson()
            {
                type = "name",
                value = learningSpace.Name
            };

            var somth = learningSpace.LearningElements;

            //Which Learning Elements are in each Space
            foreach (var element in learningSpace.LearningElements)
            {
                listLearningElements.Add(element);
                int elementIndex = listLearningElements.IndexOf(element);
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
        
        
        //Create Learning Elements & Fill into Learning World
        // This Part only adds learning Elements which are in the Learning World (not into Spaces)
        // But it creates all ELements in the List (this includes Spaces & learning World)

        foreach (var element in learningWorld.LearningElements)
        {
            listLearningElements.Add(element);
        }
        int learningElementId = 0;

        foreach (var learningElement in listLearningElements)
        {
            IdentifierJson learningElementIdentifier = new IdentifierJson()
            {
                type = "FileName",
                value = learningElement.Name
            };

            LearningElementJson learningElementJson = new LearningElementJson()
            {
                id = learningElementId,
                identifier = learningElementIdentifier,
                //elementType = learningElement.ContentType,
            };
            
            learningElementId++;
            
            //Add Learning Elements to Learning World
            learningWorldJson.learningElements.Add(learningElementJson);
        }
        
        
        
        

        // Create DocumentRoot & JSON Document
        DocumentRootJson rootJson = new DocumentRootJson()
        {
            learningWorld = learningWorldJson
        };
        
        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonFile = JsonSerializer.Serialize(rootJson,options);
        
        //Create Filepath
        BackupFileGenerator createFolders = new BackupFileGenerator();
        createFolders.CreateBackupFolders();
        File.WriteAllText("XMLFilesForExport/DSL_Document.json", jsonFile);
        Console.WriteLine(jsonFile);



    }
}