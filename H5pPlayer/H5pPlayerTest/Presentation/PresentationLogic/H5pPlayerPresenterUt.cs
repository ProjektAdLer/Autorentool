using H5pPlayer.BusinessLogic.UseCases.StartH5pPlayer;
using H5pPlayer.Presentation.PresentationLogic;

namespace H5pPlayerTest.Presentation.PresentationLogic;

[TestFixture]
public class H5pPlayerPresenterUt
{
    /// <summary>
    /// todo: test for State has changed 
    /// </summary>
    [Test]
    public void SetH5pIsCompletable()
    {
        var h5pPlayerVm = CreateH5pPlayerVm();
        var systemUnderTest = CreateH5pPlayerPresenter(h5pPlayerVm);
        
        systemUnderTest.SetH5pIsCompletable();
        
        Assert.That(h5pPlayerVm.IsCompletable, Is.True);
    }



    [Test]
    public void SetInvalidPathError()
    {
        var h5pPlayerVm = CreateH5pPlayerVm();
        var systemUnderTest = CreateH5pPlayerPresenter(h5pPlayerVm);
        var errorTo = CreateErrorOutputTO("PathIs wrong Test", "InvalidPath");

        systemUnderTest.ErrorOutput(errorTo);
        
        Assert.That(h5pPlayerVm.InvalidPathErrorVm.ErrorTextForInvalidPath, 
            Is.EqualTo(errorTo.ErrorTextForInvalidPath));
        Assert.That(h5pPlayerVm.InvalidPathErrorVm.InvalidPath, Is.EqualTo(errorTo.InvalidPath));
    }

    private static StartH5pPlayerErrorOutputTO CreateErrorOutputTO(
        string errorTextForInvalidPath,
        string invalidPath)
    {
        var errorTo = new StartH5pPlayerErrorOutputTO();
        errorTo.ErrorTextForInvalidPath = errorTextForInvalidPath;
        errorTo.InvalidPath = invalidPath;
        return errorTo;
    }

    
    private static H5pPlayerViewModel CreateH5pPlayerVm()
    {
        Action fakeAction = () => { };
        var h5pPlayerVm = new H5pPlayerViewModel(fakeAction);
        return h5pPlayerVm;
    }

    private static H5pPlayerPresenter CreateH5pPlayerPresenter(H5pPlayerViewModel? h5pPlayerVm = null)
    {
        Action fakeAction = () => { };
        h5pPlayerVm ??= new H5pPlayerViewModel(fakeAction);
        var systemUnderTest = new H5pPlayerPresenter(h5pPlayerVm);
        return systemUnderTest;
    }
}