using BusinessLogic.Commands;
using BusinessLogic.Entities;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class CreateLearningElementUt
{
    [Test]
    public void Execute_CreatesImageTransferElement_WorldParent()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.WorldParent, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Transfer, ContentTypeEnum.Image, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, mappingAction);

        Assert.IsEmpty(testParameter.WorldParent.LearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.WorldParent.LearningElements, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.WorldParent.LearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(testParameter.Name));
            Assert.That(element.Shortname, Is.EqualTo(testParameter.ShortName));
            Assert.That(element.Parent, Is.EqualTo(testParameter.WorldParent));
            Assert.That(element.LearningContent, Is.EqualTo(testParameter.Content));
            Assert.That(element.Authors, Is.EqualTo(testParameter.Authors));
            Assert.That(element.Description, Is.EqualTo(testParameter.Description));
            Assert.That(element.Goals, Is.EqualTo(testParameter.Goals));
            Assert.That(element.Workload, Is.EqualTo(testParameter.Workload));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(ImageTransferElement)));
        });
    }

    [Test]
    public void Execute_CreatesVideoTransferElement_WorldParent()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.WorldParent, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Transfer, ContentTypeEnum.Video, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, mappingAction);

        Assert.IsEmpty(testParameter.WorldParent.LearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.WorldParent.LearningElements, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.WorldParent.LearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(testParameter.Name));
            Assert.That(element.Shortname, Is.EqualTo(testParameter.ShortName));
            Assert.That(element.Parent, Is.EqualTo(testParameter.WorldParent));
            Assert.That(element.LearningContent, Is.EqualTo(testParameter.Content));
            Assert.That(element.Authors, Is.EqualTo(testParameter.Authors));
            Assert.That(element.Description, Is.EqualTo(testParameter.Description));
            Assert.That(element.Goals, Is.EqualTo(testParameter.Goals));
            Assert.That(element.Workload, Is.EqualTo(testParameter.Workload));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(VideoTransferElement)));
        });
    }

    [Test]
    public void Execute_CreatesPdfTransferElement_WorldParent()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.WorldParent, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Transfer, ContentTypeEnum.Pdf, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, mappingAction);

        Assert.IsEmpty(testParameter.WorldParent.LearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.WorldParent.LearningElements, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.WorldParent.LearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(testParameter.Name));
            Assert.That(element.Shortname, Is.EqualTo(testParameter.ShortName));
            Assert.That(element.Parent, Is.EqualTo(testParameter.WorldParent));
            Assert.That(element.LearningContent, Is.EqualTo(testParameter.Content));
            Assert.That(element.Authors, Is.EqualTo(testParameter.Authors));
            Assert.That(element.Description, Is.EqualTo(testParameter.Description));
            Assert.That(element.Goals, Is.EqualTo(testParameter.Goals));
            Assert.That(element.Workload, Is.EqualTo(testParameter.Workload));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(PdfTransferElement)));
        });
    }

    [Test]
    public void Execute_CreatesImageTransferElement_SpaceParent()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.SpaceParent, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Transfer, ContentTypeEnum.Image, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.LearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.LearningElements, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.SpaceParent.LearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(testParameter.Name));
            Assert.That(element.Shortname, Is.EqualTo(testParameter.ShortName));
            Assert.That(element.Parent, Is.EqualTo(testParameter.SpaceParent));
            Assert.That(element.LearningContent, Is.EqualTo(testParameter.Content));
            Assert.That(element.Authors, Is.EqualTo(testParameter.Authors));
            Assert.That(element.Description, Is.EqualTo(testParameter.Description));
            Assert.That(element.Goals, Is.EqualTo(testParameter.Goals));
            Assert.That(element.Workload, Is.EqualTo(testParameter.Workload));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(ImageTransferElement)));
        });
    }

    [Test]
    public void Execute_CreatesVideoTransferElement_SpaceParent()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.SpaceParent, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Transfer, ContentTypeEnum.Video, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.LearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.LearningElements, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.SpaceParent.LearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(testParameter.Name));
            Assert.That(element.Shortname, Is.EqualTo(testParameter.ShortName));
            Assert.That(element.Parent, Is.EqualTo(testParameter.SpaceParent));
            Assert.That(element.LearningContent, Is.EqualTo(testParameter.Content));
            Assert.That(element.Authors, Is.EqualTo(testParameter.Authors));
            Assert.That(element.Description, Is.EqualTo(testParameter.Description));
            Assert.That(element.Goals, Is.EqualTo(testParameter.Goals));
            Assert.That(element.Workload, Is.EqualTo(testParameter.Workload));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(VideoTransferElement)));
        });
    }

    [Test]
    public void Execute_CreatesPdfTransferElement_SpaceParent()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.SpaceParent, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Transfer, ContentTypeEnum.Pdf, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.LearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.LearningElements, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.SpaceParent.LearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(testParameter.Name));
            Assert.That(element.Shortname, Is.EqualTo(testParameter.ShortName));
            Assert.That(element.Parent, Is.EqualTo(testParameter.SpaceParent));
            Assert.That(element.LearningContent, Is.EqualTo(testParameter.Content));
            Assert.That(element.Authors, Is.EqualTo(testParameter.Authors));
            Assert.That(element.Description, Is.EqualTo(testParameter.Description));
            Assert.That(element.Goals, Is.EqualTo(testParameter.Goals));
            Assert.That(element.Workload, Is.EqualTo(testParameter.Workload));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(PdfTransferElement)));
        });
    }

    [Test]
    public void Execute_CreateNewTransferElement_ThrowsException()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.SpaceParent, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Transfer, ContentTypeEnum.H5P, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, mappingAction);

        var ex = Assert.Throws<ApplicationException>(() => command.Execute());
        Assert.That(ex!.Message, Is.EqualTo("No Valid ContentType assigned"));
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void Execute_CreatesVideoActivationElement_WorldParent()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.WorldParent, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Activation, ContentTypeEnum.Video, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, mappingAction);

        Assert.IsEmpty(testParameter.WorldParent.LearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.WorldParent.LearningElements, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.WorldParent.LearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(testParameter.Name));
            Assert.That(element.Shortname, Is.EqualTo(testParameter.ShortName));
            Assert.That(element.Parent, Is.EqualTo(testParameter.WorldParent));
            Assert.That(element.LearningContent, Is.EqualTo(testParameter.Content));
            Assert.That(element.Authors, Is.EqualTo(testParameter.Authors));
            Assert.That(element.Description, Is.EqualTo(testParameter.Description));
            Assert.That(element.Goals, Is.EqualTo(testParameter.Goals));
            Assert.That(element.Workload, Is.EqualTo(testParameter.Workload));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(VideoActivationElement)));
        });
    }
    
    [Test]
    public void Execute_CreatesH5PActivationElement_WorldParent()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.WorldParent, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Activation, ContentTypeEnum.H5P, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, mappingAction);

        Assert.IsEmpty(testParameter.WorldParent.LearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.WorldParent.LearningElements, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.WorldParent.LearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(testParameter.Name));
            Assert.That(element.Shortname, Is.EqualTo(testParameter.ShortName));
            Assert.That(element.Parent, Is.EqualTo(testParameter.WorldParent));
            Assert.That(element.LearningContent, Is.EqualTo(testParameter.Content));
            Assert.That(element.Authors, Is.EqualTo(testParameter.Authors));
            Assert.That(element.Description, Is.EqualTo(testParameter.Description));
            Assert.That(element.Goals, Is.EqualTo(testParameter.Goals));
            Assert.That(element.Workload, Is.EqualTo(testParameter.Workload));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(H5PActivationElement)));
        });
    }
    
    [Test]
    public void Execute_CreatesVideoActivationElement_SpaceParent()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.SpaceParent, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Activation, ContentTypeEnum.Video, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.LearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.LearningElements, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.SpaceParent.LearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(testParameter.Name));
            Assert.That(element.Shortname, Is.EqualTo(testParameter.ShortName));
            Assert.That(element.Parent, Is.EqualTo(testParameter.SpaceParent));
            Assert.That(element.LearningContent, Is.EqualTo(testParameter.Content));
            Assert.That(element.Authors, Is.EqualTo(testParameter.Authors));
            Assert.That(element.Description, Is.EqualTo(testParameter.Description));
            Assert.That(element.Goals, Is.EqualTo(testParameter.Goals));
            Assert.That(element.Workload, Is.EqualTo(testParameter.Workload));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(VideoActivationElement)));
        });
    }
    
    [Test]
    public void Execute_CreatesH5PActivationElement_SpaceParent()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.SpaceParent, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Activation, ContentTypeEnum.H5P, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.LearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.LearningElements, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.SpaceParent.LearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(testParameter.Name));
            Assert.That(element.Shortname, Is.EqualTo(testParameter.ShortName));
            Assert.That(element.Parent, Is.EqualTo(testParameter.SpaceParent));
            Assert.That(element.LearningContent, Is.EqualTo(testParameter.Content));
            Assert.That(element.Authors, Is.EqualTo(testParameter.Authors));
            Assert.That(element.Description, Is.EqualTo(testParameter.Description));
            Assert.That(element.Goals, Is.EqualTo(testParameter.Goals));
            Assert.That(element.Workload, Is.EqualTo(testParameter.Workload));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(H5PActivationElement)));
        });
    }
    
    [Test]
    public void Execute_CreateNewActivationElement_ThrowsException()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.SpaceParent, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Activation, ContentTypeEnum.Image, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, mappingAction);

        var ex = Assert.Throws<ApplicationException>(() => command.Execute());
        Assert.That(ex!.Message, Is.EqualTo("No Valid ContentType assigned"));
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void Execute_CreatesH5PInteractionElement_WorldParent()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.WorldParent, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Interaction, ContentTypeEnum.H5P, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, mappingAction);

        Assert.IsEmpty(testParameter.WorldParent.LearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.WorldParent.LearningElements, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.WorldParent.LearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(testParameter.Name));
            Assert.That(element.Shortname, Is.EqualTo(testParameter.ShortName));
            Assert.That(element.Parent, Is.EqualTo(testParameter.WorldParent));
            Assert.That(element.LearningContent, Is.EqualTo(testParameter.Content));
            Assert.That(element.Authors, Is.EqualTo(testParameter.Authors));
            Assert.That(element.Description, Is.EqualTo(testParameter.Description));
            Assert.That(element.Goals, Is.EqualTo(testParameter.Goals));
            Assert.That(element.Workload, Is.EqualTo(testParameter.Workload));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(H5PInteractionElement)));
        });
    }
    
    [Test]
    public void Execute_CreatesH5PInteractionElement_SpaceParent()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.SpaceParent, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Interaction, ContentTypeEnum.H5P, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.LearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.LearningElements, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.SpaceParent.LearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(testParameter.Name));
            Assert.That(element.Shortname, Is.EqualTo(testParameter.ShortName));
            Assert.That(element.Parent, Is.EqualTo(testParameter.SpaceParent));
            Assert.That(element.LearningContent, Is.EqualTo(testParameter.Content));
            Assert.That(element.Authors, Is.EqualTo(testParameter.Authors));
            Assert.That(element.Description, Is.EqualTo(testParameter.Description));
            Assert.That(element.Goals, Is.EqualTo(testParameter.Goals));
            Assert.That(element.Workload, Is.EqualTo(testParameter.Workload));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(H5PInteractionElement)));
        });
    }
    
    [Test]
    public void Execute_CreateNewInteractionElement_ThrowsException()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.SpaceParent, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Interaction, ContentTypeEnum.Image, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, mappingAction);

        var ex = Assert.Throws<ApplicationException>(() => command.Execute());
        Assert.That(ex!.Message, Is.EqualTo("No Valid ContentType assigned"));
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void Execute_CreatesH5PTestElement_WorldParent()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.WorldParent, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Test, ContentTypeEnum.H5P, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, mappingAction);

        Assert.IsEmpty(testParameter.WorldParent.LearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.WorldParent.LearningElements, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.WorldParent.LearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(testParameter.Name));
            Assert.That(element.Shortname, Is.EqualTo(testParameter.ShortName));
            Assert.That(element.Parent, Is.EqualTo(testParameter.WorldParent));
            Assert.That(element.LearningContent, Is.EqualTo(testParameter.Content));
            Assert.That(element.Authors, Is.EqualTo(testParameter.Authors));
            Assert.That(element.Description, Is.EqualTo(testParameter.Description));
            Assert.That(element.Goals, Is.EqualTo(testParameter.Goals));
            Assert.That(element.Workload, Is.EqualTo(testParameter.Workload));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(H5PTestElement)));
        });
    }
    
    [Test]
    public void Execute_CreatesH5PTestElement_SpaceParent()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.SpaceParent, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Test, ContentTypeEnum.H5P, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.LearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.LearningElements, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.SpaceParent.LearningElements.First();
        Assert.Multiple(() =>
        {
            Assert.That(element.Name, Is.EqualTo(testParameter.Name));
            Assert.That(element.Shortname, Is.EqualTo(testParameter.ShortName));
            Assert.That(element.Parent, Is.EqualTo(testParameter.SpaceParent));
            Assert.That(element.LearningContent, Is.EqualTo(testParameter.Content));
            Assert.That(element.Authors, Is.EqualTo(testParameter.Authors));
            Assert.That(element.Description, Is.EqualTo(testParameter.Description));
            Assert.That(element.Goals, Is.EqualTo(testParameter.Goals));
            Assert.That(element.Workload, Is.EqualTo(testParameter.Workload));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(H5PTestElement)));
        });
    }
    
    [Test]
    public void Execute_CreateNewTestElement_ThrowsException()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.SpaceParent, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Test, ContentTypeEnum.Image, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, mappingAction);

        var ex = Assert.Throws<ApplicationException>(() => command.Execute());
        Assert.That(ex!.Message, Is.EqualTo("No Valid ContentType assigned"));
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void Execute_NoValidElementType_ThrowsException()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.SpaceParent, testParameter.Name, testParameter.ShortName,
            (ElementTypeEnum)5, ContentTypeEnum.Image, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, mappingAction);

        var ex = Assert.Throws<ApplicationException>(() => command.Execute());
        Assert.That(ex!.Message, Is.EqualTo("no valid ElementType assigned"));
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void UndoRedo_UndoesRedoesCreateLearningElement_WorldParent()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.WorldParent, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Transfer, ContentTypeEnum.Image, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, mappingAction);

        Assert.IsEmpty(testParameter.WorldParent.LearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.WorldParent.LearningElements, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.IsEmpty(testParameter.WorldParent.LearningElements);
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(testParameter.WorldParent.LearningElements, Has.Count.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<ILearningElementParent> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.WorldParent, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Transfer, ContentTypeEnum.Image, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        Assert.IsFalse(actionWasInvoked);
    }
}

public class TestParameter
{
    public readonly LearningWorld WorldParent;
    public readonly LearningSpace SpaceParent;
    public readonly string Name;
    public readonly string ShortName;
    public readonly LearningContent Content;
    public readonly string Authors;
    public readonly string Description;
    public readonly string Goals;
    public readonly LearningElementDifficultyEnum Difficulty;
    public readonly int Workload;

    internal TestParameter()
    {
        WorldParent = new LearningWorld("w", "x", "y", "z", "zz", "zzz");
        SpaceParent = new LearningSpace("l", "m", "n", "o", "p");
        Name = "a";
        ShortName = "b";
        Content = new("bar", "foo", new byte[] {0x01, 0x02});
        Authors = "d";
        Description = "e";
        Goals = "f";
        Difficulty = LearningElementDifficultyEnum.Easy;
        Workload = 3;
    }
}