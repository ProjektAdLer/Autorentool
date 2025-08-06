using System.Linq;
using NUnit.Framework;
using Presentation.PresentationLogic.AuthoringToolWorkspace;
using TestHelpers;

namespace PresentationTest.PresentationLogic.AuthoringToolWorkspace;

[TestFixture]
public class AuthoringToolWorkspaceViewModelUt
{
    [Test]
    public void AuthoringToolWorkspaceViewModel_Constructor_EnumerableStartsEmpty()
    {
        IAuthoringToolWorkspaceViewModel systemUnderTest = GetViewModelForTesting();

        Assert.That(systemUnderTest.LearningWorlds, Is.Empty);
    }

    [Test]
    public void AuthoringToolWorkspaceViewModel_RemoveLearningWorld_RemovesLearningWorldFromEnumerable()
    {
        var viewModel = ViewModelProvider.GetLearningWorld();

        var systemUnderTest = GetViewModelForTesting();
        systemUnderTest._learningWorlds.Add(viewModel);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorlds.Count(), Is.EqualTo(1));
            Assert.That(systemUnderTest.LearningWorlds, Contains.Item(viewModel));
        });
        systemUnderTest.RemoveLearningWorld(viewModel);

        Assert.That(systemUnderTest.LearningWorlds, Is.Empty);
    }

    [Test]
    public void AuthoringToolWorkspaceViewModel_RemoveLearningWorld_RaisesStateChangeEventWithCurrentState()
    {
        var viewModel = ViewModelProvider.GetLearningWorld();
        var handlerCalled = false;

        var systemUnderTest = GetViewModelForTesting();
        systemUnderTest._learningWorlds.Add(viewModel);
        Assert.Multiple(() =>
        {
            Assert.That(systemUnderTest.LearningWorlds.Count(), Is.EqualTo(1));
            Assert.That(systemUnderTest.LearningWorlds, Contains.Item(viewModel));
        });

        systemUnderTest.PropertyChanged += (caller, changedEventArgs) =>
        {
            if (handlerCalled) Assert.Fail("handler called twice");
            handlerCalled = true;
            Assert.Multiple(() =>
            {
                Assert.That(caller, Is.EqualTo(systemUnderTest));
                Assert.That(changedEventArgs.PropertyName, Is.EqualTo(nameof(systemUnderTest.LearningWorlds)));
            });
        };

        systemUnderTest.RemoveLearningWorld(viewModel);

        Assert.That(handlerCalled, Is.True);
    }

    private AuthoringToolWorkspaceViewModel GetViewModelForTesting()
    {
        return new AuthoringToolWorkspaceViewModel();
    }
}