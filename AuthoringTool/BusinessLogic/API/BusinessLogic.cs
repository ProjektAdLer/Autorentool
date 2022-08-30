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
        return Mapper.Map<LearningWorld>(DataAccess.LoadLearningWorldFromFile(filepath));
    }
    
    public void SaveLearningSpace(LearningSpace learningSpace, string filepath)
    {
        DataAccess.SaveLearningSpaceToFile(Mapper.Map<LearningSpacePe>(learningSpace), filepath);
    }

    public LearningSpace LoadLearningSpace(string filepath)
    {
        return Mapper.Map<LearningSpace>(DataAccess.LoadLearningSpaceFromFile(filepath));
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
        return Mapper.Map<LearningWorld>(DataAccess.LoadLearningWorldFromStream(stream));
    }

    public LearningSpace LoadLearningSpaceFromStream(Stream stream)
    {
        return Mapper.Map<LearningSpace>(DataAccess.LoadLearningSpaceFromStream(stream));
    }

    public LearningElement LoadLearningElementFromStream(Stream stream)
    {
        return Mapper.Map<LearningElement>(DataAccess.LoadLearningElementFromStream(stream));
    }

    public string FindSuitableNewSavePath(string targetFolder, string fileName, string fileEnding)
    {
        return DataAccess.FindSuitableNewSavePath(targetFolder, fileName, fileEnding);
    }
    public IAuthoringToolConfiguration Configuration { get; }
  
}