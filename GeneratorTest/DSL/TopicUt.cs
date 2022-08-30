using Generator.DSL;
using NUnit.Framework;

namespace GeneratorTest.DSL;

[TestFixture]
public class TopicUt
{

    [Test]
    public void TopicJson_SetParameters_ObjectsAreEqual()
    {
        //Arrange
        var topic = new TopicJson();
        var ident = new IdentifierJson( "type", "value");
        List<int> contentList = new List<int>();
        List<int> reqlist = new List<int>();
        List<RequirementJson> requirementJsons = new List<RequirementJson>();
        
        contentList.Add(0);
        contentList.Add(1);
        
        reqlist.Add(0);
        reqlist.Add(1);
        
        var requirements = new RequirementJson("type", reqlist);

        requirementJsons.Add(requirements);
        
        //Act
        topic.Identifier = ident;
        topic.Name = "topicname";
        topic.Requirements = requirementJsons;
        topic.TopicContent = contentList;
        topic.TopicId = 1;

        //Assert
        Assert.That(topic.Identifier, Is.EqualTo(ident));
        Assert.That(topic.Name, Is.EqualTo("topicname"));
        Assert.That(topic.Requirements, Is.EqualTo(requirementJsons));
        Assert.That(topic.TopicContent, Is.EqualTo(contentList));
        Assert.That(topic.TopicId, Is.EqualTo(1));
    }
}