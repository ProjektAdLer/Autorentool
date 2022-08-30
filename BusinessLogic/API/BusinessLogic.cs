﻿using BusinessLogic.Entities;
using Shared.Configuration;

namespace BusinessLogic.API;

public class BusinessLogic : IBusinessLogic
{
    public BusinessLogic(
        IAuthoringToolConfiguration configuration,
        IDataAccess dataAccess,
        IWorldGenerator worldGenerator)
    {
        Configuration = configuration;
        DataAccess = dataAccess;
        WorldGenerator = worldGenerator;
    }
    
    
    internal IWorldGenerator WorldGenerator { get; }
    internal IDataAccess DataAccess { get;  }


    public void ConstructBackup(LearningWorld learningWorld, string filepath)
    {
        WorldGenerator.ConstructBackup(learningWorld, filepath);
    }

    public void SaveLearningWorld(LearningWorld learningWorld, string filepath)
    {
        DataAccess.SaveLearningWorldToFile(learningWorld, filepath);
    }

    public LearningWorld LoadLearningWorld(string filepath)
    {
        return DataAccess.LoadLearningWorld(filepath);
    }
    
    public void SaveLearningSpace(LearningSpace learningSpace, string filepath)
    {
        DataAccess.SaveLearningSpaceToFile(learningSpace, filepath);
    }

    public LearningSpace LoadLearningSpace(string filepath)
    {
        return DataAccess.LoadLearningSpace(filepath);
    }
    
    public void SaveLearningElement(LearningElement learningElement, string filepath)
    {
        DataAccess.SaveLearningElementToFile(learningElement, filepath);
    }

    public LearningElement LoadLearningElement(string filepath)
    {
        return DataAccess.LoadLearningElement(filepath);
    }

    public LearningContent LoadLearningContent(string filepath)
    {
        return DataAccess.LoadLearningContent(filepath);
    }

    public LearningContent LoadLearningContent(string name, Stream stream)
    {
        return DataAccess.LoadLearningContent(name, stream);
    }

    public LearningWorld LoadLearningWorld(Stream stream)
    {
        return DataAccess.LoadLearningWorld(stream);
    }

    public LearningSpace LoadLearningSpace(Stream stream)
    {
        return DataAccess.LoadLearningSpace(stream);
    }

    public LearningElement LoadLearningElement(Stream stream)
    {
        return DataAccess.LoadLearningElement(stream);
    }

    public string FindSuitableNewSavePath(string targetFolder, string fileName, string fileEnding)
    {
        return DataAccess.FindSuitableNewSavePath(targetFolder, fileName, fileEnding);
    }
    public IAuthoringToolConfiguration Configuration { get; }
  
}