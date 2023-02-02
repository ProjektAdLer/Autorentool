
using BusinessLogic.Entities;

namespace BusinessLogic.API;

public interface IWorldGenerator
{
    void ConstructBackup(World world, string filepath);
}