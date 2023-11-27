namespace Generator.ATF.AdaptivityElement;

public interface IAdaptivityElementJson : IInternalElementJson
{
    IAdaptivityContentJson AdaptivityContent { get; set; }
}