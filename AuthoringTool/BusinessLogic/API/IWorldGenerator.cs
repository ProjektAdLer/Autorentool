
using AuthoringTool.Entities;

namespace AuthoringTool.BusinessLogic.API;

public interface IWorldGenerator
{
    void ConstructBackup(LearningWorld learningWorld, string filepath);
}