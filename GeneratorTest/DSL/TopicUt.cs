﻿using Generator.DSL;
using NUnit.Framework;

namespace GeneratorTest.DSL;

[TestFixture]
public class TopicUt
{

    [Test]
    public void TopicJson_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var name = "topicname";
        var contentList = new List<int> {1, 2};
        
        //Act
        var topic = new TopicJson(1, name, contentList);

        //Assert
        Assert.That(topic.TopicName, Is.EqualTo(name));
        Assert.That(topic.TopicContent, Is.EqualTo(contentList));
        Assert.That(topic.TopicId, Is.EqualTo(1));
    }
}