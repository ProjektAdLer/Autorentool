using AuthoringTool.API.Configuration;
using AuthoringTool.DataAccess.API;
using AuthoringTool.Entities;
using ElectronNET.API;

namespace AuthoringTool.BusinessLogic.API;

internal class BusinessLogic : IBusinessLogic
{

    public BusinessLogic(
        IAuthoringToolConfiguration configuration,
        IDataAccess dataAccess)
    {
        Configuration = configuration;
        DataAccess = dataAccess;
    }
    
    
    
    public IDataAccess DataAccess { get;  }

    public bool RunningElectron => HybridSupport.IsElectronActive;

    public void ConstructBackup()
    {
        DataAccess.ConstructBackup();
    }

    public void SaveLearningWorld(LearningWorld learningWorld, string filepath)
    {
        DataAccess.SaveLearningWorldToFile(learningWorld, filepath);
    }

    public LearningWorld LoadLearningWorld(string filepath)
    {
        return DataAccess.LoadLearningWorldFromFile(filepath);
    }

    public IAuthoringToolConfiguration Configuration { get; }
  
}