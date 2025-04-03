using H5pPlayer.Presentation.PresentationLogic.ValidateH5p;

namespace H5pPlayerTest.Presentation.PresentationLogic.ValidateH5p;

[TestFixture]
public class ValidateH5pVmUt
{
    [Test]
    // ANF-ID: [HSE6]
    public void IsCompletable_SetToDifferentValue_ShouldTriggerOnChangeEvent()
    {
        var eventTriggered = false;

        var systemUnderTest = new ValidateH5pViewModel();
        var action = () => { eventTriggered = true; };
        systemUnderTest.OnChange += action;

        systemUnderTest.IsCompletable = true;

        Assert.That(eventTriggered, Is.True);
    }

    [Test]
    // ANF-ID: [HSE6]
    public void IsCompletable_SetToSameValue_ShouldNotTriggerOnChangeEvent()
    {
        var eventTriggered = false;
        Action action = () => { eventTriggered = true; };
        var systemUnderTest = new ValidateH5pViewModel();
        systemUnderTest.OnChange += action;
        
        systemUnderTest.IsCompletable = false;

        Assert.That(eventTriggered, Is.False);
    }
}

