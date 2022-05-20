using System.Text.Json;
using AuthoringTool.Entities;

namespace AuthoringTool.DataAccess.DSL;

public class CreateDSL : ICreateDSL
{
    
    private List<LearningElement> listLearningElements;
    private List<LearningSpace> listLearningSpaces;
    

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
        
        //Create Learning Elements & Fill into Learning World
        listLearningElements = new List<LearningElement>();
        listLearningElements = learningWorld.LearningElements;
        int learningElementId = 0;

        foreach (var learningElement in listLearningElements)
        {
            IdentifierJson learningElementIdentifier = new IdentifierJson()
            {
                type = "FileName",
                value = learningElement.Name,
            };
            learningElementId++;

            LearningElementJson learningElementJson = new LearningElementJson()
            {
                id = learningElementId,
                identifier = learningElementIdentifier,
                elementType = learningElement.ContentType,
            };
            
            //Add Learning Elements to Learning World
            learningWorldJson.learningElements.Add(learningElementJson);
        }
        
        // Create Learning Spaces & Fill into Learning World
        listLearningSpaces = new List<LearningSpace>();
        listLearningSpaces = learningWorld.LearningSpaces;
        int learningSpaceId = 0;

        foreach (var learningSpace in listLearningSpaces)
        {
            IdentifierJson learningSpaceIdentifier = new IdentifierJson()
            {
                type = "name",
                value = learningSpace.Name,
            };
            learningSpaceId++;

            LearningSpaceJson learningSpaceJson = new LearningSpaceJson()
            {
                spaceId = learningSpaceId,
                identifier = learningSpaceIdentifier,
                learningSpaceName = learningSpace.Name,
            };
            
            // Add Learning Space to Learning World
            learningWorldJson.learningSpaces.Add(learningSpaceJson);
        }
        
        

        // Create DocumentRoot & JSON Document
        DocumentRootJson rootJson = new DocumentRootJson()
        {
            learningWorld = learningWorldJson
        };
        
        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonFile = JsonSerializer.Serialize(rootJson,options);

        Console.WriteLine(jsonFile);



    }
}