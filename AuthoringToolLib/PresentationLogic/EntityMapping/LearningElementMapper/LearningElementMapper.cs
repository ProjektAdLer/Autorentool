using AuthoringToolLib.Entities;
using AuthoringToolLib.PresentationLogic.LearningElement;
using AuthoringToolLib.PresentationLogic.LearningElement.ActivationElement;
using AuthoringToolLib.PresentationLogic.LearningElement.InteractionElement;
using AuthoringToolLib.PresentationLogic.LearningElement.TestElement;
using AuthoringToolLib.PresentationLogic.LearningElement.TransferElement;

namespace AuthoringToolLib.PresentationLogic.EntityMapping.LearningElementMapper;

public class LearningElementMapper : ILearningElementMapper
{
    private readonly IImageTransferElementMapper _imageTransferElementMapper;
    private readonly IVideoTransferElementMapper _videoTransferElementMapper;
    private readonly IPdfTransferElementMapper _pdfTransferElementMapper;
    private readonly IH5PActivationElementMapper _h5PActivationElementMapper;
    private readonly IVideoActivationElementMapper _videoActivationElementMapper;
    private readonly IH5PInteractionElementMapper _h5PInteractionElementMapper;
    private readonly IH5PTestElementMapper _h5PTestElementMapper;
    
    private ILogger<LearningElementMapper> Logger { get; }

    public LearningElementMapper(ILogger<LearningElementMapper> logger, 
        IImageTransferElementMapper imageTransferElementMapper, IVideoTransferElementMapper videoTransferElementMapper,
        IPdfTransferElementMapper pdfTransferElementMapper, IH5PActivationElementMapper h5PActivationElementMapper,
        IVideoActivationElementMapper videoActivationElementMapper,
        IH5PInteractionElementMapper h5PInteractionElementMapper, IH5PTestElementMapper h5PTestElementMapper)
    {
        Logger = logger;
        
        _imageTransferElementMapper = imageTransferElementMapper;
        _videoTransferElementMapper = videoTransferElementMapper;
        _pdfTransferElementMapper = pdfTransferElementMapper;
        _h5PActivationElementMapper = h5PActivationElementMapper;
        _videoActivationElementMapper = videoActivationElementMapper;
        _h5PInteractionElementMapper = h5PInteractionElementMapper;
        _h5PTestElementMapper = h5PTestElementMapper;
    }
    public Entities.LearningElement ToEntity(ILearningElementViewModel viewModel)
    {
        return viewModel switch
        {
            ImageTransferElementViewModel => _imageTransferElementMapper.ToEntity(viewModel),
            VideoTransferElementViewModel => _videoTransferElementMapper.ToEntity(viewModel),
            PdfTransferElementViewModel => _pdfTransferElementMapper.ToEntity(viewModel),
            H5PActivationElementViewModel => _h5PActivationElementMapper.ToEntity(viewModel),
            VideoActivationElementViewModel => _videoActivationElementMapper.ToEntity(viewModel),
            H5PInteractionElementViewModel => _h5PInteractionElementMapper.ToEntity(viewModel),
            H5PTestElementViewModel => _h5PTestElementMapper.ToEntity(viewModel),
            _ => throw new ApplicationException("No valid learning element viewmodel")
        };
    }

    public ILearningElementViewModel ToViewModel(Entities.LearningElement entity,
        ILearningElementViewModelParent? caller = null)
    {
        //sanity check
        if (caller != null && entity.ParentName != caller.Name)
        {
            Logger.LogError(
                $"caller was not null but caller.Name != entity.ParentName: {caller.Name}!={entity.ParentName}");
        }

        return entity switch
        {
            ImageTransferElement => _imageTransferElementMapper.ToViewModel(entity, caller),
            VideoTransferElement => _videoTransferElementMapper.ToViewModel(entity, caller),
            PdfTransferElement => _pdfTransferElementMapper.ToViewModel(entity, caller),
            H5PActivationElement => _h5PActivationElementMapper.ToViewModel(entity, caller),
            VideoActivationElement => _videoActivationElementMapper.ToViewModel(entity, caller),
            H5PInteractionElement => _h5PInteractionElementMapper.ToViewModel(entity, caller),
            H5PTestElement => _h5PTestElementMapper.ToViewModel(entity, caller),
            _ => throw new ApplicationException("No valid learning element entity")
        };
    }
}