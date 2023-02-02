using System.IO.Abstractions;
using System.Text.Json;
using Generator.WorldExport;
using Microsoft.Extensions.Logging;
using PersistEntities;
using Shared.Extensions;

namespace Generator.DSL;

public class CreateDsl : ICreateDsl
{
    public List<ElementPe> ListElementsWithContents;
    public List<SpacePe> ListSpaces;
    public WorldJson WorldJson;
    public string Uuid;
    public Dictionary<int, Guid> IdDictionary;
    private List<int> _listSpaceContent;
    private List<ElementPe> _listAllElements;
    private string _booleanAlgebraRequirements;
    private string _currentConditionDirectSpaces;
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
        ListElementsWithContents = new List<ElementPe>();
        ListSpaces = new List<SpacePe>();
        _listSpaceContent = new List<int>();
        _booleanAlgebraRequirements = "";
        IdDictionary = new Dictionary<int, Guid>();
        Guid guid = Guid.NewGuid();
        Uuid = guid.ToString();
        _currentConditionDirectSpaces = "";
        _listAllElements = new List<ElementPe>();
    }

    //Search through all  Elements and look for duplicates. 
    //If a duplicate is found, the duplicate Values get a incremented Number behind them for example: (1), (2)...
    public List<SpacePe> SearchDuplicateElementNames(List<SpacePe> listSpace)
    {
        var incrementedNamesDictionary = new Dictionary<string,string>();
        
        //Get All Elements
        foreach (var space in listSpace)
        {
            foreach (var element in space.SpaceLayout.ContainedElements)
            {
                _listAllElements.Add(element);
            }
        }
        
        //Search for duplicates
        var duplicateElements = _listAllElements.GroupBy(x => x.Name).Where(x => x.Count() > 1)
            .Select(x => x).ToList();

        //To avoid duplicate names, we increment the name of the element.
        //That happens in yet another loop, because we have to respect the Space -> Element hierarchy.
        foreach (var duplicateElement in duplicateElements)
        {
            foreach (var space in listSpace)
            {
                foreach (var element in space.SpaceLayout.ContainedElements)
                {
                    if(element.Name == duplicateElement.Key)
                    {
                        string incrementedElementName;
                        
                        if (incrementedNamesDictionary.ContainsKey(element.Name))
                        {
                            incrementedElementName = StringHelper.IncrementName(incrementedNamesDictionary[element.Name]);
                            incrementedNamesDictionary[element.Name] = incrementedElementName;
                        }
                        else
                        {
                            incrementedElementName = StringHelper.IncrementName(element.Name);
                            incrementedNamesDictionary.Add(element.Name, incrementedElementName);
                        }
                        
                        element.Name = incrementedElementName;
                    }
                }
            }
        }
        return listSpace;
    }

    /// <summary>
    /// Takes a Condition and builds a boolean algebra string.
    /// Method searches recursively for all conditions and their inbound Spaces.
    /// </summary>
    /// <param name="condition"></param>
    /// <returns>A string that describes a boolean algebra expression</returns>
    public string DefineLogicalExpression(PathWayConditionPe condition)
    {
        string conditionValue = condition.Condition.ToString();
        if(conditionValue == "And")
        {
            conditionValue = "^";
        }
        else if(conditionValue == "Or")
        {
            conditionValue = "v";
        }
        
        foreach (var pathWayObject in condition.InBoundObjects)
        {
            if(pathWayObject is SpacePe)
            {
                _currentConditionDirectSpaces += "(";
                string spaceId = IdDictionary.Where(x => x.Value == pathWayObject.Id)
                    .Select(x => x.Key)
                    .FirstOrDefault().ToString();
                _currentConditionDirectSpaces += spaceId+")" + conditionValue;
            }
            else if (pathWayObject is PathWayConditionPe pathWayConditionPe)
            {
                //special case for nested conditions (conditions that are in conditions)
                if (pathWayObject.InBoundObjects.Count == 1)
                {
                    DefineLogicalExpression(pathWayConditionPe);
                }
                else
                {
                    _currentConditionDirectSpaces += "("; 
                    DefineLogicalExpression(pathWayConditionPe);
                    _currentConditionDirectSpaces += ")";
                    _currentConditionDirectSpaces += conditionValue;
                }
            }
        }
        _currentConditionDirectSpaces = _currentConditionDirectSpaces.Substring(0, _currentConditionDirectSpaces.LastIndexOf(")", StringComparison.Ordinal)+1);
        return _currentConditionDirectSpaces;
    }
    
    /// <summary>
    /// Reads the World Entity and creates an DSL Document with the given information.
    /// </summary>
    /// <param name="world"></param> Information about the wworld, topics, spaces and Elements
    public string WriteWorld(WorldPe world)
    {
        Initialize();
        //Starting ID for Spaces
        int spaceIdForDictionary = 1;
        
        // Starting Value for Space Ids & Element Ids in the DSL-Document
        int spaceId = 1;
        int spaceElementId = 1;
        
        //Initialise WorldJson with empty values, will be filled with information later in the method.
        WorldJson = new WorldJson(Uuid, new IdentifierJson("name", world.Name), new List<int>(),
            new List<TopicJson>(), new List<SpaceJson>(), new List<ElementJson>(), 
            world.Description, world.Goals);

        // CreateSpaces & fill into World
        // The SpaceId defines what the starting Id for Spaces should be. 
        // Search for  Elements in Spaces and add to listElements
        ListSpaces.AddRange(world.Spaces);
        
        foreach (var space in ListSpaces)
        {
            IdDictionary.Add(spaceIdForDictionary, space.Id);
            spaceIdForDictionary++;
        }
        
        //Search for duplicate Element Names and increment them.
        ListSpaces = SearchDuplicateElementNames(ListSpaces);

        foreach (var space in ListSpaces)
        {
            _listSpaceContent = new List<int>();
            _booleanAlgebraRequirements = "";
            _currentConditionDirectSpaces = "";
            
            IdentifierJson spaceIdentifier = new IdentifierJson("name", space.Name);
            
            //Searching for  Elements in each Space
            foreach (var element in space.SpaceLayout.ContainedElements)
            {
                string elementCategory;
                switch (element.Content.Type)
                {
                    case "png" or "jpg" or "bmp" or "webp":
                        elementCategory = "image";
                        break;
                    case "url":
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
                        throw new ArgumentException("The given Content Type is not supported - in CreateDsl.");
                }
                
                IdentifierJson elementIdentifier = new IdentifierJson("FileName", element.Name);
                List<ElementValueJson> elementValueList = new List<ElementValueJson>();
                ElementValueJson elementValueJson = new ElementValueJson("Points", element.Points.ToString());
                elementValueList.Add(elementValueJson);

                ElementJson elementJson = new ElementJson(spaceElementId,
                    elementIdentifier, element.Url, elementCategory, element.Content.Type, 
                    spaceId, elementValueList, element.Description, element.Goals);

                // Add Elements that have Content to the List, they will be copied at the end of the method.
                if (element.Content.Type != "url")
                {
                    ListElementsWithContents.Add(element);
                }
                
                //int elementIndex = ListElementsWithContents.IndexOf(element) + 1;
                _listSpaceContent.Add(spaceElementId);
                spaceElementId++;
                WorldJson.Elements.Add(elementJson);
            }
          
            // Create  Space Requirements
            // If the inbound-type is not a PathWayCondition there can only be 1 SpacePe, so we do not have to construct a boolean algebra expression.
            // If the inbound-type is a PathWayCondition, we have to construct a boolean algebra expression.
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
                        _booleanAlgebraRequirements = (IdDictionary.Where(x => x.Value == inbound.Id)
                            .Select(x => x.Key)
                            .FirstOrDefault().ToString());
                    }
                }
            }

            // Add the constructed  Space to  World
            WorldJson.Spaces.Add(new SpaceJson(spaceId,
                spaceIdentifier, _listSpaceContent, 
                space.RequiredPoints, 
                space.SpaceLayout.ContainedElements.Sum(element => element.Points),
                space.Description, space.Goals, requirements:_booleanAlgebraRequirements));
            
            spaceId++;
        }

        // Create DocumentRoot & JSON Document
        // And add the WorldJson to the DocumentRoot
        // The structure of the DSL needs DocumentRoot, because the world has its own tag
        DocumentRootJson rootJson = new DocumentRootJson(WorldJson);

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
        
        //All Lelements are created at the specified location = Easier access to files in further Export-Operations.
        //After the files are added to the Backup-Structure, these Files will be deleted.
        foreach (var element in ListElementsWithContents)
        {
            try
            {
                _fileSystem.File.Copy(element.Content.Filepath,
                    _fileSystem.Path.Join("XMLFilesForExport", $"{element.Name}.{element.Content.Type}"));
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong while creating the Elements for the Backup-Structure.");
                throw;
            }
        }

        _fileSystem.File.WriteAllText(_dslPath, jsonFile);
        Logger.LogDebug("Generated DSL Document: {JsonFile}",jsonFile);
        return _dslPath;
    }
}