using NUnit.Framework;

namespace PresentationTest.PresentationLogic.Mediator;

[TestFixture]

public class MediatorUt
{
    [Test]
    public void RequestOpenWorldDialog_SetsBool()
    {
        var mediator = new Presentation.PresentationLogic.Mediator.Mediator();
        
        mediator.RequestOpenWorldDialog();
        Assert.Multiple(() =>
        {
            Assert.That(mediator.WorldDialogOpen, Is.True);
            Assert.That(mediator.SpaceDialogOpen, Is.False);
            Assert.That(mediator.ElementDialogOpen, Is.False);
            Assert.That(mediator.ContentDialogOpen, Is.False);
        });
    }
    
    [Test]
    public void RequestOpenSpaceDialog_SetsBool()
    {
        var mediator = new Presentation.PresentationLogic.Mediator.Mediator();
        
        mediator.RequestOpenSpaceDialog();
        Assert.Multiple(() =>
        {
            Assert.That(mediator.WorldDialogOpen, Is.False);
            Assert.That(mediator.SpaceDialogOpen, Is.True);
            Assert.That(mediator.ElementDialogOpen, Is.False);
            Assert.That(mediator.ContentDialogOpen, Is.False);
        });
    }
    
    [Test]
    public void RequestOpenElementDialog_SetsBool()
    {
        var mediator = new Presentation.PresentationLogic.Mediator.Mediator();
        
        mediator.RequestOpenElementDialog();
        Assert.Multiple(() =>
        {
            Assert.That(mediator.WorldDialogOpen, Is.False);
            Assert.That(mediator.SpaceDialogOpen, Is.False);
            Assert.That(mediator.ElementDialogOpen, Is.True);
            Assert.That(mediator.ContentDialogOpen, Is.False);
        });
    }
    
    [Test]
    public void RequestOpenContentDialog_SetsBool()
    {
        var mediator = new Presentation.PresentationLogic.Mediator.Mediator();
        
        mediator.RequestOpenContentDialog();
        Assert.Multiple(() =>
        {
            Assert.That(mediator.WorldDialogOpen, Is.False);
            Assert.That(mediator.SpaceDialogOpen, Is.False);
            Assert.That(mediator.ElementDialogOpen, Is.False);
            Assert.That(mediator.ContentDialogOpen, Is.True);
        });
    }
    
    [Test]
    public void RequestOpenWorldView_SetsBool()
    {
        var mediator = new Presentation.PresentationLogic.Mediator.Mediator();
        
        mediator.RequestOpenWorldView();
        Assert.Multiple(() =>
        {
            Assert.That(mediator.WorldViewOpen, Is.True);
            Assert.That(mediator.WorldOverviewOpen, Is.False);
        });
    }
    
    [Test]
    public void RequestOpenWorldOverView_SetsBool()
    {
        var mediator = new Presentation.PresentationLogic.Mediator.Mediator();
        
        mediator.RequestOpenWorldOverview();
        Assert.Multiple(() =>
        {
            Assert.That(mediator.WorldViewOpen, Is.False);
            Assert.That(mediator.WorldOverviewOpen, Is.True);
        });
    }
    
    [Test]
    public void RequestToggleWorldDialog_TogglesBool()
    {
        var mediator = new Presentation.PresentationLogic.Mediator.Mediator();
        
        mediator.RequestToggleWorldDialog();
        Assert.Multiple(() =>
        {
            Assert.That(mediator.WorldDialogOpen, Is.True);
            Assert.That(mediator.SpaceDialogOpen, Is.False);
            Assert.That(mediator.ElementDialogOpen, Is.False);
            Assert.That(mediator.ContentDialogOpen, Is.False);
        });
        
        mediator.RequestToggleWorldDialog();
        Assert.Multiple(() =>
        {
            Assert.That(mediator.WorldDialogOpen, Is.False);
            Assert.That(mediator.SpaceDialogOpen, Is.False);
            Assert.That(mediator.ElementDialogOpen, Is.False);
            Assert.That(mediator.ContentDialogOpen, Is.False);
        });
    }
    
    [Test]
    public void RequestToggleSpaceDialog_TogglesBool()
    {
        var mediator = new Presentation.PresentationLogic.Mediator.Mediator();
        
        mediator.RequestToggleSpaceDialog();
        Assert.Multiple(() =>
        {
            Assert.That(mediator.WorldDialogOpen, Is.False);
            Assert.That(mediator.SpaceDialogOpen, Is.True);
            Assert.That(mediator.ElementDialogOpen, Is.False);
            Assert.That(mediator.ContentDialogOpen, Is.False);
        });
        
        mediator.RequestToggleSpaceDialog();
        Assert.Multiple(() =>
        {
            Assert.That(mediator.WorldDialogOpen, Is.False);
            Assert.That(mediator.SpaceDialogOpen, Is.False);
            Assert.That(mediator.ElementDialogOpen, Is.False);
            Assert.That(mediator.ContentDialogOpen, Is.False);
        });
    }
    
    [Test]
    public void RequestToggleElementDialog_TogglesBool()
    {
        var mediator = new Presentation.PresentationLogic.Mediator.Mediator();
        
        mediator.RequestToggleElementDialog();
        Assert.Multiple(() =>
        {
            Assert.That(mediator.WorldDialogOpen, Is.False);
            Assert.That(mediator.SpaceDialogOpen, Is.False);
            Assert.That(mediator.ElementDialogOpen, Is.True);
            Assert.That(mediator.ContentDialogOpen, Is.False);
        });
        
        mediator.RequestToggleElementDialog();
        Assert.Multiple(() =>
        {
            Assert.That(mediator.WorldDialogOpen, Is.False);
            Assert.That(mediator.SpaceDialogOpen, Is.False);
            Assert.That(mediator.ElementDialogOpen, Is.False);
            Assert.That(mediator.ContentDialogOpen, Is.False);
        });
    }
    
    [Test]
    public void RequestToggleContentDialog_TogglesBool()
    {
        var mediator = new Presentation.PresentationLogic.Mediator.Mediator();
        
        mediator.RequestToggleContentDialog();
        Assert.Multiple(() =>
        {
            Assert.That(mediator.WorldDialogOpen, Is.False);
            Assert.That(mediator.SpaceDialogOpen, Is.False);
            Assert.That(mediator.ElementDialogOpen, Is.False);
            Assert.That(mediator.ContentDialogOpen, Is.True);
        });
        
        mediator.RequestToggleContentDialog();
        Assert.Multiple(() =>
        {
            Assert.That(mediator.WorldDialogOpen, Is.False);
            Assert.That(mediator.SpaceDialogOpen, Is.False);
            Assert.That(mediator.ElementDialogOpen, Is.False);
            Assert.That(mediator.ContentDialogOpen, Is.False);
        });
    }
    
    [Test]
    public void RequestToggleWorldView_TogglesBool()
    {
        var mediator = new Presentation.PresentationLogic.Mediator.Mediator();
        
        mediator.RequestToggleWorldView();
        Assert.Multiple(() =>
        {
            Assert.That(mediator.WorldViewOpen, Is.True);
            Assert.That(mediator.WorldOverviewOpen, Is.False);
        });
        
        mediator.RequestToggleWorldView();
        Assert.Multiple(() =>
        {
            Assert.That(mediator.WorldViewOpen, Is.False);
            Assert.That(mediator.WorldOverviewOpen, Is.False);
        });
    }
    
    [Test]
    public void RequestToggleWorldOverview_TogglesBool()
    {
        var mediator = new Presentation.PresentationLogic.Mediator.Mediator();
        
        mediator.RequestToggleWorldOverview();
        Assert.Multiple(() =>
        {
            Assert.That(mediator.WorldViewOpen, Is.False);
            Assert.That(mediator.WorldOverviewOpen, Is.True);
        });
        
        mediator.RequestToggleWorldOverview();
        Assert.Multiple(() =>
        {
            Assert.That(mediator.WorldViewOpen, Is.False);
            Assert.That(mediator.WorldOverviewOpen, Is.False);
        });
    }

    [Test]
    public void CloseBothSides_SetsAllBoolToFalse()
    {
        var mediator = new Presentation.PresentationLogic.Mediator.Mediator();
        
        mediator.RequestOpenWorldDialog();
        mediator.RequestOpenWorldOverview();
        
        mediator.CloseBothSides();
        
        Assert.Multiple(() =>
        {
            Assert.That(mediator.WorldDialogOpen, Is.False);
            Assert.That(mediator.SpaceDialogOpen, Is.False);
            Assert.That(mediator.ElementDialogOpen, Is.False);
            Assert.That(mediator.ContentDialogOpen, Is.False);
            Assert.That(mediator.WorldViewOpen, Is.False);
            Assert.That(mediator.WorldOverviewOpen, Is.False);
        });
    }
}