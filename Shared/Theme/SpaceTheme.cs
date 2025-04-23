using System.Runtime.Serialization;

namespace Shared.Theme;

[DataContract]
public enum SpaceTheme
{
    [EnumMember] LearningArea,
    [EnumMember] EatingArea,
    [EnumMember] FnE,
    [EnumMember] SocialArea,
    [EnumMember] TechnicalArea,
    [Obsolete][EnumMember(Value = "Campus")] CampusAschaffenburg,
    [Obsolete][EnumMember] CampusKempten,
    [Obsolete][EnumMember] Suburb,
    [Obsolete][EnumMember] Arcade
}