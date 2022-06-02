using System.IO.Abstractions;
using AuthoringTool.Entities;

namespace AuthoringTool.DataAccess.DSL;

public interface ICreateDSL
{
   void WriteLearningWorld(LearningWorld learningWorld,  IFileSystem? fileSystem=null);
}