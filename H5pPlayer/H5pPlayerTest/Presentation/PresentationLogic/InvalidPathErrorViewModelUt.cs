using H5pPlayer.Presentation.PresentationLogic;

namespace H5pPlayerTest.Presentation.PresentationLogic;

[TestFixture]
public class InvalidPathErrorViewModelUt
{
    [Test]
    public void ErrorTextForInvalidPath_SetToDifferentValue_ShouldTriggerOnChangeEvent()
    {
        var eventTriggered = false;
        Action action = () => { eventTriggered = true; };
        var systemUnderTest = CreateInvalidPathErrorViewModel(action);

        systemUnderTest.ErrorTextForInvalidPath = "NewErrorText Test";

        Assert.That(eventTriggered, Is.True);
    }

    [Test]
    public void ErrorTextForInvalidPath_SetToSameValue_ShouldNotTriggerOnChangeEvent()
    {
        var eventTriggered = false;
        Action action = () => { eventTriggered = true; };
        var systemUnderTest = CreateInvalidPathErrorViewModel(action);
        
        systemUnderTest.ErrorTextForInvalidPath = "init";

        Assert.That(eventTriggered, Is.False);
    }

    
    [Test]
    public void InvalidPath_SetToDifferentValue_ShouldTriggerOnChangeEvent()
    {
        var eventTriggered = false;
        Action action = () => { eventTriggered = true; };
        var systemUnderTest = CreateInvalidPathErrorViewModel(action);

        systemUnderTest.InvalidPath = "NewErrorText Test";

        Assert.That(eventTriggered, Is.True);
    }

    [Test]
    public void InvalidPath_SetToSameValue_ShouldNotTriggerOnChangeEvent()
    {
        var eventTriggered = false;
        Action action = () => { eventTriggered = true; };
        var systemUnderTest = CreateInvalidPathErrorViewModel(action);
        
        systemUnderTest.InvalidPath = "init";

        Assert.That(eventTriggered, Is.False);
    }

        
    [Test]
    public void InvalidPathErrorIsActive_SetToDifferentValue_ShouldTriggerOnChangeEvent()
    {
        var eventTriggered = false;
        Action action = () => { eventTriggered = true; };
        var systemUnderTest = CreateInvalidPathErrorViewModel(action);

        systemUnderTest.InvalidPathErrorIsActive = true;

        Assert.That(eventTriggered, Is.True);
    }

    [Test]
    public void InvalidPathErrorIsActive_SetToSameValue_ShouldNotTriggerOnChangeEvent()
    {
        var eventTriggered = false;
        Action action = () => { eventTriggered = true; };
        var systemUnderTest = CreateInvalidPathErrorViewModel(action);
        
        systemUnderTest.InvalidPathErrorIsActive =  false;

        Assert.That(eventTriggered, Is.False);
    }


    public static InvalidPathErrorViewModel CreateInvalidPathErrorViewModel(Action? fakeAction = null)
    {
        fakeAction ??=  () => { };
        var viewModel = new InvalidPathErrorViewModel(fakeAction);
        return viewModel;
    }

}