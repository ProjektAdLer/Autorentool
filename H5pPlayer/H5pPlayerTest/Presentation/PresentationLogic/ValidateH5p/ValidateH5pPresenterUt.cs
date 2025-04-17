using H5pPlayer.Presentation.PresentationLogic.ValidateH5p;

namespace H5pPlayerTest.Presentation.PresentationLogic.ValidateH5p;

[TestFixture]
public class ValidateH5pPresenterUt
{

    [Test]
    // ANF-ID: [HSE6]
    public void SetH5pIsCompletable()
    {
        var validateH5pVm = CreateValidateH5pVm();
        var systemUnderTest = CreateValidateH5pPresenter(validateH5pVm);
        
        systemUnderTest.SetH5pIsCompletable();
        
        Assert.That(validateH5pVm.IsCompletable, Is.True);
    }

    
    

    private IValidateH5pViewModel CreateValidateH5pVm()
    {
        var viewModel = new ValidateH5pViewModel();
        Action fakeAction = () => { };
        viewModel.OnChange += fakeAction;
        return viewModel;
    }
    
    private static ValidateH5pPresenter CreateValidateH5pPresenter(IValidateH5pViewModel? validateH5pVm = null)
    {
        Action fakeAction = () => { };
        validateH5pVm ??= new ValidateH5pViewModel();
        validateH5pVm.OnChange += fakeAction;
        var systemUnderTest = new ValidateH5pPresenter(validateH5pVm);
        return systemUnderTest;
    }

}