using H5pPlayer.BusinessLogic.Entities;
using H5pPlayer.Presentation.PresentationLogic.ValidateH5p;

namespace H5pPlayerTest.Presentation.PresentationLogic.ValidateH5p;

[TestFixture]
public class ValidateH5pVmUt
{
    private ValidateH5pViewModel _systemUnderTest;
    [SetUp]
    public void Setup()
    {
        _systemUnderTest = new ValidateH5pViewModel();
    }

    [Test]
    public void Constructor()
    {
        Assert.That(_systemUnderTest.IsCompletable,Is.False);
        Assert.That(_systemUnderTest.ActiveH5PState, Is.EqualTo(H5pState.NotValidated));
    }
    
    [Test]
    // ANF-ID: [HSE6]
    public void IsCompletable_SetToDifferentValue_ShouldTriggerOnChangeEvent()
    {
        var eventTriggered = false;

        var action = () => { eventTriggered = true; };
        _systemUnderTest.OnChange += action;

        _systemUnderTest.IsCompletable = true;

        Assert.That(eventTriggered, Is.True);
    }

    [Test]
    // ANF-ID: [HSE6]
    public void IsCompletable_SetToSameValue_ShouldNotTriggerOnChangeEvent()
    {
        var eventTriggered = false;
        Action action = () => { eventTriggered = true; };
        _systemUnderTest.OnChange += action;
        
        _systemUnderTest.IsCompletable = false;

        Assert.That(eventTriggered, Is.False);
    }
    
    
    [Test]
    public void ActiveH5pState_SetToDifferentValue_ShouldTriggerOnChangeEvent()
    {
        var eventTriggered = false;

        var action = () => { eventTriggered = true; };
        _systemUnderTest.OnChange += action;

        _systemUnderTest.ActiveH5PState = H5pState.Primitive;

        Assert.That(eventTriggered, Is.True);
    }

    [Test]
    public void ActiveH5pState_SetToSameValue_ShouldNotTriggerOnChangeEvent()
    {
        var eventTriggered = false;
        Action action = () => { eventTriggered = true; };
        _systemUnderTest.OnChange += action;
        
        _systemUnderTest.ActiveH5PState = H5pState.NotValidated;


        Assert.That(eventTriggered, Is.False);
    }
}

