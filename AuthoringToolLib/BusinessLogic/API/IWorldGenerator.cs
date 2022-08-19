
using AuthoringToolLib.Entities;

namespace AuthoringToolLib.BusinessLogic.API;

public interface IWorldGenerator
{
    void ConstructBackup(LearningWorld learningWorld, string filepath);
}