using H5pPlayer.Presentation.PresentationLogic;
using NSubstitute;

namespace H5pPlayerTest.Presentation.PresentationLogic;

[TestFixture]
public class H5pPlayerViewModelUt
{

    [Test]
    public void IsCompletable_SetToDifferentValue_ShouldTriggerOnChangeEvent()
    {
        var systemUnderTest = CreateH5pPlayerVm();
        var eventTriggered = false;
        systemUnderTest.OnChange += () => eventTriggered = true;

        systemUnderTest.IsCompletable = true;

        Assert.That(eventTriggered, Is.True);
    }

    [Test]
    public void IsCompletable_SetToSameValue_ShouldNotTriggerOnChangeEvent()
    {
        var systemUnderTest = CreateH5pPlayerVm(true);
        var eventTriggered = false;
        systemUnderTest.OnChange += () => eventTriggered = true;

        systemUnderTest.IsCompletable = true;

        Assert.That(eventTriggered, Is.False);
    }

    [Test]
    public void NotifyStateChanged_ShouldInvokeOnChangeEvent()
    {
        var systemUnderTest  = CreateH5pPlayerVm();
        var onChangeMock = Substitute.For<Action>();
        systemUnderTest.OnChange += onChangeMock;

        systemUnderTest.IsCompletable = true;

        onChangeMock.Received(1).Invoke();
    }


    private H5pPlayerViewModel CreateH5pPlayerVm(bool isCompletable = false)
    {
        var viewModel = new H5pPlayerViewModel();
        viewModel.IsCompletable = isCompletable;
        return viewModel;
    }
}