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
        var h5pPlayerVm = new H5pPlayerViewModel();
        var systemUnderTest = new H5pPlayerPresenter(h5pPlayerVm);
        
        systemUnderTest.SetH5pIsCompletable();
        
        Assert.That(h5pPlayerVm.IsCompletable, Is.True);
    }
}