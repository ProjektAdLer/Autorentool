namespace Generator.DSL.AdaptivityElement;

public interface IAdaptivityContentJson : IHasType
{
    string IntroText { get; set; }
    bool ShuffleTasks { get; set; }
    List<IAdaptivityTaskJson> AdaptivityTask { get; set; }
}