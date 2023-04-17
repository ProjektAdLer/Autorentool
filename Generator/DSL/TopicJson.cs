﻿namespace Generator.DSL;

public class TopicJson : ITopicJson
{
    public TopicJson(int topicId, string topicName, List<int> topicContents)
    {
        TopicId = topicId;
        TopicName = topicName;
        TopicContents = topicContents;
    }
    
    // incremented id for all topics
    public int TopicId { get; set; }
    
    // the name of the topic
    public string TopicName { get; set; }
    
    // Which spaces are in a topic
    public List<int> TopicContents { get; set; }
}