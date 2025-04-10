using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;
using H5pPlayer.Presentation.PresentationLogic.ValidateH5p;

namespace H5pPlayer.Main;

public interface IValidateH5pFactory
{
    
    void CreateValidateH5pStructure(ICallJavaScriptAdapter callJavaScriptAdapter);
    
    
    /// <exception cref="NullReferenceException">
    /// Before call this property call <see cref="IValidateH5pFactory.CreateValidateH5pStructure"/>
    /// </exception>
    IValidateH5pPresenter? ValidateH5pPresenter { get;  }
    
    /// <exception cref="NullReferenceException">
    /// Before call this property call <see cref="IValidateH5pFactory.CreateValidateH5pStructure"/>
    /// </exception>
    public IValidateH5pViewModel? ValidateH5pVm { get; }
    
    /// <exception cref="NullReferenceException">
    /// Before call this property call <see cref="IValidateH5pFactory.CreateValidateH5pStructure"/>
    /// </exception>
    public IValidateH5pController? ValidateH5pController { get;  }
    
    /// <exception cref="NullReferenceException">
    /// Before call this property call <see cref="IValidateH5pFactory.CreateValidateH5pStructure"/>
    /// </exception>
    public IValidateH5pUc? ValidateH5pUc { get;  }
}