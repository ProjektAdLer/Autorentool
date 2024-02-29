using System.Runtime.Serialization;

namespace Shared;

[DataContract]
public enum Theme
{
    [EnumMember(Value = "Campus")] CampusAschaffenburg,
    [EnumMember] CampusKempten,
    [EnumMember] Suburb,
    [EnumMember] Arcade
}