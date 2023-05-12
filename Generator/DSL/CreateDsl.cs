using System.IO.Abstractions;
using System.Text.Json;
using Generator.WorldExport;
using Microsoft.Extensions.Logging;
using PersistEntities;
using PersistEntities.LearningContent;
using Shared.Extensions;

namespace Generator.DSL;

public class CreateDsl : ICreateDsl
{
    public List<ILearningElementPe> ElementsWithFileContent;
    public List<LearningSpacePe> ListLearningSpaces;
    public List<TopicPe> ListTopics;
    public LearningWorldJson LearningWorldJson;
    public string Uuid;
    public Dictionary<int, Guid> DictionarySpaceIdToGuid;
    private List<int> _listLearningSpaceElements;
    private List<ILearningElementPe> _listAllLearningElements;
    private string _booleanAlgebraRequirements;
    private string _currentConditionSpace;
    private string _author;
    private string _language;
    private IFileSystem _fileSystem;
    private string _dslPath;
    private string _xmlFilesForExportPath;
    private ILogger<CreateDsl> Logger { get; }

    /// <summary>
    /// Read the PersistEntities and create a Dsl Document with a specified syntax.
    /// </summary>
    /// <param name="fileSystem"></param>
    /// <param name="logger"></param>
#pragma warning disable CS8618 //@Dimitri_Bigler Lists are always initiated, Constructor just doesnt know.
    public CreateDsl(IFileSystem fileSystem, ILogger<CreateDsl> logger)
#pragma warning restore CS8618
    {
        Initialize();
        _fileSystem = fileSystem;
        Logger = logger;
    }

    private void Initialize()
    {
        ElementsWithFileContent = new List<ILearningElementPe>();
        ListLearningSpaces = new List<LearningSpacePe>();
        ListTopics = new List<TopicPe>();
        _listLearningSpaceElements = new List<int>();
        _booleanAlgebraRequirements = "";
        DictionarySpaceIdToGuid = new Dictionary<int, Guid>();
        Guid guid = Guid.NewGuid();
        Uuid = guid.ToString();
        _currentConditionSpace = "";
        _listAllLearningElements = new List<ILearningElementPe>();
    }

    //Search through all LearningElements and look for duplicates. 
    //If a duplicate is found, the duplicate Values get a incremented Number behind them for example: (1), (2)...
    public List<LearningSpacePe> SearchDuplicateLearningElementNames(List<LearningSpacePe> listLearningSpace)
    {
        var dictionaryIncrementedElementNames = new Dictionary<string,string>();
        
        //Get All LearningElements
        foreach (var space in listLearningSpace)
        {
            foreach (var element in space.LearningSpaceLayout.ContainedLearningElements)
            {
                _listAllLearningElements.Add(element);
            }
        }
        
        //Search for duplicates
        var duplicateLearningElements = _listAllLearningElements.GroupBy(x => x.Name).Where(x => x.Count() > 1)
            .Select(x => x).ToList();

        //To avoid duplicate names, we increment the name of the learning element.
        //That happens in yet another loop, because we have to respect the Space -> Element hierarchy.
        foreach (var duplicateElement in duplicateLearningElements)
        {
            foreach (var learningSpace in listLearningSpace)
            {
                foreach (var element in learningSpace.LearningSpaceLayout.ContainedLearningElements)
                {
                    if(element.Name == duplicateElement.Key)
                    {
                        string incrementedElementName;
                        //Increment LearningElement names, if they are already in the dictionary
                        if (dictionaryIncrementedElementNames.ContainsKey(element.Name))
                        {
                            incrementedElementName = StringHelper.IncrementName(dictionaryIncrementedElementNames[element.Name]);
                            dictionaryIncrementedElementNames[element.Name] = incrementedElementName;
                        }
                        //If LearningElement ist not in the dictionary, increment the name and add it to the dictionary.
                        else
                        {
                            incrementedElementName = StringHelper.IncrementName(element.Name);
                            dictionaryIncrementedElementNames.Add(element.Name, incrementedElementName);
                        }
                        
                        element.Name = incrementedElementName;
                    }
                }
            }
        }
        return listLearningSpace;
    }

    /// <summary>
    /// Takes a Condition and builds a boolean algebra string.
    /// Method searches recursively for all conditions and their inbound Spaces.
    /// </summary>
    /// <param name="learningCondition"></param>
    /// <returns>A string that describes a boolean algebra expression</returns>
    public string DefineLogicalExpression(PathWayConditionPe learningCondition)
    {
        string condition = learningCondition.Condition.ToString();
        if(condition == "And")
        {
            condition = "^";
        }
        else if(condition == "Or")
        {
            condition = "v";
        }
        
        foreach (var learningObject in learningCondition.InBoundObjects)
        {
            if(learningObject is LearningSpacePe)
            {
                _currentConditionSpace += "(";
                string spaceId = DictionarySpaceIdToGuid.Where(x => x.Value == learningObject.Id)
                    .Select(x => x.Key)
                    .FirstOrDefault().ToString();
                _currentConditionSpace += spaceId+")" + condition;
            }
            else if (learningObject is PathWayConditionPe pathWayConditionPe)
            {
                //special case for nested conditions (conditions that are in conditions)
                if (learningObject.InBoundObjects.Count == 1)
                {
                    DefineLogicalExpression(pathWayConditionPe);
                }
                else
                {
                    _currentConditionSpace += "("; 
                    DefineLogicalExpression(pathWayConditionPe);
                    _currentConditionSpace += ")";
                    _currentConditionSpace += condition;
                }
            }
        }
        _currentConditionSpace = _currentConditionSpace.Substring(0, _currentConditionSpace.LastIndexOf(")", StringComparison.Ordinal)+1);
        return _currentConditionSpace;
    }

    /// <summary>
    /// Reads the LearningWorld Entity and creates an DSL Document with the given information.
    /// </summary>
    /// <param name="learningWorld">The learning world to be written to the DSL document</param>
    /// <exception cref="ArgumentOutOfRangeException">The world contains an element whos content type is not supported.</exception>
    /// Information about the learningWorld, topics, spaces and elements
    public string WriteLearningWorld(LearningWorldPe learningWorld)
    {
        Initialize();

        //Setting Authors and Language for DSL Root
        _author = learningWorld.Authors;
        _language = "de";

        //Starting ID for LearningSpaces
        var learningSpaceIdForDictionary = 1;
        
        // Starting Value for Learning Space Ids, Learning Element Ids & Topic Ids in the DSL-Document
        var learningSpaceId = 1;
        var learningSpaceElementId = 1;
        int topicId = 1;
        
        //Initialise learningWorldJson with empty values, will be filled with information later in the method.
        LearningWorldJson = new LearningWorldJson(new LmsElementIdentifierJson("moduleName", learningWorld.Name),
            learningWorld.Name, new List<TopicJson>(), new List<LearningSpaceJson>(),
            new List<LearningElementJson>(), learningWorld.Description, learningWorld.Goals.Split("\n"));

        // Create Learning Spaces & fill into Learning World
        // The learningSpaceId defines what the starting Id for Spaces should be. 
        // Search for Learning Elements in Spaces and add to listLearningElements
        ListLearningSpaces.AddRange(learningWorld.LearningSpaces);
        
        foreach (var space in ListLearningSpaces)
        {
            DictionarySpaceIdToGuid.Add(learningSpaceIdForDictionary, space.Id);
            learningSpaceIdForDictionary++;
        }
        
        //Search for duplicate LearningElement Names and increment them.
        ListLearningSpaces = SearchDuplicateLearningElementNames(ListLearningSpaces);

        ListTopics.AddRange(learningWorld.Topics);
        
        foreach (var topic in ListTopics)
        {
            LearningWorldJson.Topics.Add(new TopicJson(topicId, topic.Name, new List<int>()));
            topicId++;
        }
        
        foreach (var space in ListLearningSpaces)
        {
            _listLearningSpaceElements = new List<int>();
            _booleanAlgebraRequirements = "";
            _currentConditionSpace = "";
            
            var learningSpaceIdentifier = new LmsElementIdentifierJson("name", space.Name);
            
            if (space.AssignedTopic != null)
            {
                var assignedTopic = LearningWorldJson.Topics.Find(topic => topic.TopicName == space.AssignedTopic.Name);
                assignedTopic?.TopicContents.Add(learningSpaceId);
            }
            //Searching for Learning Elements in each Space
            foreach (var element in space.LearningSpaceLayout.ContainedLearningElements)
            {
                var elementType = element.LearningContent switch
                {
                    FileContentPe fileContent => fileContent.Type,
                    LinkContentPe => "link",
                    _ => throw new ArgumentOutOfRangeException()
                };
                var elementCategory = element.LearningContent switch
                {
                    FileContentPe { Type: "png" or "jpg" or "bmp" or "webp" } => "image",
                    FileContentPe
                    {
                        Type: "txt" or "c" or "h" or "cpp" or "cc" or "c++" or "py" or
                        "js" or "php" or "html" or "css"
                    } => "text",
                    FileContentPe { Type: "h5p" } => "h5p",
                    FileContentPe { Type: "pdf" } => "pdf",
                    LinkContentPe => "video",
                    _ => throw new ArgumentException("The given LearningContent Type is not supported - in CreateDsl."),
                };
                var url = element.LearningContent is LinkContentPe link ? link.Link : "";
                
                
                var learningElementIdentifier = new LmsElementIdentifierJson("moduleName", element.Name);

                var learningElementJson = new LearningElementJson(learningSpaceElementId,
                    learningElementIdentifier, element.Name, url, elementCategory, elementType, 
                    learningSpaceId, element.Points, element.Description, element.Goals.Split("\n"));

                // Add Elements that have Content to the List, they will be copied at the end of the method.
                // Every Element without Content will be added to the LearningSpaceJson.
                if (element.LearningContent is not LinkContentPe)
                {
                    ElementsWithFileContent.Add(element);
                }
                
                _listLearningSpaceElements.Add(learningSpaceElementId);
                learningSpaceElementId++;
                LearningWorldJson.Elements.Add(learningElementJson);
            }
          
            // Create Learning Space Requirements
            // If the inbound-type is not a PathWayCondition there can only be 1 LearningSpacePe,
            // so we do not have to construct a boolean algebra expression.
            // The only other thing the inbound-type can be is a PathWayCondition,
            // we have to construct a boolean algebra expression.
            if (space.InBoundObjects.Count > 0)
            {
                foreach (var inbound in space.InBoundObjects)
                {
                    if (inbound is PathWayConditionPe curCondition)
                    {
                        _booleanAlgebraRequirements = DefineLogicalExpression(curCondition);
                    }
                    //It can only be 1 Space that does not have a condition with it.
                    else
                    {
                        _booleanAlgebraRequirements = (DictionarySpaceIdToGuid.Where(x => x.Value == inbound.Id)
                            .Select(x => x.Key)
                            .FirstOrDefault().ToString());
                    }
                }
            }

            // Add the constructed Learning Space to Learning World
            LearningWorldJson.Spaces.Add(new LearningSpaceJson(learningSpaceId,
                learningSpaceIdentifier, space.Name, _listLearningSpaceElements, 
                space.RequiredPoints, 
                space.Description, space.Goals.Split("\n"), _booleanAlgebraRequirements));
            
            learningSpaceId++;
        }

        // Create DocumentRoot & JSON Document
        // And add the learningWorldJson to the DocumentRoot
        // The structure of the DSL needs DocumentRoot, because the learningWorld has its own tag
        var rootJson = new DocumentRootJson("0.3", "0.3.2", _author, _language, LearningWorldJson);

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
        
        var createFolders = new BackupFileGenerator(_fileSystem);
        createFolders.CreateBackupFolders();
        
        //All LearningElements are created at the specified location = Easier access to files in further Export-Operations.
        //After the files are added to the Backup-Structure, these Files will be deleted.
        foreach (var learningElement in ElementsWithFileContent)
        {
            try
            {
                //we know that all elements in this list have a FileContent, so we can safely cast it. - n.stich
                var castedFileContent = (FileContentPe)learningElement.LearningContent;
                _fileSystem.File.Copy(castedFileContent.Filepath,
                    _fileSystem.Path.Join("XMLFilesForExport", $"{learningElement.Name}.{castedFileContent.Type}"));
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong while creating the LearningElements for the Backup-Structure.");
                throw;
            }
        }

        _fileSystem.File.WriteAllText(_dslPath, jsonFile);
        Logger.LogDebug("Generated DSL Document: {JsonFile}",jsonFile);
        return _dslPath;
    }
}