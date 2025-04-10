using H5pPlayer.BusinessLogic.Api.JavaScript;
using H5pPlayer.BusinessLogic.UseCases.DisplayH5p;
using H5pPlayer.Presentation.PresentationLogic.DisplayH5p;

namespace H5pPlayer.Main;

public interface IDisplayH5pFactory
{
    void CreateDisplayH5pStructure(ICallJavaScriptAdapter callJavaScriptAdapter);

    /// <exception cref="NullReferenceException">
    /// Before call this property call <see cref="IDisplayH5pFactory.CreateDisplayH5pStructure"/>
    /// </exception>
     IDisplayH5pPresenter? DisplayH5pPresenter { get;  }
    
    /// <exception cref="NullReferenceException">
    /// Before call this property call <see cref="IDisplayH5pFactory.CreateDisplayH5pStructure"/>
    /// </exception>
     IDisplayH5pViewModel? DisplayH5pVm { get;  }
     
     /// <exception cref="NullReferenceException">
     /// Before call this property call <see cref="IDisplayH5pFactory.CreateDisplayH5pStructure"/>
     /// </exception>
    IDisplayH5pUC? DisplayH5pUc { get;  }
     
    /// <exception cref="NullReferenceException">
    /// Before call this property call <see cref="IDisplayH5pFactory.CreateDisplayH5pStructure"/>
    /// </exception>
     IDisplayH5pController? DisplayH5pController { get;  }
}