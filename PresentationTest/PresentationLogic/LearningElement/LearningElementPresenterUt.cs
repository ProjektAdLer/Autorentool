using NUnit.Framework;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Shared;

namespace PresentationTest.PresentationLogic.LearningElement;

[TestFixture]

public class LearningElementPresenterUt
{
    [Test]
    public void RemoveLearningElementFromParentAssignment_RemovesElementFromWorld()
    {
        var systemUnderTest = new LearningElementPresenter();
        var parent = new LearningWorldViewModel("a", "b", "c", "d", "e", "f");
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var element = new LearningElementViewModel("a", "b", content,
            "e", "f","g", LearningElementDifficultyEnum.Easy, parent, 4,17f, 29f);
        parent.LearningElements.Add(element);
        
        Assert.That(parent.LearningElements, Contains.Item(element));
        
        systemUnderTest.RemoveLearningElementFromParentAssignment(element);
        
        Assert.That(parent.LearningElements, Is.Empty);
    }
    
    [Test]
    public void RemoveLearningElementFromParentAssignment_RemovesElementFromSpace()
    {
        var systemUnderTest = new LearningElementPresenter();
        var parent = new LearningSpaceViewModel("a", "b", "c", "d", "e");
        var content = new LearningContentViewModel("bar", "foo", new byte[] {0x01, 0x02});
        var element = new LearningElementViewModel("a", "b", content ,
            "e", "f","g", LearningElementDifficultyEnum.Hard, parent, 9,17f, 29f);
        parent.LearningElements.Add(element);
        
        Assert.That(parent.LearningElements, Contains.Item(element));
        
        systemUnderTest.RemoveLearningElementFromParentAssignment(element);
        
        Assert.That(parent.LearningElements, Is.Empty);
    }
}