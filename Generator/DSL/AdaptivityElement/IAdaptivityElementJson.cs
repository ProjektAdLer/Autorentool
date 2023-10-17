namespace Generator.DSL.AdaptivityElement;

public interface IAdaptivityElementJson : IInternalElementJson
{
    IAdaptivityContentJson AdaptivityContent { get; set; }
}