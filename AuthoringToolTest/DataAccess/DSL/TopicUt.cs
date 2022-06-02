using System.Collections.Generic;
using AuthoringTool.DataAccess.DSL;
using NUnit.Framework;

namespace AuthoringToolTest.DataAccess.DSL;

[TestFixture]
public class TopicUt
{

    [Test]
    public void TopicJson_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var topic = new TopicJson();
        var ident = new IdentifierJson();
        var requirements = new RequirementJson();
        List<int> contentList = new List<int>();
        List<int> reqlist = new List<int>();
        List<RequirementJson> requirementJsons = new List<RequirementJson>();
        
        contentList.Add(0);
        contentList.Add(1);
        
        reqlist.Add(0);
        reqlist.Add(1);

        ident.type = "type";
        ident.value = "value";

        requirements.type = "type";
        requirements.value = reqlist;
        
        requirementJsons.Add(requirements);
        
        //Act
        topic.identifier = ident;
        topic.name = "topicname";
        topic.requirements = requirementJsons;
        topic.topicContent = contentList;
        topic.topicId = 1;

        //Assert
        Assert.That(topic.identifier, Is.EqualTo(ident));
        Assert.That(topic.name, Is.EqualTo("topicname"));
        Assert.That(topic.requirements, Is.EqualTo(requirementJsons));
        Assert.That(topic.topicContent, Is.EqualTo(contentList));
        Assert.That(topic.topicId, Is.EqualTo(1));
    }
}