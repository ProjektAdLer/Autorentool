using System.Text.Json.Serialization;

namespace Generator.ATF;

public class FrameStoryJson : IFrameStoryJson
{
    [JsonConstructor]
    public FrameStoryJson(string frameStoryIntro, string frameStoryOutro)
    {
        FrameStoryIntro = frameStoryIntro;
        FrameStoryOutro = frameStoryOutro;
    }
    
    // The introduction to the frame story
    public string FrameStoryIntro { get; set; }
    
    // The outro of the frame story
    public string FrameStoryOutro { get; set; }
}