﻿
namespace AuthoringTool.DataAccess.XmlClasses.Entities.Files.xml;

public interface IFilesXmlFiles : IXmlSerializable
{

    List<FilesXmlFile> File { get; set; }
}