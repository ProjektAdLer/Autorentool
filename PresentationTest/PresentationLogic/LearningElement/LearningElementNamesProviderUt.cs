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
        var elements = new[]
        {
            ViewModelProvider.GetLearningElement(), ViewModelProvider.GetLearningElement(),
            ViewModelProvider.GetLearningElement()
        };
        world.AllLearningElements.Returns(elements);
        worldPresenter.LearningWorldVm.Returns(world);

        var systemUnderTest = GetSystemUnderTest(worldPresenter);

        var result = systemUnderTest.ElementNames;

        Assert.That(result, Is.EquivalentTo(elements.Select(el => (el.Id, el.Name))));
        _ = world.Received(1).AllLearningElements;
    }

    private LearningElementNamesProvider GetSystemUnderTest(ILearningWorldPresenter? worldPresenter = null)
    {
        worldPresenter ??= Substitute.For<ILearningWorldPresenter>();

        return new LearningElementNamesProvider(worldPresenter);
    }
}