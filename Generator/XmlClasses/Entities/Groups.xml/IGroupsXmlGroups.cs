﻿namespace Generator.XmlClasses.Entities.Groups.xml;

public interface IGroupsXmlGroups : IXmlSerializable
{
    GroupsXmlGroupingsList GroupingsList { get; set; }
}