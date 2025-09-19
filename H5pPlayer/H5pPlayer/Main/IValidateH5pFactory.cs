using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.UseCases.TerminateH5pPlayer;
using H5pPlayer.BusinessLogic.UseCases.ValidateH5p;
using H5pPlayer.Presentation.PresentationLogic.ValidateH5p;
using Microsoft.Extensions.Logging;

namespace H5pPlayer.Main;

public interface IValidateH5pFactory
{
    
    public void CreateValidateH5pStructure(
        ICallJavaScriptAdapter callJavaScriptAdapter,
        ITerminateH5pPlayerUcPort terminateH5pPlayerUc,
        ILoggerFactory loggerFactory);
    
    /// <exception cref="NullReferenceException">
    /// Before call this property call <see cref="IValidateH5pFactory.CreateValidateH5pStructure"/>
    /// </exception>
    public IValidateH5pViewModel? ValidateH5pVm { get; }
    
    /// <exception cref="NullReferenceException">
    /// Before call this property call <see cref="IValidateH5pFactory.CreateValidateH5pStructure"/>
    /// </exception>
    IValidateH5pPresenter? ValidateH5pPresenter { get;  }
    
    /// <exception cref="NullReferenceException">
    /// Before call this property call <see cref="IValidateH5pFactory.CreateValidateH5pStructure"/>
    /// </exception>
    ITerminateH5pPlayerUcPort? TerminateH5pPlayerUc { get; }
    
    /// <exception cref="NullReferenceException">
    /// Before call this property call <see cref="IValidateH5pFactory.CreateValidateH5pStructure"/>
    /// </exception>
    public IValidateH5pController? ValidateH5pController { get;  }
    
    /// <exception cref="NullReferenceException">
    /// Before call this property call <see cref="IValidateH5pFactory.CreateValidateH5pStructure"/>
    /// </exception>
    public IValidateH5pUc? ValidateH5pUc { get;  }
}