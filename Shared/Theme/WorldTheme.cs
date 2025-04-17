using System.Runtime.Serialization;

namespace Shared.Theme;

[DataContract]
public enum WorldTheme
{
    [EnumMember] CampusAschaffenburg,
    [EnumMember] CampusKempten,
    [EnumMember] Suburb,
    [EnumMember] Company
}