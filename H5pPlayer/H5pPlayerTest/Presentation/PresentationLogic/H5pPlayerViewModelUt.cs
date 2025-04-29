using H5pPlayer.Presentation.PresentationLogic;

namespace H5pPlayerTest.Presentation.PresentationLogic;

[TestFixture]
public class H5pPlayerViewModelUt
{

    /// <summary>
    /// OnChange is tested in: <see cref="IsCompletable_SetToDifferentValue_ShouldTriggerOnChangeEvent"/>
    /// because only "+=" operation allowed on Actions.-> cant be asserted!
    /// </summary>
    [Test]
    public void Constructor_Standard()
    {
        var systemUnderTest = CreateH5pPlayerVm();

        Assert.That(systemUnderTest.IsCompletable , Is.False);
        Assert.That(systemUnderTest.InvalidPathErrorVm , Is.Not.Null);
    }

    [Test]
    // ANF-ID: [HSE6]
    public void IsCompletable_SetToDifferentValue_ShouldTriggerOnChangeEvent()
    {
        var eventTriggered = false;
        Action action = () => { eventTriggered = true; };
        var systemUnderTest = CreateH5pPlayerVm(action);

        systemUnderTest.IsCompletable = true;

        Assert.That(eventTriggered, Is.True);
    }

    [Test]
    // ANF-ID: [HSE6]
    public void IsCompletable_SetToSameValue_ShouldNotTriggerOnChangeEvent()
    {
        var eventTriggered = false;
        Action action = () => { eventTriggered = true; };
        var systemUnderTest = CreateH5pPlayerVm(action);
        
        systemUnderTest.IsCompletable = false;

        Assert.That(eventTriggered, Is.False);
    }

   

    private H5pPlayerViewModel CreateH5pPlayerVm(
        Action? fakeAction = null)
    {
        fakeAction ??=  () => { };
        var viewModel = new H5pPlayerViewModel(fakeAction);
        return viewModel;
    }
}