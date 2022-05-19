using AuthoringTool.Entities;
using AuthoringTool.PresentationLogic.LearningWorld;

namespace AuthoringTool.DataAccess.DSL;

public class CreateDSL : ICreateDSL
{
   private string learningWorldName;
   public void WriteLearningWorld(LearningWorld learningWorld)
    {
        learningWorldName = learningWorld.Name;
    }
}