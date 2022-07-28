namespace AuthoringTool.DataAccess.XmlClasses.Entities;

public interface IGroupsXmlGroups : IXmlSerializable
{
    GroupsXmlGroupingsList GroupingsList { get; set; }
}