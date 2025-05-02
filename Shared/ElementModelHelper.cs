namespace Shared;

public static class ElementModelHelper
{
    public static bool IsObsolete(ElementModel model)
    {
        var memberInfo = typeof(ElementModel).GetMember(model.ToString()).FirstOrDefault();
        return memberInfo?.GetCustomAttributes(typeof(ObsoleteAttribute), false).Length > 0;
    }
    
    public static ElementModel GetAlternateValue(ElementModel model)
    {
        var memberInfo = typeof(ElementModel).GetMember(model.ToString()).FirstOrDefault();
        var alternateValue = memberInfo?.GetCustomAttributes(typeof(AlternateValueAttribute), false).FirstOrDefault();
        
        return alternateValue != null ?
            (ElementModel) ((AlternateValueAttribute) alternateValue).AlternateValue :
            ElementModel.a_npc_defaultdark_female;
    }
}