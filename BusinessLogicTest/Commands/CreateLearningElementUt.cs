using BusinessLogic.Commands;
using BusinessLogic.Commands.Element;
using BusinessLogic.Entities;
using BusinessLogic.Entities.LearningContent;
using NUnit.Framework;
using Shared;

namespace BusinessLogicTest.Commands;

[TestFixture]

public class CreateLearningElementUt
{
    [Test]
    public void Execute_CreatesImageTransferElement()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.SpaceParent, 0, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Transfer, ContentTypeEnum.Image, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points, testParameter.PositionX,testParameter.PositionY, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.ContainedLearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.SpaceParent.ContainedLearningElements.First();
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
            Assert.That(element.Points, Is.EqualTo(testParameter.Points));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(ImageTransferElement)));
        });
    }

    [Test]
    public void Execute_CreatesVideoTransferElement()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.SpaceParent, 0, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Transfer, ContentTypeEnum.Video, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points, testParameter.PositionX, testParameter.PositionY, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.ContainedLearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.SpaceParent.ContainedLearningElements.First();
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
            Assert.That(element.Points, Is.EqualTo(testParameter.Points));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(VideoTransferElement)));
        });
    }

    [Test]
    public void Execute_CreatesPdfTransferElement()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.SpaceParent, 0, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Transfer, ContentTypeEnum.PDF, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points,testParameter.PositionX, testParameter.PositionY, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.ContainedLearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.SpaceParent.ContainedLearningElements.First();
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
            Assert.That(element.Points, Is.EqualTo(testParameter.Points));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(PdfTransferElement)));
        });
    }
    
    [Test]
    public void Execute_CreatesTextTransferElement()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.SpaceParent, 0, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Transfer, ContentTypeEnum.Text, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points, testParameter.PositionX, testParameter.PositionY, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.ContainedLearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.SpaceParent.ContainedLearningElements.First();
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
            Assert.That(element.Points, Is.EqualTo(testParameter.Points));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(TextTransferElement)));
        });
    }

    [Test]
    public void Execute_CreateNewTransferElement_ThrowsException()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var ex = Assert.Throws<ApplicationException>(() =>
        {
            var createLearningElement = new CreateLearningElement(testParameter.SpaceParent, 0, testParameter.Name,
                testParameter.ShortName, ElementTypeEnum.Transfer, ContentTypeEnum.H5P, testParameter.Content,
                testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
                testParameter.Workload, testParameter.Points, testParameter.PositionX, testParameter.PositionY, mappingAction);
        });
        Assert.That(ex!.Message, Is.EqualTo("No Valid ContentType assigned"));
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void Execute_CreatesVideoActivationElement()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.SpaceParent, 0, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Activation, ContentTypeEnum.Video, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points, testParameter.PositionX, testParameter.PositionY, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.ContainedLearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.SpaceParent.ContainedLearningElements.First();
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
            Assert.That(element.Points, Is.EqualTo(testParameter.Points));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(VideoActivationElement)));
        });
    }
    
    [Test]
    public void Execute_CreatesH5PActivationElement()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.SpaceParent, 0, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Activation, ContentTypeEnum.H5P, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points, testParameter.PositionX, testParameter.PositionY, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.ContainedLearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.SpaceParent.ContainedLearningElements.First();
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
            Assert.That(element.Points, Is.EqualTo(testParameter.Points));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(H5PActivationElement)));
        });
    }
    
    [Test]
    public void Execute_CreateNewActivationElement_ThrowsException()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var ex = Assert.Throws<ApplicationException>(() =>
        {
            var createLearningElement = new CreateLearningElement(testParameter.SpaceParent, 0, testParameter.Name, testParameter.ShortName,
                ElementTypeEnum.Activation, ContentTypeEnum.Image, testParameter.Content,
                testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
                testParameter.Workload, testParameter.Points, testParameter.PositionX, testParameter.PositionY, mappingAction);
        });
        Assert.That(ex!.Message, Is.EqualTo("No Valid ContentType assigned"));
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void Execute_CreatesH5PInteractionElement()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.SpaceParent, 0, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Interaction, ContentTypeEnum.H5P, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points, testParameter.PositionX, testParameter.PositionY, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.ContainedLearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.SpaceParent.ContainedLearningElements.First();
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
            Assert.That(element.Points, Is.EqualTo(testParameter.Points));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(H5PInteractionElement)));
        });
    }
    
    [Test]
    public void Execute_CreateNewInteractionElement_ThrowsException()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var ex = Assert.Throws<ApplicationException>(() =>
        {
            var createLearningElement = new CreateLearningElement(testParameter.SpaceParent, 0, testParameter.Name, testParameter.ShortName,
                ElementTypeEnum.Interaction, ContentTypeEnum.Image, testParameter.Content,
                testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
                testParameter.Workload, testParameter.Points, testParameter.PositionX, testParameter.PositionY, mappingAction);
        });
        Assert.That(ex!.Message, Is.EqualTo("No Valid ContentType assigned"));
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void Execute_CreatesH5PTestElement()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.SpaceParent, 0, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Test, ContentTypeEnum.H5P, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points, testParameter.PositionX, testParameter.PositionY, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.ContainedLearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.IsTrue(actionWasInvoked);
        var element = testParameter.SpaceParent.ContainedLearningElements.First();
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
            Assert.That(element.Points, Is.EqualTo(testParameter.Points));
            Assert.That(element.Difficulty, Is.EqualTo(testParameter.Difficulty));
            Assert.That(element, Is.InstanceOf(typeof(H5PTestElement)));
        });
    }
    
    [Test]
    public void Execute_CreateNewTestElement_ThrowsException()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var ex = Assert.Throws<ApplicationException>(() =>
        {
            var createLearningElement = new CreateLearningElement(testParameter.SpaceParent, 0, testParameter.Name,
                testParameter.ShortName, ElementTypeEnum.Test, ContentTypeEnum.Image, testParameter.Content,
                testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
                testParameter.Workload, testParameter.Points, testParameter.PositionX, testParameter.PositionY, mappingAction);
        });
        Assert.That(ex!.Message, Is.EqualTo("No Valid ContentType assigned"));
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void Execute_AddsImageTransferElement()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var element = new ImageTransferElement(testParameter.Name, testParameter.ShortName, testParameter.SpaceParent,
            testParameter.Content, testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points,1,2);
        
        var command = new CreateLearningElement(testParameter.SpaceParent, 0, element, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.ContainedLearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(testParameter.SpaceParent.ContainedLearningElements.First(), Is.EqualTo(element));
        Assert.That(testParameter.SpaceParent.ContainedLearningElements.First(), Is.InstanceOf(typeof(ImageTransferElement)));
        Assert.IsTrue(actionWasInvoked);
    }
    
    [Test]
    public void Execute_AddsVideoTransferElement()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var element = new VideoTransferElement(testParameter.Name, testParameter.ShortName, testParameter.SpaceParent,
            testParameter.Content, testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points,1,2);
        
        var command = new CreateLearningElement(testParameter.SpaceParent, 0, element, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.ContainedLearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(testParameter.SpaceParent.ContainedLearningElements.First(), Is.EqualTo(element));
        Assert.That(testParameter.SpaceParent.ContainedLearningElements.First(), Is.InstanceOf(typeof(VideoTransferElement)));
        Assert.IsTrue(actionWasInvoked);
    }
    
    [Test]
    public void Execute_AddsPdfTransferElement()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var element = new PdfTransferElement(testParameter.Name, testParameter.ShortName, testParameter.SpaceParent,
            testParameter.Content, testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points,1,2);
        
        var command = new CreateLearningElement(testParameter.SpaceParent, 0, element, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.ContainedLearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(testParameter.SpaceParent.ContainedLearningElements.First(), Is.EqualTo(element));
        Assert.That(testParameter.SpaceParent.ContainedLearningElements.First(), Is.InstanceOf(typeof(PdfTransferElement)));
        Assert.IsTrue(actionWasInvoked);
    }
    
    [Test]
    public void Execute_AddsVideoActivationElement()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var element = new VideoActivationElement(testParameter.Name, testParameter.ShortName, testParameter.SpaceParent,
            testParameter.Content, testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points,1,2);
        
        var command = new CreateLearningElement(testParameter.SpaceParent, 0, element, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.ContainedLearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(testParameter.SpaceParent.ContainedLearningElements.First(), Is.EqualTo(element));
        Assert.That(testParameter.SpaceParent.ContainedLearningElements.First(), Is.InstanceOf(typeof(VideoActivationElement)));
        Assert.IsTrue(actionWasInvoked);
    }
    
    [Test]
    public void Execute_AddsH5PActivationElement()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var element = new H5PActivationElement(testParameter.Name, testParameter.ShortName, testParameter.SpaceParent,
            testParameter.Content, testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points,1,2);
        
        var command = new CreateLearningElement(testParameter.SpaceParent, 0, element, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.ContainedLearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(testParameter.SpaceParent.ContainedLearningElements.First(), Is.EqualTo(element));
        Assert.That(testParameter.SpaceParent.ContainedLearningElements.First(), Is.InstanceOf(typeof(H5PActivationElement)));
        Assert.IsTrue(actionWasInvoked);
    }
    
    [Test]
    public void Execute_AddsH5PInteractionElement()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var element = new H5PInteractionElement(testParameter.Name, testParameter.ShortName, testParameter.SpaceParent,
            testParameter.Content, testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points,1,2);
        
        var command = new CreateLearningElement(testParameter.SpaceParent, 0, element, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.ContainedLearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(testParameter.SpaceParent.ContainedLearningElements.First(), Is.EqualTo(element));
        Assert.That(testParameter.SpaceParent.ContainedLearningElements.First(), Is.InstanceOf(typeof(H5PInteractionElement)));
        Assert.IsTrue(actionWasInvoked);
    }
    
    [Test]
    public void Execute_AddsH5PTestElement()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var element = new H5PTestElement(testParameter.Name, testParameter.ShortName, testParameter.SpaceParent,
            testParameter.Content, testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points,1,2);
        
        var command = new CreateLearningElement(testParameter.SpaceParent, 0, element, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.ContainedLearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(testParameter.SpaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(testParameter.SpaceParent.ContainedLearningElements.First(), Is.EqualTo(element));
        Assert.That(testParameter.SpaceParent.ContainedLearningElements.First(), Is.InstanceOf(typeof(H5PTestElement)));
        Assert.IsTrue(actionWasInvoked);
    }
    
    [Test]
    public void Execute_NoValidElementType_ThrowsException()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;

        var ex = Assert.Throws<ApplicationException>(() =>
        {
            var createLearningElement = new CreateLearningElement(testParameter.SpaceParent, 0, testParameter.Name, testParameter.ShortName,
                (ElementTypeEnum)5, ContentTypeEnum.Image, testParameter.Content,
                testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
                testParameter.Workload, testParameter.Points, testParameter.PositionX, testParameter.PositionY, mappingAction);
        });
        Assert.That(ex!.Message, Is.EqualTo("no valid ElementType assigned"));
        Assert.IsFalse(actionWasInvoked);
    }
    
    [Test]
    public void CreateLearningElement_SetElementAsSelectedInParent()
    {
        var testParameter = new TestParameter();
        var actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var element = new H5PTestElement(testParameter.Name, testParameter.ShortName, testParameter.SpaceParent,
            testParameter.Content, testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points,1,2);
        
        var command = new CreateLearningElement(testParameter.SpaceParent, 0, element, mappingAction);

        Assert.IsEmpty(testParameter.SpaceParent.ContainedLearningElements);
        Assert.IsFalse(actionWasInvoked);

        command.Execute();
        
        Assert.That(actionWasInvoked, Is.True);
        Assert.That(testParameter.SpaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        
        Assert.Multiple(() =>
        {
            Assert.That(testParameter.SpaceParent.ContainedLearningElements.First(), Is.InstanceOf(typeof(H5PTestElement))); 
            Assert.That(testParameter.SpaceParent.ContainedLearningElements.First(), Is.EqualTo(element));
            Assert.That(testParameter.SpaceParent.SelectedLearningElement, Is.EqualTo(element));
        });
    }

    [Test]
    public void UndoRedo_UndoesRedoesCreateLearningElement()
    {
        var testParameter = new TestParameter();
        var spaceParent = testParameter.SpaceParent;
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(spaceParent, 1, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Transfer, ContentTypeEnum.Image, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points, testParameter.PositionX, testParameter.PositionY,
            mappingAction);
        var element2 = new LearningElement("x", "x", null!, "x", "x", "x", LearningElementDifficultyEnum.Easy);
        spaceParent.LearningSpaceLayout.LearningElements = new ILearningElement?[]{element2, null, null, null, null, null};
        spaceParent.SelectedLearningElement = element2;
        

        Assert.That(spaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(spaceParent.ContainedLearningElements.First(), Is.EqualTo(element2));
        Assert.That(spaceParent.SelectedLearningElement, Is.EqualTo(element2));
        Assert.IsFalse(actionWasInvoked);

        command.Execute();

        Assert.That(spaceParent.ContainedLearningElements.Count(), Is.EqualTo(2));
        Assert.That(spaceParent.SelectedLearningElement, Is.EqualTo(spaceParent.ContainedLearningElements.Last()));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Undo();
        
        Assert.That(spaceParent.ContainedLearningElements.Count(), Is.EqualTo(1));
        Assert.That(spaceParent.ContainedLearningElements.First(), Is.EqualTo(element2));
        Assert.That(spaceParent.SelectedLearningElement, Is.EqualTo(element2));
        Assert.IsTrue(actionWasInvoked); actionWasInvoked = false;
        
        command.Redo();
        
        Assert.That(spaceParent.ContainedLearningElements.Count(), Is.EqualTo(2));
        Assert.That(spaceParent.SelectedLearningElement, Is.EqualTo(spaceParent.ContainedLearningElements.Last()));
        Assert.IsTrue(actionWasInvoked);
    }

    [Test]
    public void Undo_MementoIsNull_ThrowsException()
    {
        var testParameter = new TestParameter();
        bool actionWasInvoked = false;
        Action<LearningSpace> mappingAction = _ => actionWasInvoked = true;
        var command = new CreateLearningElement(testParameter.SpaceParent, 0, testParameter.Name, testParameter.ShortName,
            ElementTypeEnum.Transfer, ContentTypeEnum.Image, testParameter.Content,
            testParameter.Authors, testParameter.Description, testParameter.Goals, testParameter.Difficulty,
            testParameter.Workload, testParameter.Points, testParameter.PositionX, testParameter.PositionY, mappingAction);
        
        var ex = Assert.Throws<InvalidOperationException>(() => command.Undo());
        Assert.That(ex!.Message, Is.EqualTo("_memento is null"));
        Assert.IsFalse(actionWasInvoked);
    }
}

public class TestParameter
{
    public readonly LearningSpace SpaceParent;
    public readonly string Name;
    public readonly string ShortName;
    public readonly LearningContent Content;
    public readonly string Authors;
    public readonly string Description;
    public readonly string Goals;
    public readonly LearningElementDifficultyEnum Difficulty;
    public readonly int Workload;
    public readonly int Points;
    public readonly double PositionX;
    public readonly double PositionY;

    internal TestParameter()
    {
        SpaceParent = new LearningSpace("l", "m", "n", "o", "p", 0, new LearningSpaceLayout(new ILearningElement?[6], FloorPlanEnum.Rectangle2X3));
        Name = "a";
        ShortName = "b";
        Content = new FileContent("bar", "foo", "");
        Authors = "d";
        Description = "e";
        Goals = "f";
        Difficulty = LearningElementDifficultyEnum.Easy;
        Workload = 3;
        Points = 4;
        PositionX = 5;
        PositionY = 6;
    }
}