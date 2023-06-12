using BusinessLogic.Commands;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningSpace;
using Presentation.PresentationLogic.LearningWorld;
using Presentation.PresentationLogic.SelectedViewModels;
using Shared;
using TestHelpers;

namespace PresentationTest.PresentationLogic.SelectedViewModels;

[TestFixture]

public class SelectedViewModelsProviderUt
{
    [Test]
    public void SetLearningWorld_SetsLearningWorld()
    {
        var onUndoRedo = Substitute.For<IOnUndoRedo>();
        var systemUnderTest = new SelectedViewModelsProvider(onUndoRedo);
        var learningWorld = new LearningWorldViewModel("a", "b", "c", "d", "e", "f", "g");
        
        systemUnderTest.SetLearningWorld(learningWorld, null);
        
        Assert.That(systemUnderTest.LearningWorld, Is.EqualTo(learningWorld));
    }
    
    [Test]
    public void SetLearningObjectInPathWay_SetsLearningObjectInPathWay()
    {
        var onUndoRedo = Substitute.For<IOnUndoRedo>();
        var systemUnderTest = new SelectedViewModelsProvider(onUndoRedo);
        var learningObjectInPathWay = new LearningSpaceViewModel("a", "b", "d", Theme.Campus);
        
        systemUnderTest.SetLearningObjectInPathWay(learningObjectInPathWay, null);
        
        Assert.That(systemUnderTest.LearningObjectInPathWay, Is.EqualTo(learningObjectInPathWay));
    }

    [Test]
    public void SetLearningElement_SetsLearningElement()
    {
        var onUndoRedo = Substitute.For<IOnUndoRedo>();
        var systemUnderTest = new SelectedViewModelsProvider(onUndoRedo);
        var learningElement = ViewModelProvider.GetLearningElement();
        
        systemUnderTest.SetLearningElement(learningElement, null);
        
        Assert.That(systemUnderTest.LearningElement, Is.EqualTo(learningElement));
    }
}