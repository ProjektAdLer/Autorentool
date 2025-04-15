using System.Runtime.Serialization;

namespace Shared.Theme;

[DataContract]
public enum SpaceTheme
{
    [EnumMember(Value = "Campus")] CampusAschaffenburg,
    [EnumMember] CampusKempten,
    [EnumMember] Suburb,
    [EnumMember] Arcade
}