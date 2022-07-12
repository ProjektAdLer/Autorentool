namespace AuthoringTool.DataAccess.XmlClasses.Entities;

public interface IGroupsXmlGroups : IXmlSerializable
{
    void SetParameters(GroupsXmlGroupingsList? groupingsList);
}