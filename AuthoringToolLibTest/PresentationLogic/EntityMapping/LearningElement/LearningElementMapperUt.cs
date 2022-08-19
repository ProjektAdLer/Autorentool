using System;
using AuthoringToolLib.Entities;
using AuthoringToolLib.PresentationLogic.EntityMapping.LearningElementMapper;
using AuthoringToolLib.PresentationLogic.LearningContent;
using AuthoringToolLib.PresentationLogic.LearningElement;
using AuthoringToolLib.PresentationLogic.LearningElement.ActivationElement;
using AuthoringToolLib.PresentationLogic.LearningElement.InteractionElement;
using AuthoringToolLib.PresentationLogic.LearningElement.TestElement;
using AuthoringToolLib.PresentationLogic.LearningElement.TransferElement;
using AuthoringToolLib.PresentationLogic.LearningWorld;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace AuthoringToolLibTest.PresentationLogic.EntityMapping.LearningElement;

[TestFixture]

public class LearningElementMapperUt
{
    [Test]
    public void LearningElementMapper_ToEntity_CallsImageTransferMapper()
    {
        var contentViewModel = new LearningContentViewModel("a", "b", Array.Empty<byte>());
        var world = new LearningWorldViewModel("baba", "bubu", "", "", "", "");
        var viewModel = new ImageTransferElementViewModel("name", "shortname", world, contentViewModel,"authors",
            "description", "goals",LearningElementDifficultyEnum.Easy, 1, 2,3);
        var imageTransferElementMapper = Substitute.For<IImageTransferElementMapper>();
        
        var systemUnderTest = CreateTestableLearningElementMapper(imageTransferElementMapper : imageTransferElementMapper);
        systemUnderTest.ToEntity(viewModel);

        imageTransferElementMapper.Received().ToEntity(viewModel);
    }
    
    [Test]
    public void LearningElementMapper_ToEntity_CallsVideoTransferElementMapper()
    {
        var contentViewModel = new LearningContentViewModel("a", "b", Array.Empty<byte>());
        var world = new LearningWorldViewModel("baba", "bubu", "", "", "", "");
        var viewModel = new VideoTransferElementViewModel("name", "shortname", world, contentViewModel,"authors",
            "description", "goals",LearningElementDifficultyEnum.Easy, 1, 2,3);
        var videoTransferElementMapper = Substitute.For<IVideoTransferElementMapper>();
        
        var systemUnderTest = CreateTestableLearningElementMapper(videoTransferElementMapper : videoTransferElementMapper);
        systemUnderTest.ToEntity(viewModel);

        videoTransferElementMapper.Received().ToEntity(viewModel);
    }
    
    [Test]
    public void LearningElementMapper_ToEntity_CallsPdfTransferElementMapper()
    {
        var contentViewModel = new LearningContentViewModel("a", "b", Array.Empty<byte>());
        var world = new LearningWorldViewModel("baba", "bubu", "", "", "", "");
        var viewModel = new PdfTransferElementViewModel("name", "shortname", world, contentViewModel,"authors",
            "description", "goals",LearningElementDifficultyEnum.Easy, 1, 2,3);
        var pdfTransferElementMapper = Substitute.For<IPdfTransferElementMapper>();
        
        var systemUnderTest = CreateTestableLearningElementMapper(pdfTransferElementMapper : pdfTransferElementMapper);
        systemUnderTest.ToEntity(viewModel);

        pdfTransferElementMapper.Received().ToEntity(viewModel);
    }
    
    [Test]
    public void LearningElementMapper_ToEntity_CallsH5PActivationElementMapper()
    {
        var contentViewModel = new LearningContentViewModel("a", "b", Array.Empty<byte>());
        var world = new LearningWorldViewModel("baba", "bubu", "", "", "", "");
        var viewModel = new H5PActivationElementViewModel("name", "shortname", world, contentViewModel,"authors",
            "description", "goals",LearningElementDifficultyEnum.Easy, 1, 2,3);
        var h5PActivationElementMapper = Substitute.For<IH5PActivationElementMapper>();
        
        var systemUnderTest = CreateTestableLearningElementMapper(h5PActivationElementMapper : h5PActivationElementMapper);
        systemUnderTest.ToEntity(viewModel);

        h5PActivationElementMapper.Received().ToEntity(viewModel);
    }
    
    [Test]
    public void LearningElementMapper_ToEntity_CallsVideoActivationElementMapper()
    {
        var contentViewModel = new LearningContentViewModel("a", "b", Array.Empty<byte>());
        var world = new LearningWorldViewModel("baba", "bubu", "", "", "", "");
        var viewModel = new VideoActivationElementViewModel("name", "shortname", world, contentViewModel,"authors",
            "description", "goals",LearningElementDifficultyEnum.Easy, 1, 2,3);
        var videoActivationElementMapper = Substitute.For<IVideoActivationElementMapper>();
        
        var systemUnderTest = CreateTestableLearningElementMapper(videoActivationElementMapper : videoActivationElementMapper);
        systemUnderTest.ToEntity(viewModel);

        videoActivationElementMapper.Received().ToEntity(viewModel);
    }
    
    [Test]
    public void LearningElementMapper_ToEntity_CallsH5PInteractionElementMapper()
    {
        var contentViewModel = new LearningContentViewModel("a", "b", Array.Empty<byte>());
        var world = new LearningWorldViewModel("baba", "bubu", "", "", "", "");
        var viewModel = new H5PInteractionElementViewModel("name", "shortname", world, contentViewModel,"authors",
            "description", "goals",LearningElementDifficultyEnum.Easy, 1, 2,3);
        var h5PInteractionElementMapper = Substitute.For<IH5PInteractionElementMapper>();
        
        var systemUnderTest = CreateTestableLearningElementMapper(h5PInteractionElementMapper : h5PInteractionElementMapper);
        systemUnderTest.ToEntity(viewModel);

        h5PInteractionElementMapper.Received().ToEntity(viewModel);
    }
    
    [Test]
    public void LearningElementMapper_ToEntity_CallsH5PTestElementMapper()
    {
        var contentViewModel = new LearningContentViewModel("a", "b", Array.Empty<byte>());
        var world = new LearningWorldViewModel("baba", "bubu", "", "", "", "");
        var viewModel = new H5PTestElementViewModel("name", "shortname", world, contentViewModel,"authors",
            "description", "goals",LearningElementDifficultyEnum.Easy, 1, 2,3);
        var h5PTestElementMapper = Substitute.For<IH5PTestElementMapper>();
        
        var systemUnderTest = CreateTestableLearningElementMapper(h5PTestElementMapper : h5PTestElementMapper);
        systemUnderTest.ToEntity(viewModel);

        h5PTestElementMapper.Received().ToEntity(viewModel);
    }

    [Test]
    public void LearningElementMapper_ToEntity_ThrowsNoValidLearningElementViewModel()
    {
        var systemUnderTest = CreateTestableLearningElementMapper();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.ToEntity(null));
        Assert.That(ex!.Message, Is.EqualTo("No valid learning element viewmodel"));
    }
    
    [Test]
    public void LearningElementMapper_ToViewModel_CallsImageTransferElementMapper()
    {
        var content = new LearningContent("content", "pdf", Array.Empty<byte>());
        var world = new LearningWorldViewModel("baba", "bubu", "", "", "", "");
        var entity = new ImageTransferElement("name", "shortname",world.Name, content,"authors", "description",
            "goals",LearningElementDifficultyEnum.Easy, 1, 3, 2);
        
        var imageTransferElementMapper = Substitute.For<IImageTransferElementMapper>();
        
        var systemUnderTest = CreateTestableLearningElementMapper(imageTransferElementMapper : imageTransferElementMapper);
        systemUnderTest.ToViewModel(entity);

        imageTransferElementMapper.Received().ToViewModel(entity);
    }
    
    [Test]
    public void LearningElementMapper_ToViewModel_CallsVideoTransferElementMapper()
    {
        var content = new LearningContent("content", "pdf", Array.Empty<byte>());
        var world = new LearningWorldViewModel("baba", "bubu", "", "", "", "");
        var entity = new VideoTransferElement("name", "shortname",world.Name, content,"authors", "description",
            "goals",LearningElementDifficultyEnum.Easy, 1, 3, 2);
        
        var videoTransferElementMapper = Substitute.For<IVideoTransferElementMapper>();
        
        var systemUnderTest = CreateTestableLearningElementMapper(videoTransferElementMapper : videoTransferElementMapper);
        systemUnderTest.ToViewModel(entity);

        videoTransferElementMapper.Received().ToViewModel(entity);
    }
    
    [Test]
    public void LearningElementMapper_ToViewModel_CallsPdfTransferElementMapper()
    {
        var content = new LearningContent("content", "pdf", Array.Empty<byte>());
        var world = new LearningWorldViewModel("baba", "bubu", "", "", "", "");
        var entity = new PdfTransferElement("name", "shortname",world.Name, content,"authors", "description",
            "goals",LearningElementDifficultyEnum.Easy, 1, 3, 2);
        
        var pdfTransferElementMapper = Substitute.For<IPdfTransferElementMapper>();
        
        var systemUnderTest = CreateTestableLearningElementMapper(pdfTransferElementMapper : pdfTransferElementMapper);
        systemUnderTest.ToViewModel(entity);

        pdfTransferElementMapper.Received().ToViewModel(entity);
    }
    
    [Test]
    public void LearningElementMapper_ToViewModel_CallsH5PActivationElementMapper()
    {
        var content = new LearningContent("content", "pdf", Array.Empty<byte>());
        var world = new LearningWorldViewModel("baba", "bubu", "", "", "", "");
        var entity = new H5PActivationElement("name", "shortname",world.Name, content,"authors", "description",
            "goals",LearningElementDifficultyEnum.Easy, 1, 3, 2);
        
        var h5PActivationElementMapper = Substitute.For<IH5PActivationElementMapper>();
        
        var systemUnderTest = CreateTestableLearningElementMapper(h5PActivationElementMapper : h5PActivationElementMapper);
        systemUnderTest.ToViewModel(entity);

        h5PActivationElementMapper.Received().ToViewModel(entity);
    }
    
    [Test]
    public void LearningElementMapper_ToViewModel_CallsVideoActivationElementMapper()
    {
        var content = new LearningContent("content", "pdf", Array.Empty<byte>());
        var world = new LearningWorldViewModel("baba", "bubu", "", "", "", "");
        var entity = new VideoActivationElement("name", "shortname",world.Name, content,"authors", "description",
            "goals",LearningElementDifficultyEnum.Easy, 1, 3, 2);
        
        var videoActivationElementMapper = Substitute.For<IVideoActivationElementMapper>();
        
        var systemUnderTest = CreateTestableLearningElementMapper(videoActivationElementMapper : videoActivationElementMapper);
        systemUnderTest.ToViewModel(entity);

        videoActivationElementMapper.Received().ToViewModel(entity);
    }
    
    [Test]
    public void LearningElementMapper_ToViewModel_CallsH5PInteractionElementMapper()
    {
        var content = new LearningContent("content", "pdf", Array.Empty<byte>());
        var world = new LearningWorldViewModel("baba", "bubu", "", "", "", "");
        var entity = new H5PInteractionElement("name", "shortname",world.Name, content,"authors", "description",
            "goals",LearningElementDifficultyEnum.Easy, 1, 3, 2);
        
        var h5PInteractionElementMapper = Substitute.For<IH5PInteractionElementMapper>();
        
        var systemUnderTest = CreateTestableLearningElementMapper(h5PInteractionElementMapper : h5PInteractionElementMapper);
        systemUnderTest.ToViewModel(entity);

        h5PInteractionElementMapper.Received().ToViewModel(entity);
    }
    
    [Test]
    public void LearningElementMapper_ToViewModel_CallsH5PTestElementMapper()
    {
        var content = new LearningContent("content", "pdf", Array.Empty<byte>());
        var world = new LearningWorldViewModel("baba", "bubu", "", "", "", "");
        var entity = new H5PTestElement("name", "shortname",world.Name, content,"authors", "description",
            "goals",LearningElementDifficultyEnum.Easy, 1, 3, 2);
        
        var h5PTestElementMapper = Substitute.For<IH5PTestElementMapper>();
        
        var systemUnderTest = CreateTestableLearningElementMapper(h5PTestElementMapper : h5PTestElementMapper);
        systemUnderTest.ToViewModel(entity);

        h5PTestElementMapper.Received().ToViewModel(entity);
    }
    
    [Test]
    public void LearningElementMapper_ToViewModel_ThrowsNoValidLearningElementEntity()
    {
        var systemUnderTest = CreateTestableLearningElementMapper();

        var ex = Assert.Throws<ApplicationException>(() => systemUnderTest.ToViewModel(null));
        Assert.That(ex!.Message, Is.EqualTo("No valid learning element entity"));
    }

    [Test]
    public void LearningElementMapper_ToViewModel_LogsSanityCheck()
    {
        var world = new LearningWorldViewModel("bubu", "", "", "", "", "");
        var entity = new ImageTransferElement("name", "shortname", "blabla",null, "authors", "description",
            "goals",LearningElementDifficultyEnum.Easy, 1, 3, 2); 
        var logger = Substitute.For<ILogger<LearningElementMapper>>();
        
        var systemUnderTest = CreateTestableLearningElementMapper(logger);

        systemUnderTest.ToViewModel(entity, world);

        logger.Received().LogError("caller was not null but caller.Name != entity.ParentName: bubu!=blabla");
    }

    private LearningElementMapper CreateTestableLearningElementMapper(ILogger<LearningElementMapper>? logger = null,
        IImageTransferElementMapper? imageTransferElementMapper = null, IVideoTransferElementMapper? videoTransferElementMapper = null,
        IPdfTransferElementMapper? pdfTransferElementMapper = null, IH5PActivationElementMapper? h5PActivationElementMapper = null,
        IVideoActivationElementMapper? videoActivationElementMapper = null, IH5PInteractionElementMapper? h5PInteractionElementMapper = null,
        IH5PTestElementMapper? h5PTestElementMapper = null)
    {
        logger ??= Substitute.For<ILogger<LearningElementMapper>>();
        imageTransferElementMapper ??= Substitute.For<IImageTransferElementMapper>();
        videoTransferElementMapper ??= Substitute.For<IVideoTransferElementMapper>();
        pdfTransferElementMapper ??= Substitute.For<IPdfTransferElementMapper>();
        h5PActivationElementMapper ??= Substitute.For<IH5PActivationElementMapper>();
        videoActivationElementMapper ??= Substitute.For<IVideoActivationElementMapper>();
        h5PInteractionElementMapper ??= Substitute.For<IH5PInteractionElementMapper>();
        h5PTestElementMapper ??= Substitute.For<IH5PTestElementMapper>();
        return new LearningElementMapper(logger, imageTransferElementMapper, videoTransferElementMapper,
            pdfTransferElementMapper, h5PActivationElementMapper, videoActivationElementMapper, h5PInteractionElementMapper, h5PTestElementMapper);
    }
}