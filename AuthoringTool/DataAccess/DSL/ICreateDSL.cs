using System.IO.Abstractions;
using AuthoringTool.Entities;

namespace AuthoringTool.DataAccess.DSL;

public interface ICreateDSL
{
   string WriteLearningWorld(LearningWorld learningWorld);
}