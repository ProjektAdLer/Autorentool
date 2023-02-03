
using BusinessLogic.Entities;

namespace BusinessLogic.API;

public interface IWorldGenerator
{
    void ConstructBackup(LearningWorld learningWorld, string filepath);
}