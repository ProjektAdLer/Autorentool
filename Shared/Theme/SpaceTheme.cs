using System.Runtime.Serialization;

namespace Shared.Theme;

[DataContract]
public enum SpaceTheme
{
    [Obsolete][EnumMember(Value = "Campus")] CampusAschaffenburg,
    [Obsolete][EnumMember] CampusKempten,
    [Obsolete][EnumMember] Suburb,
    [Obsolete][EnumMember] Arcade,
    [EnumMember] LearningArea,
    [EnumMember] EatingArea,
    [EnumMember] FnE,
    [EnumMember] SocialArea,
    [EnumMember] TechnicalArea
}