namespace Generator.DSL.AdaptivityElement;

public interface IAdaptivityElementJson : IElementJson
{
    IAdaptivityContentJson AdaptivityContent { get; set; }
}