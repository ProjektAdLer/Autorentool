using System.IO.Abstractions;
using System.Text.Json;
using Generator.WorldExport;
using Microsoft.Extensions.Logging;
using PersistEntities;
using Shared.Extensions;

namespace Generator.DSL;

public class CreateDsl : ICreateDsl
{
    public List<LearningElementPe> ContentListLearningElements;
    public List<LearningSpacePe> ListLearningSpaces;
    public LearningWorldJson? LearningWorldJson;
    private List<int> _listLearningSpaceContent;
    private List<int> _requirements;
    public List<LearningElementPe> ListAllLearningElements;
    private IFileSystem _fileSystem;
    public string Uuid;
    private string _dslPath;
    private string _xmlFilesForExportPath;
    internal ILogger<CreateDsl> Logger { get; }

    /// <summary>
    /// Read the AuthoringToolLib Entities and create a Dsl Document with a specified syntax.
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
        ContentListLearningElements = new List<LearningElementPe>();
        ListLearningSpaces = new List<LearningSpacePe>();
        _listLearningSpaceContent = new List<int>();
        _requirements = new List<int>();
        ListAllLearningElements = new List<LearningElementPe>();
        Guid guid = Guid.NewGuid();
        Uuid = guid.ToString();
    }

    //Search through all LearningElements and look for duplicates. 
    //If a duplicate is found, the duplicate Values get a incremented Number behind them for example: (1), (2)...
    public List<LearningSpacePe> SearchDuplicateLearningElementNames(List<LearningSpacePe> listLearningSpace)
    {
        var incrementedNamesDictionary = new Dictionary<string,string>();
        
        //Get All LearningElements
        foreach (var learningSpace in listLearningSpace)
        {
            foreach (var element in learningSpace.LearningElements)
            {
                ListAllLearningElements.Add(element);
            }
        }
        
        //Search for duplicates
        var duplicateLearningElements = ListAllLearningElements.GroupBy(x => x.Name).Where(x => x.Count() > 1)
            .Select(x => x).ToList();

        //To avoid duplicate names, we increment the name of the learning element.
        //That happens in yet another loop, because we have to respect the Space -> Element hierarchy.
        foreach (var duplicateElement in duplicateLearningElements)
        {
            foreach (var learningSpace in listLearningSpace)
            {
                foreach (var element in learningSpace.LearningElements)
                {
                    if(element.Name == duplicateElement.Key)
                    {
                        var incrementedElementName = "";
                        
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

        return listLearningSpace;
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

        int learningSpaceIdForDictionary = 1;
        

        var idDictionary = new Dictionary<int, Guid>();
        foreach (var space in ListLearningSpaces)
        {
            idDictionary.Add(learningSpaceIdForDictionary, space.Id);
            learningSpaceIdForDictionary++;
        }
        
        //Search for duplicate LearningElement Names and increment them.
        ListLearningSpaces = SearchDuplicateLearningElementNames(ListLearningSpaces);

        // Starting Value for Learning Space Ids & Learning Element Ids
        int learningSpaceId = 1;
        int learningSpaceElementId = 1;

        foreach (var learningSpace in ListLearningSpaces)
        {
            _listLearningSpaceContent = new List<int>();
            IdentifierJson learningSpaceIdentifier = new IdentifierJson("name", learningSpace.Name);
            
            
            //Searching for Learning Elements in each Space
            foreach (var element in learningSpace.LearningElements)
            {
                string elementCategory;
                switch (element.LearningContent.Type)
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
                        throw new ArgumentException("The given LearningContent Type is not supported - in CreateDsl.");
                }
                IdentifierJson learningElementIdentifier = new IdentifierJson("FileName", element.Name);
                List<LearningElementValueJson> learningElementValueList = new List<LearningElementValueJson>();
                LearningElementValueJson learningElementValueJson = new LearningElementValueJson("Points", element.Points.ToString());
                learningElementValueList.Add(learningElementValueJson);

                LearningElementJson learningElementJson = new LearningElementJson(learningSpaceElementId,
                    learningElementIdentifier, element.Url, elementCategory, element.LearningContent.Type, 
                    learningSpaceId, learningElementValueList, element.Description, element.Goals);

                // Add Elements that have Content to the List, they will be copied at the end of the method.
                if (element.LearningContent.Type != "url")
                {
                    ContentListLearningElements.Add(element);
                }

                //int elementIndex = ContentListLearningElements.IndexOf(element) + 1;
                _listLearningSpaceContent.Add(learningSpaceElementId);
                learningSpaceElementId++;
                LearningWorldJson.LearningElements.Add(learningElementJson);
            }

            _requirements = new List<int>();
            foreach (var connectIn in learningSpace.InBoundSpaces)
            {
                _requirements.Add(idDictionary.Where(x => x.Value == connectIn.Id)
                    .Select(x => x.Key)
                    .FirstOrDefault());
            }
      
            // Add Learning Space to Learning World
            LearningWorldJson.LearningSpaces.Add(new LearningSpaceJson(learningSpaceId,
                learningSpaceIdentifier, _listLearningSpaceContent, 
                learningSpace.RequiredPoints, 
                learningSpace.LearningElements.Sum(element => element.Points),
                learningSpace.Description, learningSpace.Goals, _requirements));
            
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
        foreach (var learningElement in ContentListLearningElements)
            {
                try
                {
                    _fileSystem.File.Copy(learningElement.LearningContent.Filepath,
                        _fileSystem.Path.Join("XMLFilesForExport", $"{learningElement.Name}.{learningElement.LearningContent.Type}"));
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