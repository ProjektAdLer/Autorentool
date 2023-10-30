using BusinessLogic.Entities;

namespace BusinessLogic.API;

public interface IWorldGenerator
{
    /// <summary>
    /// Creates the ATF document, reads it, creates the needed folder structure for the backup, fills the folders with
    /// the needed xml-files and saves it to the desired location as .mbz file. 
    /// </summary>
    /// <param name="learningWorld"></param> Information about the learningWorld, topics, spaces and elements
    /// <param name="filepath"></param> Desired filepath for the .mbz file. Given by user, when Export Button is pressed.
    void ConstructBackup(LearningWorld learningWorld, string filepath);

    string ExtractAtfFromBackup(string filepath);
}