using AuthoringTool.DataAccess.PersistEntities;

namespace AuthoringTool.DataAccess.DSL;

public interface ICreateDSL
{
   string WriteLearningWorld(LearningWorldPe learningWorld);
}