using System.Linq;
using NSubstitute;
using NUnit.Framework;
using Presentation.PresentationLogic.LearningElement;
using Presentation.PresentationLogic.LearningWorld;
using TestHelpers;

namespace PresentationTest.PresentationLogic.LearningElement;

[TestFixture]
public class LearningElementNamesProviderUt
{
    /// <summary>
    /// Regression test for #342 https://github.com/ProjektAdLer/Autorentool/issues/342
    /// </summary>
    [Test]
    public void GetElementNames_GetsAllLearningElements_ReturnsAllEntries()
    {
        var worldPresenter = Substitute.For<ILearningWorldPresenter>();
        var world = Substitute.For<ILearningWorldViewModel>();
        var learningElements = new[]
        {
            ViewModelProvider.GetLearningElement(), ViewModelProvider.GetLearningElement(),
            ViewModelProvider.GetLearningElement()
        };
        var storyElements = new[]
        {
            ViewModelProvider.GetLearningElement(content: ViewModelProvider.GetStoryContent())
        };
        world.AllLearningElements.Returns(learningElements);
        world.AllStoryElements.Returns(storyElements);
        worldPresenter.LearningWorldVm.Returns(world);

        var systemUnderTest = GetSystemUnderTest(worldPresenter);

        var result = systemUnderTest.ElementNames;

        Assert.That(result, Is.EquivalentTo(learningElements.Concat(storyElements).Select(el => (el.Id, el.Name))));
        _ = world.Received(1).AllLearningElements;
        _ = world.Received(1).AllStoryElements;
    }

    private LearningElementNamesProvider GetSystemUnderTest(ILearningWorldPresenter? worldPresenter = null)
    {
        worldPresenter ??= Substitute.For<ILearningWorldPresenter>();

        return new LearningElementNamesProvider(worldPresenter);
    }
}