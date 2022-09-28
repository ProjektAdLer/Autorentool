using System.Collections.Generic;
using AngleSharp.Dom;
using Bunit;
using NSubstitute;
using NUnit.Framework;
using Presentation.Components.ModalDialog;
using Presentation.PresentationLogic.LearningContent;
using Presentation.PresentationLogic.ModalDialog;
using TestContext = Bunit.TestContext;

namespace PresentationTest.PresentationLogic.ModalDialog;

[TestFixture]
public class ModalDialogFactoryUt
{
    private TestContext ctx = null!;

    [SetUp]
    public void Setup()
    {
        ctx = new TestContext();
    }

    [Test]
    public void ILearningSpaceViewModalDialogFactory_GetCreateLearningElementFragment_returnsFragment()
    {
        // Arrange
        ILearningSpaceViewModalDialogFactory systemUnderTest = CreateFactoryForTesting();

        // Act
        var fragment = systemUnderTest.GetCreateLearningElementFragment(null, _ => { });
        var renderedFragment = ctx.Render(fragment);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(fragment, Is.Not.Null);
            Assert.That(renderedFragment, Is.Not.Null);
        });
    }


    [Test]
    public void ILearningSpaceViewModalDialogFactory_GetCreateLearningElement_CallsModalDialogInputFieldsFactory()
    {
        //Arrange
        var spaceViewInputFieldsFactory = Substitute.For<ILearningSpaceViewModalDialogInputFieldsFactory>();
        ILearningSpaceViewModalDialogFactory systemUnderTest = CreateFactoryForTesting(spaceViewInputFieldsFactory);

        //Act
        systemUnderTest.GetCreateLearningElementFragment(null, _ => { });

        //Assert
        spaceViewInputFieldsFactory.Received()
            .GetCreateLearningElementInputFields(Arg.Any<LearningContentViewModel>());
    }

    [Test]
    public void ILearningSpaceViewModalDialogFactory_GetCreateLearningElement_ReturnsFragment()
    {
        //Arrange
        ILearningSpaceViewModalDialogFactory systemUnderTest = CreateFactoryForTesting();

        //Act
        var fragment = systemUnderTest.GetCreateLearningElementFragment(null, _ => { });
        var renderedFragment = ctx.Render(fragment);

        //Assert
        TestCreateOrEditLearningElementOrSpaceModalDialog(renderedFragment, true, true);
    }


    [Test]
    public void ILearningSpaceViewModalDialogFactory_GetEditLearningElement_CallsModalDialogInputFieldsFactory()
    {
        //Arrange
        var spaceViewInputFieldsFactory = Substitute.For<ILearningSpaceViewModalDialogInputFieldsFactory>();
        ILearningSpaceViewModalDialogFactory systemUnderTest = CreateFactoryForTesting(spaceViewInputFieldsFactory);

        //Act
        systemUnderTest.GetEditLearningElementFragment(new Dictionary<string, string>(), _ => { });

        //Assert
        spaceViewInputFieldsFactory.Received().GetEditLearningElementInputFields();
    }

    [Test]
    public void ILearningSpaceViewModalDialogFactory_GetEditLearningElement_ReturnsFragment()
    {
        //Arrange
        ILearningSpaceViewModalDialogFactory systemUnderTest = CreateFactoryForTesting();

        //Act
        var fragment =
            systemUnderTest.GetEditLearningElementFragment(new Dictionary<string, string>() {{"UnitTest", "TestValue"}},
                _ => { });
        var renderedFragment = ctx.Render(fragment);

        //Assert
        TestCreateOrEditLearningElementOrSpaceModalDialog(renderedFragment, false, true);
    }

    [Test]
    public void ILearningSpaceViewModalDialogFactory_GetEditLearningSpace_CallsModalDialogInputFieldsFactory()
    {
        //Arrange
        var spaceViewInputFieldsFactory = Substitute.For<ILearningSpaceViewModalDialogInputFieldsFactory>();
        ILearningSpaceViewModalDialogFactory systemUnderTest = CreateFactoryForTesting(spaceViewInputFieldsFactory);

        //Act
        systemUnderTest.GetEditLearningSpaceFragment(new Dictionary<string, string>(), _ => { });

        //Assert
        spaceViewInputFieldsFactory.Received().GetEditLearningSpaceInputFields();
    }

    [Test]
    public void ILearningSpaceViewModalDialogFactory_GetEditLearningSpace_ReturnsFragment()
    {
        //Arrange
        ILearningSpaceViewModalDialogFactory systemUnderTest = CreateFactoryForTesting();

        //Act
        var fragment =
            systemUnderTest.GetEditLearningSpaceFragment(new Dictionary<string, string>() {{"UnitTest", "TestValue"}},
                _ => { });
        var renderedFragment = ctx.Render(fragment);

        //Assert
        TestCreateOrEditLearningElementOrSpaceModalDialog(renderedFragment, false, false);
    }

    [Test]
    public void ILearningWorldViewModalDialogFactory_GetCreateLearningSpace_CallsModalDialogInputFieldsFactory()
    {
        //Arrange
        var worldViewInputFieldsFactory = Substitute.For<ILearningWorldViewModalDialogInputFieldsFactory>();
        ILearningWorldViewModalDialogFactory systemUnderTest =
            CreateFactoryForTesting(worldViewInputFieldsFactory: worldViewInputFieldsFactory);

        //Act
        systemUnderTest.GetCreateLearningSpaceFragment(_ => { });

        //Assert
        worldViewInputFieldsFactory.Received().GetCreateLearningSpaceInputFields();
    }

    [Test]
    public void ILearningWorldViewModalDialogFactory_GetCreateLearningSpace_ReturnsFragment()
    {
        //Arrange
        ILearningWorldViewModalDialogFactory systemUnderTest = CreateFactoryForTesting();

        //Act
        var fragment = systemUnderTest.GetCreateLearningSpaceFragment(_ => { });
        var renderedFragment = ctx.Render(fragment);

        //Assert
        TestCreateOrEditLearningElementOrSpaceModalDialog(renderedFragment, true, false);
    }

    [Test]
    public void ILearningWorldViewModalDialogFactory_GetEditLearningSpace_CallsModalDialogInputFieldsFactory()
    {
        //Arrange
        var worldViewInputFieldsFactory = Substitute.For<ILearningWorldViewModalDialogInputFieldsFactory>();
        ILearningWorldViewModalDialogFactory systemUnderTest =
            CreateFactoryForTesting(worldViewInputFieldsFactory: worldViewInputFieldsFactory);

        //Act
        systemUnderTest.GetEditLearningSpaceFragment(new Dictionary<string, string>(), _ => { });

        //Assert
        worldViewInputFieldsFactory.Received().GetEditLearningSpaceInputFields();
    }

    [Test]
    public void ILearningWorldViewModalDialogFactory_GetEditLearningSpace_ReturnsFragment()
    {
        //Arrange
        ILearningWorldViewModalDialogFactory systemUnderTest = CreateFactoryForTesting();

        //Act
        var fragment =
            systemUnderTest.GetEditLearningSpaceFragment(new Dictionary<string, string>() {{"UnitTest", "TestValue"}},
                _ => { });
        var renderedFragment = ctx.Render(fragment);

        //Assert
        TestCreateOrEditLearningElementOrSpaceModalDialog(renderedFragment, false, false);
    }

    private static void TestCreateOrEditLearningElementOrSpaceModalDialog(IRenderedFragment renderedFragment,
        bool isCreate, bool isLearningElement)
    {
        Assert.Multiple(() => { Assert.That(renderedFragment, Is.Not.Null); });
        TestModalDialogBasicStructure(renderedFragment);
        TestCreateOrEditHeader(renderedFragment, isCreate, isLearningElement);
        TestCreateOrEditDialogBody(renderedFragment, isLearningElement);
        TestFooterOkCancel(renderedFragment);
    }

    private static void TestModalDialogBasicStructure(IRenderedFragment renderedFragment)
    {
        Assert.Multiple(() =>
        {
            Assert.That(renderedFragment.Find("div.modal-content").Children, Has.Length.EqualTo(3));
            Assert.That(renderedFragment.Find("div.modal-content").Children[0].ClassName, Is.EqualTo("modal-header"));
            Assert.That(renderedFragment.Find("div.modal-content").Children[1].ClassName, Is.EqualTo("modal-body"));
            Assert.That(renderedFragment.Find("div.modal-content").Children[2].ClassName, Is.EqualTo("modal-footer"));
        });
    }

    private static void TestCreateOrEditHeader(IRenderedFragment renderedFragment, bool isCreate,
        bool isLearningElement)
    {
        var createOrEdit = isCreate ? "Create new" : "Edit existing";
        var learningElementOrSpace = isLearningElement ? "learning element" : "learning space";
        TestDialogHeader(renderedFragment, $"{createOrEdit} {learningElementOrSpace}", true);
    }

    private static void TestCreateOrEditDialogBody(IRenderedFragment renderedFragment, bool isElement)
    {
        var learningElementOrSpace = isElement ? "learning element" : "learning space";
        TestModalBody(renderedFragment, $"Please enter the required data for the {learningElementOrSpace} below:",
            true);
    }

    [Test]
    public void
        IAuthoringToolWorkspaceViewModalDialogFactory_GetInformationMessageFragment_ReturnsFragment()
    {
        //Arrange
        IAuthoringToolWorkspaceViewModalDialogFactory systemUnderTest = CreateFactoryForTesting();
        const string message = "Test message";

        //Act
        var fragment = systemUnderTest.GetInformationMessageFragment(_ => { }, message);
        var renderedFragment = ctx.Render(fragment);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(renderedFragment, Is.Not.Null);
            TestModalDialogBasicStructure(renderedFragment);
            TestDialogHeader(renderedFragment, "Information", false);
            TestModalBody(renderedFragment, message, false);
            TestFooterOk(renderedFragment);
        });
    }

    [Test]
    public void
        IAuthoringToolWorkspaceViewModalDialogFactory_GetSaveUnsavedWorldsFragment_ReturnsFragment()
    {
        //Arrange
        IAuthoringToolWorkspaceViewModalDialogFactory systemUnderTest = CreateFactoryForTesting();
        const string unsavedWorldName = "Unsaved World";

        //Act
        var fragment = systemUnderTest.GetSaveUnsavedWorldsFragment(_ => { }, unsavedWorldName);
        var renderedFragment = ctx.Render(fragment);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(renderedFragment, Is.Not.Null);
            TestModalDialogBasicStructure(renderedFragment);
            TestDialogHeader(renderedFragment, "Save unsaved worlds?", true);
            TestModalBody(renderedFragment, $"World {unsavedWorldName} has unsaved changes. Do you want to save it?",
                false);
            TestFooterYesNoCancel(renderedFragment);
        });
    }

    [Test]
    public void IAuthoringToolWorkspaceViewModalDialogFactory_GetDeleteUnsavedWorldFragment_ReturnsFragment()
    {
        //Arrange
        IAuthoringToolWorkspaceViewModalDialogFactory systemUnderTest = CreateFactoryForTesting();
        const string deletedUnsavedWorldName = "World";

        //Act
        var fragment = systemUnderTest.GetDeleteUnsavedWorldFragment(_ => { }, deletedUnsavedWorldName);
        var renderedFragment = ctx.Render(fragment);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(renderedFragment, Is.Not.Null);
            TestModalDialogBasicStructure(renderedFragment);
            TestDialogHeader(renderedFragment, "Save deleted world?", false);
            TestModalBody(renderedFragment,
                $"Deleted world {deletedUnsavedWorldName} has unsaved changes. Do you want to save it?", false);
            TestFooterYesNo(renderedFragment);
        });
    }

    [Test]
    public void
        IAuthoringToolWorkspaceViewModalDialogFactory_GetCreateLearningWorldFragment_CallsModalDialogInputFieldsFactory()
    {
        //Arrange
        var authoringToolWorkspaceViewInputFieldsFactory =
            Substitute.For<IAuthoringToolWorkspaceViewModalDialogInputFieldsFactory>();
        IAuthoringToolWorkspaceViewModalDialogFactory systemUnderTest =
            CreateFactoryForTesting(
                authoringToolWorkspaceViewModalDialogInputFieldsFactory: authoringToolWorkspaceViewInputFieldsFactory);

        //Act
        systemUnderTest.GetCreateLearningWorldFragment(_ => { });

        //Assert
        authoringToolWorkspaceViewInputFieldsFactory.Received().GetCreateLearningWorldInputFields();
    }

    [Test]
    public void IAuthoringToolWorkspaceViewModalDialogFactory_GetCreateLearningWorldFragment_ReturnsFragment()
    {
        //Arrange
        var authoringToolWorkspaceViewInputFieldsFactory =
            Substitute.For<IAuthoringToolWorkspaceViewModalDialogInputFieldsFactory>();
        authoringToolWorkspaceViewInputFieldsFactory.GetCreateLearningWorldInputFields().Returns(
            new ModalDialogInputField[]
                {new("UnitTest", ModalDialogInputType.Text, true)});
        IAuthoringToolWorkspaceViewModalDialogFactory systemUnderTest =
            CreateFactoryForTesting(
                authoringToolWorkspaceViewModalDialogInputFieldsFactory: authoringToolWorkspaceViewInputFieldsFactory);

        //Act
        var fragment = systemUnderTest.GetCreateLearningWorldFragment(_ => { });
        var renderedFragment = ctx.Render(fragment);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(renderedFragment, Is.Not.Null);
            TestModalDialogBasicStructure(renderedFragment);
            TestDialogHeader(renderedFragment, "Create new learning world", true);
            TestModalBody(renderedFragment, "Please enter the required data for the learning world below:", true);
            TestFooterOkCancel(renderedFragment);
        });
    }

    [Test]
    public void
        IAuthoringToolWorkspaceViewModalDialogFactory_GetEditLearningWorldFragment_CallsModalDialogInputFieldsFactory()
    {
        //Arrange
        var authoringToolWorkspaceViewInputFieldsFactory =
            Substitute.For<IAuthoringToolWorkspaceViewModalDialogInputFieldsFactory>();
        IAuthoringToolWorkspaceViewModalDialogFactory systemUnderTest =
            CreateFactoryForTesting(
                authoringToolWorkspaceViewModalDialogInputFieldsFactory: authoringToolWorkspaceViewInputFieldsFactory);

        //Act
        systemUnderTest.GetEditLearningWorldFragment(new Dictionary<string, string>(), _ => { });

        //Assert
        authoringToolWorkspaceViewInputFieldsFactory.Received().GetEditLearningWorldInputFields();
    }

    [Test]
    public void IAuthoringToolWorkspaceViewModalDialogFactory_GetEditLearningWorldFragment_ReturnsFragment()
    {
        //Arrange
        var authoringToolWorkspaceViewInputFieldsFactory =
            Substitute.For<IAuthoringToolWorkspaceViewModalDialogInputFieldsFactory>();
        authoringToolWorkspaceViewInputFieldsFactory.GetEditLearningWorldInputFields().Returns(
            new ModalDialogInputField[]
                {new("UnitTest", ModalDialogInputType.Text, true)});
        IAuthoringToolWorkspaceViewModalDialogFactory systemUnderTest =
            CreateFactoryForTesting(
                authoringToolWorkspaceViewModalDialogInputFieldsFactory: authoringToolWorkspaceViewInputFieldsFactory);

        //Act
        var fragment =
            systemUnderTest.GetEditLearningWorldFragment(new Dictionary<string, string>() {{"UnitTest", "TestValue"}},
                _ => { });
        var renderedFragment = ctx.Render(fragment);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(renderedFragment, Is.Not.Null);
            TestModalDialogBasicStructure(renderedFragment);
            TestDialogHeader(renderedFragment, "Edit existing learning world", true);
            TestModalBody(renderedFragment, "Please enter the required data for the learning world below:", true);
            TestFooterOkCancel(renderedFragment);
        });
    }

    [Test]
    public void
        IAuthoringToolWorkspaceViewModalDialogFactory_GetErrorStateFragment_ReturnsFragment()
    {
        //Arrange
        IAuthoringToolWorkspaceViewModalDialogFactory systemUnderTest = CreateFactoryForTesting();
        const string message = "Test message";

        //Act
        var fragment = systemUnderTest.GetErrorStateFragment(_ => { }, message);
        var renderedFragment = ctx.Render(fragment);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(renderedFragment, Is.Not.Null);
            TestModalDialogBasicStructure(renderedFragment);
            TestDialogHeader(renderedFragment, "Exception encountered", false);
            TestModalBody(renderedFragment, message, false);
            TestFooterOk(renderedFragment);
        });
    }

    private static void TestDialogHeader(IRenderedFragment renderedFragment, string title, bool hasCloseButton)
    {
        var header = renderedFragment.Find("div.modal-header");
        var headerLength = hasCloseButton ? 2 : 1;
        Assert.That(header, Is.Not.Null);
        Assert.That(header.Children, Has.Length.EqualTo(headerLength));

        Assert.Multiple(() =>
        {
            Assert.That(header.Children[0].TagName, Is.EqualTo("H4"));
            Assert.That(header.Children[0].ClassName, Is.EqualTo("modal-title"));
            Assert.That(renderedFragment.Find("H4.modal-title").InnerHtml,
                Is.EqualTo($"{title}"));
        });

        if (!hasCloseButton) return;
        Assert.That(header.Children, Has.Length.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(header.Children[1].TagName, Is.EqualTo("BUTTON"));
            Assert.That(header.Children[1].ClassName, Is.EqualTo("close"));
            Assert.That(header.Children[1].InnerHtml, Is.EqualTo("×"));
        });
    }

    private static void TestModalBody(IRenderedFragment renderedFragment, string message, bool hasInputField)
    {
        var body = renderedFragment.Find("div.modal-body");
        var bodyLength = hasInputField ? 2 : 1;
        Assert.That(body, Is.Not.Null);
        Assert.That(body.Children, Has.Length.EqualTo(bodyLength));

        Assert.Multiple(() =>
        {
            Assert.That(body.Children[0].TagName, Is.EqualTo("SPAN"));
            Assert.That(body.Children[0].InnerHtml, Is.EqualTo(message));
        });

        if (hasInputField) TestModalInputWithOneTextInput(body.Children[1]);
    }

    private static void TestModalInputWithOneTextInput(IParentNode modalInput)
    {
        Assert.That(modalInput.Children, Has.Length.EqualTo(1));
        Assert.That(modalInput.Children[0].Children, Has.Length.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(modalInput.Children[0].Children[0].ClassName, Is.EqualTo("col"));
            Assert.That(modalInput.Children[0].Children[0].InnerHtml,
                Is.EqualTo("UnitTest"));
            Assert.That(modalInput.Children[0].Children[1].ClassName, Is.EqualTo("col"));
            Assert.That(modalInput.Children[0].Children[1].Children,
                Has.Length.EqualTo(1));
            Assert.That(modalInput.Children[0].Children[1].Children[0].Id,
                Is.EqualTo("modal-input-field-unittest"));
        });
    }


    private static void TestFooterOk(IRenderedFragment renderedFragment)
    {
        Assert.That(renderedFragment.Find("div.modal-footer").Children, Has.Length.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(renderedFragment.Find("div.modal-footer").Children[0].TagName, Is.EqualTo("BUTTON"));
            Assert.That(renderedFragment.Find("div.modal-footer").Children[0].Id, Is.EqualTo("btn-ok"));
            Assert.That(renderedFragment.Find("button.btn-primary").Id, Is.EqualTo("btn-ok"));
            Assert.That(renderedFragment.Find("button.btn-primary").InnerHtml, Is.EqualTo("OK"));
        });
    }

    private static void TestFooterOkCancel(IRenderedFragment renderedFragment)
    {
        Assert.That(renderedFragment.Find("div.modal-footer").Children, Has.Length.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(renderedFragment.Find("div.modal-footer").Children[0].Id, Is.EqualTo("btn-cancel"));
            Assert.That(renderedFragment.Find("div.modal-footer").Children[0].TagName, Is.EqualTo("BUTTON"));
            Assert.That(renderedFragment.Find("button.btn-warning").Id, Is.EqualTo("btn-cancel"));
            Assert.That(renderedFragment.Find("button.btn-warning").InnerHtml, Is.EqualTo("Cancel"));

            Assert.That(renderedFragment.Find("div.modal-footer").Children[1].Id, Is.EqualTo("btn-ok"));
            Assert.That(renderedFragment.Find("div.modal-footer").Children[1].TagName, Is.EqualTo("BUTTON"));
            Assert.That(renderedFragment.Find("button.btn-success").Id, Is.EqualTo("btn-ok"));
            Assert.That(renderedFragment.Find("button.btn-success").InnerHtml, Is.EqualTo("OK"));
        });
    }

    private static void TestFooterYesNo(IRenderedFragment renderedFragment)
    {
        Assert.That(renderedFragment.Find("div.modal-footer").Children, Has.Length.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(renderedFragment.Find("div.modal-footer").Children[0].Id, Is.EqualTo("btn-yes"));
            Assert.That(renderedFragment.Find("div.modal-footer").Children[0].TagName, Is.EqualTo("BUTTON"));
            Assert.That(renderedFragment.Find("button.btn-success").Id, Is.EqualTo("btn-yes"));
            Assert.That(renderedFragment.Find("button.btn-success").InnerHtml, Is.EqualTo("Yes"));

            Assert.That(renderedFragment.Find("div.modal-footer").Children[1].Id, Is.EqualTo("btn-no"));
            Assert.That(renderedFragment.Find("div.modal-footer").Children[1].TagName, Is.EqualTo("BUTTON"));
            Assert.That(renderedFragment.Find("button.btn-danger").Id, Is.EqualTo("btn-no"));
            Assert.That(renderedFragment.Find("button.btn-danger").InnerHtml, Is.EqualTo("No"));
        });
    }

    private static void TestFooterYesNoCancel(IRenderedFragment renderedFragment)
    {
        Assert.That(renderedFragment.Find("div.modal-footer").Children, Has.Length.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(renderedFragment.Find("div.modal-footer").Children[0].Id, Is.EqualTo("btn-yes"));
            Assert.That(renderedFragment.Find("div.modal-footer").Children[0].TagName, Is.EqualTo("BUTTON"));
            Assert.That(renderedFragment.Find("button.btn-success").Id, Is.EqualTo("btn-yes"));
            Assert.That(renderedFragment.Find("button.btn-success").InnerHtml, Is.EqualTo("Yes"));

            Assert.That(renderedFragment.Find("div.modal-footer").Children[1].Id, Is.EqualTo("btn-no"));
            Assert.That(renderedFragment.Find("div.modal-footer").Children[1].TagName, Is.EqualTo("BUTTON"));
            Assert.That(renderedFragment.Find("button.btn-danger").Id, Is.EqualTo("btn-no"));
            Assert.That(renderedFragment.Find("button.btn-danger").InnerHtml, Is.EqualTo("No"));

            Assert.That(renderedFragment.Find("div.modal-footer").Children[2].Id, Is.EqualTo("btn-cancel"));
            Assert.That(renderedFragment.Find("div.modal-footer").Children[2].TagName, Is.EqualTo("BUTTON"));
            Assert.That(renderedFragment.Find("button.btn-warning").Id, Is.EqualTo("btn-cancel"));
            Assert.That(renderedFragment.Find("button.btn-warning").InnerHtml, Is.EqualTo("Cancel"));
        });
    }

    private static ModalDialogFactory CreateFactoryForTesting(
        ILearningSpaceViewModalDialogInputFieldsFactory? spaceViewInputFieldsFactory = null,
        ILearningWorldViewModalDialogInputFieldsFactory? worldViewInputFieldsFactory = null,
        IAuthoringToolWorkspaceViewModalDialogInputFieldsFactory?
            authoringToolWorkspaceViewModalDialogInputFieldsFactory = null)
    {
        var modalDialogInputFieldReturnValue = new ModalDialogInputField[]
            {new("UnitTest", ModalDialogInputType.Text, true)};

        if (spaceViewInputFieldsFactory == null)
        {
            spaceViewInputFieldsFactory = Substitute.For<ILearningSpaceViewModalDialogInputFieldsFactory>();

            spaceViewInputFieldsFactory
                .GetCreateLearningElementInputFields(Arg.Any<LearningContentViewModel>())
                .Returns(modalDialogInputFieldReturnValue);

            spaceViewInputFieldsFactory.GetEditLearningElementInputFields().Returns(modalDialogInputFieldReturnValue);

            spaceViewInputFieldsFactory.GetEditLearningSpaceInputFields().Returns(modalDialogInputFieldReturnValue);
        }

        if (worldViewInputFieldsFactory == null)
        {
            worldViewInputFieldsFactory = Substitute.For<ILearningWorldViewModalDialogInputFieldsFactory>();

            worldViewInputFieldsFactory.GetCreateLearningSpaceInputFields().Returns(modalDialogInputFieldReturnValue);

            worldViewInputFieldsFactory.GetEditLearningSpaceInputFields().Returns(modalDialogInputFieldReturnValue);
        }

        authoringToolWorkspaceViewModalDialogInputFieldsFactory ??=
            Substitute.For<IAuthoringToolWorkspaceViewModalDialogInputFieldsFactory>();

        return new ModalDialogFactory(spaceViewInputFieldsFactory, worldViewInputFieldsFactory,
            authoringToolWorkspaceViewModalDialogInputFieldsFactory);
    }
}