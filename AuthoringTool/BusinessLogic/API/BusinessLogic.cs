using AuthoringTool.API.Configuration;
using AuthoringTool.DataAccess.API;
using AuthoringTool.DataAccess.PersistEntities;
using AuthoringTool.Entities;
using AutoMapper;
using ElectronWrapper;

namespace AuthoringTool.BusinessLogic.API;

internal class BusinessLogic : IBusinessLogic
{

    public BusinessLogic(
        IAuthoringToolConfiguration configuration,
        IDataAccess dataAccess,
        IHybridSupportWrapper hybridSupport,
        IMapper mapper)
    {
        Configuration = configuration;
        DataAccess = dataAccess;
        HybridSupport = hybridSupport;
        Mapper = mapper;
    }
    
    
    
    internal IDataAccess DataAccess { get;  }
    internal IHybridSupportWrapper HybridSupport { get; }
    internal  IMapper Mapper { get; }

    public bool RunningElectron => HybridSupport.IsElectronActive;

    public void ConstructBackup(LearningWorld learningWorld, string filepath)
    {
        DataAccess.ConstructBackup(Mapper.Map<LearningWorldPe>(learningWorld), filepath);
    }

    public void SaveLearningWorld(LearningWorld learningWorld, string filepath)
    {
        DataAccess.SaveLearningWorldToFile(Mapper.Map<LearningWorldPe>(learningWorld), filepath);
    }

    public LearningWorld LoadLearningWorld(string filepath)
    {
        var learningWorld = Mapper.Map<LearningWorld>(DataAccess.LoadLearningWorldFromFile(filepath));
        AddParentToLearningElements(learningWorld);
        return learningWorld;
    }
    
    public void SaveLearningSpace(LearningSpace learningSpace, string filepath)
    {
        DataAccess.SaveLearningSpaceToFile(Mapper.Map<LearningSpacePe>(learningSpace), filepath);
    }

    public LearningSpace LoadLearningSpace(string filepath)
    {
        var learningSpace = Mapper.Map<LearningSpace>(DataAccess.LoadLearningSpaceFromFile(filepath));
        AddParentToLearningElements(learningSpace);
        return learningSpace;
    }
    
    public void SaveLearningElement(LearningElement learningElement, string filepath)
    {
        DataAccess.SaveLearningElementToFile(Mapper.Map<LearningElementPe>(learningElement), filepath);
    }

    public LearningElement LoadLearningElement(string filepath)
    {
        return Mapper.Map<LearningElement>(DataAccess.LoadLearningElementFromFile(filepath));
    }

    public LearningContent LoadLearningContent(string filepath)
    {
        return Mapper.Map<LearningContent>(DataAccess.LoadLearningContentFromFile(filepath));
    }

    public LearningContent LoadLearningContentFromStream(string name, Stream stream)
    {
        return Mapper.Map<LearningContent>(DataAccess.LoadLearningContentFromStream(name, stream));
    }

    public LearningWorld LoadLearningWorldFromStream(Stream stream)
    {
        var learningWorld = Mapper.Map<LearningWorld>(DataAccess.LoadLearningWorldFromStream(stream));
        AddParentToLearningElements(learningWorld);
        return learningWorld;
    }

    public LearningSpace LoadLearningSpaceFromStream(Stream stream)
    {
        var learningSpace = Mapper.Map<LearningSpace>(DataAccess.LoadLearningSpaceFromStream(stream));
        AddParentToLearningElements(learningSpace);
        return learningSpace;
    }

    public LearningElement LoadLearningElementFromStream(Stream stream)
    {
        return Mapper.Map<LearningElement>(DataAccess.LoadLearningElementFromStream(stream));
    }
    
    private static void AddParentToLearningElements(ILearningElementParent learningElementParent)
    {
        switch (learningElementParent)
        {
            case LearningWorld learningWorld:
                foreach (var element in learningWorld.LearningElements)
                {
                    element.Parent = learningWorld;
                }

                foreach (var space in learningWorld.LearningSpaces)
                {
                    AddParentToLearningElements(space);
                }
                break;
            
            case LearningSpace learningSpace:
                foreach (var element in learningSpace.LearningElements)
                {
                    element.Parent = learningSpace;
                }
                break;
        }
    }

    public string FindSuitableNewSavePath(string targetFolder, string fileName, string fileEnding)
    {
        return DataAccess.FindSuitableNewSavePath(targetFolder, fileName, fileEnding);
    }
    public IAuthoringToolConfiguration Configuration { get; }
  
}