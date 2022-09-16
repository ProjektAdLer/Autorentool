using BusinessLogic.API;
using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NSubstitute;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class LoadLearningElementUt
{
    [Test]
    public void Execute_LoadsLearningElement_ImageTransfer()
    {
        var world = new LearningWorld("a", "b", "b", "b", "b", "b");
        var element =
            new ImageTransferElement("a", "b", world, null!, "a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9, 2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(world, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(world.LearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(world.LearningElements, Has.Count.EqualTo(1));
        Assert.That(world.LearningElements[0], Is.EqualTo(element));
        Assert.That(world.LearningElements[0], Is.InstanceOf(typeof(ImageTransferElement)));
        Assert.IsTrue(actionWasInvoked);
    }
    
    [Test]
    public void Execute_LoadsLearningElement_VideoTransfer()
    {
        var world = new LearningWorld("a", "b", "b", "b", "b", "b");
        var element =
            new VideoTransferElement("a", "b", world, null!, "a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9,2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(world, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(world.LearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(world.LearningElements, Has.Count.EqualTo(1));
        Assert.That(world.LearningElements[0], Is.EqualTo(element));
        Assert.That(world.LearningElements[0], Is.InstanceOf(typeof(VideoTransferElement)));
        Assert.IsTrue(actionWasInvoked);
    }
    
    [Test]
    public void Execute_LoadsLearningElement_PdfTransfer()
    {
        var space = new LearningWorld("a", "b", "b", "b", "b", "b");
        var element =
            new PdfTransferElement("a", "b", space, null!, "a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9, 2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(space.LearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(space.LearningElements, Has.Count.EqualTo(1));
        Assert.That(space.LearningElements[0], Is.EqualTo(element));
        Assert.That(space.LearningElements[0], Is.InstanceOf(typeof(PdfTransferElement)));
        Assert.IsTrue(actionWasInvoked);
    }
    
    [Test]
    public void Execute_LoadsLearningElement_TextTransfer()
    {
        var world = new LearningWorld("a", "b", "b", "b", "b", "b");
        var element =
            new TextTransferElement("a", "b", world, null!, "a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9, 2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(world, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(world.LearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(world.LearningElements, Has.Count.EqualTo(1));
        Assert.That(world.LearningElements[0], Is.EqualTo(element));
        Assert.That(world.LearningElements[0], Is.InstanceOf(typeof(TextTransferElement)));
        Assert.IsTrue(actionWasInvoked);
    }
    
    [Test]
    public void Execute_LoadsLearningElement_VideoActivation()
    {
        var space = new LearningWorld("a", "b", "b", "b", "b", "b");
        var element =
            new VideoActivationElement("a", "b", space, null!, "a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9, 2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(space.LearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(space.LearningElements, Has.Count.EqualTo(1));
        Assert.That(space.LearningElements[0], Is.EqualTo(element));
        Assert.That(space.LearningElements[0], Is.InstanceOf(typeof(VideoActivationElement)));
        Assert.IsTrue(actionWasInvoked);
    }
    
    [Test]
    public void Execute_LoadsLearningElement_H5PActivation()
    {
        var world = new LearningWorld("a", "b", "b", "b", "b", "b");
        var element =
            new H5PActivationElement("a", "b", world, null!, "a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9,2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(world, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(world.LearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(world.LearningElements, Has.Count.EqualTo(1));
        Assert.That(world.LearningElements[0], Is.EqualTo(element));
        Assert.That(world.LearningElements[0], Is.InstanceOf(typeof(H5PActivationElement)));
        Assert.IsTrue(actionWasInvoked);
    }
    
    [Test]
    public void Execute_LoadsLearningElement_H5PInteraction()
    {
        var space = new LearningWorld("a", "b", "b", "b", "b", "b");
        var element =
            new H5PInteractionElement("a", "b", space, null!, "a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9, 2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(space.LearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(space.LearningElements, Has.Count.EqualTo(1));
        Assert.That(space.LearningElements[0], Is.EqualTo(element));
        Assert.That(space.LearningElements[0], Is.InstanceOf(typeof(H5PInteractionElement)));
        Assert.IsTrue(actionWasInvoked);
    }
    
    [Test]
    public void Execute_LoadsLearningElement_H5PTest()
    {
        var world = new LearningWorld("a", "b", "b", "b", "b", "b");
        var element =
            new H5PTestElement("a", "b", world, null!, "a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9,2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(world, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(world.LearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(world.LearningElements, Has.Count.EqualTo(1));
        Assert.That(world.LearningElements[0], Is.EqualTo(element));
        Assert.That(world.LearningElements[0], Is.InstanceOf(typeof(H5PTestElement)));
        Assert.IsTrue(actionWasInvoked);
    }
    
    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(world,"element", mockBusinessLogic, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesLoadLearningElement()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var world = new LearningWorld("a", "b", "c", "d", "e", "f");
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var element =
            new LearningElement("a", "b", null!, "a", "b",
                "c", LearningElementDifficultyEnum.Easy, world, 1, 9, 2,3);
        mockBusinessLogic.LoadLearningElement(Arg.Any<string>()).Returns(element);
        var command = new LoadLearningElement(world, "element", mockBusinessLogic, mappingAction);
        
        Assert.That(world.LearningElements, Is.Empty);
        
        command.Execute();
        
        Assert.That(world.LearningElements, Has.Count.EqualTo(1));
        Assert.That(world.LearningElements[0], Is.EqualTo(element));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(world.LearningElements, Is.Empty);
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(world.LearningElements, Has.Count.EqualTo(1));
        Assert.That(world.LearningElements[0], Is.EqualTo(element));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(world.LearningElements, Is.Empty);
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(world.LearningElements, Has.Count.EqualTo(1));
        Assert.That(world.LearningElements[0], Is.EqualTo(element));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
    }
}