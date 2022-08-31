namespace Generator.XmlClasses;

public interface IXmlBackupFactory
{
    /// <summary>
    /// Use all Methods of the current class.
    /// </summary>
    void CreateXmlBackupFactory();

    /// <summary>
    /// Set the parameter of the gradebook.xml file and create it.
    /// </summary>
    void CreateGradebookXml();

    void CreateGroupsXml();
    void CreateMoodleBackupXml();
    void CreateOutcomesXml();
    void CreateQuestionsXml();
    void CreateRolesXml();
    void CreateScalesXml();
}