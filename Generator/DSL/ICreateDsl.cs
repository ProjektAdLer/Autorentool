using AuthoringTool.DataAccess.PersistEntities;

namespace AuthoringTool.DataAccess.DSL;

public interface ICreateDsl
{
   string WriteLearningWorld(LearningWorldPe learningWorld);
}