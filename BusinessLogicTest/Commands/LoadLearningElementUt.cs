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
        var space = new LearningSpace("a", "b", "b", "b", "b", 5);
        var element =
            new ImageTransferElement("a", "b", space, null!, "a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9, 2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(space.LearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.Null);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(space.LearningElements, Has.Count.EqualTo(1));
        Assert.That(space.LearningElements[0], Is.EqualTo(element));
        Assert.That(space.LearningElements[0], Is.InstanceOf(typeof(ImageTransferElement)));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
    }
    
    [Test]
    public void Execute_LoadsLearningElement_VideoTransfer()
    {
        var space = new LearningSpace("a", "b", "b", "b", "b", 2);
        var element =
            new VideoTransferElement("a", "b", space, null!, "a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9,2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(space.LearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.Null);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(space.LearningElements, Has.Count.EqualTo(1));
        Assert.That(space.LearningElements[0], Is.EqualTo(element));
        Assert.That(space.LearningElements[0], Is.InstanceOf(typeof(VideoTransferElement)));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
    }
    
    [Test]
    public void Execute_LoadsLearningElement_PdfTransfer()
    {
        var space = new LearningSpace("a", "b", "b", "b", "b", 1);
        var element =
            new PdfTransferElement("a", "b", space, null!, "a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9, 2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(space.LearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.Null);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(space.LearningElements, Has.Count.EqualTo(1));
        Assert.That(space.LearningElements[0], Is.EqualTo(element));
        Assert.That(space.LearningElements[0], Is.InstanceOf(typeof(PdfTransferElement)));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
    }
    
    [Test]
    public void Execute_LoadsLearningElement_TextTransfer()
    {
        var space = new LearningSpace("a", "b", "b", "b", "b", 8);
        var element =
            new TextTransferElement("a", "b", space, null!, "a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9, 2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(space.LearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.Null);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(space.LearningElements, Has.Count.EqualTo(1));
        Assert.That(space.LearningElements[0], Is.EqualTo(element));
        Assert.That(space.LearningElements[0], Is.InstanceOf(typeof(TextTransferElement)));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
    }

    [Test]
    public void Execute_LoadsLearningElement_VideoActivation()
    {
        var space = new LearningSpace("a", "b", "b", "b", "b", 9);
        var element =
            new VideoActivationElement("a", "b", space, null!, "a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9, 2, 3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, filepath, mockBusinessLogic, mappingAction);

        Assert.That(space.LearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.Null);

        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(space.LearningElements, Has.Count.EqualTo(1));
        Assert.That(space.LearningElements[0], Is.EqualTo(element));
        Assert.That(space.LearningElements[0], Is.InstanceOf(typeof(VideoActivationElement)));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
    }

    [Test]
    public void Execute_LoadsLearningElement_H5PActivation()
    {
        var space = new LearningSpace("a", "b", "b", "b", "b", 5);
        var element =
            new H5PActivationElement("a", "b", space, null!, "a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9,2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(space.LearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.Null);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(space.LearningElements, Has.Count.EqualTo(1));
        Assert.That(space.LearningElements[0], Is.EqualTo(element));
        Assert.That(space.LearningElements[0], Is.InstanceOf(typeof(H5PActivationElement)));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
    }
    
    [Test]
    public void Execute_LoadsLearningElement_H5PInteraction()
    {
        var space = new LearningSpace("a", "b", "b", "b", "b", 6);
        var element =
            new H5PInteractionElement("a", "b", space, null!, "a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9, 2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(space.LearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.Null);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(space.LearningElements, Has.Count.EqualTo(1));
        Assert.That(space.LearningElements[0], Is.EqualTo(element));
        Assert.That(space.LearningElements[0], Is.InstanceOf(typeof(H5PInteractionElement)));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
    }
    
    [Test]
    public void Execute_LoadsLearningElement_H5PTest()
    {
        var space = new LearningSpace("a", "b", "b", "b", "b", 7);
        var element =
            new H5PTestElement("a", "b", space, null!, "a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9,2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(space.LearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.Null);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(space.LearningElements, Has.Count.EqualTo(1));
        Assert.That(space.LearningElements[0], Is.EqualTo(element));
        Assert.That(space.LearningElements[0], Is.InstanceOf(typeof(H5PTestElement)));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
    }
    
    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var space = new LearningSpace("a", "b", "c", "d", "e", 3);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space,"element", mockBusinessLogic, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesLoadLearningElement()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var space = new LearningSpace("a", "b", "c", "d", "e", 2);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var element =
            new LearningElement("a", "b", null!, "a", "b",
                "c", LearningElementDifficultyEnum.Easy, space, 1, 9, 2,3);
        var element2 = new LearningElement("f", "g", null!, "h", "i", "j",
            LearningElementDifficultyEnum.Easy, space, 5, 2, 1,5);
        mockBusinessLogic.LoadLearningElement(Arg.Any<string>()).Returns(element);
        space.LearningElements.Add(element2);
        space.SelectedLearningElement = element2;
        var command = new LoadLearningElement(space, "element", mockBusinessLogic, mappingAction);
        
        Assert.That(space.LearningElements, Has.Count.EqualTo(1));
        Assert.That(space.LearningElements[0], Is.EqualTo(element2));
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element2));
        
        command.Execute();
        
        Assert.That(space.LearningElements, Has.Count.EqualTo(2));
        Assert.That(space.LearningElements[0], Is.EqualTo(element2));
        Assert.That(space.LearningElements[1], Is.EqualTo(element));
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(space.LearningElements, Has.Count.EqualTo(1));
        Assert.That(space.LearningElements[0], Is.EqualTo(element2));
        Assert.That(space.SelectedLearningElement.Name, Is.EqualTo(element2.Name));
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element2));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(space.LearningElements, Has.Count.EqualTo(2));
        Assert.That(space.LearningElements[0], Is.EqualTo(element2));
        Assert.That(space.LearningElements[1], Is.EqualTo(element));
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(space.LearningElements, Has.Count.EqualTo(1));
        Assert.That(space.LearningElements[0], Is.EqualTo(element2));
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element2));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(space.LearningElements, Has.Count.EqualTo(2));
        Assert.That(space.LearningElements[0], Is.EqualTo(element2));
        Assert.That(space.LearningElements[1], Is.EqualTo(element));
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
    }
}