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
        var testParameter = new TestParameter();
        var space = testParameter.SpaceParent;
        var element =
            new ImageTransferElement("a", "b", space, null!, "url","a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9, 2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, 0, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(space.ContainedLearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.Null);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element));
        Assert.That(space.ContainedLearningElements.First(), Is.InstanceOf(typeof(ImageTransferElement)));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
    }
    
    [Test]
    public void Execute_LoadsLearningElement_VideoTransfer()
    {
        var testParameter = new TestParameter();
        var space = testParameter.SpaceParent;
        var element =
            new VideoTransferElement("a", "b", space, null!, "url","a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9,2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, 0, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(space.ContainedLearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.Null);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element));
        Assert.That(space.ContainedLearningElements.First(), Is.InstanceOf(typeof(VideoTransferElement)));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
    }
    
    [Test]
    public void Execute_LoadsLearningElement_PdfTransfer()
    {
        var testParameter = new TestParameter();
        var space = testParameter.SpaceParent;
        var element =
            new PdfTransferElement("a", "b", space, null!, "url","a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9, 2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, 0, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(space.ContainedLearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.Null);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element));
        Assert.That(space.ContainedLearningElements.First(), Is.InstanceOf(typeof(PdfTransferElement)));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
    }
    
    [Test]
    public void Execute_LoadsLearningElement_TextTransfer()
    {
        var testParameter = new TestParameter();
        var space = testParameter.SpaceParent;
        var element =
            new TextTransferElement("a", "b", space, null!, "url","a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9, 2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, 0, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(space.ContainedLearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.Null);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element));
        Assert.That(space.ContainedLearningElements.First(), Is.InstanceOf(typeof(TextTransferElement)));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
    }

    [Test]
    public void Execute_LoadsLearningElement_VideoActivation()
    {
        var testParameter = new TestParameter();
        var space = testParameter.SpaceParent;
        var element =
            new VideoActivationElement("a", "b", space, null!, "url","a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9, 2, 3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, 0, filepath, mockBusinessLogic, mappingAction);

        Assert.That(space.ContainedLearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.Null);

        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element));
        Assert.That(space.ContainedLearningElements.First(), Is.InstanceOf(typeof(VideoActivationElement)));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
    }

    [Test]
    public void Execute_LoadsLearningElement_H5PActivation()
    {
        var testParameter = new TestParameter();
        var space = testParameter.SpaceParent;
        var element =
            new H5PActivationElement("a", "b", space, null!, "url","a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9,2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, 0, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(space.ContainedLearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.Null);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element));
        Assert.That(space.ContainedLearningElements.First(), Is.InstanceOf(typeof(H5PActivationElement)));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
    }
    
    [Test]
    public void Execute_LoadsLearningElement_H5PInteraction()
    {
        var testParameter = new TestParameter();
        var space = testParameter.SpaceParent;
        var element =
            new H5PInteractionElement("a", "b", space, null!, "url","a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9, 2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, 0, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(space.ContainedLearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.Null);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element));
        Assert.That(space.ContainedLearningElements.First(), Is.InstanceOf(typeof(H5PInteractionElement)));
        Assert.IsTrue(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
    }
    
    [Test]
    public void Execute_LoadsLearningElement_H5PTest()
    {
        var testParameter = new TestParameter();
        var space = testParameter.SpaceParent;
        var element =
            new H5PTestElement("a", "b", space, null!, "url","a", "b",
                "c", LearningElementDifficultyEnum.Easy, 1, 9,2,3);
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        const string filepath = "c:\\temp\\test";
        mockBusinessLogic.LoadLearningElement(filepath).Returns(element);
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var command = new LoadLearningElement(space, 0, filepath, mockBusinessLogic, mappingAction);
        
        Assert.That(space.ContainedLearningElements, Is.Empty);
        Assert.IsFalse(actionWasInvoked);
        Assert.That(space.SelectedLearningElement, Is.Null);
        
        command.Execute();

        mockBusinessLogic.Received().LoadLearningElement(filepath);
        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element));
        Assert.That(space.ContainedLearningElements.First(), Is.InstanceOf(typeof(H5PTestElement)));
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

        var command = new LoadLearningElement(space, 0, "element", mockBusinessLogic, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void UndoRedo_UndoesAndRedoesLoadLearningElement()
    {
        var mockBusinessLogic = Substitute.For<IBusinessLogic>();
        var testParameter = new TestParameter();
        var space = testParameter.SpaceParent;
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var element =
            new LearningElement("a", "b", null!, "url","a", "b",
                "c", LearningElementDifficultyEnum.Easy, space, 1, 9, 2,3);
        var element2 = new LearningElement("f", "g", null!, "url","h", "i", "j",
            LearningElementDifficultyEnum.Easy, space, 5, 2, 1,5);
        mockBusinessLogic.LoadLearningElement(Arg.Any<string>()).Returns(element);
        space.LearningSpaceLayout.LearningElements = new ILearningElement?[] {element2, null, null, null, null, null};
        space.SelectedLearningElement = element2;
        var command = new LoadLearningElement(space, 1, "element", mockBusinessLogic, mappingAction);
        
        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element2));
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element2));
        
        command.Execute();
        
        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(2));
        Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element2));
        Assert.That(space.ContainedLearningElements.Skip(1).First(), Is.EqualTo(element));
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element2));
        Assert.That(space.SelectedLearningElement.Name, Is.EqualTo(element2.Name));
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element2));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(2));
        Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element2));
        Assert.That(space.ContainedLearningElements.Skip(1).First(), Is.EqualTo(element));
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element2));
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element2));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(space.ContainedLearningElements.Count(), Is.EqualTo(2));
        Assert.That(space.ContainedLearningElements.First(), Is.EqualTo(element2));
        Assert.That(space.ContainedLearningElements.Skip(1).First(), Is.EqualTo(element));
        Assert.That(space.SelectedLearningElement, Is.EqualTo(element));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
    }
}